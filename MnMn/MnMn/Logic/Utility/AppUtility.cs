/*
This file is part of MnMn.

MnMn is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MnMn is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MnMn.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility
{
    public static class AppUtility
    {
        public static T LoadSetting<T>(Stream stream, FileType fileType, ILogger logger)
            where T : ModelBase, new()
        {
            var loadDataName = typeof(T).Name;
            logger.Debug($"read: {loadDataName}");

            T result = null;

            if(stream != null) {
                switch(fileType) {
                    case FileType.Json:
                        result = SerializeUtility.LoadJsonDataFromStream<T>(stream);
                        break;

                    case FileType.Binary:
                        result = SerializeUtility.LoadBinaryDataFromStream<T>(stream);
                        break;

                    default:
                        throw new NotImplementedException();
                }

                if(result != null) {
                    logger.Debug($"reading: {loadDataName}");
                } else {
                    logger.Debug($"reading: {loadDataName} is null");
                }
            } else {
                logger.Debug($"read stream is null: {loadDataName}");
            }

            return result ?? new T();
        }

        /// <summary>
        /// 設定ファイルの読込。
        /// <para>設定ファイルが読み込めない場合、new Tを使用する。</para>
        /// </summary>
        /// <typeparam name="T">読み込むデータ型</typeparam>
        /// <param name="path">読み込むファイル</param>
        /// <param name="fileType">ファイル種別</param>
        /// <param name="logger">ログ出力</param>
        /// <returns>読み込んだデータ。読み込めなかった場合は new T を返す。</returns>
        public static T LoadSetting<T>(string path, FileType fileType, ILogger logger)
            where T : ModelBase, new()
        {
            var loadDataName = typeof(T).Name;
            logger.Debug($"load: {loadDataName}", path);

            T result = null;

            if(File.Exists(path)) {
                try {
                    var fileInfo = new FileInfo(path);
                    if(fileInfo.Length == 0) {
                        logger.Debug($"load file is empty: {loadDataName}", fileInfo);
                    } else {
                        switch(fileType) {
                            case FileType.Json:
                                result = SerializeUtility.LoadJsonDataFromFile<T>(path);
                                break;

                            case FileType.Binary:
                                result = SerializeUtility.LoadBinaryDataFromFile<T>(path);
                                break;

                            default:
                                throw new NotImplementedException();
                        }
                    }
                } catch(Exception ex) {
                    logger.Warning($"loading: {loadDataName}", ex.ToString());
                }

                if(result != null) {
                    logger.Debug($"loading: {loadDataName}");
                } else {
                    logger.Debug($"loading: {loadDataName} is null");
                }
            } else {
                logger.Debug($"load file not found: {loadDataName}", path);
            }

            return result ?? new T();
        }

        public static void SaveSetting<T>(Stream stream, T model, FileType fileType, ILogger logger)
            where T : ModelBase
        {
            var saveDataName = typeof(T).Name;
            logger.Debug($"write: {saveDataName}");

            // ファイルへ出力
            switch(fileType) {
                case FileType.Json:
                    SerializeUtility.SaveJsonDataToStream(stream, model);
                    break;

                case FileType.Binary:
                    SerializeUtility.SaveBinaryDataToStream(stream, model);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 設定ファイルの出力。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="model"></param>
        /// <param name="fileType"></param>
        /// <param name="usingTemporary">一時出力を使用するか</param>
        /// <param name="logger"></param>
        public static void SaveSetting<T>(string path, T model, FileType fileType, bool usingTemporary, ILogger logger)
            where T : ModelBase
        {
            var saveDataName = typeof(T).Name;
            logger.Debug($"save: {saveDataName}", path);

            // 一時ファイル用パス
            var tempPath = path + FileNameUtility.GetTemporaryExtension("out");

            // 出力に使用するパス
            string outputPath = null;

            if(usingTemporary) {
                outputPath = tempPath;
                if(FileUtility.Exists(tempPath)) {
                    logger.Debug($"save existis temp path: {saveDataName}", tempPath);
                    FileUtility.Delete(tempPath);
                }
            } else {
                outputPath = path;
            }

            // ファイルへ出力
            switch(fileType) {
                case FileType.Json:
                    SerializeUtility.SaveJsonDataToFile(outputPath, model);
                    break;

                case FileType.Binary:
                    SerializeUtility.SaveBinaryDataToFile(outputPath, model);
                    break;

                default:
                    throw new NotImplementedException();
            }

            if(usingTemporary) {
                // すでにファイルが存在する場合は退避させる
                var existisOldFile = File.Exists(path);
                var srcPath = path + FileNameUtility.GetTemporaryExtension("src");
                if(existisOldFile) {
                    File.Move(path, srcPath);
                }
                bool swapError = false;
                try {
                    // 入れ替え
                    File.Move(tempPath, path);
                } catch(IOException ex) {
                    logger.Warning(ex);
                    swapError = true;
                }
                if(swapError) {
                    // 旧ファイルの復帰
                    if(!File.Exists(path) && File.Exists(srcPath)) {
                        File.Move(srcPath, path);
                    }
                } else {
                    // 旧ファイルの削除
                    if(existisOldFile) {
                        File.Delete(srcPath);
                    }
                }
            }
        }
    }
}
