using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Data;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Laboratory;
using Microsoft.Win32;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Local
{
    public class SmileVideoLaboratoryManagerViewModel: SmileVideoCustomManagerViewModelBase
    {
        public SmileVideoLaboratoryManagerViewModel(Mediation mediation)
            : base(mediation)
        {
            PlayDragAndDrop = new DelegateDragAndDrop() {
                DragEnterAction = PlayDragEnterAndOver,
                DragOverAction = PlayDragEnterAndOver,
                DropAction = PlayDragDop,
            };
        }

        #region property

        public string PlayInputVideoSourceFilePath
        {
            get { return Setting.Laboratory.PlayInputVideoSourceFilePath; }
            set { SetPropertyValue(Setting.Laboratory, value, nameof(Setting.Laboratory.PlayInputVideoSourceFilePath)); }
        }
        public string PlayInputCommentSourceFilePath
        {
            get { return Setting.Laboratory.PlayInputCommentSourceFilePath; }
            set { SetPropertyValue(Setting.Laboratory, value, nameof(Setting.Laboratory.PlayInputCommentSourceFilePath)); }
        }

        public IDragAndDrop PlayDragAndDrop { get; }

        #endregion

        #region command

        public ICommand OpenPlayInputVideoSourceFilePathCommand
        {
            get
            {
                return CreateCommand(o => {
                    var filters = new DialogFilterList() {
                        new DialogFilterItem("video", new [] { "*.mp4", "*.flv" }),
                    };
                    PlayInputVideoSourceFilePath = OpenDialogInputPath(PlayInputVideoSourceFilePath, filters);
                });
            }
        }

        public ICommand OpenPlayInputCommentSourceFilePathCommand
        {
            get
            {
                return CreateCommand(o => {
                    var filters = new DialogFilterList() {
                        new DialogFilterItem("comment", new [] { "*.xml" }),
                    };
                    PlayInputCommentSourceFilePath = OpenDialogInputPath(PlayInputCommentSourceFilePath, filters);
                });
            }
        }

        public ICommand PlayInputFileCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var videoFilePath = PlayInputVideoSourceFilePath;
                        var commentFilePath = PlayInputCommentSourceFilePath;

                        PlayInputFileAsync(videoFilePath, commentFilePath);
                    },
                    o => {
                        return
                            !string.IsNullOrWhiteSpace(PlayInputVideoSourceFilePath)
                            ||
                            !string.IsNullOrWhiteSpace(PlayInputCommentSourceFilePath)
                        ;
                    }
                );
            }
        }

        #endregion

        #region function

        string OpenDialogInputPath(string path, DialogFilterList filters)
        {
            var existFile = File.Exists(path);

            var dialog = new OpenFileDialog() {
                FileName = existFile ? path : string.Empty,
                InitialDirectory = existFile ? Path.GetDirectoryName(path) : Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Filter = filters.FilterText,
                CheckFileExists = true,
            };
            if(dialog.ShowDialog().GetValueOrDefault()) {
                return dialog.FileName;
            }

            return path;
        }

        Task PlayInputFileAsync(string videoFilePath, string commentFilePath)
        {
            var videoFile = new FileInfo(videoFilePath);
            var commentFile = new FileInfo(commentFilePath);

            var vm = new SmileVideoLaboratoryPlayerViewModel(Mediation);
            var task = vm.LoadAsync(videoFile, commentFile);
            Mediation.Request(new ShowViewRequestModel(RequestKind.ShowView, ServiceType.SmileVideo, vm, ShowViewState.Foreground));

            return Task.CompletedTask;
        }

        bool HasExtension(string path, IEnumerable<string> extensions)
        {
            var dotExtension = Path.GetExtension(path);
            if(dotExtension.Length <= ".".Length) {
                return false;
            }

            var ext = dotExtension.Substring(1);

            return extensions.Any(s => string.Equals(s, ext, StringComparison.OrdinalIgnoreCase));
        }

        bool IsEnabledVideoFilePath(string path)
        {
            return HasExtension(path, Constants.AppSmileVideoLaboratoryPlayVideoExtensions);
        }

        bool IsEnabledMsgFilePath(string path)
        {
            return HasExtension(path, Constants.AppSmileVideoLaboratoryPlayMsgExtensions);
        }

        void PlayDragEnterAndOver(UIElement sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;

            if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
                var filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                if(filePaths.Length == 1) {
                    var filePath = filePaths[0];
                    if(Directory.Exists(filePath)) {
                        e.Effects = DragDropEffects.Move;
                    } else {
                        if(IsEnabledVideoFilePath(filePath) || IsEnabledMsgFilePath(filePath)) {
                            e.Effects = DragDropEffects.Move;
                        }
                    }
                } else if(filePaths.Length == 2) {
                    if(!filePaths.All(s => Directory.Exists(s))) {
                        var f1 = filePaths[0];
                        var f2 = filePaths[1];

                        var f1v = IsEnabledVideoFilePath(f1);
                        var f1m = IsEnabledMsgFilePath(f1);
                        var f2v = IsEnabledVideoFilePath(f2);
                        var f2m = IsEnabledMsgFilePath(f2);

                        if((f1v && f2m) || (f2v && f1m)) {
                            e.Effects = DragDropEffects.Move;
                        }
                    }
                }
            }

            Debug.WriteLine(e.Effects);
        }

        private void PlayDragDop(UIElement sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;

            var filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
            if(filePaths.Length == 1) {
                var filePath = filePaths[0];
                if(Directory.Exists(filePath)) {
                    var files = Directory.EnumerateFiles(filePath)
                        .Where(f => IsEnabledVideoFilePath(f) || IsEnabledMsgFilePath(f))
                        .ToArray()
                    ;

                    var video = files.FirstOrDefault(f => IsEnabledVideoFilePath(f));
                    var msg = files.FirstOrDefault(f => IsEnabledMsgFilePath(f));

                    if(!string.IsNullOrWhiteSpace(video)) {
                        PlayInputVideoSourceFilePath = video;
                    }
                    if(!string.IsNullOrWhiteSpace(msg)) {
                        PlayInputCommentSourceFilePath = msg;
                    }
                } else {
                    if(IsEnabledVideoFilePath(filePath)) {
                        PlayInputVideoSourceFilePath = filePath;
                    } else if(IsEnabledMsgFilePath(filePath)) {
                        PlayInputCommentSourceFilePath = filePath;
                    }
                }
            } else if(filePaths.Length == 2) {
                var f1 = filePaths[0];
                var f2 = filePaths[1];

                var f1v = IsEnabledVideoFilePath(f1);
                var f1m = IsEnabledMsgFilePath(f1);
                var f2v = IsEnabledVideoFilePath(f2);
                var f2m = IsEnabledMsgFilePath(f2);

                if(f1v && f2m) {
                    PlayInputVideoSourceFilePath = f1;
                    PlayInputCommentSourceFilePath = f2;
                } else if(f2v && f1m) {
                    PlayInputVideoSourceFilePath = f2;
                    PlayInputCommentSourceFilePath = f1;
                }
            }

        }
        
        #endregion

        #region SmileVideoCustomManagerViewModelBase

        public override Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan, bool force)
        {
            return GarbageCollectionDummyResult;
        }

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override void InitializeView(MainWindow view)
        { }

        public override void UninitializeView(MainWindow view)
        { }

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return Enumerable.Empty<ManagerViewModelBase>();
        }

        protected override void HideViewCore()
        { }

        protected override void ShowViewCore()
        { }

        #endregion
    }
}
