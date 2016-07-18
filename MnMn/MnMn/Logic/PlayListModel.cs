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
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    public class PlayListModel<TModel>: CollectionModel<TModel>
    {
        #region variable
        #endregion

        public PlayListModel()
            : base()
        { }

        #region property

        /// <summary>
        /// 現在アイテム。
        /// </summary>
        public TModel CurrenItem { get; private set; }

        int CurrenIndex { get; set; }

        /// <summary>
        /// プレイリストはランダム移動か。
        /// </summary>
        public bool IsRandomPlay { get; set; }

        /// <summary>
        /// プレイリストは変更可能か。
        /// </summary>
        public bool CanItemChange { get { return Count > 1; } }

        #endregion

        #region function

        public TModel ChangeNextItem()
        {
            CheckUtility.Enforce(CanItemChange);

            var index = CurrenIndex;
            if(index == Count - 1) {
                index = 0;
            } else if(index >= 0) {
                index += 1;
            } else {
                index = 0;
            }

            return ChangeItem(index);
        }

        public TModel ChangePrevItem()
        {
            CheckUtility.Enforce(CanItemChange);

            var index = CurrenIndex;
            if(index == 0) {
                index = Count - 1;
            } else if(0 < index) {
                index -= 1;
            } else {
                index = 0;
            }

            return ChangeItem(index);
        }

        TModel ChangeItem(int index)
        {
            var targetItem = Items[index];

            CurrenItem = targetItem;
            CurrenIndex = index;

            return CurrenItem;
        }

        #endregion

        #region CollectionModel

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Add | e.Action == NotifyCollectionChangedAction.Reset) {
                if(CurrenItem == null) {
                    CurrenItem = this.FirstOrDefault();
                } else if(Items.IndexOf(CurrenItem) == -1) {
                    CurrenItem = this.FirstOrDefault();
                }
                CurrenIndex = Items.IndexOf(CurrenItem);
            }

            base.OnCollectionChanged(e);
        }

        #endregion
    }
}
