using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.Library.Bridging.IF.ReadOnly
{
    public interface IReadOnlyCompileParameter
    {
        #region property

        /// 概要:
        ///     コンパイラを起動するときに使用する、オプションのコマンド ライン引数を取得または設定します。
        ///
        /// 戻り値:
        ///     コンパイラに対する追加のコマンド ライン引数。
        string CompilerOptions { get; }

        ///
        /// 概要:
        ///     実行可能ファイルを生成するかどうかを示す値を取得または設定します。
        ///
        /// 戻り値:
        ///     実行可能ファイルを生成する場合は true。それ以外の場合は false。
        bool GenerateExecutable { get; }

        /// 概要:
        ///     メモリ内で出力を生成するかどうかを示す値を取得または設定します。
        ///
        /// 戻り値:
        ///     コンパイラがメモリ内で出力を生成する場合は true。それ以外の場合は false。
        bool GenerateInMemory { get; }

        /// 概要:
        ///     コンパイルされた実行可能ファイルにデバッグ情報を含めるかどうかを示す値を取得または設定します。
        ///
        /// 戻り値:
        ///     デバッグ情報を生成する場合は true。それ以外の場合は false。
        bool IncludeDebugInformation { get; }

        /// 概要:
        ///     警告をエラーとして扱うかどうかを示す値を取得または設定します。
        ///
        /// 戻り値:
        ///     警告をエラーとして扱う場合は true。それ以外の場合は false。
        bool TreatWarningsAsErrors { get; }

        /// 概要:
        ///     コンパイラがコンパイルを中止する警告レベルを取得または設定します。
        ///
        /// 戻り値:
        ///     コンパイラがコンパイルを中止する警告レベル。
        int WarningLevel { get; }

        IReadOnlyList<string> AssemblyNames { get; }

        IReadOnlyList<KeyValuePair<string, string>> ProviderOptions { get; }

        #endregion
    }
}
