using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using Microsoft.IO;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Extensions
{
    public static class RecyclableMemoryStreamManagerExtensions
    {
        public static MemoryStream GetStreamWidthAutoTag(this RecyclableMemoryStreamManager memoryStreamManager, byte[] buffer, int offset, int count, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1, [CallerMemberName] string callerMemberName = "")
        {
            var tag = $"{callerMemberName}: {callerFilePath}({callerLineNumber})";
            return memoryStreamManager.GetStream(tag, buffer, offset, count);
        }
        public static MemoryStream GetStreamWidthAutoTag(this RecyclableMemoryStreamManager memoryStreamManager, byte[] buffer, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1, [CallerMemberName] string callerMemberName = "")
        {
            return GetStreamWidthAutoTag(memoryStreamManager, buffer, 0, buffer.Length, callerFilePath, callerLineNumber, callerMemberName);
        }
    }
}
