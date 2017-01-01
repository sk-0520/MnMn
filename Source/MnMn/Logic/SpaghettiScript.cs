using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.Compatibility;
using ContentTypeTextNet.MnMn.Library.SpaghettiAssembly.Define;
using ContentTypeTextNet.MnMn.Library.SpaghettiAssembly.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    public class SpaghettiScript: DisposeFinalizeBase, IUriCompatibility
    {
        public SpaghettiScript(string domainName, ILogger logger)
        {
            Logger = logger;
            Logger.Information($"create domain: {domainName}");
            //var domainInfo = new AppDomainSetup() {
            //    ApplicationBase = Constants.AssemblyRootDirectoryPath,
            //    ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
            //};
            //LocalDomain = AppDomain.CreateDomain(domainName, AppDomain.CurrentDomain.Evidence, domainInfo);
            LocalDomain = AppDomain.CreateDomain(domainName);
        }

        #region property

        AppDomain LocalDomain { get; }
        ILogger Logger { get; }

        IDictionary<string, IList<SpaghettiPreparationData>> Preparations { get; } = new Dictionary<string, IList<SpaghettiPreparationData>>();

        #endregion

        #region function

        IEnumerable<KeyValuePair<string, SpaghettiPreparationData>> ConstructPreparationsCore(IEnumerable<FileInfo> files, IEnumerable<string> keys)
        {
            foreach(var file in files) {
                var baseName = Path.GetFileNameWithoutExtension(file.Name);
                foreach(var key in keys) {
                    var sourceExists
                        = string.Equals(key, baseName, StringComparison.InvariantCultureIgnoreCase)
                        || baseName.StartsWith(key + Constants.SpaghettiScriptSplit, StringComparison.InvariantCultureIgnoreCase)
                    ;

                    if(sourceExists) {
                        var data = new SpaghettiPreparationData() {
                            State = Define.ScriptState.None,
                            File = file,
                        };
                        Logger.Trace($"script construct core: {key}, {file.Name}");
                        yield return new KeyValuePair<string, SpaghettiPreparationData>(key, data);
                    }
                }
            }
        }

        /// <summary>
        /// 指定ディレクトリからスクリプトを準備する。
        /// </summary>
        /// <param name="baseDirectory"></param>
        public void ConstructPreparations(DirectoryInfo baseDirectory, IEnumerable<string> keys)
        {
            baseDirectory.Refresh();
            if(!baseDirectory.Exists) {
                Logger.Information($"not found script dir: {baseDirectory}");
                return;
            }

            var pairs = ConstructPreparationsCore(baseDirectory.EnumerateFiles(Constants.SpaghettiScriptSearchPattern), keys);
            var map = pairs
                .GroupBy(p => p.Key, d => d.Value)
                .Select(g => new { Key = g.Key, Values = g.OrderBy(f => f.File.Name) })
                .ToDictionary(i => i.Key, i => new List<SpaghettiPreparationData>(i.Values))
            ;
            foreach(var pair in map) {
                Logger.Debug($"script construct: {pair.Key}, count: {pair.Value.Count}");
                Preparations.Add(pair.Key, pair.Value);
            }
        }

        public bool HasKey(string key)
        {
            return Preparations.ContainsKey(key);
        }

        SpaghettiSourceModel LoadSource(string key, SpaghettiPreparationData data)
        {
            using(var logger = new TimeLogger(Logger, $"load: {key}, {data.File.FullName}")) {
                var model = SerializeUtility.LoadXmlSerializeFromFile<SpaghettiSourceModel>(data.File.FullName);
                return model;
            }
        }

        CodeCompilerBase CreateSpaghettiAssembly(string key, CodeLanguage codeLanguage)
        {
            var codeMakerMap = new Dictionary<CodeLanguage, string> {
                [CodeLanguage.CSharp] = typeof(CSharpCodeCompiler).FullName,
            };
            var spaghettiAssembly = (CodeCompilerBase)LocalDomain.CreateInstanceAndUnwrap(
                nameof(Library.SpaghettiAssembly),
                codeMakerMap[codeLanguage]
            );

            //spaghettiAssembly.GetType().Assembly.ger

            spaghettiAssembly.Identifier = key;

            return spaghettiAssembly;
        }

        void CompileSource(string key, SpaghettiPreparationData data, SpaghettiSourceModel source)
        {
            var spaghettiAssembly = CreateSpaghettiAssembly(key, source.CodeLanguage);
            spaghettiAssembly.CompileMessage += SpaghettiAssembly_CompileMessage;

            spaghettiAssembly.Compile(source.Parameter, source.Code);

            spaghettiAssembly.CompileMessage -= SpaghettiAssembly_CompileMessage;
        }

        #endregion

        #region IUriCompatibility

        public string ConvertUri(string key, string uri, ServiceType serviceType)
        {
            foreach(var data in Preparations[key]) {
                switch(data.State) {
                    case Define.ScriptState.None: {
                            // コンパイル
                            var source = LoadSource(key, data);
                            CompileSource(key, data, source);
                        }
                        break;
                }
            }

            return uri;
        }


        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    try {
                        AppDomain.Unload(LocalDomain);
                    } catch(Exception ex) {
                        Logger.Error(ex);
                    }
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        private void SpaghettiAssembly_CompileMessage(object sender, CompileMessageEventArgs e)
        {
            LogPutDelegate put;
            switch(e.Kind) {
                case CompileMessageKind.PreProcessor:
                case CompileMessageKind.Compile:
                    put = Logger.Information;
                    break;

                case CompileMessageKind.Error:
                    put = Logger.Error;
                    break;

                default:
                    throw new NotImplementedException();
            }

            put($"[{e.DomainName}/{e.Identifier}] {e.Kind}: {e.Message}");
        }

    }
}
