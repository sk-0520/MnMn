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
