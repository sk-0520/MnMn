using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    public class SpaghettiScript: DisposeFinalizeBase
    {
        public SpaghettiScript(string domainName, ILogger logger)
        {
            Logger = logger;
            Logger.Information($"create domain: {domainName}");
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
    }
}
