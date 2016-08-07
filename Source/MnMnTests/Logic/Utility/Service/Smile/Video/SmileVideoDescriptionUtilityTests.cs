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
    public class SmileVideoDescriptionUtilityTests
    {
        [TestMethod()]
        public void ConvertSafeXamlTest()
        {
            var s = SmileVideoDescriptionUtility.ConvertSafeXaml("<Run></Run>");
            Assert.AreEqual(s, "<TextBlock><TextBlock>");
        }
    }
}