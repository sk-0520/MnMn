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
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App.Update
{
    public class LightweightUpdateDownloadItemViewModel : ViewModelBase, IDownloadItem
    {
        #region define

        static object uniqueItemObject = new object();

        #endregion

        #region variable

        Uri _downloadUri;
        DownloadState _downLoadState;

        long _downloadedSize;

        string _downloadTitle;

        #endregion

        public LightweightUpdateDownloadItemViewModel(Mediation mediation, LightweightUpdateModel model)
        {
            Mediation = mediation;
            Model = model;

            var lightweightUpdateDir = VariableConstants.GetLightweightUpdateDirectory();
            ArchivePath = Path.Combine(lightweightUpdateDir.FullName, PathUtility.AppendExtension(Constants.GetTimestampFileName(Model.Timestamp), "zip"));

            NetworkSetting = Mediation.GetNetworkSetting();
        }

        #region property

        Mediation Mediation { get; }
        LightweightUpdateModel Model { get; }
        IReadOnlyNetworkSetting NetworkSetting { get; }

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
            setting.RunningInformation.LightweightUpdateTimestamp = Model.Timestamp;

            Mediation.Order(new OrderModel(OrderKind.Reboot, ServiceType.Application));
        }

        void SetDownloadTitle(string expandPath)
        {
            if(string.IsNullOrEmpty(expandPath)) {
                DownloadTitle = Properties.Resources.String_App_LightweightUpdate;
            } else {
                DownloadTitle = $"{Properties.Resources.String_App_LightweightUpdate}: {expandPath}";
            }
        }


        #endregion

        #region IDownloadItem

        public Uri DownloadUri
        {
            get { return this._downloadUri; }
            private set { SetVariableValue(ref this._downloadUri, value); }
        }

        public string DownloadTitle
        {
            get { return this._downloadTitle; }
            set { SetVariableValue(ref this._downloadTitle, value); }
        }

        public DownloadState DownloadState
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

        public object DownloadUniqueItem { get; } = uniqueItemObject;

        public IProgress<double> DownloadingProgress { get; set; }

        public ImageSource Image
        {
            get
            {
                var image= new BitmapImage();
                using(Initializer.BeginInitialize(image)) {
                    image.UriSource = SharedConstants.GetEntryUri("Resources/MnMn-Update_Lightweight.png");
                }

                return image;
            }
        }

        public ICommand OpenDirectoryCommand => CreateCommand(o => { }, o => false);

        public ICommand ExecuteTargetCommand => CreateCommand(o => { }, o => false);

        public ICommand AutoExecuteTargetCommand => CreateCommand(o => ExpandArchive(), o => DownloadState == DownloadState.Completed);

        public async Task StartAsync()
        {
            DownloadState = DownloadState.Preparation;

            Cancellation = new CancellationTokenSource();
            DownloadedSize = 0;
            DownloadingProgress?.Report(0);
            SetDownloadTitle(null);

            using(var host = new HttpUserAgentHost(NetworkSetting, Mediation.Logger))
            using(var userAgent = host.CreateHttpUserAgent()) {
                userAgent.Timeout = Constants.ArchiveLightweightUpdateTimeout;
                using(var archiveStream = new ZipArchive(new FileStream(ArchivePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read), ZipArchiveMode.Create)) {
                    DownloadState = DownloadState.Downloading;

                    foreach(var tartget in Model.Targets) {
                        if(Cancellation.IsCancellationRequested) {
                            DownloadState = DownloadState.Failure;
                            return;
                        }
                        DownloadUri = new Uri(tartget.Key);
                        var expand = tartget.Extends["expand"];
                        SetDownloadTitle(expand);
                        var entry = archiveStream.CreateEntry(expand);
                        using(var entryStream = entry.Open()) {
                            Mediation.Logger.Debug(DownloadUri.ToString());
                            var response = await userAgent.GetAsync(DownloadUri);
                            if(response.IsSuccessStatusCode) {
                                var stream = await response.Content.ReadAsStreamAsync();
                                await stream.CopyToAsync(entryStream);
                                DownloadedSize += 1;
                                DownloadingProgress?.Report(DownloadedSize / (double)DownloadTotalSize);
                            } else {
                                Mediation.Logger.Error(response.ToString());
                                DownloadState = DownloadState.Failure;
                                return;
                            }
                        }
                        //await Task.Delay(Constants.ArchiveLightweightUpdateWaitTime);
                    }
                }
            }

            DownloadingProgress?.Report(1);
            DownloadState = DownloadState.Completed;
            SetDownloadTitle(null);
        }

        public void Cancel()
        {
            Cancellation.Cancel();
        }

        #endregion
    }
}
