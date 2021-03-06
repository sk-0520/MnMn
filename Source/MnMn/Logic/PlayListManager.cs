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
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;

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

        /// <summary>
        /// 現在アイテムはプレイリスト内で最終アイテムか。
        /// </summary>
        public bool IsLastItem
        {
            get
            {
                if(!CanItemChange) {
                    throw new InvalidOperationException(nameof(CanItemChange));
                }

                if(IsRandom) {
                    return Count == PlayedItems.Count;
                } else {
                    return CurrenItem.Equals(Items.Last());
                }

            }
        }

        protected bool CalledFirstItem { get; set; }

        #endregion

        #region function

        /// <summary>
        /// 一番最初に呼び出す必要あり。
        /// </summary>
        /// <returns></returns>
        public TModel GetFirstItem()
        {
            CalledFirstItem = true;

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
            var baseItems = items.ToEvaluatedSequence();
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
            CheckUtility.Enforce(CalledFirstItem);

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
            CheckUtility.Enforce(CalledFirstItem);

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

        public void ChangeCurrentItem(TModel item)
        {
            CheckUtility.Enforce(CalledFirstItem);

            var index = Items.IndexOf(item);
            if(index != -1) {
                ChangeItem(index);
            }
        }

        void RefreshCurrentIndex()
        {
            CurrenIndex = Items.IndexOf(CurrenItem);
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
                RefreshCurrentIndex();
            }

            base.OnCollectionChanged(e);
            base.OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(CanItemChange)));
        }

        protected override void RemoveItem(int index)
        {
            var targetItem = Items[index];

            base.RemoveItem(index);

            PlayedItems.Remove(index);

            RefreshCurrentIndex();
        }


        #endregion
    }
}