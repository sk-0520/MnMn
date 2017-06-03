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
using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    public abstract class MediationCustomBase: MediationBase
    {
        protected MediationCustomBase(Mediation mediation, string uriListPath, string uriParametersPath, string requestHeaderPath, string requestParametersPath, string requestMappingsPath, string expressionsPath)
            : base(uriListPath, uriParametersPath, requestHeaderPath, requestParametersPath, requestMappingsPath, expressionsPath)
        {
            Mediation = mediation;

            Script = CreateScript();
        }

        #region property

        protected Mediation Mediation { get; private set; }

        protected virtual string ScriptDirectoryPath { get; }

        #endregion

        #region function

        internal abstract object RequestShowView(ShowViewRequestModel requet);

        internal abstract void SetManager(ServiceType serviceType, ManagerPackModelBase managerPack);

        protected abstract IEnumerable<string> GetCustomKeys();

        protected static IEnumerable<string> GetMediationKeys(Type type)
        {
            // 突貫工事
            foreach(var field  in type.GetFields()) {
                yield return (string)field.GetValue(type);
            }
        }

        #endregion

        #region MediationBase

        public override ILogger Logger { get { return Mediation.Logger; } }

        protected override IEnumerable<string> GetKeys()
        {
            var baseKeys = base.GetKeys();
            var customKeys = GetCustomKeys();

            return baseKeys
                .Concat(customKeys)
                .GroupBy(k => k)
                .Select(s => s.Key)
            ;
        }

        protected override SpaghettiScript CreateScript()
        {
            var myType = GetType();
            var domainName = myType.Name;

            var script = new SpaghettiScript(domainName, Mediation.Logger);

            script.ConstructPreparations(new DirectoryInfo(ScriptDirectoryPath), GetKeys());

            return script;
        }

        #endregion
    }
}
