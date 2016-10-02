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
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility
{
    /// <summary>
    /// ItemsControlのItem移動とか共通部分処理。
    /// </summary>
    public static class ItemsControlUtility
    {
        #region define

        public const int notFoundNextIndex = -1;

        #endregion

        #region function

        public static int GetNextIndex<T>(CollectionModel<T> items, T currentItem, bool isUp)
        {
            var currentItemIndex = items.IndexOf(currentItem);
            if(isUp && currentItemIndex == 0) {
                return notFoundNextIndex;
            }
            if(!isUp && currentItemIndex == items.Count - 1) {
                return notFoundNextIndex;
            }
            var nextIndex = isUp
                ? currentItemIndex - 1
                : currentItemIndex + 1
            ;
            return nextIndex;
        }

        public static int GetNextIndex<TModel, TViewModel>(MVMPairCollectionBase<TModel, TViewModel> items, TViewModel currentItem, bool isUp)
            where TModel : ModelBase
            where TViewModel : ViewModelBase
        {
            return GetNextIndex(items.ViewModelList, currentItem, isUp);
        }

        public static bool CanMoveNext<T>(CollectionModel<T> items, T currentItem, bool isUp)
        {
            return GetNextIndex(items, currentItem, isUp) != notFoundNextIndex;
        }

        public static bool CanMoveNext<TModel, TViewModel>(MVMPairCreateDelegationCollection<TModel, TViewModel> items, TViewModel currentItem, bool isUp)
            where TModel : ModelBase
            where TViewModel : ViewModelBase
        {
            return GetNextIndex(items, currentItem, isUp) != notFoundNextIndex;
        }

        public static void MoveItem<T>(CollectionModel<T> items, T currentItem, bool isUp)
        {
            var nextIndex = GetNextIndex(items, currentItem, isUp);
            if(nextIndex == notFoundNextIndex) {
                throw new ArgumentOutOfRangeException();
            }
            var currentItemIndex = items.IndexOf(currentItem);
            items.SwapIndex(currentItemIndex, nextIndex);
        }

        public static void MoveItem<TModel, TViewModel>(MVMPairCollectionBase<TModel, TViewModel> items, TViewModel currentItem, bool isUp)
            where TModel : ModelBase
            where TViewModel : ViewModelBase
        {
            var nextIndex = GetNextIndex(items.ViewModelList, currentItem, isUp);
            if(nextIndex == notFoundNextIndex) {
                throw new ArgumentOutOfRangeException();
            }
            var currentItemIndex = items.ViewModelList.IndexOf(currentItem);
            items.SwapIndex(currentItemIndex, nextIndex);
        }

        #endregion
    }
}
