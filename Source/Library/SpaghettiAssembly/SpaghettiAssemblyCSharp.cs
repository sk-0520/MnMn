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

        protected override CodeDomProvider CreateProvider()
        {
            var provider = new CSharpCodeProvider();

            return provider;
        }
        
        #endregion
    }
}
