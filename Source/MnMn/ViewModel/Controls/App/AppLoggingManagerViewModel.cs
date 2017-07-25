/*
This file is part of MnMn.

MnMn is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MnMn is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MnMn.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using ContentTypeTextNet.Library.SharedLibrary.Data;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic;
using Microsoft.Win32;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App
{
    public class AppLoggingManagerViewModel: ManagerViewModelBase, ILogAppender
    {
        #region variable

        LogItemModel _selectedLogItem;
        bool _attachmentOutputLogging;

        #endregion

        public AppLoggingManagerViewModel(Mediator mediator, AppLogger appLogge)
            : base(mediator)
        {
            LogList = new FixedSizeCollectionModel<LogItemModel>(appLogge.StockItems, Constants.LogViewCount);
            BindingOperations.EnableCollectionSynchronization(LogList, new object());
            appLogge.IsStock = false;
            appLogge.LogCollector = this;

            LogItems = CollectionViewSource.GetDefaultView(LogList);
        }

        #region property

        FixedSizeCollectionModel<LogItemModel> LogList { get; }
        public IReadOnlyList<LogItemModel> LogListViewer => LogList;
        public ICollectionView LogItems { get; }

        ListBox LogListBox { get; set; }

        public LogItemModel SelectedLogItem
        {
            get { return this._selectedLogItem; }
            set
            {
                if(SetVariableValue(ref this._selectedLogItem, value)) {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        TextWriter Writer { get; set; }
        public bool AttachmentOutputLogging
        {
            get { return this._attachmentOutputLogging; }
            set
            {
                if(AttachmentOutputLogging) {
                    if(Writer != null) {
                        if(!VariableConstants.HasOptionLogDirectoryPath) {
                            Mediation.Logger.LoggerConfig.PutsStream = false;
                        }
                        ((AppLogger)Mediation.Logger).DetachStream(Writer);
                        Writer.Dispose();
                        Writer = null;
                    }
                    SetVariableValue(ref this._attachmentOutputLogging, value);
                } else {
                    var result = AttachmentOutputLoggingFromDialog();
                    SetVariableValue(ref this._attachmentOutputLogging, result);
                }
            }
        }

        #endregion

        #region command

        public ICommand ClearAllLogCommand
        {
            get { return CreateCommand(o => ClearAllLog()); }
        }

        public ICommand CopyAllLogCommand
        {
            get { return CreateCommand(o => CopyAllLog()); }
        }

        public ICommand CopySelectedLogCommand
        {
            get
            {
                return CreateCommand(
                    o => CopySingleLog(SelectedLogItem),
                    o => SelectedLogItem != null
                );
            }
        }

        public ICommand SaveAllLogCommand
        {
            get { return CreateCommand(o => SaveAllLogFromDialog()); }
        }

        #endregion

        #region function

        void ClearAllLog()
        {
            LogList.Clear();
        }

        string GetTextAllLog()
        {
            return string.Join(Environment.NewLine, LogList.Select(l => LogUtility.MakeLogDetailText(l)));
        }

        void CopySingleLog(LogItemModel logItem)
        {
            ShellUtility.SetClipboard(LogUtility.MakeLogDetailText(logItem), Mediation.Logger);
        }

        void CopyAllLog()
        {
            ShellUtility.SetClipboard(GetTextAllLog(), Mediation.Logger);
        }

        void SaveAllLogFromDialog()
        {
            var filter = new DialogFilterList();
            filter.Add(new DialogFilterItem(Properties.Resources.String_Log_Filter_Log, "*.log"));

            var dialog = new SaveFileDialog();
            dialog.Filter = filter.FilterText;
            if(dialog.ShowDialog().GetValueOrDefault()) {
                try {
                    using(var stream = dialog.OpenFile()) {
                        WriteLog(stream);
                    }
                } catch(Exception ex) {
                    Mediation.Logger.Error(ex);
                }
            }
        }

        public void WriteLog(Stream stream)
        {
            using(var writer = new StreamWriter(stream, Encoding.UTF8, Constants.TextFileSaveBuffer, true)) {
                writer.Write(GetTextAllLog());
            }
        }

        bool AttachmentOutputLoggingFromDialog()
        {
            var filter = new DialogFilterList();
            filter.Add(new DialogFilterItem(Properties.Resources.String_Log_Filter_Log, "*.log"));

            var dialog = new SaveFileDialog();
            dialog.Filter = filter.FilterText;
            if(dialog.ShowDialog().GetValueOrDefault()) {
                try {
                    Writer = new StreamWriter(dialog.OpenFile()) {
                        AutoFlush = true,
                    };
                    Mediation.Logger.LoggerConfig.PutsStream = true;
                    ((AppLogger)Mediation.Logger).AttachStream(Writer, true);
                    return true;
                } catch(Exception ex) {
                    Mediation.Logger.Error(ex);
                }
            }

            return false;
        }

        #endregion

        #region AppLoggingManagerViewModel

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return Enumerable.Empty<ManagerViewModelBase>();
        }

        protected override void ShowViewCore()
        {
            if(LogList.Any()) {
                LogListBox.Dispatcher.BeginInvoke(new Action(() => {
                    SelectedLogItem = null;
                    LogListBox.ScrollIntoView(LogList.Last());
                }), DispatcherPriority.ApplicationIdle);
            }
        }

        protected override void HideViewCore()
        { }

        public override Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan, bool force)
        {
            return GarbageCollectionDummyResult;
        }

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override Task UninitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override void InitializeView(MainWindow view)
        {
            LogListBox = view.information.listLog;
        }

        public override void UninitializeView(MainWindow view)
        { }

        #endregion

        #region ILogAppender

        public void AddLog(LogItemModel item)
        {
            var isLastSelect = SelectedLogItem == null;

            lock(LogList) {
                isLastSelect = isLastSelect || LogList.LastOrDefault() == SelectedLogItem;
                LogList.Add(item);
            }

            if(IsVisible && isLastSelect) {
                LogListBox?.Dispatcher.BeginInvoke(new Action(() => {
                    LogListBox.ScrollIntoView(item);
                }), DispatcherPriority.ApplicationIdle);
            }
        }

        #endregion
    }
}
