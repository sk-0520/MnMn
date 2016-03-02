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
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Search
{
    public class SmileVideoSearchManagerViewModel: SmileVideoCustomManagerViewModelBase
    {
        #region property

        DefinedElementModel _selectedMethod;
        DefinedElementModel _selectedSort;
        DefinedElementModel _selectedType;

        string _inputQuery = "ACV";

        SmileVideoSearchGroupViewModel _selectedSearchGroup;

        #endregion

        public SmileVideoSearchManagerViewModel(Mediation mediation, SmileVideoSearchModel searchModel, SmileVideoSettingModel setting)
            : base(mediation)
        {
            SearchModel = searchModel;

            SelectedMethod = MethodItems.First();
            SelectedSort = SortItems.First();
            SelectedType = TypeItems.First();
        }


        #region property

        SmileVideoSearchModel SearchModel { get; }

        public IList<DefinedElementModel> MethodItems => SearchModel.Methods;
        public IList<DefinedElementModel> SortItems => SearchModel.Sort;
        public IList<DefinedElementModel> TypeItems => SearchModel.Type;

        public CollectionModel<SmileVideoSearchGroupViewModel> SearchGroups { get; } = new CollectionModel<SmileVideoSearchGroupViewModel>();

        public DefinedElementModel SelectedMethod
        {
            get { return this._selectedMethod; }
            set { SetVariableValue(ref this._selectedMethod, value); }
        }
        public DefinedElementModel SelectedSort
        {
            get { return this._selectedSort; }
            set { SetVariableValue(ref this._selectedSort, value); }
        }
        public DefinedElementModel SelectedType
        {
            get { return this._selectedType; }
            set { SetVariableValue(ref this._selectedType, value); }
        }

        public string InputQuery
        {
            get { return this._inputQuery; }
            set { SetVariableValue(ref this._inputQuery, value); }
        }

        public SmileVideoSearchGroupViewModel SelectedSearchGroup
        {
            get { return this._selectedSearchGroup; }
            set {
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

        public ICommand SearchCommand
        {
            get
            {
                return CreateCommand(o => {
                    LoadSearchAsync().ConfigureAwait(false);
                });
            }
        }

        #endregion

        #region function

        public Task LoadSearchAsync()
        {
            var nowMethod = SelectedMethod;
            var nowSort = SelectedSort;
            var nowType = SelectedType;
            var nowQuery = InputQuery;

            return LoadSearchAsync(nowMethod, nowSort, nowType, nowQuery);
        }
        Task LoadSearchAsync(DefinedElementModel method, DefinedElementModel sort, DefinedElementModel type, string query)
        {
            // 存在する場合は該当タブへ遷移
            var selectViewModel = RestrictUtility.IsNotNull(
                SearchGroups.FirstOrDefault(i => i.Query == query && i.Type.Key == type.Key),
                viewModel => {
                    viewModel.SetContextElements(method, sort);
                    return viewModel;
                },
                () => {
                    var viewModel = new SmileVideoSearchGroupViewModel(Mediation, SearchModel, method, sort, type, query);
                    SearchGroups.Insert(0, viewModel);
                    return viewModel;
                }
            );
            return selectViewModel.LoadAsync(Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan, true).ContinueWith(task => {
                SelectedSearchGroup = selectViewModel;
            });
        }

        #endregion

        #region ManagerViewModelBase

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
