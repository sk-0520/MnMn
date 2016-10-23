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
using ContentTypeTextNet.MnMn.MnMn.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MnMnTest.Logic
{
    [TestClass]
    public class PlayListModelTest
    {
        public class Item
        {
            public Item(int number)
            {
                Number = number;
            }
            public int Number { get; }
        }

        [TestMethod]
        public void SequenceNextTest()
        {
            var list = new PlayListModel<Item>() {
                IsRandom = false,
            };

            list.AddRange(Enumerable.Range(1, 10).Select(i => new Item(i)));
            for(var i = 0; i < 10; i++) {
                var item = list.ChangeNextItem();
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
            var list = new PlayListModel<Item>() {
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
    }
}
