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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video
{
    public class SmileVideoDownloader: Downloader
    {
        //public SmileVideoDownloader(Uri downloadUri, IHttpUserAgentCreator userAgentCreator, Uri referrerUri, SmileVideoMovieType movieType)
        //    : base(downloadUri, userAgentCreator)
        //{
        //    ReferrerUri = referrerUri;
        //    MovieType = movieType;
        //}

        public SmileVideoDownloader(Uri downloadUri, IHttpUserAgentCreator userAgentCreator, Uri referrerUri, Mediation mediation, SmileVideoMovieType movieType, CancellationToken cancelToken)
            : base(downloadUri, userAgentCreator, cancelToken)
        {
            ReferrerUri = referrerUri;
            Mediation = mediation;
            MovieType = movieType;
        }


        #region property

        public Uri ReferrerUri { get; }
        public TimeSpan WatchToMovieWaitTime { get; set; } = Constants.ServiceSmileVideoWatchToMovieWaitTime;

        public string PageHtml { get; private set; }

        Mediation Mediation { get; set; }

        SmileVideoMovieType MovieType { get;  }

        public SmileVideoWatchDataModel WatchData { get; private set; }

        #endregion

        #region SmileVideoDownloader

        protected override async Task<Stream> GetStreamAsync()
        {
            try {
                UserAgent = UserAgentCreator.CreateHttpUserAgent();
                //// DMC形式は不要だと思うけど他との互換性のため残しとく
                //var task = await SmileVideoInformationUtility.LoadWatchPageHtmlSource(UserAgent, ReferrerUri);
                var watchData = new WatchData(Mediation);
                var watchHtmlSource = await watchData.LoadWatchPageHtmlSourceAsync(ReferrerUri, MovieType, UserAgentCreator);
                WatchData = watchData.GetWatchData(watchHtmlSource.Result);

                PageHtml = WatchData.HtmlSource;

                //cancel = false;
                UserAgent.DefaultRequestHeaders.Referrer = ReferrerUri;
                IfUsingSetRangeHeader();
                var getTask = await UserAgent.GetAsync(DownloadUri, HttpCompletionOption.ResponseHeadersRead);
                var response = getTask;

                ResponseHeaders = response.Content.Headers;
                return await response.Content.ReadAsStreamAsync();
            } catch(Exception ex) {
                Debug.WriteLine(ex);
                return null;
            }
        }

        #endregion
    }
}
