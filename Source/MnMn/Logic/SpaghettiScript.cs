using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.Define.CodeExecutor;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.CodeExecutor;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.Compatibility;
using ContentTypeTextNet.MnMn.Library.Bridging.Model;
using ContentTypeTextNet.MnMn.Library.Bridging.Model.CodeExecutor;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    public class SpaghettiScript: 
        DisposeFinalizeBase, 
        IUriCompatibility,
        IRequestCompatibility,
        IResponseCompatibility
    {
        public SpaghettiScript(string domainName, ILogger logger)
        {
            Logger = logger;
            Logger.Information($"create domain: {domainName}");
            var domainInfo = new AppDomainSetup() {
                ApplicationBase = Constants.AssemblyRootDirectoryPath,
                ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
            };
            LocalDomain = AppDomain.CreateDomain(domainName, AppDomain.CurrentDomain.Evidence, domainInfo);
            //LocalDomain = AppDomain.CreateDomain(domainName);
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

                var appendLibrary = new[] {
                    Path.Combine(Constants.LibraryDirectoryPath, "ContentTypeTextNet.SharedLibrary.dll"),
                    Path.Combine(Constants.LibraryDirectoryPath, "Bridging.dll"),
                    Path.Combine(Constants.AssemblyPath),
                };
                var unionAssemblyNames = model.Parameter.AssemblyNames
                    .Concat(appendLibrary)
                    .GroupBy(s => s)
                    .Select(g => g.Key)
                ;

                model.Parameter.AssemblyNames.InitializeRange(unionAssemblyNames);

                var defaultNameSpace = new[] {
                    "System",
                    "System.IO",
                    "System.Linq",
                    "System.Collections",
                    "System.Collections.Generic",
                    "System.Text",
                    "System.Text.RegularExpressions",
                    "System.Net.Http.Headers",
                };
                var appendAppNameSpace = new[] {
                    typeof(IConvertCompatibility),
                    typeof(IRequestCompatibility),
                    typeof(IResponseCompatibility),
                    typeof(IUriCompatibility),
                    typeof(ServiceType),
                    typeof(CheckModel),
                }.Select(t => t.Namespace);
                var unionNameSpace = model.NameSpace
                    .Concat(defaultNameSpace)
                    .Concat(appendAppNameSpace)
                    .GroupBy(s => s)
                    .Select(g => g.Key)
                    .Select(u => $"using {u};")
                ;
                switch(model.CodeLanguage) {
                    case CodeLanguage.CSharp: {
                            var code = new StringBuilder();
                            //code.AppendLine("#pragma warning disable CS0105");
                            code.AppendLine(string.Join(Environment.NewLine, unionNameSpace));
                            code.AppendLine();
                            code.AppendLine("#line 1");
                            model.Code = code.ToString() + model.Code;
                        }
                        break;

                    default:
                        throw new NotImplementedException();
                }


                return model;
            }
        }

        ICodeExecutor CreateSpaghettiAssembly(string key, CodeLanguage codeLanguage)
        {
            var codeMakerMap = new Dictionary<CodeLanguage, string> {
                [CodeLanguage.CSharp] = "ContentTypeTextNet.MnMn.Library.SpaghettiAssembly.SpaghettiAssemblyCSharp",
            };
            var spaghettiAssembly = (ICodeExecutor)LocalDomain.CreateInstanceAndUnwrap(
                "SpaghettiAssembly",
                codeMakerMap[codeLanguage]
            );

            var initializeModel = new CodeInitializeModel() {
                Identifier = key,
                DomainName = LocalDomain.FriendlyName,
            };

            spaghettiAssembly.Initialize(initializeModel);

            return spaghettiAssembly;
        }

        bool CompileSource(string key, SpaghettiPreparationData data, SpaghettiSourceModel source)
        {
            var codeExecutor = CreateSpaghettiAssembly(key, source.CodeLanguage);

            codeExecutor.CompileMessage += SpaghettiAssembly_CompileMessage;

            var compiled = codeExecutor.Compile(source.Parameter, source.Code, "SpaghettiCode");

            codeExecutor.CompileMessage -= SpaghettiAssembly_CompileMessage;

            if(compiled) {
                data.State = ScriptState.Success;
                data.CodeExecutor = codeExecutor;

                return true;
            } else {
                data.State = ScriptState.Error;

                return false;
            }
        }

        struct Void { }

        TResult DoFunc<TResult>(string key, bool usingResult, bool recycle, int resultOriginIndex, Type interfaceType, string methodName, params object[] args)
        {
            var resultValue = default(TResult);
            var recycleValue = args[resultOriginIndex];
            var isChanged = false;

            foreach(var data in Preparations[key]) {
                Exception error = null;

                switch(data.State) {
                    case ScriptState.None: {
                            // コンパイル
                            try {
                                var source = LoadSource(key, data);

                                data.SkipNext = source.SkipNext;

                                if(CompileSource(key, data, source)) {
                                    goto ScriptState_Success;
                                }
                            } catch(Exception ex) {
                                error = ex;
                            }
                        }
                        goto ScriptState_Error;

                    case ScriptState.Success:
                        ScriptState_Success:
                        {
                            if(!data.CodeExecutor.HasType(interfaceType)) {
                                Logger.Information($"{key}; not impl <{interfaceType}>, {data.File.Name}");
                                continue;
                            }

                            try {
                                var invokeArgs = new List<object>(args);
                                invokeArgs[resultOriginIndex] = recycleValue;

                                var rawConvertedNewValue = data.CodeExecutor.Invoke(methodName, invokeArgs.ToArray());
                                isChanged = true;

                                if(data.SkipNext) {
                                    if(usingResult) {
                                        return (TResult)rawConvertedNewValue;
                                    } else {
                                        return default(TResult);
                                    }
                                } else {
                                    if(recycle) {
                                        recycleValue = (TResult)rawConvertedNewValue;
                                    }
                                    if(usingResult) {
                                        resultValue = (TResult)rawConvertedNewValue;
                                    }
                                }
                            } catch(Exception ex) {
                                error = ex;
                                goto ScriptState_Error;
                            }
                        }
                        break;

                    case ScriptState.Error:
                        ScriptState_Error:
                        {
                            var msg = error == null
                                ? "unknown error"
                                : error.Message
                            ;
                            Logger.Error($"{key}/{data.File.Name}", error);
                        }
                        break;
                }
            }

            if(usingResult && isChanged) {
                return resultValue;
            } else {
                return default(TResult);
            }
        }

        void DoAction(string key, bool recycle, int resultOriginIndex, Type interfaceType, string methodName, params object[] args)
        {
            DoFunc<Void>(key, false, recycle, resultOriginIndex, interfaceType, methodName, args);
        }

        #endregion

        #region IUriCompatibility

        public string ConvertUri(string key, string uri, ServiceType serviceType)
        {
            return DoFunc<string>(key, true, true, 1, typeof(IUriCompatibility), nameof(IUriCompatibility.ConvertUri), key, uri, serviceType);
        }

        #endregion

        #region IRequestCompatibility

        public IDictionary<string, string> ConvertRequestHeader(string key, IDictionary<string, string> requestHeaders, ServiceType serviceType)
        {
            return DoFunc<IDictionary<string, string>>(key, true, true, 1, typeof(IRequestCompatibility), nameof(IRequestCompatibility.ConvertRequestHeader), key, requestHeaders, serviceType);
        }

        public IDictionary<string, string> ConvertRequestParameter(string key, IDictionary<string, string> requestParams, ServiceType serviceType)
        {
            return DoFunc<IDictionary<string, string>>(key, true, true, 1, typeof(IRequestCompatibility), nameof(IRequestCompatibility.ConvertRequestParameter), key, requestParams, serviceType);
        }

        public string ConvertRequestMapping(string key, string mapping, ServiceType serviceType)
        {
            return DoFunc<string>(key, true, true, 1, typeof(IRequestCompatibility), nameof(IRequestCompatibility.ConvertRequestMapping), key, mapping, serviceType);
        }

        #endregion

        #region IResponseCompatibility

        public CheckModel CheckResponseHeader(string key, Uri uri, HttpHeaders headers, ServiceType serviceType)
        {
            return DoFunc<CheckModel>(key, false, true, 1, typeof(IResponseCompatibility), nameof(IResponseCompatibility.CheckResponseHeader), key, uri, headers, serviceType);
        }

        public void ConvertBinary(string key, Uri uri, Stream stream, ServiceType serviceType)
        {
            DoAction(key, false, 2, typeof(IResponseCompatibility), nameof(IResponseCompatibility.ConvertBinary), key, uri, stream, serviceType);
        }

        public Encoding GetEncoding(string key, Uri uri, Stream stream, ServiceType serviceType)
        {
            return DoFunc<Encoding>(key, true, false, 2, typeof(IResponseCompatibility), nameof(IResponseCompatibility.GetEncoding), key, uri, stream, serviceType);
        }

        public string ConvertString(string key, Uri uri, string text, ServiceType serviceType)
        {
            return DoFunc<string>(key, true, true, 2, typeof(IResponseCompatibility), nameof(IResponseCompatibility.ConvertString), key, uri, text, serviceType);
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
