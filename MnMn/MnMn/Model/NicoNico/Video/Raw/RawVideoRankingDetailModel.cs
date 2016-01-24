using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video.Raw
{
    public class RawVideoRankingDetailModel:ModelBase
    {
        /// <summary>
        /// 動画ID。
        /// </summary>
        public string VideoId { get; set; }
        /// <summary>
        /// タイトル。
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 紹介文。
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// サムネイルURL。
        /// </summary>
        public string ThumbnailUrl { get; set; }
        /// <summary>
        /// 長さ。
        /// <para>mmm:ss形式</para>
        /// </summary>
        public string Length { get; set; }
        /// <summary>
        /// 投稿日時。
        /// </summary>
        public string FirstRetrieve { get; set; }
        /// <summary>
        /// 再生数。
        /// </summary>
        public string ViewCounter { get; set; }
        /// <summary>
        /// コメント数
        /// </summary>
        public string CommentNum { get; set; }
        /// <summary>
        /// マイリスト数
        /// </summary>
        public string MylistCounter { get; set; }

    }
}
