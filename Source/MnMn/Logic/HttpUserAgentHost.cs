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
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.MnMn.MnMn.IF;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    public class HttpUserAgentHost: DisposeFinalizeBase, ICreateHttpUserAgent
    {
        public HttpUserAgentHost()
        { }

        #region property

        /// <summary>
        /// HttpClient用ハンドラ。
        /// </summary>
        protected HttpClientHandler ClientHandler { get; private set; } = new HttpClientHandler() {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };

        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(ClientHandler != null) {
                    ClientHandler.Dispose();
                    ClientHandler = null;
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region ICreateHttpUserAgent

        public HttpClient CreateHttpUserAgent()
        {
            return new HttpClient(ClientHandler, false);
        }

        #endregion

    }
}
