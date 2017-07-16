using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MnMnTest.Logic.Utility.Service.Smile.Video
{
    [TestClass]
    public class SmileVideoDmcObjectUtilityTest
    {
        // ":["","","","
        // archive_aac_192kbps","archive_aac_64kbps
        [TestMethod]
        public void GetVideoWeight4Test()
        {
            var items = new[] {
                "archive_h264_1000kbps_540p", //50<75
                "archive_h264_2000kbps_720p", //75<100
                "archive_h264_300kbps_360p",  //25<50
                "archive_h264_600kbps_360p",  // 0<25
            };
            var mux = new RawSmileVideoDmcSrcIdToMultiplexerModel();
            mux.SrcId.VideoSrcIds.InitializeRange(items);
            var tests = new[] {
                new { num = 100, result = "archive_h264_2000kbps_720p" },
                new { num = 70, result = "archive_h264_1000kbps_540p" },
                new { num = 40, result = "archive_h264_600kbps_360p" },
                new { num = 10, result = "archive_h264_300kbps_360p" },
                new { num = 0, result = "archive_h264_300kbps_360p" },
            };
            foreach(var test in tests) {
#pragma warning disable CS0612 // 型またはメンバーが古い形式です
                var src = SmileVideoDmcObjectUtility.GetVideoWeight(mux, test.num);
#pragma warning restore CS0612 // 型またはメンバーが古い形式です
                Assert.IsTrue(src == test.result, $"result:{src}, test:{test.num} - {test.result}");
            }
        }

        [TestMethod]
        public void GetVideoWeight2Test()
        {
            var items = new[] {
                "archive_h264_1000kbps_540p",
                "archive_h264_600kbps_360p",
            };
            var mux = new RawSmileVideoDmcSrcIdToMultiplexerModel();
            mux.SrcId.VideoSrcIds.InitializeRange(items);
            var tests = new[] {
                new { num = 100, result = "archive_h264_1000kbps_540p" },
                new { num = 70, result = "archive_h264_1000kbps_540p" },
                new { num = 51, result = "archive_h264_1000kbps_540p" },
                new { num = 50, result = "archive_h264_1000kbps_540p" },
                new { num = 49, result = "archive_h264_600kbps_360p" },
                new { num = 40, result = "archive_h264_600kbps_360p" },
                new { num = 10, result = "archive_h264_600kbps_360p" },
                new { num = 0, result = "archive_h264_600kbps_360p" },
            };
            foreach(var test in tests) {
#pragma warning disable CS0612 // 型またはメンバーが古い形式です
                var src = SmileVideoDmcObjectUtility.GetVideoWeight(mux, test.num);
#pragma warning restore CS0612 // 型またはメンバーが古い形式です
                Assert.IsTrue(src == test.result, $"result:{src}, test:{test.num} - {test.result}");
            }
        }
        [TestMethod]
        public void GetVideoWeight10Test()
        {
            var items = new[] {
                "archive_h264_1kbps_0p",
                "archive_h264_2kbps_0p",
                "archive_h264_3kbps_0p",
                "archive_h264_4kbps_0p",
                "archive_h264_5kbps_0p",
                "archive_h264_6kbps_0p",
                "archive_h264_7kbps_0p",
                "archive_h264_8kbps_0p",
                "archive_h264_9kbps_0p",
                "archive_h264_10kbps_0p",
            };
            var mux = new RawSmileVideoDmcSrcIdToMultiplexerModel();
            mux.SrcId.VideoSrcIds.InitializeRange(items);
            var tests = new[] {
                new { num = 100, result = "archive_h264_10kbps_0p" },
                new { num = 90, result = "archive_h264_10kbps_0p" },
                new { num = 80, result = "archive_h264_9kbps_0p" },
                new { num = 70, result = "archive_h264_8kbps_0p" },
                new { num = 60, result = "archive_h264_7kbps_0p" },
                new { num = 50, result = "archive_h264_6kbps_0p" },
                new { num = 40, result = "archive_h264_5kbps_0p" },
                new { num = 30, result = "archive_h264_4kbps_0p" },
                new { num = 20, result = "archive_h264_3kbps_0p" },
                new { num = 10, result = "archive_h264_2kbps_0p" },
                new { num = 1, result = "archive_h264_1kbps_0p" },
                new { num = 0, result = "archive_h264_1kbps_0p" },
            };
            foreach(var test in tests) {
#pragma warning disable CS0612 // 型またはメンバーが古い形式です
                var src = SmileVideoDmcObjectUtility.GetVideoWeight(mux, test.num);
#pragma warning restore CS0612 // 型またはメンバーが古い形式です
                Assert.IsTrue(src == test.result, $"result:{src}, test:{test.num} - {test.result}");
            }
        }
    }
}
