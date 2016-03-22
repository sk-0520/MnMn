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
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility
{
    /// <summary>
    /// キャッシュだったりなんだったりの画像取得処理を一元化。
    /// </summary>
    public static class ImageLoaderUtility
    {
        public static BitmapSource GetBitmapSource(Stream stream)
        {
            BitmapSource bitmap = null;
            Application.Current.Dispatcher.Invoke(new Action(() => {
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
    }
}
