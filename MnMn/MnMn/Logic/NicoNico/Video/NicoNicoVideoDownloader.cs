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

namespace ContentTypeTextNet.MnMn.MnMn.Logic.NicoNico.Video
{
    public class NicoNicoVideoDownloader: Downloader
    {
        public NicoNicoVideoDownloader(Uri downloadUri, ICreateHttpUserAgent userAgentCreator, Uri referrerUri)
            : base(downloadUri, userAgentCreator)
        {
            ReferrerUri = referrerUri;
        }


        #region property

        public Uri ReferrerUri { get; }
        public TimeSpan WatchToMovieWaitTime {get;set;} = Constants.ServiceNicoNicoVideoWatchToMovieWaitTime;

        #endregion

        #region NicoNicoVideoDownloader

        protected override Task<Stream> GetStreamAsync(out bool cancel)
        {
            UserAgent = UserAgentCreator.CreateHttpUserAgent();
            try {
                var dummy = UserAgent.GetStringAsync(ReferrerUri).ConfigureAwait(true);
                Thread.Sleep(WatchToMovieWaitTime);
            } catch(Exception ex) {
                Debug.WriteLine(ex);
                cancel = true;
                return new Task<Stream>(() => null);
            }

            cancel = false;
            UserAgent.DefaultRequestHeaders.Referrer = ReferrerUri;
            return UserAgent.GetStreamAsync(DownloadUri);
        }

        #endregion
    }
}
