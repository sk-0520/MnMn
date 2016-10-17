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

        static string GetWeight(IEnumerable<string> weights, int threshold)
        {
            Debug.Assert((Constants.ServiceSmileVideoDownloadDmcWeightRange.Head <= threshold) && (threshold <= Constants.ServiceSmileVideoDownloadDmcWeightRange.Tail));

            var count = weights.Count();
            var range = Constants.ServiceSmileVideoDownloadDmcWeightRange.Tail - Constants.ServiceSmileVideoDownloadDmcWeightRange.Head;
            var index = threshold / (range / count);

            if(count <= index) {
                index = count - 1;
            }

            return weights.ElementAt(index);
        }

        public static string GetVideoWeight(RawSmileVideoDmcSrcIdToMultiplexerModel mux, int threshold)
        {
            var items = mux.VideoSrcIds
                .Select(s => new { Source = s, Match = Constants.ServiceSmileVideoDownloadDmcWeightVideoSort.Match(s) })
                .Where(b => b.Match.Success)
                .Select(b => new { Source = b.Source, Kbs = b.Match.Groups["KBS"].Value, Scan = b.Match.Groups["SCAN"].Value })
                .OrderBy(b => b.Kbs, new NaturalStringComparer())
                .ThenBy(b => b.Scan, new NaturalStringComparer())
                .Select(b => b.Source)
            ;

            return GetWeight(items, threshold);
        }

    }
}
