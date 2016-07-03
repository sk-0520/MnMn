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
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.History
{
    public class SmileVideoHistoryManagerViewModel: SmileVideoCustomManagerViewModelBase
    {
        #region variable

        SmileVideoFinderViewModelBase _selectedItem;

        #endregion

        public SmileVideoHistoryManagerViewModel(Mediation mediation)
            : base(mediation)
        {
            Session = Mediation.GetResultFromRequest<SmileSessionViewModel>(new RequestModel(RequestKind.Session, ServiceType.Smile));

            AccountHistory = new SmileVideoAccountHistoryFinderViewModel(Mediation);
            ApplicationHistory = new SmileVideoApplicationHistoryFinderViewModel(Mediation);

            ItemsList = new CollectionModel<SmileVideoFinderViewModelBase>() {
                AccountHistory,
                ApplicationHistory,
            };
        }

        #region property

        SmileSessionViewModel Session { get; }

        //LoadPageHtmlDocument
        public SmileVideoAccountHistoryFinderViewModel AccountHistory { get; }
        public SmileVideoApplicationHistoryFinderViewModel ApplicationHistory { get; }

        #endregion

        #region command
        #endregion

        #region function

        public CollectionModel<SmileVideoFinderViewModelBase> ItemsList { get; }

        public SmileVideoFinderViewModelBase SelectedItem
        {
            get { return this._selectedItem; }
            set
            {
                var prevItem = this._selectedItem;
                if(SetVariableValue(ref this._selectedItem, value)) {
                    if(prevItem != null && this._selectedItem.CanLoad) {
                        this._selectedItem.LoadDefaultCacheAsync();
                    }
                }
            }
        }

        #endregion

        #region SmileVideoCustomManagerViewModelBase

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        protected override void ShowView()
        {
            base.ShowView();

            if(SelectedItem != null) {
                return;
            }

            var target = ItemsList.First();
            target.LoadDefaultCacheAsync().ContinueWith(task => {
                SelectedItem = target;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion
    }
}
