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

namespace ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile
{
    internal static class SmileMediationKey
    {
        public const string webSite = "smile-web-site";

        public const string videoLogin = "video-session-login";
        public const string videoLogout = "video-session-logout";
        public const string videoCheck = "video-session-check";

        public const string userPage = "smile-user-page";
        public const string userMyListPage = "smile-user-mylist-page";
        public const string userPostVideo = "smile-user-post-video";
        public const string userFavoriteMyListPage = "smile-user-fav-mylist-page";

        public const string marketVideoRelation = "smile-market-video-relation";

        public const string pediaWordArticle = "smile-pedia-word-article";

        public const string suggestionComplete = "smile-suggestion-complete";

        public const string contentsSearch = "smile-contents-search";

        public const string mylistDefault = "smile-video-mylist-account-default";
        public const string mylistDefaultVideoAdd = "smile-video-mylist-account-default-add";
        public const string mylistDefaultVideoDelete = "smile-video-mylist-account-default-delete";
        public const string mylistVideoAdd = "smile-video-mylist-account-add";
        public const string mylistVideoDelete = "smile-video-mylist-account-delete";
        public const string mylistGroup = "smile-video-mylist-account-group";
        public const string mylistGroupToken = "smile-video-mylist-account-group-token";
        public const string mylistGroupDelete = "smile-video-mylist-account-group-delete";
        public const string mylistGroupUpdate = "smile-video-mylist-account-group-update";
        public const string mylistGroupCreate = "smile-video-mylist-account-group-create";
        public const string mylistGroupSort = "smile-video-mylist-account-group-sort";
        public const string mylist = "smile-video-mylist-user";
        public const string mylistSearch = "smile-video-mylist-search";

        public const string inputIsScrapingVideoId = "convert-is-scraping-video-id";
        public const string inputGetVideoId = "convert-get-video-id";
        public const string inputGetMyListId = "convert-get-mylist-id";
        public const string inputGetUserId = "convert-get-user-id";
    }
}
