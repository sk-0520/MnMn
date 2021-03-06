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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MnMnTest.Logic
{
    [TestClass]
    public class PlayListManagerTest
    {
        public class Item
        {
            public Item(int number)
            {
                Number = number;
            }
            public int Number { get; }

            public override string ToString()
            {
                return Number.ToString();
            }
        }

        [TestMethod]
        public void SequenceNextTest()
        {
            var list = new PlayListManager<Item>() {
                IsRandom = false,
            };

            list.AddRange(Enumerable.Range(1, 10).Select(i => new Item(i)));
            for(var i = 0; i < 10; i++) {

                var item = i == 0
                    ? list.GetFirstItem()
                    : list.ChangeNextItem()
                ;
                Assert.IsTrue(i + 1 == item.Number);
            }
            for(var i = 0; i < 10; i++) {
                var item = list.ChangeNextItem();
                Assert.IsTrue(i + 1 == item.Number);
            }
        }
        [TestMethod]
        public void SequencePrevTest()
        {
            var list = new PlayListManager<Item>() {
                IsRandom = false,
            };

            list.AddRange(Enumerable.Range(1, 10).Select(i => new Item(i)));

            var now = list.GetFirstItem();
            Assert.IsTrue(now.Number == 1);

            for(var i = 10; 0 < i; i--) {
                var prev = list.ChangePrevItem();
                Assert.IsTrue(prev.Number == i);
            }
        }

        [TestMethod]
        public void RandomNextTest()
        {
            foreach(var _ in Enumerable.Range(1, 100)) {
                var list = new PlayListManager<Item>() {
                    IsRandom = true,
                };

                list.AddRange(Enumerable.Range(1, 3).Select(i => new Item(i)));

                var item1 = list.GetFirstItem();
                var item2 = list.ChangeNextItem();
                var item3 = list.ChangeNextItem();

                Assert.IsTrue(item1 != item2);
                Assert.IsTrue(item1 != item3);
                Assert.IsTrue(item2 != item3);

                foreach(var __ in Enumerable.Range(1, 100)) {
                    var next1 = list.ChangeNextItem();
                    var next2 = list.ChangeNextItem();
                    var next3 = list.ChangeNextItem();

                    Assert.IsTrue(next1 != next2);
                    Assert.IsTrue(next1 != next3);
                    Assert.IsTrue(next2 != next3);
                }
            }
        }
        [TestMethod]
        public void RandomNext2Test()
        {
            var list = new PlayListManager<Item>() {
                IsRandom = true,
            };

            list.AddRange(Enumerable.Range(1, 3).Select(i => new Item(i)));

            var item1 = list.GetFirstItem();
            var item2 = list.ChangeNextItem();
            var item3 = list.ChangeNextItem();
            var items = new[] { item1, item2, item3 };

            foreach(var _ in Enumerable.Range(1, 100)) {
                var next1 = list.ChangeNextItem();
                var next2 = list.ChangeNextItem();
                var next3 = list.ChangeNextItem();
                var nexts = new[] { next1, next2, next3 };
                CollectionAssert.AreEqual(items, nexts);
            }
        }

        [TestMethod]
        public void ChangeCurrentItemTest()
        {
            var list = new PlayListManager<Item>();
            list.AddRange(Enumerable.Range(1, 10).Select(i => new Item(i)));

            var prevItem = list[4];
            var selectItem = list[5];
            var nextItem = list[6];

            list.GetFirstItem();
            list.ChangeCurrentItem(selectItem);
            Assert.IsTrue(list.CurrenItem == selectItem);
            Assert.IsTrue(list.ChangePrevItem() == prevItem);
            Assert.IsTrue(list.ChangeNextItem() == selectItem);
            Assert.IsTrue(list.ChangeNextItem() == nextItem);
        }

        //[TestMethod]
        [Obsolete]
        public void Issue705ReproduceTest()
        {
            {
                var list = new PlayListManager<Item>();
                list.AddRange(Enumerable.Range(1, 10).Select(i => new Item(i)));

                var num1 = list.GetFirstItem();
                var num2 = list.ChangeNextItem();
                var num3 = list.ChangeNextItem();
                var num4 = list.ChangeNextItem();

                // ここで古いのを破棄
                list.Remove(num1);

                var num5 = list.ChangeNextItem();
                Assert.IsTrue(num5.Number == 6);
            }
            {
                // 消した分だけずれる試験
                var list = new PlayListManager<Item>();
                list.AddRange(Enumerable.Range(1, 10).Select(i => new Item(i)));

                var num1 = list.GetFirstItem();
                var num2 = list.ChangeNextItem();
                var num3 = list.ChangeNextItem();
                var num4 = list.ChangeNextItem();

                list.Remove(num1);
                list.Remove(num2);
                list.Remove(num3);

                var num5 = list.ChangeNextItem();
                Assert.IsTrue(num5.Number == 8);
            }
            {
                // 落ちるパターン
                var list = new PlayListManager<Item>();
                list.AddRange(Enumerable.Range(1, 5).Select(i => new Item(i)));

                var num1 = list.GetFirstItem();
                var num2 = list.ChangeNextItem();
                var num3 = list.ChangeNextItem();

                list.Remove(num1);
                list.Remove(num2);
                list.Remove(num3);

                try {
                    var num4 = list.ChangeNextItem();
                    Assert.Fail();
                } catch {
                    Assert.IsTrue(true);
                }
            }
        }

        [TestMethod]
        public void Issue705Test()
        {
            {
                var list = new PlayListManager<Item>();
                list.AddRange(Enumerable.Range(1, 10).Select(i => new Item(i)));

                var num1 = list.GetFirstItem();
                var num2 = list.ChangeNextItem();
                var num3 = list.ChangeNextItem();
                var num4 = list.ChangeNextItem();

                // ここで古いのを破棄
                list.Remove(num1);

                var num5 = list.ChangeNextItem();
                Assert.IsTrue(num5.Number == 5);
            }
            {
                // 消した分だけずれる試験
                var list = new PlayListManager<Item>();
                list.AddRange(Enumerable.Range(1, 10).Select(i => new Item(i)));

                var num1 = list.GetFirstItem();
                var num2 = list.ChangeNextItem();
                var num3 = list.ChangeNextItem();
                var num4 = list.ChangeNextItem();

                list.Remove(num1);
                list.Remove(num2);
                list.Remove(num3);

                var num5 = list.ChangeNextItem();
                Assert.IsTrue(num5.Number == 5);
            }
            {
                // 落ちるパターン
                var list = new PlayListManager<Item>();
                list.AddRange(Enumerable.Range(1, 5).Select(i => new Item(i)));

                var num1 = list.GetFirstItem();
                var num2 = list.ChangeNextItem();
                var num3 = list.ChangeNextItem();

                list.Remove(num1);
                list.Remove(num2);
                list.Remove(num3);

                var num4 = list.ChangeNextItem();
                Assert.IsTrue(num4.Number == 4);
            }
        }

        [TestMethod]
        public void IsLastItemTest()
        {
            var list = new PlayListManager<Item>();
            list.AddRange(Enumerable.Range(1, 3).Select(i => new Item(i)));

            list.GetFirstItem();
            Assert.IsFalse(list.IsLastItem);

            list.ChangeNextItem();
            Assert.IsFalse(list.IsLastItem);

            list.ChangeNextItem();
            Assert.IsTrue(list.IsLastItem);

            list.ChangeNextItem();
            Assert.IsFalse(list.IsLastItem);
        }

        [TestMethod]
        public void RemoveTest()
        {
            var list = new PlayListManager<Item>();
            list.AddRange(Enumerable.Range(1, 3).Select(i => new Item(i)));

            var num1 = list.GetFirstItem();
            var num2 = list.ChangeNextItem();
            var num3 = list.ChangeNextItem();

            Assert.IsTrue(list.IsLastItem);

            list.Remove(num1);

            Assert.IsTrue(list.IsLastItem);

            list.ChangeNextItem();
            Assert.IsFalse(list.IsLastItem);

            list.ChangeNextItem();
            Assert.IsTrue(list.IsLastItem);
        }
    }
}