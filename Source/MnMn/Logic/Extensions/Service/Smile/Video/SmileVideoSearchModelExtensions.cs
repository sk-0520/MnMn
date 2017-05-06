using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.IF.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Extensions.Service.Smile.Video
{
    public static class SmileVideoSearchModelExtensions
    {
        #region function

        /// <summary>
        /// 公式検索・コンテンツサーチを透過的に切り替え。
        /// </summary>
        public static ISmileVideoSearchDefine GetSearchTypeDefine(this SmileVideoSearchModel model, SmileVideoSearchType searchType)
        {
            switch(searchType) {
                case Define.Service.Smile.Video.SmileVideoSearchType.Official:
                    return model.Official;

                case Define.Service.Smile.Video.SmileVideoSearchType.Contents:
                    return model.Contents;

                default:
                    throw new NotImplementedException();
            }
        }

        public static ISmileVideoSearchDefine GetDefaultSearchTypeDefine(this SmileVideoSearchModel model)
        {
            return GetSearchTypeDefine(model, Constants.ServiceSmileVideoSearchType);
        }

        #endregion
    }
}
