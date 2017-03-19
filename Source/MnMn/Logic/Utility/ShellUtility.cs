using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.IF;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility
{
    public static class ShellUtility
    {
        static Process ExecuteFileCore(string path, string args, ILogger logger)
        {
            try {
                if(args != null) {
                    return Process.Start(path, args);
                } else {
                    return Process.Start(path);
                }
            } catch(Exception ex) {
                logger.Warning(ex);
            }

            return null;
        }

        /// <summary>
        /// ファイルを選択した状態でエクスプローラを開く。
        /// </summary>
        /// <param name="file">対象ファイル。</param>
        /// <param name="logger">ロガー。</param>
        /// <returns></returns>
        public static Process OpenFileInDirectory(FileInfo file, ILogger logger)
        {
            return ExecuteFileCore("explorer.exe", $"/select,\"{file.FullName}\"", logger);
        }

        /// <summary>
        /// ディレクトリを開く。
        /// </summary>
        /// <param name="file">対象ファイル。</param>
        /// <param name="logger">ロガー。</param>
        /// <returns></returns>
        public static Process OpenDirectory(DirectoryInfo directory, ILogger logger)
        {
            return ExecuteFileCore(directory.FullName, null, logger);
        }

        /// <summary>
        /// ファイルを関連付けられたプログラムで起動。
        /// </summary>
        /// <param name="file">対象ファイル。</param>
        /// <param name="logger">ロガー。</param>
        /// <returns></returns>
        public static Process ExecuteFile(FileInfo file, ILogger logger)
        {
            return ExecuteFileCore(file.FullName, null, logger);
        }

        /// <summary>
        /// URI をシステムブラウザで開く。
        /// </summary>
        /// <param name="uri">開くURI。</param>
        /// <param name="logger">ロガー。</param>
        /// <returns></returns>
        public static Process OpenUriInSystemBrowser(Uri uri, ILogger logger)
        {
            try {
                return Process.Start(uri.OriginalString);
            } catch(Exception ex) {
                logger.Warning(ex);
            }

            return null;
        }

        /// <summary>
        /// クリップボードにコピーする。
        /// </summary>
        /// <param name="text">コピーする文字列。</param>
        /// <param name="logger">ロガー。</param>
        /// <returns>成功・失敗。</returns>
        public static bool SetClipboard(string text, ILogger logger)
        {
            try {
                Clipboard.SetText(text);
                return true;
            } catch(Exception ex) {
                logger.Warning(ex);
            }

            return false;
        }
    }
}
