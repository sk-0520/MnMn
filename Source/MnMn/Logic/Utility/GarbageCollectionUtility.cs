using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility
{
    /// <summary>
    /// GC 処理の共通化。
    /// <para>基本的にこいつは例外を投げない。</para>
    /// </summary>
    public static class GarbageCollectionUtility
    {
        static IReadOnlyCheckResult<long> RemoveFileCore(FileInfo file, bool judgeCreateTime, bool judgeWriteTime, bool judgeAccessTime, IEnumerable<DateTime> judgeTimestamps, CacheSpan cacheSpan, bool force)
        {
            Debug.Assert(file != null);

            try {
                file.Refresh();

                if(file.Exists) {
                    var timestamps = judgeTimestamps.ToEvaluatedSequence();
                    if(judgeCreateTime) {
                        timestamps.Add(file.CreationTime);
                    }
                    if(judgeWriteTime) {
                        timestamps.Add(file.LastWriteTime);
                    }
                    if(judgeAccessTime) {
                        timestamps.Add(file.LastAccessTime);
                    }

                    Debug.Assert(timestamps.Any());

                    var timestamp = timestamps.Max();

                    if(force || !cacheSpan.IsCacheTime(timestamp)) {
                        var fileSize = file.Length;
                        file.Delete();
                        return CheckResultModel.Success(fileSize);
                    }
                }
            } catch(Exception ex) {
                return CheckResultModel.Failure<long>(ex.ToString());
            }

            return CheckResultModel.Success<long>(0);
        }

        public static IReadOnlyCheckResult<long> RemoveFile(FileInfo file, CacheSpan cacheSpan, bool force)
        {
            if(file == null) {
                throw new ArgumentNullException(nameof(file));
            }

            return RemoveFileCore(file, true, true, false, Enumerable.Empty<DateTime>(), cacheSpan, force);
        }

        public static IReadOnlyCheckResult<long> RemoveFile(FileInfo file, IEnumerable<DateTime> addJudgeTimestamps, CacheSpan cacheSpan, bool force)
        {
            if(file == null) {
                throw new ArgumentNullException(nameof(file));
            }
            if(addJudgeTimestamps == null) {
                throw new ArgumentNullException(nameof(addJudgeTimestamps));
            }

            return RemoveFileCore(file, true, true, false, addJudgeTimestamps, cacheSpan, force);
        }

        public static IReadOnlyCheckResult<long> RemoveFile(FileInfo file, DateTime addJudgeTimestamp, CacheSpan cacheSpan, bool force)
        {
            if(file == null) {
                throw new ArgumentNullException(nameof(file));
            }

            return RemoveFileCore(file, true, true, false, new[] { addJudgeTimestamp }, cacheSpan, force);
        }

        public static IReadOnlyCheckResult<long> RemoveTemporaryFile(FileInfo file)
        {
            if(file == null) {
                return CheckResultModel.Failure<long>("null file");
            }

            try {
                file.Refresh();
                if(file.Exists) {
                    var size = file.Length;
                    file.Delete();
                    return CheckResultModel.Success(size);
                }
            } catch(Exception ex) {
                return CheckResultModel.Failure<long>(ex.ToString());
            }

            return CheckResultModel.Success<long>(0);
        }

    }
}
