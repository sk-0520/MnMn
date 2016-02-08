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
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public class SmileVideoManagerViewModel: ViewModelBase
    {
        #region variable

        SmileVideoManagerViewModelBase _selectedManager;

        #endregion

        public SmileVideoManagerViewModel(Mediation mediation)
        {
            Mediation = mediation;

            var rankingResponse = Mediation.Request(new RequestModel(RequestKind.RankingDefine, ServiceType.SmileVideo));
            var rankingModel = (SmileVideoRankingModel)rankingResponse.Result;
            RankingManager = new SmileVideoRankingManagerViewModel(Mediation, rankingModel);

            var searchResponse = Mediation.Request(new RequestModel(RequestKind.SearchDefine, ServiceType.SmileVideo));
            var searchModel = (SmileVideoSearchModel)searchResponse.Result;
            SearchManager = new SmileVideoSearchManagerViewModel(Mediation, searchModel);

            SwitchViewCommand.Execute(ManagerItems.First());
        }

        #region property

        Mediation Mediation { get; set; }

        public SmileVideoRankingManagerViewModel RankingManager { get;  }
        public SmileVideoSearchManagerViewModel SearchManager { get; }
        public IEnumerable<SmileVideoManagerViewModelBase> ManagerItems => new SmileVideoManagerViewModelBase[] { SearchManager, RankingManager };

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

        public async void Temp_OpenPlayer(string videoId)
        {
            var vm = new SmileVideoPlayerViewModel(Mediation);
            var window = new SmileVideoPlayerWindow() {
                DataContext = vm,
            };
            vm.SetView(window);
            window.Show();

            await vm.LoadAsync(videoId, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan);
        }

        public void Temp_Find(string videoId)
        {
            var ss = new ContentsSearch(Mediation);
            ss.GetAsnc(
                Define.Service.Smile.SmileContentsSearchService.Video, 
                "ACV", 
                Define.Service.Smile.SmileContentsSearchType.Tag, 
                Define.Service.Smile.SmileContentsSearchField.ViewCounter,
                new [] {
                Define.Service.Smile.SmileContentsSearchField.ContentId,
                Define.Service.Smile.SmileContentsSearchField.Title,
                Define.Service.Smile.SmileContentsSearchField.Description,
                Define.Service.Smile.SmileContentsSearchField.StartTime,
                Define.Service.Smile.SmileContentsSearchField.Tags,
                Define.Service.Smile.SmileContentsSearchField.CategoryTags,
                Define.Service.Smile.SmileContentsSearchField.CommentCounter,
                Define.Service.Smile.SmileContentsSearchField.ViewCounter,
                Define.Service.Smile.SmileContentsSearchField.MylistCounter,
                Define.Service.Smile.SmileContentsSearchField.StartTime,
                },
                Library.SharedLibrary.Define.OrderBy.Asc, 
                0, 
                100
            ).ContinueWith(task => {
            });
        }

        public async void OpenPlayer(string videoId)
        {
            var vm = new SmileVideoPlayerViewModel(Mediation);
            var window = new SmileVideoPlayerWindow() {
                DataContext = vm,
            };
            vm.SetView(window);
            window.Show();

            await vm.LoadAsync(videoId, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan);
        }

        #endregion
    }
}
