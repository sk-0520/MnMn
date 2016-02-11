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
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public class SmileVideoSearchItemViewModel: SmileVideoFinderViewModelBase
    {
        #region variable

        int _totalCount;

        #endregion

        public SmileVideoSearchItemViewModel(Mediation mediation, SmileVideoSearchModel searchModel, SmileVideoElementModel method, SmileVideoElementModel sort, SmileVideoElementModel type, string query, int index, int count)
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

        public SmileVideoElementModel Method { get; }
        public SmileVideoElementModel Sort { get; }
        public SmileVideoElementModel Type { get; }
        public string Query { get; }

        public int Index { get; }
        public int Count { get; }

        public int TotalCount
        {
            get { return this._totalCount; }
            set { SetVariableValue(ref this._totalCount, value); }
        }

        #endregion

        #region function

        public Task LoadAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan)
        {
            var search = new ContentsSearch(Mediation);
            return search.GetAsnc(
                SearchModel.Service,
                Query,
                Type.Key,
                Method.Key,
                SearchModel.Results,
                Sort.Key,
                Index,
                Count
            ).ContinueWith(task => {
                var result = task.Result;
                TotalCount = RawValueUtility.ConvertInteger(result.Meta.TotalCount);
                var list = result.Data.Select((item, index) => new SmileVideoInformationViewModel(Mediation, item, index + Index + 1));
                return list;
            }).ContinueWith(task => {
                var list = task.Result;
                VideoInformationList.InitializeRange(list);
            }, TaskScheduler.FromCurrentSynchronizationContext()).ContinueWith(task => {
                var loader = new SmileVideoInformationLoader(VideoInformationList);
                return loader.LoadInformationAsync(thumbCacheSpan);
            }).ContinueWith(task => {
                var loader = new SmileVideoInformationLoader(VideoInformationList);
                loader.LoadThumbnaiImageAsync(imageCacheSpan);
            });
        }

        #endregion

        #region ViewModelBase

        public override bool CanLoad
        {
            get
            {
                return true;
            }
        }

        #endregion
    }
}
