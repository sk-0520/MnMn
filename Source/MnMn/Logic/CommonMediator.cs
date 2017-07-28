using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    public class CommonMediator : CustomMediatorBase
    {
        public CommonMediator(Mediator mediator)
            : base(mediator, Constants.CommonUriListPath, Constants.CommonUriParametersListPath, Constants.CommonRequestHeadersListPath, Constants.CommonRequestParametersListPath, Constants.CommonRequestMappingsListPath, Constants.CommonExpressionsPath)
        { }

        #region CustomMediatorBase

        protected override string ScriptDirectoryPath { get; } = Path.Combine(Constants.SpaghettiDirectoryPath, Constants.ServiceCommonName);

        protected override IEnumerable<string> GetCustomKeys()
        {
            return GetMediatorKeys(typeof(CommonMediatorKey));
        }

        internal override object RequestShowView(ShowViewRequestModel requet)
        {
            throw new NotSupportedException();
        }

        internal override void SetManager(ServiceType serviceType, ManagerPackModelBase managerPack)
        {
            throw new NotSupportedException();
        }

        public override ResponseModel Request(RequestModel request)
        {
            if(request.ServiceType != ServiceType.Common) {
                ThrowNotSupportRequest(request);
            }

            throw new NotImplementedException();
        }

        public override IReadOnlyUriResult GetUri(string key, IDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            if(serviceType != ServiceType.Common) {
                ThrowNotSupportGetUri(key, replaceMap, serviceType);
            }

            return GetUriCore(key, replaceMap, serviceType);
        }

        public override string ConvertUri(string key, string uri, ServiceType serviceType)
        {
            if(serviceType != ServiceType.Common) {
                ThrowNotSupportConvertUri(key, uri, serviceType);
            }

            return ConvertUriCore(key, uri, serviceType);
        }

        public override IDictionary<string, string> GetRequestHeader(string key, IDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            if(serviceType != ServiceType.Common) {
                ThrowNotSupportGetRequestHeader(key, replaceMap, serviceType);
            }

            return GetRequestHeaderCore(key, replaceMap, serviceType);
        }

        public override IDictionary<string, string> ConvertRequestHeader(string key, IDictionary<string, string> requestHeaders, ServiceType serviceType)
        {
            if(serviceType != ServiceType.Common) {
                ThrowNotSupportConvertRequestHeader(key, requestHeaders, serviceType);
            }

            return ConvertRequestHeaderCore(key, requestHeaders, serviceType);
        }

        public override IDictionary<string, string> GetRequestParameter(string key, IDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            if(serviceType != ServiceType.Common) {
                ThrowNotSupportGetRequestParameter(key, replaceMap, serviceType);
            }

            return GetRequestParameterCore(key, replaceMap, serviceType);
        }

        public override IReadOnlyMappingResult GetRequestMapping(string key, IDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            if(serviceType != ServiceType.Common) {
                ThrowNotSupportGetRequestMapping(key, replaceMap, serviceType);
            }

            return GetRequestMappingCore(key, replaceMap, serviceType);
        }

        public override IReadOnlyExpression GetExpression(string key, ServiceType serviceType)
        {
            if(serviceType != ServiceType.Common) {
                ThrowNotSupportGetExpression(key, serviceType);
            }

            return GetExpressionCore(key, serviceType);
        }

        public override IReadOnlyExpression GetExpression(string key, string id, ServiceType serviceType)
        {
            if(serviceType != ServiceType.Common) {
                ThrowNotSupportGetExpression(key, id, serviceType);
            }

            return GetExpressionCore(key, id, serviceType);
        }

        public override IDictionary<string, string> ConvertRequestParameter(string key, IDictionary<string, string> requestParams, ServiceType serviceType)
        {
            if(serviceType != ServiceType.Common) {
                ThrowNotSupportConvertRequestParameter(key, requestParams, serviceType);
            }

            return ConvertRequestParameterCore(key, requestParams, serviceType);
        }

        public override string ConvertRequestMapping(string key, string mapping, ServiceType serviceType)
        {
            if(serviceType != ServiceType.Common) {
                ThrowNotSupportConvertRequestMapping(key, mapping, serviceType);
            }

            return ConvertRequestMappingCore(key, mapping, serviceType);
        }

        public override IReadOnlyCheck CheckResponseHeader(string key, Uri uri, HttpHeaders headers, ServiceType serviceType)
        {
            return CheckResponseHeaderCore(key, uri, headers, serviceType);
        }

        public override void ConvertBinary(string key, Uri uri, Stream stream, ServiceType serviceType)
        {
            ConvertBinaryCore(key, uri, stream, serviceType);
        }

        public override Encoding GetEncoding(string key, Uri uri, Stream stream, ServiceType serviceType)
        {
            return GetEncodingCore(key, uri, stream, serviceType);
        }

        public override string ConvertString(string key, Uri uri, string text, ServiceType serviceType)
        {
            return ConvertStringCore(key, uri, text, serviceType);
        }


        #endregion
    }
}
