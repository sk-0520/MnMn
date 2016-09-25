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
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using Newtonsoft.Json.Linq;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile
{
    public static class SmileMyListUtility
    {
        public static bool IsSuccessResponse(RawSmileAccountMyListGroupModel rawModel)
        {
            return string.Compare(rawModel.Status.Trim(), "ok", true) == 0;
        }

        public static string GetMyListId(SmileJsonResultModel resultModel)
        {
            CheckUtility.Enforce(resultModel.IsSuccess);
            return resultModel.Result.GetValue("id").Value<string>();
        }

        public static IEnumerable<Color> GetColorsFromExtends(IReadOnlyDictionary<string, string> extends)
        {
            var colorCodes = extends
                .Where(p => p.Key == "color")
                .Select(p => p.Value)
            ;

            foreach(var colorCode in colorCodes) {
                yield return (Color)ColorConverter.ConvertFromString(colorCode);
            }
        }

        public static string TrimTitle(string rawTitle)
        {
            //var target = "マイリスト ";

            //if(rawTitle.IndexOf(target) == 0) {
            //    return rawTitle.Substring(target.Length);
            //}

            //return rawTitle;

            var patterns = new[] {
                new {
                    Word = Constants.ServiceSmileMyListTitleTrimHead,
                    Head = true,
                },
                new {
                    Word = Constants.ServiceSmileMyListTitleTrimTail,
                    Head = false,
                },
            };

            var workTitle = rawTitle;

            foreach(var pattern in patterns) {
                if(pattern.Head) {
                    if(workTitle.StartsWith(pattern.Word)) {
                        workTitle = workTitle.Substring(pattern.Word.Length);
                    }
                } else {
                    if(workTitle.EndsWith(pattern.Word)) {
                        workTitle = workTitle.Substring(0, workTitle.Length - pattern.Word.Length);
                    }
                }
            }

            return workTitle;
        }
    }
}
