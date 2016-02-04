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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.MnMn.MnMn.IF;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video
{
    public class SmileVideoDownloader: Downloader
    {
        public SmileVideoDownloader(Uri downloadUri, ICreateHttpUserAgent userAgentCreator, Uri referrerUri)
            : base(downloadUri, userAgentCreator)
        {
            ReferrerUri = referrerUri;
        }


        #region property

        public Uri ReferrerUri { get; }
        public TimeSpan WatchToMovieWaitTime {get;set;} = Constants.ServiceSmileVideoWatchToMovieWaitTime;

        #endregion

        #region SmileVideoDownloader

        protected override Task<Stream> GetStreamAsync(out bool cancel)
        {
            try {
                UserAgent = UserAgentCreator.CreateHttpUserAgent();
                var t = UserAgent.GetStringAsync(ReferrerUri);
                t.Wait();
                cancel = false;
                UserAgent.DefaultRequestHeaders.Referrer = ReferrerUri;
                IfUsingSetRangeHeader();
                UserAgent.GetStringAsync(ReferrerUri);
                return UserAgent.GetStreamAsync(DownloadUri);
            } catch(Exception ex) {
                Debug.WriteLine(ex);
                cancel = true;
                return new Task<Stream>(() => null);
            }
        }

        #endregion
    }
}
