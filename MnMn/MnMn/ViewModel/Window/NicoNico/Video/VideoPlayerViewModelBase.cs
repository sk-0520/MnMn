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
using ContentTypeTextNet.MnMn.MnMn.Logic.NicoNico.Video.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.NicoNico.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Control.NicoNico.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Window.NicoNico.Video
{
    public class VideoPlayerViewModelBase: ViewModelBase
    {
        public VideoPlayerViewModelBase(Mediation mediation)
        {
            Mediation = mediation;
        }

        #region property

        Mediation Mediation { get; set; }
        public VideoInformationViewModel VideoInformationViewModel { get; set; }

        #endregion

        #region function

        public async Task InitializeAsync(string videoId)
        {
            var getthumbinfo = new Getthumbinfo(Mediation);
            var redGetthumbinfoModel = await getthumbinfo.GetAsync(videoId);
            if(GetthumbinfoUtility.IsSuccessResponse(redGetthumbinfoModel)) {
                VideoInformationViewModel = new VideoInformationViewModel(Mediation, redGetthumbinfoModel.Thumb, -1);
            }
        }

        #endregion
    }
}
