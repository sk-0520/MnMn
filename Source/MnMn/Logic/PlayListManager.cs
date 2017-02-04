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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    public class PlayListManager<TModel>: CollectionModel<TModel>
    {
        #region variable
        #endregion

        public PlayListManager()
            : this(Environment.TickCount)
        { }

        public PlayListManager(int seed)
            : base()
        {
            Seed = seed;
        }

        #region property

        public int Seed { get; }

        /// <summary>
        /// 現在アイテム。
        /// </summary>
        public TModel CurrenItem { get; private set; }

        /// <summary>
        /// 現在アイテムインデックス。
        /// </summary>
        int CurrenIndex { get; set; }

        /// <summary>
        /// プレイリストはランダム移動か。
        /// </summary>
        public bool IsRandom { get; set; }

        /// <summary>
        /// プレイリストは変更可能か。
        /// </summary>
        public bool CanItemChange { get { return Count > 1; } }

        protected Dictionary<int, TModel> PlayedItems { get; } = new Dictionary<int, TModel>();

        #endregion

        #region function

        public TModel GetFirstItem()
        {
            var index = 0;
            if(IsRandom) {
                var random = new Random(Seed);
                index = random.Next(0, Count);
            }

            if(CurrenItem != null && !PlayedItems.ContainsKey(index)) {
                PlayedItems.Add(index, CurrenItem);
            }

            return ChangeItem(index);
        }

        static int ChangeSequentialNextIndex(int currentIndex, IReadOnlyCollection<TModel> items)
        {
            var index = currentIndex;
            if(index == items.Count - 1) {
                index = 0;
            } else if(index >= 0) {
                index += 1;
            } else {
                index = 0;
            }

            return index;
        }

        static int ChangeRandomNextIndex(int currenIndex, IEnumerable<TModel> items, int seed, IReadOnlyDictionary<int, TModel> playedItems)
        {
            var baseItems = items.ToList();
            var itemsCount = baseItems.Count;
            if(itemsCount == 1) {
                return 0;
            }

            var random = new Random(seed);
            int index = random.Next(0, itemsCount);
            // indexが癌になってる
            while(index == currenIndex || playedItems.ContainsKey(index)) {
                index = random.Next(0, itemsCount);
            }

            return index;
        }

        public TModel ChangeNextItem()
        {
            CheckUtility.Enforce(CanItemChange);

            if(Count == PlayedItems.Count) {
                // 初期化！
                PlayedItems.Clear();
            }
            int index;
            if(IsRandom) {
                index = ChangeRandomNextIndex(CurrenIndex, this, Seed, PlayedItems);
            } else {
                index = ChangeSequentialNextIndex(CurrenIndex, this);
            }

            if(CurrenItem != null && !PlayedItems.ContainsKey(index)) {
                PlayedItems.Add(index, CurrenItem);
            }

            return ChangeItem(index);
        }

        int ChangeSequentialPrevIndex(int currenIndex, IReadOnlyCollection<TModel> items)
        {
            var index = currenIndex;
            if(index == 0) {
                index = Count - 1;
            } else if(0 < index) {
                index -= 1;
            } else {
                index = 0;
            }

            return index;
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
                } else {
                    var index = Items.IndexOf(CurrenItem);
                    if(index == -1) {
                        CurrenItem = this.FirstOrDefault();
                    } else {
                        CurrenItem = this[index];
                    }
                }
                CurrenIndex = Items.IndexOf(CurrenItem);
            }

            base.OnCollectionChanged(e);
            base.OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(CanItemChange)));
        }

        #endregion
    }
}