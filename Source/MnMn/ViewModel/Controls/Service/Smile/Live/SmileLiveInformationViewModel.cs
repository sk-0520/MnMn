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
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Live;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Live
{
    public class SmileLiveInformationViewModel: InformationViewModelBase
    {
        #region variable
        #endregion

        SmileLiveInformationViewModel(Mediation mediation)
        {
            Mediation = mediation;
        }

        public SmileLiveInformationViewModel(Mediation mediation, FeedSmileLiveItemModel feed)
            : this(mediation)
        {
            Feed = feed;

            InformationSource = SmileLiveInformationSource.Feed;
        }

        #region property

        Mediation Mediation { get; }

        public SmileLiveInformationSource InformationSource { get; }

        FeedSmileLiveItemModel Feed { get; }

        #endregion
    }
}
