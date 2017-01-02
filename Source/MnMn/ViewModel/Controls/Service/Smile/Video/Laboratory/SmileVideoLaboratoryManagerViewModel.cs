using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Data;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
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
        { }

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
