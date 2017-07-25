using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ReadOnly;
using ContentTypeTextNet.MnMn.Library.Bridging.Model;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.IdleTalk;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.IdleTalk.Mutter;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.IdleTalk
{
    public class IdleTalkMediator: MediatorCustomBase
    {
        public IdleTalkMediator(Mediator mediator)
            : base(mediator, Constants.IdleTalkUriListPath, Constants.IdleTalkUriParametersListPath, Constants.IdleTalkRequestHeadersListPath, Constants.IdleTalkRequestParametersListPath, Constants.IdleTalkRequestMappingsListPath, Constants.IdleTalkExpressionsPath)
        {
            MutterMediator = new IdleTalkMutterMediator(Mediator);
        }


        #region property

        IdleTalkMutterMediator MutterMediator { get; }

        #endregion

        #region MediatorCustomBase

        protected override string ScriptDirectoryPath { get; } = Path.Combine(Constants.SpaghettiDirectoryPath, Constants.ServiceName, Constants.ServiceIdleTalkName);

        protected override IEnumerable<string> GetCustomKeys()
        {
            return GetMediatorKeys(typeof(IdleTalkMediatorKey));
        }

        internal override object RequestShowView(ShowViewRequestModel request)
        {
            switch(request.ServiceType) {
                case ServiceType.IdleTalk:
                    throw new NotImplementedException();

                case ServiceType.IdleTalkMutter:
                    return MutterMediator.RequestShowView(request);

                default:
                    throw new NotImplementedException();
            }
        }

        internal override void SetManager(ServiceType serviceType, ManagerPackModelBase managerPack)
        {
            switch(serviceType) {
                case ServiceType.IdleTalk:
                    throw new NotImplementedException();

                case ServiceType.IdleTalkMutter:
                    MutterMediator.SetManager(serviceType, managerPack);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public override ResponseModel Request(RequestModel request)
        {
            switch(request.ServiceType) {
                case ServiceType.IdleTalk:
                    throw new NotImplementedException();

                case ServiceType.IdleTalkMutter:
                    return MutterMediator.Request(request);

                default:
                    ThrowNotSupportRequest(request);
                    throw new NotImplementedException();
            }
        }

        public override IReadOnlyUriResult GetUri(string key, IDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.IdleTalk:
                    return GetUriCore(key, replaceMap, serviceType);

                case ServiceType.IdleTalkMutter:
                    return MutterMediator.GetUri(key, replaceMap, serviceType);

                default:
                    ThrowNotSupportGetUri(key, replaceMap, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override string ConvertUri(string key, string uri, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.IdleTalk:
                    return ConvertUriCore(key, uri, serviceType);

                case ServiceType.IdleTalkMutter:
                    return MutterMediator.ConvertUri(key, uri, serviceType);

                default:
                    ThrowNotSupportConvertUri(key, uri, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IDictionary<string, string> GetRequestHeader(string key, IDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.IdleTalk:
                    return GetRequestHeaderCore(key, replaceMap, serviceType);

                case ServiceType.IdleTalkMutter:
                    return MutterMediator.GetRequestHeader(key, replaceMap, serviceType);

                default:
                    ThrowNotSupportGetRequestHeader(key, replaceMap, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IDictionary<string, string> ConvertRequestHeader(string key, IDictionary<string, string> requestHeaders, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.IdleTalk:
                    return ConvertRequestHeaderCore(key, requestHeaders, serviceType);

                case ServiceType.IdleTalkMutter:
                    return MutterMediator.ConvertRequestHeader(key, requestHeaders, serviceType);

                default:
                    ThrowNotSupportConvertRequestHeader(key, requestHeaders, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IDictionary<string, string> GetRequestParameter(string key, IDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.IdleTalk:
                    return GetRequestParameterCore(key, replaceMap, serviceType);

                case ServiceType.IdleTalkMutter:
                    return MutterMediator.GetRequestParameter(key, replaceMap, serviceType);

                default:
                    ThrowNotSupportGetRequestParameter(key, replaceMap, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IReadOnlyMappingResult GetRequestMapping(string key, IDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.IdleTalk:
                    return GetRequestMappingCore(key, replaceMap, serviceType);

                case ServiceType.IdleTalkMutter:
                    return MutterMediator.GetRequestMapping(key, replaceMap, serviceType);

                default:
                    ThrowNotSupportGetRequestMapping(key, replaceMap, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IReadOnlyExpression GetExpression(string key, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.IdleTalk:
                    return GetExpressionCore(key, serviceType);

                case ServiceType.IdleTalkMutter:
                    return MutterMediator.GetExpression(key, serviceType);

                default:
                    ThrowNotSupportGetExpression(key, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IReadOnlyExpression GetExpression(string key, string id, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.IdleTalk:
                    return GetExpressionCore(key, id, serviceType);

                case ServiceType.IdleTalkMutter:
                    return MutterMediator.GetExpression(key, id, serviceType);

                default:
                    ThrowNotSupportGetExpression(key, id, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IDictionary<string, string> ConvertRequestParameter(string key, IDictionary<string, string> requestParams, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.IdleTalk:
                    return ConvertRequestParameterCore(key, requestParams, serviceType);

                case ServiceType.IdleTalkMutter:
                    return MutterMediator.ConvertRequestParameter(key, requestParams, serviceType);

                default:
                    ThrowNotSupportConvertRequestParameter(key, requestParams, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override string ConvertRequestMapping(string key, string mapping, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.IdleTalk:
                    return ConvertRequestMappingCore(key, mapping, serviceType);

                case ServiceType.IdleTalkMutter:
                    return MutterMediator.ConvertRequestMapping(key, mapping, serviceType);

                default:
                    ThrowNotSupportConvertRequestMapping(key, mapping, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IReadOnlyCheck CheckResponseHeader(string key, Uri uri, HttpHeaders headers, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.IdleTalk:
                    return CheckResponseHeaderCore(key, uri, headers, serviceType);

                case ServiceType.IdleTalkMutter:
                    return MutterMediator.CheckResponseHeader(key, uri, headers, serviceType);

                default:
                    ThrowNotSupportCheckResponseHeader(key, uri, headers, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override void ConvertBinary(string key, Uri uri, Stream stream, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.IdleTalk:
                    ConvertBinaryCore(key, uri, stream, serviceType);
                    break;

                case ServiceType.IdleTalkMutter:
                    MutterMediator.ConvertBinary(key, uri, stream, serviceType);
                    break;

                default:
                    ThrowNotSupportConvertBinary(key, uri, stream, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override Encoding GetEncoding(string key, Uri uri, Stream stream, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.IdleTalk:
                    return GetEncodingCore(key, uri, stream, serviceType);

                case ServiceType.IdleTalkMutter:
                    return MutterMediator.GetEncoding(key, uri, stream, serviceType);

                default:
                    ThrowNotSupportGetEncoding(key, uri, stream, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override string ConvertString(string key, Uri uri, string text, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.IdleTalk:
                    return ConvertStringCore(key, uri, text, serviceType);

                case ServiceType.IdleTalkMutter:
                    return MutterMediator.ConvertString(key, uri, text, serviceType);

                default:
                    ThrowNotSupportConvertString(key, uri, text, serviceType);
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
