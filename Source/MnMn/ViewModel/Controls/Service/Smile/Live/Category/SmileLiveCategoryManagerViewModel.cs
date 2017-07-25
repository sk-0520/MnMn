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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Live.Api;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Live;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Live.Category;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Live
{
    public class SmileLiveCategoryManagerViewModel: SmileLiveCustomManagerViewModelBase
    {
        #region variable

        DefinedElementModel _selectedSort;
        DefinedElementModel _selectedOrder;
        DefinedElementModel _selectedCategory;

        SmileLiveCategoryGroupFinderViewModel _selectedSearchGroup;

        #endregion

        public SmileLiveCategoryManagerViewModel(Mediator mediation)
            : base(mediation)
        {
            CategoryModel = Mediation.GetResultFromRequest<SmileLiveCategoryModel>(new RequestModel(RequestKind.CategoryDefine, ServiceType.SmileLive));
            SelectedSort = SortItems.First();
            SelectedOrder = OrderItems.First();
            var selectedCategory = CategoryModel.CategoryItems.First();
            SelectedCategory = selectedCategory;
            CategoryItems = CategoryModel.CategoryItems;
        }

        #region property

        SmileLiveCategoryModel CategoryModel { get; }

        public CollectionModel<SmileLiveCategoryGroupFinderViewModel> CategoryGroups { get; } = new CollectionModel<SmileLiveCategoryGroupFinderViewModel>();

        public IList<DefinedElementModel> SortItems { get { return CategoryModel.SortItems; } }
        public IList<DefinedElementModel> OrderItems { get { return CategoryModel.OrderItems; } }
        public IList<DefinedElementModel> CategoryItems { get; }

        public DefinedElementModel SelectedSort
        {
            get { return this._selectedSort; }
            set
            {
                if(SetVariableValue(ref this._selectedSort, value)) {
                    if(this.SelectedSort != null && CategoryItems != null) {
                        SearchAsync().ConfigureAwait(false);
                    }
                }
            }
        }

        public DefinedElementModel SelectedOrder
        {
            get { return this._selectedOrder; }
            set
            {
                if(SetVariableValue(ref this._selectedOrder, value)) {
                    if(this.SelectedOrder != null && CategoryItems != null) {
                        SearchAsync().ConfigureAwait(false);
                    }
                }
            }
        }

        public DefinedElementModel SelectedCategory
        {
            get { return this._selectedCategory; }
            set
            {
                if(SetVariableValue(ref this._selectedCategory, value)) {
                    if(this.SelectedCategory != null && CategoryItems != null) {
                        SearchAsync().ConfigureAwait(false);
                    }
                }
            }
        }

        public SmileLiveCategoryGroupFinderViewModel SelectedCategoryGroup
        {
            get { return this._selectedSearchGroup; }
            set
            {
                if(SetVariableValue(ref this._selectedSearchGroup, value)) {
                    if(this._selectedSearchGroup != null) {
                        //var items = this._selectedSearchGroup.SearchItems;
                        //this._selectedSearchGroup.SearchItems.InitializeRange(items);
                    }
                }
            }
        }


        #endregion

        #region command

        public ICommand SearchCategoryCommand
        {
            get { return CreateCommand(o => SearchAsync()); }
        }

        #endregion

        #region function

        public Task SearchAsync()
        {
            var nowSortItem = SelectedSort;
            var nowOrderItem = SelectedOrder;
            var nowCategory = SelectedCategory;

            return SearchCoreAsync(nowSortItem, nowOrderItem, nowCategory);
        }

        Task SearchCoreAsync(DefinedElementModel sort, DefinedElementModel order, DefinedElementModel category)
        {
            //var test = new Logic.Service.Smile.Live.Api.Category(Mediation);
            //var a = test.LoadAsync(category.Key, sort.Key, order.Key, 1);
            //a.ContinueWith(t => {
            //    SerializeUtility.SaveXmlSerializeToFile("z:\\a.xml", t.Result);
            //});
            //return Task.CompletedTask;

            // 存在する場合は該当タブへ遷移
            var selectViewModel = RestrictUtility.IsNotNull(
                CategoryGroups.FirstOrDefault(g => g.Category.Key == category.Key),
                viewModel => {
                    viewModel.SetContextElements(sort, order);
                    return viewModel;
                },
                () => {
                    var finder = new SmileLiveCategoryGroupFinderViewModel(Mediation, CategoryModel, sort, order, category);
                    CategoryGroups.Add(finder);
                    return finder;
                }
            );

            SelectedCategoryGroup = selectViewModel;

            return SelectedCategoryGroup.LoadAsync(Constants.ServiceSmileLiveInformationCacheSpan, Constants.ServiceSmileLiveImageCacheSpan, true);
        }

        #endregion

        #region SmileLiveCustomManagerViewModelBase

        public override Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan, bool force)
        {
            return GarbageCollectionDummyResult;
        }

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override Task UninitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override void InitializeView(MainWindow view)
        { }

        public override void UninitializeView(MainWindow view)
        { }

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return Enumerable.Empty<ManagerViewModelBase>();
        }

        protected override void HideViewCore()
        { }

        protected override void ShowViewCore()
        {
            if(!CategoryGroups.Any()) {
                SearchAsync();
            }
        }

        #endregion
    }
}
