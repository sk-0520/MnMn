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

namespace ContentTypeTextNet.MnMn.MnMn.Define
{
    /// <summary>
    ///
    /// </summary>
    public enum RequestKind
    {
        /// <summary>
        /// セッション。
        /// </summary>
        Session,
        /// <summary>
        /// ランキング定義オブジェクト。
        /// </summary>
        RankingDefine,
        /// <summary>
        /// 検索定義オブジェクト。
        /// </summary>
        SearchDefine,
        /// <summary>
        /// プレイリスト定義オブジェクト
        /// </summary>
        PlayListDefine,
        /// <summary>
        /// カテゴリ定義オブジェクト。
        /// </summary>
        CategoryDefine,
        /// <summary>
        /// キャッシュ用ディレクトリパス。
        /// </summary>
        CacheDirectory,
        /// <summary>
        /// 基本設定データ。
        /// </summary>
        Setting,
        /// <summary>
        /// 表示要素へのアクセス。
        /// </summary>
        ShowView,
        ///// <summary>
        ///// 表示要素の破棄。
        ///// </summary>
        //HideView,
        /// <summary>
        /// 独自設定。
        /// </summary>
        CustomSetting,
        /// <summary>
        /// キャッシュデータ。
        /// </summary>
        CacheData,
        /// <summary>
        /// 表示ウィンドウ取得。
        /// </summary>
        WindowViewModels,
        /// <summary>
        /// 何かする。
        /// </summary>
        Process,
        /// <summary>
        /// 内臓ブラウザ処理。
        /// </summary>
        Browser,
    }
}
