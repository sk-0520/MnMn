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
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.IF.Compatibility;
using ContentTypeTextNet.MnMn.MnMn.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    public abstract class MediationBase: IGetUri, IUriCompatibility
    {
        public MediationBase()
            : this(null, null)
        { }

        protected MediationBase(string uriListPath, string uriParametersPath)
        {
            if(uriListPath != null) {
                UriList = SerializeUtility.LoadXmlSerializeFromFile<UriListModel>(uriListPath);
            } else {
                UriList = new UriListModel();
            }

            if(uriParametersPath != null) {
                UriParameters = SerializeUtility.LoadXmlSerializeFromFile<RequestParametersModel>(uriParametersPath);
            } else {
                UriParameters = new RequestParametersModel();
            }
        }

        #region property

        public static IReadOnlyDictionary<string, string> EmptyMap { get; } = new Dictionary<string, string>();

        protected UriListModel UriList { get; private set; }

        protected RequestParametersModel UriParameters { get; private set; }

        #endregion

        #region function

        public static string ReplaceString(string s, IReadOnlyDictionary<string, string> map)
        {
            return s.ReplaceRangeFromDictionary("${", "}", (Dictionary<string, string>)map);
        }

        protected void ThrowNotSupportGetUri(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            throw new NotSupportedException($"{nameof(IGetUri)} => {nameof(key)}: {key}, {nameof(replaceMap)}: {replaceMap}, {nameof(serviceType)}: {serviceType}");
        }

        protected void ThrowNotSupportConvertUri(string uri, ServiceType serviceType)
        {
            throw new NotSupportedException($"{nameof(IUriCompatibility)} => {nameof(uri)}: {uri}, {nameof(serviceType)}: {serviceType}");
        }

        protected UriItemModel GetUriItem(string key) => UriList.Items.First(ui => ui.Key == key);

        string ToUriParameterString(RequestParameterPairModel pair, UriParameterType type, IReadOnlyDictionary<string, string> replaceMap)
        {
            Debug.Assert(type != UriParameterType.None);

            var val = ReplaceString(pair.Value, replaceMap);

            switch(type) {
                case UriParameterType.Query:
                    if(pair.HasKey) {
                        return $"{pair.Key}={val}";
                    } else {
                        return val;
                    }

                case UriParameterType.Hierarchy:
                    return val;

                default:
                    throw new NotImplementedException();
            }
        }

        protected string GetFormatedUri(UriItemModel uriItem, IReadOnlyDictionary<string, string> replaceMap)
        {
            var uri = uriItem.Uri.Trim();
            if(uriItem.UriParameterType == UriParameterType.None) {
                return uri;
            }

            var convertedParams = UriParameters.Parameter
                .First(up => up.Key == uriItem.Key)
                .Pairs
                .Select(p => ToUriParameterString(p, uriItem.UriParameterType, replaceMap))
            ;
            switch(uriItem.UriParameterType) {
                case UriParameterType.Query:
                    return $"{uri}?{string.Join("&", convertedParams)}";

                case UriParameterType.Hierarchy:
                    return $"{uri}/{string.Join("/", convertedParams)}";

                default:
                    throw new NotImplementedException();
            }
        }

        protected string GetUri_Impl(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType) => GetFormatedUri(GetUriItem(key), replaceMap);

        #endregion

        #region IGetUri

        public virtual string GetUri(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
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
    }
}
