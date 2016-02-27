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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public class SmileVideoManagerViewModel: ManagerViewModelBase
    {
        #region variable

        SmileVideoManagerViewModelBase _selectedManager;

        #endregion

        public SmileVideoManagerViewModel(Mediation mediation)
            : base(mediation)
        {
            var settingResponse = Mediation.Request(new RequestModel(RequestKind.Setting, ServiceType.SmileVideo));
            Setting = (SmileVideoSettingModel)settingResponse.Result;

            var rankingResponse = Mediation.Request(new RequestModel(RequestKind.RankingDefine, ServiceType.SmileVideo));
            var rankingModel = (SmileVideoRankingModel)rankingResponse.Result;
            RankingManager = new SmileVideoRankingManagerViewModel(Mediation, rankingModel);

            var searchResponse = Mediation.Request(new RequestModel(RequestKind.SearchDefine, ServiceType.SmileVideo));
            var searchModel = (SmileVideoSearchModel)searchResponse.Result;
            SearchManager = new SmileVideoSearchManagerViewModel(Mediation, searchModel, Setting);

            NewArrivalsManager = new SmileVideoNewArrivalsManagerViewModel(Mediation);

            MyListManager = new SmileVideoMyListManagerViewModel(Mediation);

            Mediation.SetManager(ServiceType.SmileVideo, new SmileVideoManagerPackModel(SearchManager, RankingManager, NewArrivalsManager, MyListManager));

            SwitchViewCommand.Execute(ManagerItems.Last());
        }

        #region property

        SmileVideoSettingModel Setting { get; }

        public SmileVideoRankingManagerViewModel RankingManager { get; }
        public SmileVideoSearchManagerViewModel SearchManager { get; }
        public SmileVideoNewArrivalsManagerViewModel NewArrivalsManager { get; }
        public SmileVideoMyListManagerViewModel MyListManager {get;}

        public IEnumerable<SmileVideoManagerViewModelBase> ManagerItems => new SmileVideoManagerViewModelBase[] {
            SearchManager,
            RankingManager,
            NewArrivalsManager,
            MyListManager,
        };

        public SmileVideoManagerViewModelBase SelectedManager
        {
            get { return this._selectedManager; }
            set {
                if(SetVariableValue(ref this._selectedManager, value)) {
                    foreach(var item in ManagerItems.Where(i => i.Visible)) {
                        item.Visible = false;
                    }
                    SelectedManager.Visible = true;
                }
            }
        }


        #endregion

        #region command

        public ICommand SwitchViewCommand
        {
            get
            {
                return CreateCommand(o => {
                    SelectedManager = (SmileVideoManagerViewModelBase)o;
                });
            }
        }

        #endregion

        #region function

        #endregion

        public override Task InitializeAsync()
        {
            return Task.WhenAll(ManagerItems.Select(m => m.InitializeAsync()));
        }
    }
}
