using System;
using System.Linq;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MnMnTest.Logic.Utility
{
    [TestClass]
    public class AppUtilityTest
    {
        //[TestMethod]
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

        [TestMethod]
        public void ValidateUserIdTest()
        {
            var tests = new[] {
                new { Result = false, UserId = default(string) },
                new { Result = false, UserId = "" },
                new { Result = false, UserId = "a" },
                new { Result = true, UserId = "0123456789012345678901234567890123456789" },
                new { Result = false, UserId = "abcdefghijklmnopqrstuvwxyz0123456789/*-+" },
                new { Result = true, UserId = "0123456789abcdefABCDEF0123456789abcdefAB" },
                new { Result = false, UserId = "0１23456789abcdefABCDEF0123456789abcdefAB" },
            };
            foreach(var test in tests) {
                var result = AppUtility.ValidateUserId(test.UserId);
                Assert.IsTrue(result == test.Result, test.UserId);
            }
        }
    }
}
