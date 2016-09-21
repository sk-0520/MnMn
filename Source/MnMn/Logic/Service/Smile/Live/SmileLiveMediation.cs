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
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Live;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Live;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Live
{
    public class SmileLiveMediation: MediationCustomBase
    {
        public SmileLiveMediation(Mediation mediation, SmileLiveSettingModel setting)
            : base(mediation, Constants.SmileLiveUriListPath, Constants.SmileLiveUriParametersListPath, Constants.SmileVideoRequestParametersListPath, Constants.SmileVideoRequestMappingsListPath)
        {
            Category = SerializeUtility.LoadXmlSerializeFromFile<SmileLiveCategoryModel>(Constants.SmileLiveCategoryPath);
        }

        #region property

        SmileLiveCategoryModel Category { get; }

        #endregion

        #region function

        ResponseModel RequestCore(RequestModel request)
        {
            switch(request.RequestKind) {
                case RequestKind.CategoryDefine:
                    return new ResponseModel(request, Category);

                //case RequestKind.CacheData:
                //    return Request_CacheData((SmileVideoInformationCacheRequestModel)request);

                default:
                    throw new NotImplementedException();
            }
        }

        bool ValueConvertCore(out object outputValue, string inputKey, object inputValue, Type inputType, Type outputType, ServiceType serviceType)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region MediationBase

        internal override object RequestShowView(ShowViewRequestModel reque)
        {
            throw new NotImplementedException();
        }

        internal override void SetManager(ServiceType serviceType, ManagerPackModelBase managerPack)
        {
            throw new NotImplementedException();
        }

        public override ResponseModel Request(RequestModel request)
        {
            if(request.ServiceType != ServiceType.SmileLive) {
                ThrowNotSupportRequest(request);
            }

            return RequestCore(request);
        }

        public override UriResultModel GetUri(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileLive) {
                ThrowNotSupportGetUri(key, replaceMap, serviceType);
            }

            return GetUriCore(key, replaceMap, serviceType);
        }

        public override string ConvertUri(string uri, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileLive) {
                ThrowNotSupportConvertUri(uri, serviceType);
            }

            return uri;
        }

        public override IDictionary<string, string> GetRequestParameter(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileLive) {
                ThrowNotSupportGetRequestParameter(key, replaceMap, serviceType);
            }

            return GetRequestParameterCore(key, replaceMap, serviceType);
        }

        public override MappingResultModel GetRequestMapping(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileVideo) {
                ThrowNotSupportGetRequestMapping(key, replaceMap, serviceType);
            }

            return GetRequestMappingCore(key, replaceMap, serviceType);
        }

        public override IDictionary<string, string> ConvertRequestParameter(IReadOnlyDictionary<string, string> requestParams, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileVideo) {
                ThrowNotSupportConvertRequestParameter(requestParams, serviceType);
            }

            return (IDictionary<string, string>)requestParams;
        }

        public override string ConvertRequestMapping(string mapping, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileVideo) {
                ThrowNotSupportConvertRequestMapping(mapping, serviceType);
            }

            return mapping;
        }

        public override CheckModel CheckResponseHeader(Uri uri, HttpHeaders headers, ServiceType serviceType)
        {
            return CheckModel.Success();
        }

        public override byte[] ConvertBinary(Uri uri, byte[] binary, ServiceType serviceType)
        {
            return binary;
        }

        public override Encoding GetEncoding(Uri uri, byte[] binary, ServiceType serviceType)
        {
            return Encoding.UTF8;
        }

        public override string ConvertString(Uri uri, string text, ServiceType serviceType)
        {
            return text;
        }

        public override bool ConvertValue(out object outputValue, Type outputType, string inputKey, object inputValue, Type inputType, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileLive) {
                ThrowNotSupportValueConvert(inputKey, inputValue, inputType, outputType, serviceType);
            }

            return ValueConvertCore(out outputValue, inputKey, inputValue, inputType, outputType, serviceType);
        }

        #endregion

    }
}
