using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    public class OctetPerTime
    {
        public OctetPerTime(TimeSpan baseTime)
        {
            BaseTime = baseTime;
        }

        #region property

        public TimeSpan BaseTime { get; }

        Stopwatch TimeStopWatch { get; } = new Stopwatch();

        protected long SizeInBaseTime { get; private set; }
        protected long PrevSize { get; private set; }
        public long Size { get; protected set; }

        #endregion

        #region function

        public void Start()
        {
            TimeStopWatch.Restart();
            SizeInBaseTime = 0;
        }

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
