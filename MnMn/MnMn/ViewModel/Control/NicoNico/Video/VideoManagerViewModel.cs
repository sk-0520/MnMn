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
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.NicoNico.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.NicoNico.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.View.Window.NicoNico.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Window.NicoNico.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Control.NicoNico.Video
{
    public class VideoManagerViewModel: ViewModelBase
    {
        public VideoManagerViewModel(Mediation mediation)
        {
            Mediation = mediation;

            var response = Mediation.Request(new RequestModel(RequestKind.RankingDefine, ServiceType.NicoNicoVideo));
            var rankingModel = (RankingModel)response.Result;
            RankingManager = new RankingManagerViewModel(Mediation, rankingModel);
        }

        #region property

        Mediation Mediation { get; set; }

        public RankingManagerViewModel RankingManager { get; private set; }

        #endregion

        #region command
        #endregion

        #region function

        public async void Temp_OpenPlayer(string videoId)
        {
            var vm = new VideoPlayerViewModel(Mediation);
            var player = new VideoPlayerWindow() {
                DataContext = vm,
            };
            vm.SetPlayer(player.player);
            player.Show();

            await vm.InitializeAsync(videoId);
        }

        #endregion
    }
}
