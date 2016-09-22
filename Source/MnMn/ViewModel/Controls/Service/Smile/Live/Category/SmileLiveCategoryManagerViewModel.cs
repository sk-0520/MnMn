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
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Live.Api;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Live;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Live
{
    public class SmileLiveCategoryManagerViewModel: SmileLiveCustomManagerViewModelBase
    {
        #region variable

        DefinedElementModel _selectedSortItem;
        DefinedElementModel _selectedOrderItem;
        DefinedElementModel _selectedCategoryItem;

        #endregion

        public SmileLiveCategoryManagerViewModel(Mediation mediation)
            : base(mediation)
        {
            CategoryModel = Mediation.GetResultFromRequest<SmileLiveCategoryModel>(new RequestModel(RequestKind.CategoryDefine, ServiceType.SmileLive));
            SelectedSortItem = SortItems.First();
            SelectedOrderItem = OrderItems.First();
            SelectedCategoryItem = CategoryItems.First();
        }

        #region property

        SmileLiveCategoryModel CategoryModel { get; }

        public IList<DefinedElementModel> SortItems { get { return CategoryModel.SortItems; } }
        public IList<DefinedElementModel> OrderItems { get { return CategoryModel.OrderItems; } }
        public IList<DefinedElementModel> CategoryItems { get { return CategoryModel.CategoryItems; } }

        public DefinedElementModel SelectedSortItem
        {
            get { return this._selectedSortItem; }
            set { SetVariableValue(ref this._selectedSortItem, value); }
        }

        public DefinedElementModel SelectedOrderItem
        {
            get { return this._selectedOrderItem; }
            set { SetVariableValue(ref this._selectedOrderItem, value); }
        }

        public DefinedElementModel SelectedCategoryItem
        {
            get { return this._selectedCategoryItem; }
            set { SetVariableValue(ref this._selectedCategoryItem, value); }
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
            var nowSortItem = SelectedSortItem;
            var nowOrderItem = SelectedOrderItem;
            var nowCategory = SelectedCategoryItem;

            return SearchCoreAsync(nowSortItem, nowOrderItem, nowCategory);
        }

        Task SearchCoreAsync(DefinedElementModel sort, DefinedElementModel order, DefinedElementModel category)
        {
            var test = new Logic.Service.Smile.Live.Api.Category(Mediation);
            var a = test.LoadAsync(category.Key, sort.Key, order.Key, 1);
            a.ContinueWith(t => {
                SerializeUtility.SaveXmlSerializeToFile("z:\\a.xml", t.Result);
            });
            return Task.CompletedTask;
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
        { }

        #endregion
    }
}
