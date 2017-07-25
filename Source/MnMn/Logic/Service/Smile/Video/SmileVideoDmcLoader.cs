using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video
{
    /// <summary>
    /// DMC のゆりかごから墓場までを担当する。
    /// </summary>
    public class SmileVideoDmcLoader: DisposeFinalizeBase
    {
        public SmileVideoDmcLoader(string videoId, Uri uri, Mediator mediation)
        {
            VideoId = videoId;
            ApiUri = uri;
            Mediation = mediation;
            Dmc = new Dmc(Mediation);
        }

        #region property

        public string VideoId { get; }

        public Uri ApiUri { get; }

        Mediator Mediation { get; }

        Dmc Dmc { get; }

        public RawSmileVideoDmcObjectModel UsingData { get; private set; }

        RawSmileVideoDmcContentSrcIdModel DmcMultiplexer { get { return UsingData.Data.Session.ContentSrcIdSets.First().SrcIdToMultiplexers.First(); } }
        public string VideoSource { get { return DmcMultiplexer.VideoSrcIds.First(); } }
        public string AudioSource { get { return DmcMultiplexer.AudioSrcIds.First(); } }
        public string FileExtension { get { return UsingData.Data.Session.Protocol.HttpParameters.First().Parameters.First().FileExtension; } }

        int PollingCount { get; set; }

        Timer PollingTimer { get; set; }

        public bool NowPolling { get; private set; }
        public bool IsStopped { get; private set; }

        #endregion

        #region function

        /// <summary>
        ///
        /// </summary>
        /// <param name="baseData"></param>
        /// <param name="saveFile">返ってきたデータを保存するファイル。nullだと保存しない。</param>
        /// <returns>ダウンロード用URI</returns>
        public Task<Uri> StartAsync(RawSmileVideoDmcObjectModel baseData, FileInfo saveFile)
        {
            if(UsingData != null) {
                throw new InvalidOperationException(nameof(UsingData));
            }

            if(baseData == null) {
                throw new ArgumentNullException(nameof(baseData));
            }

            return Dmc.LoadAsync(ApiUri, baseData).ContinueWith(t => {
                var model = t.Result;

                if(saveFile != null) {
                    SerializeUtility.SaveXmlSerializeToFile(saveFile.FullName, model);
                }

                if(SmileVideoDmcObjectUtility.IsSuccessResponse(model)) {
                    UsingData = model;
                    return new Uri(UsingData.Data.Session.ContentUri);
                }

                Mediation.Logger.Warning($"{VideoId}: {model.Meta.Status}, {model.Meta.Message}");
                return null;
            });
        }

        Task PollingCoreAsync()
        {
            if(UsingData == null) {
                throw new InvalidOperationException(nameof(UsingData));
            }

            Mediation.Logger.Information($"{VideoId}: polling {PollingCount++}");

            UsingData.Data.Session.ModifiedTime = SmileVideoDmcObjectUtility.ConvertRawSessionTime(DateTime.Now);

            return Dmc.ReloadAsync(ApiUri, UsingData).ContinueWith(t => {
                var model = t.Result;
                UsingData = model;
            });
        }

        Task PollingAsync()
        {
            PollingTimer.Stop();

            if(NowPolling) {
                return PollingCoreAsync().ContinueWith(_ => {
                    if(NowPolling) {
                        // PollingTimer.Interval の再設定を UsingData から行うべき
                        PollingTimer.Start();
                    }
                });
            }

            return Task.CompletedTask;
        }

        public void StartPolling()
        {
            if(UsingData == null) {
                throw new InvalidOperationException(nameof(UsingData));
            }

            Mediation.Logger.Information($"{VideoId}: start polling");

            NowPolling = true;

            PollingTimer = new Timer();
            PollingTimer.Elapsed += PollingTimer_Tick;

            // UsingData から取得すべき
            PollingTimer.Interval = Constants.ServiceSmileVideoDownloadDmcPollingWaitTime.TotalMilliseconds;

            PollingTimer.Start();
        }

        public void StopPolling()
        {
            Mediation.Logger.Information($"{VideoId}: stop polling");

            NowPolling = false;
            if(PollingTimer != null) {
                PollingTimer.Stop();
                PollingTimer.Elapsed -= PollingTimer_Tick;
            }
        }

        public Task StopAsync()
        {
            if(IsStopped) {
                return Task.CompletedTask;
            }

            if(NowPolling) {
                StopPolling();
            }

            IsStopped = true;
            return Dmc.CloseAsync(ApiUri, UsingData);
        }

        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                UsingData = null;
                if(PollingTimer!= null) {
                    PollingTimer.Elapsed -= PollingTimer_Tick;
                    PollingTimer = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        void PollingTimer_Tick(object sender, EventArgs e)
        {
            var task = PollingAsync();
            task.ConfigureAwait(false);
        }

    }
}
