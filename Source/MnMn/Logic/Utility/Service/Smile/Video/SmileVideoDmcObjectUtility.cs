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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video
{
    public static class SmileVideoDmcObjectUtility
    {
        public static bool IsSuccessResponse(RawSmileVideoDmcObjectModel rawModel)
        {
            var success = new[] { "200", "201" };
            return success.Any(s => string.Equals(rawModel.Meta.Status.Trim(), s, StringComparison.OrdinalIgnoreCase));
        }

        public static bool IsSuccess( RawSmileVideoWatchDataDmcInfoModel rawModel)
        {
            var errorMessgae = (string)rawModel.error;
            if(string.IsNullOrEmpty(errorMessgae)) {
                return true;
            }

            return errorMessgae != "flv_get_error";
        }

        public static DateTime ConvertSessionTime(string time)
        {
            return RawValueUtility.ConvertUnixTimeWithMilliseconds(time, 3);
        }
        public static string ConvertRawSessionTime(DateTime time)
        {
            return RawValueUtility.ConvertRawUnixTimeWithMilliseconds(time, 3);
        }

        static int GetWeightIndex(IEnumerable<string> weights, int threshold)
        {
            Debug.Assert((Constants.ServiceSmileVideoDownloadDmcWeightRange.Head <= threshold) && (threshold <= Constants.ServiceSmileVideoDownloadDmcWeightRange.Tail));

            var count = weights.Count();
            var range = Constants.ServiceSmileVideoDownloadDmcWeightRange.Tail - Constants.ServiceSmileVideoDownloadDmcWeightRange.Head;
            var index = threshold / (range / count);

            if(count <= index) {
                index = count - 1;
            }

            return index;
        }

        static IList<string> GetSortedVideoWeights(IEnumerable<string> videos)
        {
            var items = videos
                .Select(s => new { Source = s, Match = Constants.ServiceSmileVideoDownloadDmcWeightVideoSort.Match(s) })
                .Where(b => b.Match.Success)
                .Select(b => new { Source = b.Source, Kbs = b.Match.Groups["KBS"].Value, Scan = b.Match.Groups["SCAN"].Value })
                .OrderBy(b => b.Kbs, new NaturalStringComparer())
                .ThenBy(b => b.Scan, new NaturalStringComparer())
                .Select(b => b.Source)
            ;

            return items.ToEvaluatedSequence();
        }

        static IList<string> GetSortedAudioWeights(IEnumerable<string> videos)
        {
            var items = videos
                .Select(s => new { Source = s, Match = Constants.ServiceSmileVideoDownloadDmcWeightAudioSort.Match(s) })
                .Where(b => b.Match.Success)
                .Select(b => new { Source = b.Source, Kbs = b.Match.Groups["KBS"].Value })
                .OrderBy(b => b.Kbs, new NaturalStringComparer())
                .Select(b => b.Source)
            ;

            return items.ToEvaluatedSequence();
        }

        // 勘違いテストから始めたのでテスト互換用に残してる(内部で呼んでる関数が大事)
        [Obsolete]
        public static string GetVideoWeight(RawSmileVideoDmcSrcIdToMultiplexerModel mux, int threshold)
        {
            var items = GetSortedVideoWeights(mux.SrcId.VideoSrcIds);
            var index = GetWeightIndex(items, threshold);

            return items[index];
        }

        static IEnumerable<string> GetWeights(Func<IEnumerable<string>, IList<string>> sortedWeightsGetter, IEnumerable<string> srcItems, int threshold)
        {
            var items = sortedWeightsGetter(srcItems);
            var index = GetWeightIndex(items, threshold);

            var targetWeight = items[index];
            yield return targetWeight;

            var weights = items
                .Where(s => s != targetWeight)
                .IfRevese(items.Count / 2 < index)
            ;
            foreach(var weight in weights) {
                yield return weight;
            }
        }

        public static IEnumerable<string> GetVideoWeights(RawSmileVideoDmcSrcIdToMultiplexerModel mux, int threshold)
        {
            return GetWeights(GetSortedVideoWeights, mux.SrcId.VideoSrcIds, threshold);
        }
        public static IEnumerable<string> GetVideoWeights(IEnumerable<string> values, int threshold)
        {
            return GetWeights(GetSortedVideoWeights, values, threshold);
        }

        public static IEnumerable<string> GetAudioWeights(RawSmileVideoDmcSrcIdToMultiplexerModel mux, int threshold)
        {
            return GetWeights(GetSortedAudioWeights, mux.SrcId.AudioSrcIds, threshold);
        }
        public static IEnumerable<string> GetAudioWeights(IEnumerable<string> values, int threshold)
        {
            return GetWeights(GetSortedAudioWeights, values, threshold);
        }

        public static int CompareVideo(string a, string b)
        {
            if(string.IsNullOrWhiteSpace(a)) {
                throw new ArgumentException(nameof(a));
            }
            if(string.IsNullOrWhiteSpace(b)) {
                throw new ArgumentException(nameof(b));
            }

            (string bs, string scan) GetWeight(string s)
            {
                var match = Constants.ServiceSmileVideoDownloadDmcWeightVideoSort.Match(s);
                return (
                    bs: match.Groups["KBS"].Value,
                    scan: match.Groups["SCAN"].Value
                );
            }

            var pairA = GetWeight(a);
            var pairB = GetWeight(b);

            var cmp = new NaturalStringComparer();
            var comBs = cmp.Compare(pairA.bs, pairB.bs);
            if(comBs != 0) {
                return comBs;
            }
            return cmp.Compare(pairA.scan, pairB.scan);
        }

        public static int CompareAudio(string a, string b)
        {
            if(string.IsNullOrWhiteSpace(a)) {
                throw new ArgumentException(nameof(a));
            }
            if(string.IsNullOrWhiteSpace(b)) {
                throw new ArgumentException(nameof(b));
            }

            string GetWeight(string s)
            {
                var match = Constants.ServiceSmileVideoDownloadDmcWeightAudioSort.Match(s);
                return match.Groups["KBS"].Value;
            }

            var a2 = GetWeight(a);
            var b2 = GetWeight(b);

            var cmp = new NaturalStringComparer();
            return cmp.Compare(a2, b2);
        }
    }
}
