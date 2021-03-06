﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ContentTypeTextNet.MnMn.MnMn
{
    /// <summary>
    /// XAML周りとの連携。
    /// </summary>
    partial class Constants
    {
        public const string xamlGridSplitterLength = "GridSplitterLength";

        public const string xamlStyle_SmallDefaultIconPath = "SmallDefaultIconPath";

        public static Thickness WindowDefaultThickness { get{ return new Thickness(1); } }

        /// <summary>
        /// コピー。
        /// </summary>
        public const string xamlImage_Copy = "Image_Copy";
        /// <summary>
        /// 新しいウィンドウ系。
        /// </summary>
        public const string xamlImage_OpenInNewWindow = "Image_OpenInNewWindow";
        /// <summary>
        /// 再生。
        /// </summary>
        public const string xamlImage_Navigationbar_Play = "Image_Navigationbar_Play";
        /// <summary>
        /// 未整理のブックマーク
        /// </summary>
        public const string xamlImage_Bookmark_Unorganized = "Image_Bookmark-Unorganized";
        /// <summary>
        /// プレイリスト追加
        /// </summary>
        public const string xamlImage_Playlist_Add = "Image_Playlist-Add";
        public const string xamlImage_CheckItLater = "Image_CheckItLater";

        /// <summary>
        /// 標準ブラウザアイコン。
        /// </summary>
        public const string xamlImage_Browser = "Image_Browser";

        public const string xamlIDescription_Image_DefaultBrowser = "DefaultBrowserIcon";
    }
}
