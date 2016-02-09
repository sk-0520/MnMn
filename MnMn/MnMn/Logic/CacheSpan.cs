using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    public class CacheSpan: ModelBase
    {
        #region define

        public static CacheSpan NoCache => new CacheSpan();
        public static CacheSpan InfinityCache => new CacheSpan(DateTime.MaxValue, TimeSpan.MaxValue);

        #endregion

        CacheSpan()
        {
            IsEnabled = false;
        }

        public CacheSpan(DateTime baseTIme, TimeSpan expires)
        {
            IsEnabled = true;

            BaseTime = baseTIme;
            Expires = expires;
        }

        #region property

        public bool IsEnabled { get; }

        public DateTime BaseTime { get; }
        public TimeSpan Expires { get; }

        #endregion

        #region function

        public bool IsCacheTime(DateTime dateTime)
        {
            return IsEnabled && BaseTime - dateTime < Expires;
        }

        #endregion
    }
}
