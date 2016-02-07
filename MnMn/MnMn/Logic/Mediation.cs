﻿/*
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
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    /// <summary>
    /// データ連携等々の橋渡し。
    /// </summary>
    public class Mediation: MediationBase
    {
        public Mediation()
        {
            // 二重生成だけど気にしない
            Setting = new MainSettingModel();

            Smile = new SmileMediation(this);
        }

        public Mediation(MainSettingModel mainSettingModel)
            : this()
        {
            Setting = mainSettingModel;
        }

        #region property

        MainSettingModel Setting { get; }

        /// <summary>
        /// ニコニコ関係。
        /// </summary>
        SmileMediation Smile { get; set; }

        #endregion

        #region function

        private ResponseModel Request_CacheDirectory_Impl(RequestModel request)
        {
            Debug.Assert(request.RequestKind == RequestKind.CacheDirectory);

            var map = new Dictionary<ServiceType, IEnumerable<string>>() {
                {ServiceType.Smile, new [] { Constants.ServiceName, Constants.ServiceSmileName } },
                {ServiceType.SmileVideo, new [] { Constants.ServiceName, Constants.ServiceSmileName, Constants.ServiceSmileVideoName } },
            };

            // 設定値よりコマンドラインオプションを優先する
            var baseDir = VariableConstants.HasOptionCacheRootDirectoryPath
                ? Path.Combine(VariableConstants.OptionValueCacheRootDirectoryPath, Constants.ApplicationDirectoryName)
                : Setting.CacheDirectoryPath;
            ;
            if(string.IsNullOrWhiteSpace(baseDir)) {
                baseDir = Path.Combine(Constants.CacheDirectoryPath, Constants.ApplicationDirectoryName);
            }

            var path = new List<string>() {
                baseDir,
            };
            path.AddRange(map[request.ServiceType]);

            var directoryPath = Environment.ExpandEnvironmentVariables(Path.Combine(path.ToArray()));
            
            if(Directory.Exists(directoryPath)) {
                var response = new ResponseModel(request, new DirectoryInfo(directoryPath));
                return response;
            } else {
                var response = new ResponseModel(request, Directory.CreateDirectory(directoryPath));
                return response;
            }
        }


        #endregion

        #region MediationBase

        public override ResponseModel Request(RequestModel request)
        {
            CheckUtility.DebugEnforceNotNull(request);

            if(request.RequestKind == RequestKind.CacheDirectory) {
                return Request_CacheDirectory_Impl(request);
            }

            switch(request.ServiceType) {
                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                    return Smile.Request(request);

                default:
                    ThrowNotSupportRequest(request);
                    throw new NotImplementedException();
            }
        }


        public override string GetUri(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                    return Smile.GetUri(key, replaceMap, serviceType);

                default:
                    ThrowNotSupportGetUri(key, replaceMap, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override string ConvertUri(string uri, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                    return Smile.ConvertUri(uri, serviceType);

                default:
                    ThrowNotSupportConvertUri(uri, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IDictionary<string, string> GetRequestParameter(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                    return Smile.GetRequestParameter(key, replaceMap, serviceType);

                default:
                    ThrowNotSupportGetRequestParameter(key, replaceMap, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override string GetRequestMapping(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                    return Smile.GetRequestMapping(key, replaceMap, serviceType);

                default:
                    ThrowNotSupportGetRequestMapping(key, replaceMap, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IDictionary<string, string> ConvertRequestParameter(IReadOnlyDictionary<string, string> requestParams, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                    return Smile.ConvertRequestParameter(requestParams, serviceType);

                default:
                    ThrowNotSupportConvertRequestParameter(requestParams, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override string ConvertRequestMapping(string mapping, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                    return Smile.ConvertRequestMapping(mapping, serviceType);

                default:
                    ThrowNotSupportConvertRequestMapping(mapping, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override CheckModel CheckResponseHeader(Uri uri, HttpHeaders headers, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                    return Smile.CheckResponseHeader(uri, headers, serviceType);

                default:
                    ThrowNotSupportCheckResponseHeader(uri, headers, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override byte[] ConvertBinary(Uri uri, byte[] binary, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                    return Smile.ConvertBinary(uri, binary, serviceType);

                default:
                    ThrowNotSupportConvertBinary(uri, binary, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override Encoding GetEncoding(Uri uri, byte[] binary, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                    return Smile.GetEncoding(uri, binary, serviceType);

                default:
                    ThrowNotSupportGetEncoding(uri, binary, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override string ConvertString(Uri uri, string text, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                    return Smile.ConvertString(uri, text, serviceType);

                default:
                    ThrowNotSupportConvertString(uri, text, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override bool ConvertValue(out object outputValue, Type outputType, string inputKey, object inputValue, Type inputType, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                    return Smile.ConvertValue(out outputValue, outputType, inputKey, inputValue, inputType, serviceType);

                default:
                    ThrowNotSupportValueConvert(inputKey, inputValue, inputType, outputType, serviceType);
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
