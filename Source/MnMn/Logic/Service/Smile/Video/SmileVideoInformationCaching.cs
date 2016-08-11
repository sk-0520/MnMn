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
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video
{
    /// <summary>
    /// 動画IDとそれに紐付く情報を保持。
    /// </summary>
    public class SmileVideoInformationCaching: Caching<string, SmileVideoInformationViewModel>
    {
        #region define

        const int UnOrdered = -1;

        #endregion

        public SmileVideoInformationCaching(Mediation mediation)
            : base(true)
        {
            Mediation = mediation;
        }

        #region property

        Mediation Mediation { get; }

        #endregion

        #region function

        async Task<RawSmileVideoThumbResponseModel> LoadGetthumbinfoAsync(string videoId, CacheSpan thumbCacheSpan)
        {
            var cacheDir = SmileVideoInformationUtility.GetCacheDirectory(Mediation, videoId);

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
                var getthumbinfo = new Getthumbinfo(Mediation);
                rawGetthumbinfo = await getthumbinfo.LoadAsync(videoId);
            }

            // キャッシュ構築
            try {
                SerializeUtility.SaveXmlSerializeToFile(cachedThumbFilePath, rawGetthumbinfo);
            } catch(Exception ex) {
                Mediation.Logger.Warning(ex);
            }

            return rawGetthumbinfo;

        }

        //public Task<SmileVideoInformationViewModel> LoadFromVideoIdAsync(string videoId, CacheSpan thumbCacheSpan)
        //{
        //    //return Get(videoId, async () => {
        //    //    var rawGetthumbinfo = await LoadGetthumbinfoAsync(videoId, thumbCacheSpan);
        //    //    return new SmileVideoInformationViewModel(Mediation, rawGetthumbinfo.Thumb, UnOrdered);
        //    //});
        //}

        #endregion
    }
}
