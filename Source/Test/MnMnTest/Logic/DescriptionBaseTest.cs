using System;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MnMnTest.Logic
{
    [TestClass]
    public class DescriptionBaseTest
    {
        [TestMethod]
        public void ClearImageTest()
        {
            var src = "<TextBlock>not<IMAGE />kill<Image />kill<Image Source=\"xxx\" />not<image/></TextBlock>";
            var img = DescriptionBase.ClearImage(src);
            Assert.IsTrue(img == "<TextBlock>not<IMAGE />killkillnot<image/></TextBlock>");
        }
    }
}
