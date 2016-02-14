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
using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public class SmileVideoNewArrivalsManagerViewModel: SmileVideoManagerViewModelBase
    {
        #region variable

        SmileVideoNewArrivalsFinderViewModel _selectedItem;

        #endregion

        public SmileVideoNewArrivalsManagerViewModel(Mediation mediation)
            : base(mediation)
        {
            NewArrival = new SmileVideoNewArrivalsFinderViewModel(Mediation, SmileVideoMediationKey.newarrival);
            Recent = new SmileVideoNewArrivalsFinderViewModel(Mediation, SmileVideoMediationKey.recent);
            Hotlist = new SmileVideoHotlistFinderViewModel(Mediation, SmileVideoMediationKey.hotlist);

            ItemsList = new CollectionModel<SmileVideoNewArrivalsFinderViewModel>(new[] {
                NewArrival,
                Recent,
                Hotlist,
            });
        }

        #region property

        SmileVideoNewArrivalsFinderViewModel NewArrival { get; }
        SmileVideoNewArrivalsFinderViewModel Recent { get; }
        SmileVideoNewArrivalsFinderViewModel Hotlist { get; }

        public CollectionModel<SmileVideoNewArrivalsFinderViewModel> ItemsList { get; }
        public SmileVideoNewArrivalsFinderViewModel SelectedItem
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

        #region SmileVideoManagerViewModelBase

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
