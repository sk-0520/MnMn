using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define.CodeExecutor;
using ContentTypeTextNet.MnMn.Library.Bridging.Model;
using Microsoft.CSharp;

namespace ContentTypeTextNet.MnMn.Library.SpaghettiAssembly
{
    [Serializable]
    public class SpaghettiAssemblyCSharp: SpaghettiAssemblyBase
    {
        public SpaghettiAssemblyCSharp()
            :base(CodeLanguage.CSharp)
        { }

        #region CodeCompilerBase

        protected override CodeDomProvider CreateProvider(IDictionary<string, string> providerOptions)
        {
            var map = new Dictionary<string, string>(providerOptions);

            // 未指定の場合にコンパイラバージョン限定
            if(!map.ContainsKey("CompilerVersion")) {
                map["CompilerVersion"] = "v4.0";
            }

            var provider = new CSharpCodeProvider(map);

            return provider;
        }
        
        #endregion
    }
}
