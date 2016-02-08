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
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public class SmileVideoSearchManagerViewModel: SmileVideoManagerViewModelBase
    {
        #region property

        SmileVideoElementModel _selectedMethod;
        SmileVideoElementModel _selectedSort;
        SmileVideoElementModel _selectedType;

        string _inputQuery = "ACV";

        #endregion

        public SmileVideoSearchManagerViewModel(Mediation mediation, SmileVideoSearchModel searchModel)
            : base(mediation)
        {
            SearchModel = searchModel;

            SelectedMethod = MethodItems.First();
            SelectedSort = SortItems.First();
            SelectedType = TypeItems.First();
        }


        #region property

        SmileVideoSearchModel SearchModel { get; }

        public IList<SmileVideoElementModel> MethodItems => SearchModel.Methods;
        public IList<SmileVideoElementModel> SortItems => SearchModel.Sort;
        public IList<SmileVideoElementModel> TypeItems => SearchModel.Type;

        public CollectionModel<SmileVideoSearchGroupViewModel> SearchGroupItems { get; } = new CollectionModel<SmileVideoSearchGroupViewModel>();

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

        public string InputQuery
        {
            get { return this._inputQuery; }
            set { SetVariableValue(ref this._inputQuery, value); }
        }



        #endregion

        #region command

        public ICommand SearchCommand
        {
            get
            {
                return CreateCommand(o => {
                    var vm = new SmileVideoSearchGroupViewModel(Mediation, SearchModel, SelectedMethod, SelectedSort, SelectedType, InputQuery);
                    SearchGroupItems.Add(vm);
                    vm.LoadAsync().ContinueWith(task => {
                    });
                });
            }
        }

        #endregion
    }
}
