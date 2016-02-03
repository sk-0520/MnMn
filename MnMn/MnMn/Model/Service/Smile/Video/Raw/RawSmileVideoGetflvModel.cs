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
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Attribute;

namespace ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video.Raw
{
    public class RawNicoNicoVideoGetflvModel: ModelBase
    {
        #region property

        [Name("error")]
        public string Error { get; set; }

        [Name("deleted")]
        public string Deleted { get; set; }

        [Name("thread_id")]
        public string ThreadId { get; set; }

        /// <summary>
        /// 動画の長さ。
        /// <para>秒。</para>
        /// </summary>
        [Name("l")]
        public string Length { get; set; }

        /// <summary>
        /// 動画配信サーバー。
        /// </summary>
        [Name("url")]
        public string MovieServerUrl { get; set; }

        /// <summary>
        /// 動画ページ。
        /// </summary>
        [Name("link")]
        public string Link { get; set; }

        /// <summary>
        /// メッセージサーバーURL。
        /// </summary>
        [Name("ms")]
        public string MessageServerUrl { get; set; }
        /// <summary>
        /// メッセージサーバー(予備?)URL。
        /// </summary>
        [Name("ms_sub")]
        public string MessageServerSubUrl { get; set; }

        /// <summary>
        /// クライアントユーザー。
        /// </summary>
        [Name("user_id")]
        public string UserId { get; set; }

        [Name("is_premium")]
        public string IsPremium { get; set; }

        [Name("nickname")]
        public string Nickname { get; set; }

        /// <summary>
        /// 投降時間。
        /// <para>1970/01/01 UTC</para>
        /// </summary>
        [Name("time")]
        public string Time { get; set; }

        [Name("done")]
        public string Done { get; set; }

        /// <summary>
        /// 投稿者フィルタ情報。
        /// <para>*AAAA=BBBBの形式で、動画の閲覧者が投稿したコメントが"AAAA"であれば、それを"BBBB"に置き換える。</para>
        /// </summary>
        [Name("feedrev")]
        public string Feedrev { get; set; }

        [Name("Hms")]
        public string Hms { get; set; }
        [Name("Hmsp")]
        public string Hmsp { get; set; }
        [Name("Hmst")]
        public string Hmst { get; set; }
        [Name("Hmstk")]
        public string Hmstk { get; set; }
        [Name("rpu")]
        public string Rpu { get; set; }
        //--------------
        [Name("needs_key")]
        public string NeedsKey { get; set; }
        /// <summary>
        /// 公式動画のスレッドID。
        /// </summary>
        [Name("optional_thread_id")]
        public string OptionalThreadId { get; set; }

        #endregion
    }
}
