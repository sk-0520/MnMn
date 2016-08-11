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
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video
{
    public static class SmileVideoInformationUtility
    {
        #region function

        /// <summary>
        /// 動画IDに対応するキャッシュディレクトリを取得する。
        /// <para>存在しない場合は生成される。</para>
        /// </summary>
        /// <param name="mediation"></param>
        /// <param name="videoId"></param>
        /// <returns></returns>
        public static DirectoryInfo GetCacheDirectory(Mediation mediation, string videoId)
        {
            var parentDir = mediation.GetResultFromRequest<DirectoryInfo>(new RequestModel(RequestKind.CacheDirectory, ServiceType.SmileVideo));
            var cachedDirPath = Path.Combine(parentDir.FullName, videoId);

            return Directory.CreateDirectory(cachedDirPath);
        }

        public static async Task<RawSmileVideoThumbResponseModel> LoadGetthumbinfoAsync(Mediation mediation, string videoId, CacheSpan thumbCacheSpan)
        {
            var cacheDir = SmileVideoInformationUtility.GetCacheDirectory(mediation, videoId);

            RawSmileVideoThumbResponseModel rawGetthumbinfo = null;
            var fileName = PathUtility.CreateFileName(videoId, "thumb", "xml");
            var cachedThumbFilePath = Path.Combine(cacheDir.FullName, fileName);
            if(File.Exists(cachedThumbFilePath)) {
                var fileInfo = new FileInfo(cachedThumbFilePath);
                if(thumbCacheSpan.IsCacheTime(fileInfo.LastWriteTime) && Constants.MinimumXmlFileSize <= fileInfo.Length) {
                    using(var stream = new FileStream(cachedThumbFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                        rawGetthumbinfo = Getthumbinfo.ConvertFromRawData(stream);
                    }
                }
            }
            if(rawGetthumbinfo == null || !SmileVideoGetthumbinfoUtility.IsSuccessResponse(rawGetthumbinfo)) {
                var getthumbinfo = new Getthumbinfo(mediation);
                rawGetthumbinfo = await getthumbinfo.LoadAsync(videoId);
            }

            // キャッシュ構築
            try {
                SerializeUtility.SaveXmlSerializeToFile(cachedThumbFilePath, rawGetthumbinfo);
            } catch(Exception ex) {
                mediation.Logger.Warning(ex);
            }

            return rawGetthumbinfo;

        }

        #endregion
    }
}
