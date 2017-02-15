using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video
{
    public class SmileVideoCommentScriptModel: ModelBase
    {
        #region property

        public SmileVideoCommentScriptType ScriptType { get; set; }

        public TimeSpan IsEnabledTime { get; set; }
        public Color ForeColor { get; set; }
        public SmileVideoCommentSize CommentSize { get; set; }
        /// <summary>
        /// 保持はしとくけど使わない。
        /// </summary>
        public SmileVideoCommentVertical VerticalAlign { get; set; }

        #endregion
    }
}
