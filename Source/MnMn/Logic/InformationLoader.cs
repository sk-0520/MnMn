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
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    public class InformationLoader<TInformationViewModel>
        where TInformationViewModel : InformationViewModelBase
    {
        public InformationLoader(IEnumerable<TInformationViewModel> informations, ICreateHttpUserAgent createHttpUserAgent)
        {
            InformationItems = new List<TInformationViewModel>(informations);
            CreateHttpUserAgent = createHttpUserAgent;
        }

        #region property

        protected IReadOnlyList<TInformationViewModel> InformationItems { get; }
        protected ICreateHttpUserAgent CreateHttpUserAgent { get; }

        #endregion

        #region function

        public virtual Task LoadThumbnaiImageAsync(CacheSpan cacheSpan)
        {
            var userAgent = CreateHttpUserAgent.CreateHttpUserAgent();
            var tasks = InformationItems.Select(i => i.LoadThumbnaiImageAsync(cacheSpan, userAgent));

            return Task.WhenAll(tasks).ContinueWith(t => {
                t.Dispose();
                userAgent.Dispose();
            });
        }

        public virtual Task LoadInformationAsync(CacheSpan cacheSpan)
        {
            var userAgent = CreateHttpUserAgent.CreateHttpUserAgent();
            var tasks = InformationItems.Select(i => i.LoadInformationAsync(cacheSpan, userAgent));

            return Task.WhenAll(tasks).ContinueWith(t => {
                t.Dispose();
                userAgent.Dispose();
            });
        }



        #endregion
    }
}
