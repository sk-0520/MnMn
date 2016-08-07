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
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.IF.Compatibility;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    public abstract class MediationBase:
        DisposeFinalizeBase,
        IGetUri,
        IGetRequestParameter,
        ICommunication,
        IUriCompatibility,
        IResponseCompatibility,
        IConvertCompatibility
    {
        public MediationBase()
            : this(null, null, null, null)
        { }

        protected MediationBase(string uriListPath, string uriParametersPath, string requestParametersPath, string requestMappingsPath)
        {
            if(uriListPath != null) {
                UriList = SerializeUtility.LoadXmlSerializeFromFile<UrisModel>(uriListPath);
            } else {
                UriList = new UrisModel();
            }

            if(uriParametersPath != null) {
                UriParameterList = SerializeUtility.LoadXmlSerializeFromFile<ParametersModel>(uriParametersPath);
            } else {
                UriParameterList = new ParametersModel();
            }

            if(requestParametersPath != null) {
                RequestParameterList = SerializeUtility.LoadXmlSerializeFromFile<ParametersModel>(requestParametersPath);
            } else {
                RequestParameterList = new ParametersModel();
            }

            if(requestMappingsPath != null) {
                RequestMappingList = SerializeUtility.LoadXmlSerializeFromFile<MappingsModel>(requestMappingsPath);
            } else {
                RequestMappingList = new MappingsModel();
            }
        }

        #region property

        public static IReadOnlyDictionary<string, string> EmptyMap { get; } = new Dictionary<string, string>();

        protected UrisModel UriList { get; private set; }

        protected ParametersModel UriParameterList { get; private set; }

        protected ParametersModel RequestParameterList { get; private set; }

        protected MappingsModel RequestMappingList { get; }

        #endregion

        #region function

        protected static TModel LoadModelFromFile<TModel>(string path)
            where TModel: IModel, new()
        {
            if(path != null) {
                return SerializeUtility.LoadXmlSerializeFromFile<TModel>(path);
            } else {
                return new TModel();
            }
        }

        public static string ReplaceString(string s, IReadOnlyDictionary<string, string> map)
        {
            return s?.ReplaceRangeFromDictionary("${", "}", (Dictionary<string, string>)map) ?? string.Empty;
        }

        #region ThrowNotSupport

        protected void ThrowNotSupportRequest(RequestModel request)
        {
            throw new NotSupportedException($"{nameof(ICommunication)} => {nameof(request)}: {request}");
        }

        protected void ThrowNotSupportGetUri(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            throw new NotSupportedException($"{nameof(IGetUri)} => {nameof(key)}: {key}, {nameof(replaceMap)}: {replaceMap}, {nameof(serviceType)}: {serviceType}");
        }

        protected void ThrowNotSupportGetRequestParameter(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            throw new NotSupportedException($"{nameof(IGetRequestParameter)} => {nameof(key)}: {key}, {nameof(replaceMap)}: {replaceMap}, {nameof(serviceType)}: {serviceType}");
        }

        protected void ThrowNotSupportGetRequestMapping(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            throw new NotSupportedException($"{nameof(IGetRequestParameter)} => {nameof(key)}: {key}, {nameof(replaceMap)}: {replaceMap}, {nameof(serviceType)}: {serviceType}");
        }

        protected void ThrowNotSupportConvertUri(string uri, ServiceType serviceType)
        {
            throw new NotSupportedException($"{nameof(IUriCompatibility)} => {nameof(uri)}: {uri}, {nameof(serviceType)}: {serviceType}");
        }

        protected void ThrowNotSupportConvertRequestParameter(IReadOnlyDictionary<string, string> requestParams, ServiceType serviceType)
        {
            throw new NotSupportedException($"{nameof(IRequestCompatibility)} => {nameof(requestParams)}: {requestParams}, {nameof(serviceType)}: {serviceType}");
        }

        protected void ThrowNotSupportConvertRequestMapping(string mapping, ServiceType serviceType)
        {
            throw new NotSupportedException($"{nameof(IRequestCompatibility)} => {nameof(mapping)}: {mapping}, {nameof(serviceType)}: {serviceType}");
        }

        protected void ThrowNotSupportCheckResponseHeader(Uri uri, HttpHeaders headers, ServiceType serviceType)
        {
            throw new NotSupportedException($"{nameof(IResponseCompatibility)} => {nameof(uri)}: {uri}, {nameof(headers)}: {headers}, {nameof(serviceType)}: {serviceType}");
        }

        protected void ThrowNotSupportConvertBinary(Uri uri, byte[] binary, ServiceType serviceType)
        {
            throw new NotSupportedException($"{nameof(IResponseCompatibility)} => {nameof(uri)}: {uri}, {nameof(binary)}: {binary}, {nameof(serviceType)}: {serviceType}");
        }

        protected void ThrowNotSupportGetEncoding(Uri uri, byte[] binary, ServiceType serviceType)
        {
            throw new NotSupportedException($"{nameof(IResponseCompatibility)} => {nameof(uri)}: {uri}, {nameof(binary)}: {binary}, {nameof(serviceType)}: {serviceType}");
        }

        protected void ThrowNotSupportConvertString(Uri uri, string text, ServiceType serviceType)
        {
            throw new NotSupportedException($"{nameof(IResponseCompatibility)} => {nameof(uri)}: {uri}, {nameof(serviceType)}: {serviceType}");
        }

        protected void ThrowNotSupportValueConvert(string inputKey, object inputValue, Type inputType, Type outputType, ServiceType serviceType)
        {
            throw new NotSupportedException($"{nameof(IConvertCompatibility)} => {nameof(inputKey)}: {inputKey}, {nameof(inputValue)}: {inputValue}, {nameof(inputType)}: {inputType}, {nameof(outputType)}: {outputType}, {nameof(serviceType)}: {serviceType}");
        }

        #endregion

        protected UriItemModel GetUriItem(string key) => UriList.Items.FirstOrDefault(ui => ui.Key == key);

        string ToParameterEncodeTypeString(string s, ParameterEncode encodeType)
        {
            switch(encodeType) {
                case ParameterEncode.None:
                    return s;

                case ParameterEncode.Uri:
                    return HttpUtility.UrlEncode(s);

                default:
                    throw new NotImplementedException();
            }
        }

        string ToUriParameterString(ParameterItemModel pair, UriParameterType type, IReadOnlyDictionary<string, string> replaceMap)
        {
            Debug.Assert(type != UriParameterType.None);

            var val = ToParameterEncodeTypeString(ReplaceString(pair.Value, replaceMap), pair.Encode);
            if(pair.Force && string.IsNullOrEmpty(val)) {
                return string.Empty;
            }

            switch(type) {
                case UriParameterType.Query:
                    if(pair.HasKey) {
                        var key = ToParameterEncodeTypeString(ReplaceString(pair.Key, replaceMap), pair.Encode);
                        return $"{key}={val}";
                    } else {
                        return val;
                    }

                case UriParameterType.Hierarchy:
                    return val;

                case UriParameterType.PreSuffixes: {
                        var key = ToParameterEncodeTypeString(ReplaceString(pair.Key, replaceMap), pair.Encode);
                        var pre = ToParameterEncodeTypeString(ReplaceString(pair.Prefix, replaceMap), pair.Encode);
                        var bond = ToParameterEncodeTypeString(ReplaceString(pair.Bond, replaceMap), pair.Encode);
                        var suf = ToParameterEncodeTypeString(ReplaceString(pair.Suffix, replaceMap), pair.Encode);
                        return $"{pre}{key}{bond}{val}{suf}";
                    }

                default:
                    throw new NotImplementedException();
            }
        }

        protected UriResultModel GetFormatedUri(UriItemModel uriItem, IReadOnlyDictionary<string, string> replaceMap)
        {
            var result = new UriResultModel() {
                Uri = ReplaceString(uriItem.Uri, replaceMap).Trim(),
                RequestParameterType = uriItem.RequestParameterType,
            };
            if(uriItem.UriParameterType == UriParameterType.None) {
                return result;
            }

            var convertedParams = UriParameterList.Parameters
                .FirstOrDefault(up => up.Key == uriItem.Key)
                ?.Items
                ?.Select(p => ToUriParameterString(p, uriItem.UriParameterType, replaceMap))
                ?.Where(s => !string.IsNullOrEmpty(s))
            ;
            if(convertedParams == null) {
                return result;
            }

            switch(uriItem.UriParameterType) {
                case UriParameterType.Query:
                    result.Uri = $"{result.Uri}?{string.Join("&", convertedParams)}";
                    break;

                case UriParameterType.Hierarchy:
                    result.Uri = $"{result.Uri}/{string.Join("/", convertedParams)}";
                    break;

                case UriParameterType.PreSuffixes:
                    result.Uri = $"{result.Uri}{string.Join(string.Empty, convertedParams)}";
                    break;

                default:
                    throw new NotImplementedException();
            }

            return result;
        }

        protected UriResultModel GetUri_Impl(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            var uriItem = GetUriItem(key);
            if(uriItem != null) {
                return GetFormatedUri(uriItem, replaceMap);
            } else {
                return null;
            }
        }

        protected IDictionary<string, string> GetRequestParameter_Impl(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            var targetParameter = RequestParameterList.Parameters
                .FirstOrDefault(up => up.Key == key)
            ;
            if(targetParameter == null) {
                return (IDictionary<string, string>)EmptyMap;
            }

            return targetParameter.Items
                .Where(p => p.HasKey)
                .ToDictionary(
                    p => p.Key,
                    p => ReplaceString(p.Value, replaceMap)
                )
            ;
        }

        protected string BuildMapping(string s, string target, MappingItemModel item)
        {
            var bracket = item.Brackets.FirstOrDefault(b => b.Target == target);
            if(bracket == null) {
                return s;
            }
            return $"{bracket.Open}{s}{bracket.Close}";
        }

        protected string ToMappingItemString(MappingItemModel item, IReadOnlyDictionary<string, string> replaceMap)
        {
            var value = ReplaceString(item.Value, replaceMap);

            switch(item.Type) {
                case MappingItemType.Simple:
                    return $"{BuildMapping(value, MappingItemModel.targetValue, item)}";

                case MappingItemType.Pair: {
                        var name = ReplaceString(item.Name, replaceMap);
                        var bond = ReplaceString(item.Bond, replaceMap);
                        
                        return $"{BuildMapping(name, MappingItemModel.targetName, item)}{BuildMapping(bond, MappingItemModel.targetBond, item)}{BuildMapping(value, MappingItemModel.targetValue, item)}";
                    }

                case MappingItemType.ForcePair: {
                        var name = ReplaceString(item.Name, replaceMap);
                        var bond = ReplaceString(item.Bond, replaceMap);
                        var hasEmpty = new[] { name, bond, value }.Any(s => string.IsNullOrWhiteSpace(s));
                        if(hasEmpty) {
                            var fail = ReplaceString(item.Failure, replaceMap);
                            return fail;
                        }
                        return $"{BuildMapping(name, MappingItemModel.targetName, item)}{BuildMapping(bond, MappingItemModel.targetBond, item)}{BuildMapping(value, MappingItemModel.targetValue, item)}";
                    }

                default:
                    throw new NotImplementedException();
            }
        }

        protected MappingResultModel GetRequestMapping_Impl(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            var result = new MappingResultModel();
            var mapping = RequestMappingList.Mappings
                .FirstOrDefault(up => up.Key == key)
            ;
            if(mapping == null) {
                result.Result = string.Empty;
                return result;
            }

            if(!string.IsNullOrWhiteSpace(mapping.ContentType)) {
                result.ContentType = mapping.ContentType;
            }

            var mappingParams = mapping.Items
                .ToDictionary(
                    i => i.Key,
                    i => ToMappingItemString(i, replaceMap)
                )
            ;
            var replacedContent = ReplaceString(mapping.Content.Value, mappingParams);
            var trimMap = new Dictionary<MappingContentTrim, Func<string, string>>() {
                { MappingContentTrim.None, s => s },
                { MappingContentTrim.Block, s => string.Concat(s.SkipWhile(c => char.IsWhiteSpace(c)).Reverse().SkipWhile(c => char.IsWhiteSpace(c)).Reverse()) },
                { MappingContentTrim.Line, s => string.Join(Environment.NewLine, s.SplitLines().Select(l => l.Trim())) },
                { MappingContentTrim.Content, s => string.Join(Environment.NewLine, string.Concat(s.SkipWhile(c => char.IsWhiteSpace(c)).Reverse().SkipWhile(c => char.IsWhiteSpace(c)).Reverse()).SplitLines().Select(l => l.Trim())) },
            };
            var trimedContent = trimMap[mapping.Content.Trim](replacedContent);
            if(mapping.Content.Oneline) {
                result.Result = string.Join(string.Empty, trimedContent.SplitLines());
            }else {
                result.Result = trimedContent;
            }

            return result;
        }

        #endregion

        #region ICommunication

        public virtual ResponseModel Request(RequestModel request)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IGetUri

        public virtual UriResultModel GetUri(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IUriCompatibility

        public virtual string ConvertUri(string uri, ServiceType serviceType)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IGetRequestParameter

        public virtual IDictionary<string, string> GetRequestParameter(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            throw new NotImplementedException();
        }

        public virtual MappingResultModel GetRequestMapping(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRequestCompatibility

        public virtual IDictionary<string, string> ConvertRequestParameter(IReadOnlyDictionary<string, string> requestParams, ServiceType serviceType)
        {
            throw new NotImplementedException();
        }

        public virtual string ConvertRequestMapping(string mapping, ServiceType serviceType)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region IResponseCompatibility

        public virtual CheckModel CheckResponseHeader(Uri uri, HttpHeaders headers, ServiceType serviceType)
        {
            throw new NotImplementedException();
        }

        public virtual byte[] ConvertBinary(Uri uri, byte[] binary, ServiceType serviceType)
        {
            throw new NotImplementedException();
        }

        public virtual Encoding GetEncoding(Uri uri, byte[] binary, ServiceType serviceType)
        {
            throw new NotImplementedException();
        }

        public virtual string ConvertString(Uri uri, string text, ServiceType serviceType)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region IConvertCompatibility

        public virtual bool ConvertValue(out object outputValue, Type outputType, string inputKey, object inputValue, Type inputType, ServiceType serviceType)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
