using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Model;
using ContentTypeTextNet.MnMn.Library.SpaghettiAssembly.Define;
using Microsoft.CSharp;

namespace ContentTypeTextNet.MnMn.Library.SpaghettiAssembly.Logic
{
    [Serializable]
    public class CSharpCodeCompiler: CodeCompilerBase
    {
        public CSharpCodeCompiler()
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
