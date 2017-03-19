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

        public static Process OpenFileInDirectory(FileInfo file, ILogger logger)
        {
            return ExecuteFileCore("explorer.exe", $"/select,\"{file.FullName}\"", logger);
        }

        public static Process ExecuteFile(FileInfo file, ILogger logger)
        {
            return ExecuteFileCore(file.FullName, null, logger);
        }

        public static Process OpenUriInSystemBrowser(Uri uri, ILogger logger)
        {
            try {
                return Process.Start(uri.OriginalString);
            } catch(Exception ex) {
                logger.Warning(ex);
            }

            return null;
        }

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
