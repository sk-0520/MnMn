﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    /// <summary>
    /// 時間単位でのオクテット使用量を算出。
    /// <para>octet ってなってるけど byte 前提。</para>
    /// <para><see cref="Size"/>を使っておけば幸せになれる。</para>
    /// </summary>
    public class OctetPerTime
    {
        public OctetPerTime(TimeSpan baseTime)
        {
            BaseTime = baseTime;
        }

        #region property

        /// <summary>
        /// 時間単位。
        /// </summary>
        public TimeSpan BaseTime { get; }

        Stopwatch TimeStopWatch { get; } = new Stopwatch();

        /// <summary>
        /// <see cref="BaseTime"/>中での使用量。
        /// </summary>
        protected long SizeInBaseTime { get; private set; }
        /// <summary>
        /// 前回の<see cref="BaseTime"/>中での使用量。
        /// </summary>
        protected long PrevSize { get; private set; }
        /// <summary>
        /// 直近の<see cref="BaseTime"/>での使用量。
        /// </summary>
        public long Size { get; protected set; }

        #endregion

        #region function

        /// <summary>
        /// 計測開始。
        /// </summary>
        public void Start()
        {
            TimeStopWatch.Restart();
            SizeInBaseTime = 0;
        }

        /// <summary>
        /// 資料量の加算。
        /// </summary>
        /// <param name="addSize"></param>
        public void Add(long addSize)
        {
            Debug.Assert(TimeStopWatch.IsRunning);

            var elapsedTime = TimeStopWatch.Elapsed;

            if(BaseTime <= elapsedTime) {
                Size = SizeInBaseTime + addSize;

                PrevSize = Size;
                SizeInBaseTime = 0;

                TimeStopWatch.Restart();
            } else {
                Size = PrevSize;
                SizeInBaseTime += addSize;
            }
        }

        #endregion

    }
}
