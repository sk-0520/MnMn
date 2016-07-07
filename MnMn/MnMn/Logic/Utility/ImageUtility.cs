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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility
{
    public static class ImageUtility
    {
        public static BitmapSource ColoringImage(BitmapSource rawBitmap, Color color)
        {
            var pixels = MediaUtility.GetPixels(rawBitmap);
            var pixcelWidth = 4;
            for(var i = 0; i < pixels.Length; i += pixcelWidth) {
                // 0:b 1:g 2:r 3:a
                var b = pixels[i + 0];
                var g = pixels[i + 1];
                var r = pixels[i + 2];
                pixels[i + 0] = (byte)(b + (1 - b / 255.0) * color.B);
                pixels[i + 1] = (byte)(g + (1 - g / 255.0) * color.G);
                pixels[i + 2] = (byte)(r + (1 - r / 255.0) * color.R);
            }

            var result = new WriteableBitmap(rawBitmap);
            result.Lock();
            result.WritePixels(new Int32Rect(0, 0, rawBitmap.PixelWidth, rawBitmap.PixelHeight), pixels, rawBitmap.PixelWidth * pixcelWidth, 0);
            result.Unlock();

            FreezableUtility.SafeFreeze(result);

            return result;
        }
    }
}
