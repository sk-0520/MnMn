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
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MnMnTest.Logic.Utility
{
    [TestClass]
    public class RawValueUtilityTest
    {
        [TestMethod]
        public void ConvertUnixTimeWithMillisecondsTest()
        {
            var time0 = new DateTime(2016, 10, 14, 20, 54, 12, 0);
            var a0 = RawValueUtility.ConvertUnixTimeWithMilliseconds("1476446052" + "", 0);
            Assert.IsTrue(time0 == a0);
            var time1 = new DateTime(2016, 10, 14, 20, 54, 12, 1);
            var a1 = RawValueUtility.ConvertUnixTimeWithMilliseconds("1476446052" + "1", 1);
            Assert.IsTrue(time1 == a1);
            var time2 = new DateTime(2016, 10, 14, 20, 54, 12, 12);
            var a2 = RawValueUtility.ConvertUnixTimeWithMilliseconds("1476446052" + "12", 2);
            Assert.IsTrue(time2 == a2);
            var time3 = new DateTime(2016, 10, 14, 20, 54, 12, 123);
            var a3 = RawValueUtility.ConvertUnixTimeWithMilliseconds("1476446052" + "123", 3);
            Assert.IsTrue(time3 == a3);
            var time4 = new DateTime(2016, 10, 14, 20, 54, 12, 123);
            var a4 = RawValueUtility.ConvertUnixTimeWithMilliseconds("1476446052" + "1230", 4);
            Assert.IsTrue(time3 == a3);
        }

        [TestMethod]
        public void ConvertRawUnixTimeWithMilliseconds()
        {
            var time = new DateTime(2016, 10, 14, 20, 54, 12, 123);

            var a0 = RawValueUtility.ConvertRawUnixTimeWithMilliseconds(time, 0);
            Assert.IsTrue(a0 == "1476446052" + "");
            var a1 = RawValueUtility.ConvertRawUnixTimeWithMilliseconds(time, 1);
            Assert.IsTrue(a1 == "1476446052" + "1");
            var a2 = RawValueUtility.ConvertRawUnixTimeWithMilliseconds(time, 2);
            Assert.IsTrue(a2 == "1476446052" + "12");
            var a3 = RawValueUtility.ConvertRawUnixTimeWithMilliseconds(time, 3);
            Assert.IsTrue(a3 == "1476446052" + "123");
            var a4 = RawValueUtility.ConvertRawUnixTimeWithMilliseconds(time, 4);
            Assert.IsTrue(a4 == "1476446052" + "1230");
        }
    }
}
