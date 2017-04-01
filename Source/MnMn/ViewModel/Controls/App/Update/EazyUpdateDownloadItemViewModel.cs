using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App.Update
{
    public class EazyUpdateDownloadItemViewModel : ViewModelBase, IDownloadItem
    {
        #region variable

        Uri _downloadUri;
        LoadState _downLoadState;

        long _downloadedSize;

        #endregion

        public EazyUpdateDownloadItemViewModel(Mediation mediation, EazyUpdateModel model)
        {
            Mediation = mediation;
            Model = model;

            var eazyUpdateDir = VariableConstants.GetEazyUpdateDirectory();
            ArchivePath = Path.Combine(eazyUpdateDir.FullName, PathUtility.AppendExtension(Constants.GetTimestampFileName(Model.Timestamp), "zip"));
        }

        #region property

        Mediation Mediation { get; }
        EazyUpdateModel Model { get; }

        CancellationTokenSource Cancellation { get; set; }

        string ArchivePath { get; }

        #endregion

        #region function

        void ExpandArchive()
        {
            // アーカイブ展開
            using(var archiveStream = new ZipArchive(new FileStream(ArchivePath, FileMode.Open, FileAccess.Read, FileShare.Read), ZipArchiveMode.Read)) {
                foreach(var entry in archiveStream.Entries) {
                    var path = Path.Combine(Constants.AssemblyRootDirectoryPath, entry.FullName);
                    FileUtility.MakeFileParentDirectory(path);
                    Mediation.Logger.Trace($"expand: {path}");
                    using(var entryStream = entry.Open())
                    using(var stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None)) {
                        entryStream.CopyTo(stream);
                    }
                }
            }
            var setting = Mediation.GetResultFromRequest<AppSettingModel>(new RequestModel(RequestKind.Setting, ServiceType.Application));
            setting.RunningInformation.LastEazyUpdateTimestamp = Model.Timestamp;

            Mediation.Order(new OrderModel(OrderKind.Reboot, ServiceType.Application));
        }

        #endregion

        #region IDownloadItem

        public Uri DownloadUri
        {
            get { return this._downloadUri; }
            private set { SetVariableValue(ref this._downloadUri, value); }
        }

        public LoadState DownLoadState
        {
            get { return this._downLoadState; }
            private set { SetVariableValue(ref this._downLoadState, value); }
        }

        public bool EnabledTotalSize => true;

        public DownloadUnit DownloadUnit => DownloadUnit.Count;

        public long DownloadTotalSize => Model.Targets.Count;

        public long DownloadedSize
        {
            get { return this._downloadedSize; }
            private set { SetVariableValue(ref this._downloadedSize, value); }
        }

        public bool CanRestart => false;

        public IProgress<double> DownloadingProgress { get; set; }

        public ImageSource Image => throw new NotImplementedException();

        public ICommand OpenDirectoryCommand => CreateCommand(o => { }, o => false);

        public ICommand ExecuteTargetCommand => CreateCommand(o => { }, o => false);

        public ICommand AutoExecuteTargetCommand => CreateCommand(o => ExpandArchive(), o => DownLoadState == LoadState.Loaded);

        public async Task StartAsync()
        {
            DownLoadState = LoadState.Preparation;

            Cancellation = new CancellationTokenSource();
            DownloadedSize = 0;
            DownloadingProgress?.Report(0);

            using(var host = new HttpUserAgentHost())
            using(var userAgent = host.CreateHttpUserAgent()) {
                userAgent.Timeout = Constants.ArchiveEazyUpdateTimeout;
                using(var archiveStream = new ZipArchive(new FileStream(ArchivePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read), ZipArchiveMode.Create)) {
                    DownLoadState = LoadState.Loading;

                    foreach(var tartget in Model.Targets) {
                        if(Cancellation.IsCancellationRequested) {
                            DownLoadState = LoadState.Failure;
                            return;
                        }

                        var entry = archiveStream.CreateEntry(tartget.Extends["expand"]);
                        using(var entryStream = entry.Open()) {
                            Mediation.Logger.Debug(tartget.Key);
                            var response = await userAgent.GetAsync(tartget.Key);
                            if(response.IsSuccessStatusCode) {
                                var stream = await response.Content.ReadAsStreamAsync();
                                await stream.CopyToAsync(entryStream);
                                DownloadedSize += 1;
                                DownloadingProgress?.Report(DownloadedSize / (double)DownloadTotalSize);
                            } else {
                                Mediation.Logger.Error(response.ToString());
                                DownLoadState = LoadState.Failure;
                                return;
                            }
                        }
                        //await Task.Delay(Constants.ArchiveEazyUpdateWaitTime);
                    }
                }
            }

            DownloadingProgress?.Report(1);
            DownLoadState = LoadState.Loaded;
        }

        public void Cancel()
        {
            Cancellation.Cancel();
        }

        #endregion
    }
}
