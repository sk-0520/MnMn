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
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Live
{
    public class SmileLivePlayerViewModel: ViewModelBase
    {
        public SmileLivePlayerViewModel(Mediation mediation)
        {
            Mediation = mediation;
        }

        #region proeprty

        Mediation Mediation { get; }

        #endregion

        #region function

        internal Task LoadAsync(SmileLiveInformationViewModel information, bool forceEconomy, CacheSpan informationCacheSpan, CacheSpan imageCacheSpan)
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
