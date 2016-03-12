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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api.V2;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Search
{
    public class SmileVideoSearchItemFinderViewModel: SmileVideoFinderViewModelBase
    {
        #region variable

        int _totalCount;

        #endregion

        public SmileVideoSearchItemFinderViewModel(Mediation mediation, SmileVideoSearchModel searchModel, DefinedElementModel method, DefinedElementModel sort, DefinedElementModel type, string query, int index, int count)
            :base(mediation)
        {
            SearchModel = searchModel;

            Method = method;
            Sort = sort;
            Type = type;
            Query = query;

            Index = index;
            Count = count;
        }

        #region property

        SmileVideoSearchModel SearchModel { get; }

        public DefinedElementModel Method { get; }
        public DefinedElementModel Sort { get; }
        public DefinedElementModel Type { get; }
        public string Query { get; }

        public int Index { get; }
        public int Count { get; }

        public int TotalCount
        {
            get { return this._totalCount; }
            set { SetVariableValue(ref this._totalCount, value); }
        }

        #endregion

        #region SmileVideoFinderViewModelBase

        protected override Task LoadAsync_Impl(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan, object extends)
        {
            var search = new ContentsSearch(Mediation);

            return search.SearchAsync(
                SearchModel.Service,
                Query,
                Type.Key,
                Method.Key,
                SearchModel.Results,
                Sort.Key,
                Index,
                Count
            ).ContinueWith(task => {
                FinderLoadState = SmileVideoFinderLoadState.VideoSourceChecking;
                var result = task.Result;
                TotalCount = RawValueUtility.ConvertInteger(result.Meta.TotalCount);
                var list = result.Data.Select((item, index) => new SmileVideoInformationViewModel(Mediation, item, index + Index + 1));
                return list;
            }).ContinueWith(task => {
                var list = task.Result;
                SetItems(list);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion
    }
}