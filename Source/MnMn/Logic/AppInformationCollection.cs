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
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Data;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    public class AppInformationCollection: InformationCollection
    {
        public AppInformationCollection(ICommunication communication)
        {
            Communication = communication;
        }

        #region property

        ICommunication Communication { get; }

        #endregion

        #region function

        public InformationGroup GetAppConfig()
        {
            var group = new InformationGroup("app.config");

            foreach(string key in ConfigurationManager.AppSettings) {
                var value = ConfigurationManager.AppSettings.Get(key);
                group.Items.Add(key, value);
            }

            return group;
        }

        #endregion

        #region InformationCollection

        public override FileVersionInfo GetVersionInfo
        {
            get { return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location); }
        }

        public override InformationGroup GetApplication()
        {
            var result = base.GetApplication();

            result.Items.Add("BuildType", Constants.BuildType);

            if(Communication != null) {
                var setting = Communication.GetResultFromRequest<AppSettingModel>(new Model.Request.RequestModel(RequestKind.Setting, ServiceType.Application));
                result.Items.Add("LightweightUpdate", setting.RunningInformation.LightweightUpdateTimestamp.ToString("u"));
            }

            return result;
        }

        public override IEnumerable<InformationGroup> Get()
        {
            return new[] {
                GetApplication(),
                // MnMn 独自処理
                GetAppConfig(),
                GetCPU(),
                GetMemory(),
                GetEnvironment(),
                GetScreen(),
            };
        }

        #endregion
    }
}
