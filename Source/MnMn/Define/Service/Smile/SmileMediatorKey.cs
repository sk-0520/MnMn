﻿/*
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
    internal static class SmileMediatorKey
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

        public const string channelPage = "smile-channel-page";
        public const string channelThumbnail = "smile-channel-thumbnail";
        public const string channelVideoFeed = "smile-channel-video-feed";

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

        public const string isScrapingVideoId = "is-scraping-video-id";
        public const string getVideoId = "get-video-id";
        public const string convertChannelId = "convert-channel-id";
        public const string getMylistId = "get-mylist-id";
        public const string getUserId = "get-user-id";
        public const string needCorrectionVideoId = "need-correction-video-id";


        public const string userInformationFromHtml = "user-information-from-html";

        public static class Id
        {
            public const string videoLogin_userValue = "user-value";
            public const string videoLogin_userR18 = "user-r18";

            public const string getVideoId_prefixId = "prefix-id";
            public const string getVideoId_numberId = "number-id";

            public const string convertChannelId_normalizationId = "normalization-id";
            public const string convertChannelId_numberOnly = "number-only";

            public const string userInformationFromHtml_userId = "user-id";
            public const string userInformationFromHtml_version = "version";
            public const string userInformationFromHtml_isPremium = "is-premium";
            public const string userInformationFromHtml_gender = "gender";
            public const string userInformationFromHtml_birthday = "birthday";
            public const string userInformationFromHtml_location = "location";
            public const string userInformationFromHtml_mylistCount = "mylist-count";
        }

    }
}
