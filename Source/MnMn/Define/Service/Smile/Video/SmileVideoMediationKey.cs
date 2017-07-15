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

namespace ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video
{
    internal static class SmileVideoMediationKey
    {
        public const string getthumbinfo = "video-getthumbinfo";
        public const string ranking = "video-ranking";
        public const string getflvNormal = "video-getflv-normal";
        public const string getflvScraping = "video-getflv-scraping";
        public const string getthreadkey = "video-getthreadkey";
        public const string getrelation = "video-getrelation";

        public const string watchDataPage = "video-watch-data-page";

        public const string msg = "video-msg";
        public const string msgWithThread = "video-msg-with-thread";
        public const string msgPost = "video-msg-post";
        public const string msgPostKey = "video-msg-post-key";

        [Obsolete]
        public const string tagRelation = "video-tag-relation";

        public const string tagTrend = "video-tag-trend";
        public const string tagFeed = "video-tag-feed";

        public const string newarrival = "video-newarrival";
        public const string recent = "video-recent";
        public const string hotlist = "video-hotlist";
        public const string recommendation = "video-recommendation";
        public const string recommendationPage = "video-recommendation-page";
        public const string recommendationApi = "video-recommendation-api";

        public const string historyApp = "video-history-app";
        public const string historyPage = "video-history-page";
        public const string historyApi = "video-history-api";
        public const string historyRemove = "video-history-remove";

        public const string inputEconomyMode = "getflv-economy-mode";

        public const string dmc = "video-dmc";
        public const string dmcReload = "video-dmc-reload";

        public const string search = "video-search";
    }
}
