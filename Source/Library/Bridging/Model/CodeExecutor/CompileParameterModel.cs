using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ReadOnly;

namespace ContentTypeTextNet.MnMn.Library.Bridging.Model.CodeExecutor
{
    /// <summary>
    /// <see cref="CompilerParameters"/>のシリアライズ用データ。
    /// </summary>
    [Serializable]
    public class CompileParameterModel : ModelBase, IReadOnlyCompileParameter
    {
        #region IReadOnlyCompileParameter

        //
        // 概要:
        //     コンパイラを起動するときに使用する、オプションのコマンド ライン引数を取得または設定します。
        //
        // 戻り値:
        //     コンパイラに対する追加のコマンド ライン引数。
        public string CompilerOptions { get; set; }

        //
        // 概要:
        //     実行可能ファイルを生成するかどうかを示す値を取得または設定します。
        //
        // 戻り値:
        //     実行可能ファイルを生成する場合は true。それ以外の場合は false。
        public bool GenerateExecutable { get; set; } = false;
        //
        // 概要:
        //     メモリ内で出力を生成するかどうかを示す値を取得または設定します。
        //
        // 戻り値:
        //     コンパイラがメモリ内で出力を生成する場合は true。それ以外の場合は false。
        public bool GenerateInMemory { get; set; } = true;
        //
        // 概要:
        //     コンパイルされた実行可能ファイルにデバッグ情報を含めるかどうかを示す値を取得または設定します。
        //
        // 戻り値:
        //     デバッグ情報を生成する場合は true。それ以外の場合は false。
        public bool IncludeDebugInformation { get; set; } = false;
        //
        // 概要:
        //     警告をエラーとして扱うかどうかを示す値を取得または設定します。
        //
        // 戻り値:
        //     警告をエラーとして扱う場合は true。それ以外の場合は false。
        public bool TreatWarningsAsErrors { get; set; } = true;
        //
        // 概要:
        //     コンパイラがコンパイルを中止する警告レベルを取得または設定します。
        //
        // 戻り値:
        //     コンパイラがコンパイルを中止する警告レベル。
        public int WarningLevel { get; set; } = 4;

        public CollectionModel<string> AssemblyNames { get; set; } = new CollectionModel<string>();
        IReadOnlyList<string> IReadOnlyCompileParameter.AssemblyNames => AssemblyNames;

        public CollectionModel<KeyValuePair<string, string>> ProviderOptions { get; set; } = new CollectionModel<KeyValuePair<string, string>>();
        IReadOnlyList<KeyValuePair<string, string>> IReadOnlyCompileParameter.ProviderOptions => ProviderOptions;

        #endregion
    }
}
