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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api.V2;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Search
{
    public class SmileVideoSearchItemFinderViewModel : SmileVideoFinderViewModelBase
    {
        #region variable

        int _totalCount;

        #endregion

        public SmileVideoSearchItemFinderViewModel(Mediation mediation, SmileVideoSearchModel searchModel, IReadOnlyDefinedElement method, IReadOnlyDefinedElement sort, SearchType type, string query, int index, int count)
            : base(mediation, index)
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

        public IReadOnlyDefinedElement Method { get; }
        public IReadOnlyDefinedElement Sort { get; }
        public SearchType Type { get; }
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

        Task LoadContentsSearchAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan, object extends)
        {
            var search = new ContentsSearch(Mediation);

            var typeElement = SmileVideoSearchUtility.GetSearchTypeFromElements(SearchModel.GetDefaultSearchTypeDefine().Type, Type);

            return search.SearchAsync(
                SearchModel.Contents.Service,
                Query,
                typeElement.Key,
                Method.Key,
                SearchModel.Contents.Results,
                Sort.Key,
                Index,
                Count
            ).ContinueWith(task => {
                FinderLoadState = SourceLoadState.SourceChecking;
                var result = task.Result;
                TotalCount = RawValueUtility.ConvertInteger(result.Meta.TotalCount);

                var list = result.Data.Select(item => {
                    var request = new SmileVideoInformationCacheRequestModel(new SmileVideoInformationCacheParameterModel(item));
                    return Mediation.GetResultFromRequest<SmileVideoInformationViewModel>(request);
                });
                return list;
            }).ContinueWith(task => {
                var list = task.Result;
                return SetItemsAsync(list, thumbCacheSpan);
            }, CancelLoading.Token, TaskContinuationOptions.AttachedToParent, TaskScheduler.Current);
        }

        Task LoadOfficialSearchAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan, object extends)
        {
            var search = new Logic.Service.Smile.Video.Api.V1.Search(Mediation);

            var typeElement = SmileVideoSearchUtility.GetSearchTypeFromElements(SearchModel.GetDefaultSearchTypeDefine().Type, Type);

            return search.SearchAsync(
                Query,
                typeElement.Key,
                Method.Key,
                Sort.Key,
                (Index / SearchModel.GetDefaultSearchTypeDefine().MaximumCount) + 1,
                Count
            ).ContinueWith(t => {
                FinderLoadState = SourceLoadState.SourceChecking;
                var result = t.Result;
                TotalCount = RawValueUtility.ConvertInteger(result.Count);

                var list = result.List.Select(item => {
                    var request = new SmileVideoInformationCacheRequestModel(new SmileVideoInformationCacheParameterModel(item));
                    return Mediation.GetResultFromRequest<SmileVideoInformationViewModel>(request);
                });
                return list;
            }).ContinueWith(task => {
                var list = task.Result;
                return SetItemsAsync(list, thumbCacheSpan);
            });
        }
        #endregion

        #region SmileVideoFinderViewModelBase

        protected override Task LoadCoreAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan, object extends)
        {
            switch(Constants.ServiceSmileVideoSearchType) {
                case SmileVideoSearchType.Contents:
                    return LoadContentsSearchAsync(thumbCacheSpan, imageCacheSpan, extends);

                case SmileVideoSearchType.Official:
                    return LoadOfficialSearchAsync(thumbCacheSpan, imageCacheSpan, extends);

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
