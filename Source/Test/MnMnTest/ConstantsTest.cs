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
using System.Configuration;
using System.Diagnostics;
using ContentTypeTextNet.MnMn.MnMn;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MnMnTest
{
    [TestClass]
    public class ConstantsTest
    {
        [TestMethod]
        public void AppConfigTest()
        {
            var type = typeof(Constants);
            var props = type.GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            foreach(var prop in props) {
                try {
                    var a = prop.GetValue(null);
                    Trace.WriteLine($"property => {prop}, value = {a}");
                } catch(Exception ex) {
                    Assert.Fail($"property => {prop}, exception => {ex}");
                }
            }
        }
    }
}
