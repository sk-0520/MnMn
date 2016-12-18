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
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video
{
    public static class SmileVideoInformationUtility
    {
        #region define

        /// <summary>
        /// Flashを変換した後のファイル拡張子。
        /// </summary>
        public const string flashConvertedExtension = "flv";

        public const string launcherParameterVideoId = "video-id";
        public const string launcherParameterVideoTitle = "video-title";
        public const string launcherParameterVideoPage = "video-page";

        public const string customCopyFormatVideoId = "video-id";
        public const string customCopyFormatVideoTitle = "video-title";
        public const string customCopyFormatVideoPage = "video-page";

        #endregion

        #region function

        /// <summary>
        /// 動画IDに対応するキャッシュディレクトリパスを取得する。
        /// <para>ディレクトリの作成などはサポートしない。</para>
        /// </summary>
        /// <param name="mediation"></param>
        /// <param name="videoId"></param>
        /// <returns></returns>
        public static DirectoryInfo GetCacheDirectory(Mediation mediation, string videoId)
        {
            var parentDir = mediation.GetResultFromRequest<DirectoryInfo>(new RequestModel(RequestKind.CacheDirectory, ServiceType.SmileVideo));
            var cachedDirPath = Path.Combine(parentDir.FullName, videoId);

            return new DirectoryInfo(cachedDirPath);
        }

        public static FileInfo GetGetthumbinfoFile(Mediation mediation, string videoId)
        {
            var cacheDir = GetCacheDirectory(mediation, videoId);
            var fileName = PathUtility.CreateFileName(videoId, "thumb", "xml");
            return new FileInfo(Path.Combine(cacheDir.FullName, fileName));
        }

        public static RawSmileVideoThumbResponseModel LoadGetthumbinfo(FileInfo targetFile)
        {
            if(targetFile.Exists) {
                using(var stream = new FileStream(targetFile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                    return Getthumbinfo.ConvertFromRawData(stream);
                }
            }

            return null;
        }

        public static async Task<RawSmileVideoThumbResponseModel> LoadGetthumbinfoAsync(Mediation mediation, string videoId, CacheSpan thumbCacheSpan)
        {
            RawSmileVideoThumbResponseModel rawGetthumbinfo = null;
            var cachedThumbFile = GetGetthumbinfoFile(mediation, videoId);
            if(cachedThumbFile.Exists) {
                if(thumbCacheSpan.IsCacheTime(cachedThumbFile.LastWriteTime) && Constants.MinimumXmlFileSize <= cachedThumbFile.Length) {
                    //using(var stream = new FileStream(cachedThumbFile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                    //    rawGetthumbinfo = Getthumbinfo.ConvertFromRawData(stream);
                    //}
                    rawGetthumbinfo = LoadGetthumbinfo(cachedThumbFile);
                }
            }
            var isSave = false;
            if(rawGetthumbinfo == null || !SmileVideoGetthumbinfoUtility.IsSuccessResponse(rawGetthumbinfo)) {
                var getthumbinfo = new Getthumbinfo(mediation);
                rawGetthumbinfo = await getthumbinfo.LoadAsync(videoId);
                isSave = true;
            }

            // キャッシュ構築
            if(isSave) {
                try {
                    SerializeUtility.SaveXmlSerializeToFile(cachedThumbFile.FullName, rawGetthumbinfo);
                } catch(Exception ex) {
                    mediation.Logger.Warning(ex);
                }
            }

            return rawGetthumbinfo;
        }

        /// <summary>
        /// 外部プログラムで実行する際のパラメータを生成する。
        /// </summary>
        /// <param name="information"></param>
        /// <param name="baseParameter"></param>
        /// <returns></returns>
        public static string MakeLauncherParameter(SmileVideoInformationViewModel information, string baseParameter)
        {
            var map = new Dictionary<string, string>() {
                { launcherParameterVideoId, information.VideoId },
                { launcherParameterVideoTitle, information.Title },
                { launcherParameterVideoPage, information.WatchUrl.OriginalString },
            };
            var result = AppUtility.ReplaceString(baseParameter, map);

            return result;
        }

        public static Task<string> LoadWatchPageHtmlSource(HttpClient userAgent, Uri watchPageUri)
        {
            return userAgent.GetStringAsync(watchPageUri).ContinueWith(t => {
                return t.Result;
            });
        }

        public static Task<string> LoadWatchPageHtmlSource(ICreateHttpUserAgent userAgentCreator, Uri watchPageUri)
        {
            var userAgent = userAgentCreator.CreateHttpUserAgent();
            return LoadWatchPageHtmlSource(userAgent, watchPageUri);
        }

        public static string GetDmcRoleKey(string video, string audio)
        {
            return $"[{video}]-[{audio}]";
        }

        public static string GetCustomFormatedText(SmileVideoInformationViewModel information, string customCopyFormat)
        {
            var map = new Dictionary<string, string>() {
                { customCopyFormatVideoId, information.VideoId },
                { customCopyFormatVideoTitle, information.Title },
                { customCopyFormatVideoPage, information.WatchUrl.OriginalString },
            };
            var result = AppUtility.ReplaceString(customCopyFormat, map);

            return result;
        }

    #endregion
}
}
