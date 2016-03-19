using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}