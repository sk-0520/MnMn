using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Live.Api;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Live.Raw;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Live;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Live
{
    public static class SmileLiveInformationUtility
    {
        #region define

        public const string launcherParameterVideoId = "video-id";
        public const string launcherParameterVideoTitle = "video-title";
        public const string launcherParameterVideoPage = "video-page";

        #endregion

        #region function

        /// <summary>
        /// 外部プログラムで実行する際のパラメータを生成する。
        /// </summary>
        /// <param name="information"></param>
        /// <param name="baseParameter"></param>
        /// <returns></returns>
        public static string MakeLauncherParameter(SmileLiveInformationViewModel information, string baseParameter)
        {
            var map = new Dictionary<string, string>() {
                { launcherParameterVideoId, information.Id },
                { launcherParameterVideoTitle, information.Title },
                { launcherParameterVideoPage, information.WatchUrl.OriginalString },
            };
            var result = AppUtility.ReplaceString(baseParameter, map);

            return result;
        }

        public static DirectoryInfo GetCacheDirectory(Mediator mediator, string videoId)
        {
            var parentDir = mediator.GetResultFromRequest<DirectoryInfo>(new RequestModel(RequestKind.CacheDirectory, ServiceType.SmileLive));
            var cachedDirPath = Path.Combine(parentDir.FullName, videoId);

            return new DirectoryInfo(cachedDirPath);
        }


        public static FileInfo GetGetthumbinfoFile(Mediator mediator, string videoId)
        {
            var cacheDir = GetCacheDirectory(mediator, videoId);
            var fileName = PathUtility.CreateFileName(videoId, "player-status", "xml");
            return new FileInfo(Path.Combine(cacheDir.FullName, fileName));
        }

        public static RawSmileLiveGetPlayerStatusModel LoadGetthumbinfo(FileInfo targetFile)
        {
            if(targetFile.Exists) {
                using(var stream = new FileStream(targetFile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                    return GetPlayerStatus.ConvertFromRawStream(stream);
                }
            }

            return null;
        }


        public static async Task<RawSmileLiveGetPlayerStatusModel> LoadGetPlayerStatusAsync(Mediator mediator, string liveId, CacheSpan thumbCacheSpan)
        {
            RawSmileLiveGetPlayerStatusModel rawGetthumbinfo = null;
            var cachedThumbFile = GetGetthumbinfoFile(mediator, liveId);
            if(cachedThumbFile.Exists) {
                if(thumbCacheSpan.IsCacheTime(cachedThumbFile.LastWriteTime) && Constants.MinimumXmlFileSize <= cachedThumbFile.Length) {
                    rawGetthumbinfo = LoadGetthumbinfo(cachedThumbFile);
                }
            }
            var isSave = false;
            if(rawGetthumbinfo == null || !SmileLiveGetPlayerStatusUtility.IsSuccessResponse(rawGetthumbinfo)) {
                var getPlayerStatus = new GetPlayerStatus(mediator);
                rawGetthumbinfo = await getPlayerStatus.LoadAsync(liveId);
                isSave = true;
            }

            // キャッシュ構築
            if(isSave) {
                try {
                    SerializeUtility.SaveXmlSerializeToFile(cachedThumbFile.FullName, rawGetthumbinfo);
                } catch(Exception ex) {
                    mediator.Logger.Warning(ex);
                }
            }

            return rawGetthumbinfo;
        }


        #endregion
    }
}
