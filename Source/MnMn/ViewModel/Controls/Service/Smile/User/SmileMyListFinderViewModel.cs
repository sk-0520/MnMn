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
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.MyList;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.User
{
    /// <summary>
    /// やっぱサービス分けミスってんなぁ。
    /// </summary>
    public class SmileMyListFinderViewModel: SmileVideoMyListFinderViewModelBase
    {
        public SmileMyListFinderViewModel(Mediator mediation, RawSmileUserMyListGroupModel group)
            : base(mediation, false)
        {
            MyListGroup = group;
            InitializeCacheData();
        }

        #region property

        RawSmileUserMyListGroupModel MyListGroup { get; }

        #endregion

        #region SmileVideoMyListFinderViewModelBase

        public override string MyListId
        {
            get { return MyListGroup?.MyListId; }
        }

        public override string MyListName
        {
            get { return MyListGroup?.Title; }
        }

        public override int MyListItemCount
        {
            get { return MyListGroup.Count; }
        }

        #endregion
    }
}
