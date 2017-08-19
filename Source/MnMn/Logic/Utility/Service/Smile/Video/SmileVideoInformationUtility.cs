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
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.HalfBakedApi;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video;
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

        #endregion

        #region function

        static bool IsKeywordElement(IReadOnlyDefinedElement element, string extendKey)
        {
            return element.GetBooleanInExtends(extendKey);
        }

        public static bool IsCustomCopyElement(IReadOnlyDefinedElement element)
        {
            return IsKeywordElement(element, "custom-copy");
        }

        public static bool IsLauncherParameterElement(IReadOnlyDefinedElement element)
        {
            return IsKeywordElement(element, "parameter");
        }

        public static IDictionary<string, string> GetElementPropertyValue(SmileVideoInformationViewModel information, IEnumerable<IReadOnlyDefinedElement> elements)
        {
            var type = information.GetType();

            var result = elements
                .Select(i => new { Element = i, Property = type.GetProperty(i.Extends["property"]) })
                .Select(i => new { Element = i.Element, Property = i.Property, Value = i.Property.GetValue(information) })
                .ToDictionary(
                    i => i.Element.Key,
                    i => (string)i.Value
                )
            ;

            return result;
        }

        /// <summary>
        /// 動画IDに対応するキャッシュディレクトリパスを取得する。
        /// <para>ディレクトリの作成などはサポートしない。</para>
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="videoId"></param>
        /// <returns></returns>
        public static DirectoryInfo GetCacheDirectory(Mediator mediator, string videoId)
        {
            var parentDir = mediator.GetResultFromRequest<DirectoryInfo>(new RequestModel(RequestKind.CacheDirectory, ServiceType.SmileVideo));
            var cachedDirPath = Path.Combine(parentDir.FullName, videoId);

            return new DirectoryInfo(cachedDirPath);
        }

        public static FileInfo GetGetthumbinfoFile(Mediator mediator, string videoId)
        {
            var cacheDir = GetCacheDirectory(mediator, videoId);
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

        public static async Task<RawSmileVideoThumbResponseModel> LoadGetthumbinfoAsync(Mediator mediator, string videoId, CacheSpan thumbCacheSpan)
        {
            RawSmileVideoThumbResponseModel rawGetthumbinfo = null;
            var cachedThumbFile = GetGetthumbinfoFile(mediator, videoId);
            if(cachedThumbFile.Exists) {
                if(thumbCacheSpan.IsCacheTime(cachedThumbFile.LastWriteTime) && Constants.MinimumXmlFileSize <= cachedThumbFile.Length) {
                    rawGetthumbinfo = LoadGetthumbinfo(cachedThumbFile);
                }
            }
            var isSave = false;
            if(rawGetthumbinfo == null || !SmileVideoGetthumbinfoUtility.IsSuccessResponse(rawGetthumbinfo)) {
                var getthumbinfo = new Getthumbinfo(mediator);
                rawGetthumbinfo = await getthumbinfo.LoadAsync(videoId);

                if(SmileIdUtility.IsScrapingVideoId(rawGetthumbinfo.Thumb.VideoId, mediator)) {
                    // getthumbinfo で取得できない部分を補完する
                    var watchPage = new WatchPage(mediator);
                    watchPage.LoadGetthumbinfoAsync(rawGetthumbinfo.Thumb.WatchUrl);
                }

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

        /// <summary>
        /// 外部プログラムで実行する際のパラメータを生成する。
        /// </summary>
        /// <param name="information"></param>
        /// <param name="baseParameter"></param>
        /// <returns></returns>
        public static string MakeLauncherParameter(SmileVideoInformationViewModel information, IReadOnlySmileVideoKeyword keyword, string baseParameter)
        {
            var map = GetElementPropertyValue(information, keyword.Items.Where(i => IsLauncherParameterElement(i)));
            var result = AppUtility.ReplaceString(baseParameter, map);
            return result;
        }

        public static Task<string> LoadWatchPageHtmlSource(HttpClient userAgent, Uri watchPageUri)
        {
            return userAgent.GetStringAsync(watchPageUri).ContinueWith(t => {
                return t.Result;
            });
        }

        public static Task<string> LoadWatchPageHtmlSource(IHttpUserAgentCreator userAgentCreator, Uri watchPageUri)
        {
            var userAgent = userAgentCreator.CreateHttpUserAgent();
            return LoadWatchPageHtmlSource(userAgent, watchPageUri);
        }

        public static string GetDmcRoleKey(string video, string audio)
        {
            return $"[{video}]-[{audio}]";
        }

        public static string GetCustomFormatedText(SmileVideoInformationViewModel information, IReadOnlySmileVideoKeyword keyword, string customCopyFormat)
        {
            var map = GetElementPropertyValue(information, keyword.Items.Where(i => IsLauncherParameterElement(i)));
            var result = AppUtility.ReplaceString(customCopyFormat, map);
            return result;
        }

        /// <summary>
        /// 再生中・ダウンロード中のデータは再生できないものとする。
        /// </summary>
        /// <param name="videoInformation"></param>
        /// <returns></returns>
        public static bool CheckCanPlay(SmileVideoInformationViewModel videoInformation, ILogger logger)
        {
            if(videoInformation.IsDownloading) {
                logger.Information($"downloading: {videoInformation.VideoId}");
                return false;
            }
            if(videoInformation.IsPlaying) {
                logger.Information($"playing: {videoInformation.VideoId}");
                return false;
            }

            return true;
        }

        public static bool IsEquals(SmileVideoInformationViewModel a, SmileVideoInformationViewModel b)
        {
            if(a == null) {
                throw new ArgumentNullException(nameof(a));
            }
            if(b == null) {
                throw new ArgumentNullException(nameof(b));
            }

            if(a == b) {
                return true;
            }

            return SmileVideoVideoItemUtility.IsEquals(a.ToVideoItemModel(), b.ToVideoItemModel());
        }

        #endregion
    }
}
