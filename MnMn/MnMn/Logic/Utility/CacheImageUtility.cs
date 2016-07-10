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
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility
{
    /// <summary>
    /// キャッシュだったりなんだったりの画像取得処理を一元化。
    /// </summary>
    public static class CacheImageUtility
    {
        public static BitmapSource GetBitmapSource(Stream stream)
        {
            BitmapSource bitmap = null;
            Application.Current?.Dispatcher.Invoke(new Action(() => {
                var image = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                FreezableUtility.SafeFreeze(image);
                bitmap = image;
            }));
            return bitmap;
        }

        public static BitmapSource GetBitmapSource(byte[] binary)
        {
            using(var stream = new MemoryStream(binary)) {
                return GetBitmapSource(stream);
            }
        }

        static void SaveBitmapSource(BitmapSource bitmapSource, string savePath, BitmapEncoder encoder, ILogger logger)
        {
            var frame = BitmapFrame.Create(bitmapSource);
            encoder.Frames.Add(frame);
            FileUtility.MakeFileParentDirectory(savePath);
            using(var stream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.Read)) {
                encoder.Save(stream);
            }
        }

        public static Task SaveBitmapSourceToPngAsync(BitmapSource bitmapSource, string savePath, ILogger logger)
        {
            return Task.Run(() => {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                    var encoder = new PngBitmapEncoder();
                    SaveBitmapSource(bitmapSource, savePath, encoder, logger);
                }));
            });
        }

        public static async Task<BitmapSource> LoadBitmapBinaryAsync(HttpClient client, Uri loadUri, int maxCount, TimeSpan waitTime, ILogger logger)
        {
            var count = 0;
            byte[] binary = null;
            do {
                try {
                    logger.Trace($"img -> {loadUri}");
                    binary = await client.GetByteArrayAsync(loadUri);
                } catch(Exception ex) {
                    logger.Error($"error img -> {loadUri}");
                    logger.Warning(ex);
                    if(count != 0) {
                        Thread.Sleep(waitTime);
                    }
                }
            } while(count++ < maxCount && binary == null);

            if(binary != null) {
               return CacheImageUtility.GetBitmapSource(binary);
            }

            return null;
        }

        public static Task<BitmapSource> LoadBitmapBinaryDefaultAsync(HttpClient client, Uri loadUri, ILogger logger)
        {
            return LoadBitmapBinaryAsync(client, loadUri, 3, TimeSpan.FromSeconds(1), logger);
        }

        public static BitmapSource LoadBitmapBinary(string path)
        {
            using(var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                return GetBitmapSource(stream);
            }
        }

        public static bool ExistImage(string path, CacheSpan cacheSpan)
        {
            if(File.Exists(path)) {
                var fileInfo = new FileInfo(path);
                return cacheSpan.IsCacheTime(fileInfo.LastWriteTime) && Constants.MinimumPngFileSize <= fileInfo.Length;
            }

            return false;
        }
    }
}
