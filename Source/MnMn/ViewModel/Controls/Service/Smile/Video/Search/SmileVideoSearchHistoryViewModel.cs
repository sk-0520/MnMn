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
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Search
{
    public class SmileVideoSearchHistoryViewModel: SingleModelWrapperViewModelBase<SmileVideoSearchHistoryModel>
    {
        public SmileVideoSearchHistoryViewModel(SmileVideoSearchHistoryModel model)
            : base(model)
        { }

        #region property

        public string Query
        {
            get { return Model.Query; }
        }

        public SearchType SearchType
        {
            get { return Model.SearchType; }
        }

        public int TotalCount
        {
            get { return Model.TotalCount; }
            set { SetModelValue(value); }
        }

        public int Count
        {
            get { return Model.Count; }
            set { SetModelValue(value); }
        }

        public DateTime LastTimestamp
        {
            get { return Model.LastTimestamp; }
            set { SetModelValue(value); }
        }

        #endregion

        #region SingleModelWrapperViewModelBase

        public override string ToString()
        {
            return Query;
        }

        #endregion
    }
}
