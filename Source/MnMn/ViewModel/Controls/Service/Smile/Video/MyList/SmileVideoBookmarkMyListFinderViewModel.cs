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
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.MyList
{
    public class SmileVideoBookmarkMyListFinderViewModel: SmileVideoItemsMyListFinderViewModel
    {
        public SmileVideoBookmarkMyListFinderViewModel(Mediation mediation, SmileMyListBookmarkItemModel item)
            : base(mediation, item)
        {
            BookmarkItem = item;
        }

        #region property

        SmileMyListBookmarkItemModel BookmarkItem { get; }

        public string MyListCustomName
        {
            get { return BookmarkItem.MyListCustomName; }
            set { SetPropertyValue(BookmarkItem, value, nameof(BookmarkItem.MyListCustomName)); }
        }

        #endregion

        #region command

        public ICommand RefreshBookmarkItemCommand
        {
            get
            {
                return CreateCommand(
                    o => {

                    }
                );
            }
        }


        #endregion

        #region SmileVideoItemsMyListFinderViewModel

        public override string MyListName
        {
            get { return BookmarkItem?.MyListCustomName ?? base.MyListName; }
        }

        #endregion

    }
}
