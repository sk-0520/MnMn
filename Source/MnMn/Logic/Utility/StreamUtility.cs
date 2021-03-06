﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility
{
    /// <summary>
    ///<see cref="RecyclableMemoryStreamManager"/> でいい感じにストリームを作成する。
    /// </summary>
    public static class StreamUtility
    {
        public static Stream ToStream(string text, Encoding encoding, int bufferSize)
        {
            var stream = GlobalManager.MemoryStream.GetStreamWidthAutoTag();
            using(var writer = new StreamWriter(stream, encoding, bufferSize, true)) {
                writer.Write(text);
            }
            stream.Position = 0;

            return stream;
        }

        public static Stream ToStream(string text, Encoding encoding)
        {
            return ToStream(text, encoding, Constants.TextStreamWriteBuffer);
        }

        public static Stream ToUtf8Stream(string text)
        {
            return ToStream(text, Encoding.UTF8, Constants.TextStreamWriteBuffer);
        }
    }
}
