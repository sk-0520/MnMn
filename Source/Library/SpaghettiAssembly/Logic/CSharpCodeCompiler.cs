using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.SpaghettiAssembly.Define;
using Microsoft.CSharp;

namespace ContentTypeTextNet.MnMn.Library.SpaghettiAssembly.Logic
{
    public class CSharpCodeCompiler: CodeCompilerBase
    {
        public CSharpCodeCompiler(CompilerParameters compilerParameters)
            :base(CodeLanguage.CSharp, compilerParameters)
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
