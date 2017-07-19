using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video
{
    /// <summary>
    /// DMC のゆりかごから墓場までを担当する。
    /// </summary>
    public class SmileVideoDmcLoader: DisposeFinalizeBase
    {
        public SmileVideoDmcLoader(string videoId, Uri uri, RawSmileVideoDmcObjectModel baseData, Mediation mediation)
        {
            VideoId = videoId;
            Uri = uri;
            BaseData = baseData;
            Mediation = mediation;
        }

        #region property

        public string VideoId { get; }

        public Uri Uri { get; }

        RawSmileVideoDmcObjectModel BaseData { get; }

        Mediation Mediation { get; }

        #endregion

        #region function

        #endregion
    }
}
