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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App
{
    public class AppLoggingManagerViewModel: ManagerViewModelBase, ILogAppender
    {
        public AppLoggingManagerViewModel(Mediation mediation, AppLogger appLogge)
            : base(mediation)
        {
            //TODO: 件数即値
            LogList = new FixedSizeCollectionModel<LogItemModel>(appLogge.StockItems, 5000);
            appLogge.IsStock = false;
            appLogge.LogCollector = this;

            LogItems = CollectionViewSource.GetDefaultView(LogList);
        }

        #region property

        FixedSizeCollectionModel<LogItemModel> LogList { get; }
        public ICollectionView LogItems { get; }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region AppLoggingManagerViewModel

        public override Task GarbageCollectionAsync()
        {
            return Task.CompletedTask;
        }

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override void InitializeView(MainWindow view)
        { }

        public override void UninitializeView(MainWindow view)
        { }

        #endregion

        #region ILogAppender

        public void AddLog(LogItemModel item)
        {
            LogList.Add(item);
        }

        #endregion
    }
}
