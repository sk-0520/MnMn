using System;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MnMnTest.Logic.Utility.Service.Smile.Video
{
    [TestClass]
    public class SmileVideoMsgUtilityTest
    {
        [TestMethod]
        public void ElapsedTimeTest()
        {
            var time1 = TimeSpan.FromMinutes(12);
            var s = SmileVideoMsgUtility.ConvertRawElapsedTime(time1);
            var time2 = SmileVideoMsgUtility.ConvertElapsedTime(s);

            Assert.IsTrue(time1 == time2);
        }
    }
}
