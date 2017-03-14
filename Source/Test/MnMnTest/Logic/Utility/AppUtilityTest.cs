using System;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MnMnTest.Logic.Utility
{
    [TestClass]
    public class AppUtilityTest
    {
        [TestMethod]
        public void CreateUserIdTest()
        {
            var logger = new Logger();
            logger.LoggerConfig.PutsConsole = true;

            // 非同期でも一緒という悲しみ
            //var result = Task.WhenAll(
            //    Task.Run(() => AppUtility.CreateUserId(logger)),
            //    Task.Run(() => AppUtility.CreateUserId(logger))
            //).Result;
            var result = new [] {
                AppUtility.CreateUserId(logger),
                AppUtility.CreateUserId(logger),
            };
            // 同一環境で二つ動かしてんだから一緒になるはず
            Assert.IsTrue(result[0] == result[1]);
        }
    }
}
