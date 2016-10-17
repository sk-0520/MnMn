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

        public static DateTime ConvertSessionTIme(string time)
        {
            return RawValueUtility.ConvertUnixTimeWithMilliseconds(time, 3);
        }
        public static string ConvertRawSessionTIme(DateTime time)
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

        static IList<string> GetSoredVideoWeights(IEnumerable<string> videos)
        {
            var items = videos
                .Select(s => new { Source = s, Match = Constants.ServiceSmileVideoDownloadDmcWeightVideoSort.Match(s) })
                .Where(b => b.Match.Success)
                .Select(b => new { Source = b.Source, Kbs = b.Match.Groups["KBS"].Value, Scan = b.Match.Groups["SCAN"].Value })
                .OrderBy(b => b.Kbs, new NaturalStringComparer())
                .ThenBy(b => b.Scan, new NaturalStringComparer())
                .Select(b => b.Source)
            ;

            return items.ToList();
        }

        static IList<string> GetSoredAudioWeights(IEnumerable<string> videos)
        {
            var items = videos
                .Select(s => new { Source = s, Match = Constants.ServiceSmileVideoDownloadDmcWeightAudioSort.Match(s) })
                .Where(b => b.Match.Success)
                .Select(b => new { Source = b.Source, Kbs = b.Match.Groups["KBS"].Value })
                .OrderBy(b => b.Kbs, new NaturalStringComparer())
                .Select(b => b.Source)
            ;

            return items.ToList();
        }

        // 勘違いテストから始めたのでテスト互換用に残してる(内部で呼んでる関数が大事)
        [Obsolete]
        public static string GetVideoWeight(RawSmileVideoDmcSrcIdToMultiplexerModel mux, int threshold)
        {
            var items = GetSoredVideoWeights(mux.VideoSrcIds);
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
            return GetWeights(GetSoredVideoWeights, mux.VideoSrcIds, threshold);
        }

        public static IEnumerable<string> GetAudioWeights(RawSmileVideoDmcSrcIdToMultiplexerModel mux, int threshold)
        {
            return GetWeights(GetSoredAudioWeights, mux.AudioSrcIds, threshold);
        }

    }
}
