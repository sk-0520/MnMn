﻿/*
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

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Live
{
    public abstract class SmileLiveFinderViewModelBase: FinderViewModelBase<SmileLiveInformationViewModel, SmileLiveFinderItemViewModel>
    {
        public SmileLiveFinderViewModelBase(Mediation mediation)
            : base(mediation)
        { }

        #region property
        #endregion

        #region command
        #endregion

        #region function



        #endregion

        #region FinderViewModelBase

        public override CacheSpan DefaultInformationCacheSpan => Constants.ServiceSmileLiveInformationCacheSpan;
        public override CacheSpan DefaultImageCacheSpan => Constants.ServiceSmileLiveImageCacheSpan;
        public override object DefaultExtends { get; } = null;

        protected override Task LoadInformationAsync(IEnumerable<SmileLiveInformationViewModel> informationItems, CacheSpan informationCacheSpan)
        {
            throw new NotImplementedException();
        }

        protected override Task LoadImageAsync(IEnumerable<SmileLiveInformationViewModel> informationItems, CacheSpan imageCacheSpan)
        {
            throw new NotImplementedException();
        }

        protected override Task LoadCoreAsync(CacheSpan informationCacheSpan, CacheSpan imageCacheSpan, object extends)
        {
            throw new NotImplementedException();
        }

        internal override void ChangeSortItems()
        {
            throw new NotImplementedException();
        }

        protected override SmileLiveFinderItemViewModel CreateFinderItem(SmileLiveInformationViewModel information, int number)
        {
            return new SmileLiveFinderItemViewModel(information, number);
        }


        #endregion
    }
}
