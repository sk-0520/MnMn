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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video
{
    public static class SmileVideoSearchUtility
    {
        static string ConvertTypeValueFromSearchType(SearchType searchType)
        {
            if(searchType == SearchType.Tag) {
                return "tag";
            }

            return "keyword";
        }

        public static DefinedElementModel GetSearchTypeFromElements(IEnumerable<DefinedElementModel> elements, SearchType searchType)
        {
            var typeValue = ConvertTypeValueFromSearchType(searchType);
            return elements.First(e => e.Extends.Any(w => w.Key == "type" && w.Value == typeValue));
        }

        public static SearchType ConvertSearchTypeFromElement(DefinedElementModel element)
        {
            if(element.Key == "tag") {
                return SearchType.Tag;
            }

            return SearchType.Keyword;
        }

        [Obsolete]
        public static SmileVideoSearchPinModel FindPinItem(IEnumerable<SmileVideoSearchPinModel> items, string query, SearchType searchType)
        {
            return items.FirstOrDefault(p => p.SearchType == searchType && p.Query == query);
        }

        [Obsolete]
        public static bool IsPinItem(IEnumerable<SmileVideoSearchPinModel> items, string query, SearchType searchType)
        {
            return FindPinItem(items, query, searchType) != null;
        }
    }
}
