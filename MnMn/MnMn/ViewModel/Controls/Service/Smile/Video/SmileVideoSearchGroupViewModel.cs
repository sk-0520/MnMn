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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public class SmileVideoSearchGroupViewModel:ViewModelBase
    {
        #region variable

        SmileVideoElementModel _selectedMethod;
        SmileVideoElementModel _selectedSort;
        SmileVideoElementModel _selectedType;

        ICollectionView _selectedVideoInformationItems;

        #endregion

        public SmileVideoSearchGroupViewModel(Mediation mediation, SmileVideoSearchModel searchModel, SmileVideoElementModel method, SmileVideoElementModel sort, SmileVideoElementModel type, string query)
        {
            Mediation = mediation;
            SearchModel = searchModel;
            Query = query;

            SetContextElements(method, sort, type);
        }

        #region property

        Mediation Mediation { get; }
        SmileVideoSearchModel SearchModel { get; }

        public IList<SmileVideoElementModel> MethodItems => SearchModel.Methods;
        public IList<SmileVideoElementModel> SortItems => SearchModel.Sort;
        public IList<SmileVideoElementModel> TypeItems => SearchModel.Type;

        public string Query { get; }

        public SmileVideoElementModel SelectedMethod
        {
            get { return this._selectedMethod; }
            set { SetVariableValue(ref this._selectedMethod, value); }
        }
        public SmileVideoElementModel SelectedSort
        {
            get { return this._selectedSort; }
            set { SetVariableValue(ref this._selectedSort, value); }
        }
        public SmileVideoElementModel SelectedType
        {
            get { return this._selectedType; }
            set { SetVariableValue(ref this._selectedType, value); }
        }

        public CollectionModel<SmileVideoSearchItemViewModel> SearchItems { get; } = new CollectionModel<SmileVideoSearchItemViewModel>();

        public ICollectionView SelectedVideoInformationItems {
            get { return this._selectedVideoInformationItems; }
            set { SetVariableValue(ref this._selectedVideoInformationItems, value); }
        }

        #endregion

        #region command
        #endregion

        #region function

        SmileVideoElementModel GetContextElemetFromChangeElement(IEnumerable<SmileVideoElementModel> items, SmileVideoElementModel element)
        {
            if(items.Any(i => i == element)) {
                return element;
            } else {
                return items.FirstOrDefault(i => i.Key == element.Key);
            }
        }

        public void SetContextElements(SmileVideoElementModel method, SmileVideoElementModel sort, SmileVideoElementModel type)
        {
            SelectedMethod = GetContextElemetFromChangeElement(MethodItems, method);
            SelectedSort = GetContextElemetFromChangeElement(SortItems, sort);
            SelectedType = GetContextElemetFromChangeElement(TypeItems, type);
        }

        public Task LoadAsync()
        {
            var vm = new SmileVideoSearchItemViewModel(Mediation, SearchModel, SelectedMethod, SelectedSort, SelectedType, Query, 0, 100);
            return vm.LoadAsync().ContinueWith(task => {
                SearchItems.Add(vm);
                SelectedVideoInformationItems = vm.VideoInformationItems;
            });
        }

        #endregion
    }
}
