using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video
{
    public struct SmileVideoMsgRangeModel : IModel
    {
        public SmileVideoMsgRangeModel(int headMinutes, int tailMinutes, int rangeCount, int allCount)
        {
            HeadMinutes = headMinutes;
            TailMinutes = tailMinutes;
            RangeCount = rangeCount;
            AllCount = allCount;
        }
        #region property

        public int HeadMinutes { get; }
        public int TailMinutes { get; }
        public int RangeCount { get; }
        public int AllCount { get; }

        #endregion

        #region object

        public override string ToString()
        {
            return $"{HeadMinutes}-{TailMinutes}:{RangeCount},{Math.Abs(AllCount)}";
        }

        #endregion

        #region IModel

        public string DisplayText => ToString();

        public void Correction()
        { }

        #endregion
    }
}
