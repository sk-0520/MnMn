using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video.Tests
{
    [TestClass()]
    public class SmileVideoDescriptionUtilityTest
    {
        [TestMethod()]
        public void ConvertSafeXamlTest_Simple()
        {
            var s1 = SmileVideoDescriptionUtility.ConvertSafeXaml("<Run></Run>");
            Assert.AreEqual(s1, "<TextBlock></TextBlock>");
            var s2 = SmileVideoDescriptionUtility.ConvertSafeXaml("<Run>value</Run>");
            Assert.AreEqual(s2, "<TextBlock>value</TextBlock>");
        }
        [TestMethod()]
        public void ConvertSafeXamlTest_Double()
        {
            var s = SmileVideoDescriptionUtility.ConvertSafeXaml("<Run>a</Run><Run>b</Run>");
            Assert.AreEqual(s, "<TextBlock>a</TextBlock><TextBlock>b</TextBlock>");
        }
        [TestMethod()]
        public void ConvertSafeXamlTest_Nested()
        {
            var s = SmileVideoDescriptionUtility.ConvertSafeXaml("<Run><Run>a</Run></Run>");
            Assert.AreEqual(s, "<TextBlock><TextBlock>a</TextBlock></TextBlock>");
        }

        static string GetParam(string s)
        {
            var m = Regex.Match(s, "CommandParameter='(.+?)'");
            return m.Groups[1].Value;
        }

        [TestMethod]
        public void ConvertLinkFromPlainTextTest()
        {
            var list = new[] {
                new { Success = true, Uri = "http://abc", },
                new { Success = true, Uri = "http://abc.com", },
                new { Success = true, Uri = "http://abc.com/index", },
                new { Success = true, Uri = "http://abc.com/index.html", },
                new { Success = true, Uri = "http://abc.com/index.html?", },
                new { Success = true, Uri = "http://abc.com/index.html?param", },
                new { Success = true, Uri = "http://abc.com/index.html?param=123", },
                new { Success = true, Uri = "http://abc.com/index.html?param=%0d%0a", },
            };
            foreach(var u in list) {
                var s = SmileVideoDescriptionUtility.ConvertLinkFromPlainText(u.Uri);
                var a = GetParam(s);
                if(u.Success) {
                    Assert.AreEqual(a, u.Uri);
                } else {
                    Assert.AreNotEqual(a, u.Uri);
                }
            }
        }
    }
}