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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public class SmileVideoMyListManagerViewModel: SmileVideoManagerViewModelBase
    {
        #region variable

        SmileVideoMyListFinderViewModel _selectedFinder;

        #endregion

        public SmileVideoMyListManagerViewModel(Mediation mediation)
            : base(mediation)
        { }

        #region property

        public CollectionModel<SmileVideoMyListFinderViewModel> AccountMyList { get; } = new CollectionModel<SmileVideoMyListFinderViewModel>();

        public SmileVideoMyListFinderViewModel SelectedFinder
        {
            get { return this._selectedFinder; }
            set
            {
                if(SetVariableValue(ref this._selectedFinder, value)) {
                }
            }
        }

        #endregion

        #region command

        public ICommand ReloadAccountMyListCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        LoadAccountMyListAsync().ConfigureAwait(false);
                    }
                );
            }
        }

        #endregion

        #region function

        public async Task LoadAccountMyListAsync()
        {
            var list = new List<SmileVideoMyListFinderViewModel>();

            // とりあえずマイリスト
            var defaultMyList = new SmileVideoAccountMyListDefaultFinderViewModel(Mediation);
            var task = defaultMyList.LoadDefaultCacheAsync();
            list.Add(defaultMyList);

            // 自身のマイリスト一覧
            var session = MediationUtility.GetResultFromRequestResponse<SmileSessionViewModel>(Mediation, new RequestModel(RequestKind.Session, ServiceType.Smile));
            var mylist = new MyList(Mediation, session) {
                SessionSupport = true,
            };
            var group = await mylist.GetAccountGroupAsync();
            if(MyListUtility.IsSuccessResponse(group) && group.Groups.Any()) {
                
            }

            AccountMyList.InitializeRange(list);

            SelectedFinder = defaultMyList;
            Debug.WriteLine(group);
        }

        #endregion

    }
}
