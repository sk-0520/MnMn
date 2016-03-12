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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    [DataContract]
    public class RawSmileVideoRecommendItemModel: ModelBase
    {
        #region property

        [DataMember(Name = "item_type")]
        public string ItemType { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "thumbnail_url")]
        public string ThumbnailUrl { get; set; }

        [DataMember(Name = "title_short")]
        public string TitleShort { get; set; }

        [DataMember(Name = "view_counter")]
        public string ViewCounter { get; set; }

        [DataMember(Name = "num_res")]
        public string NumRes { get; set; }

        [DataMember(Name = "mylist_counter")]
        public string MylistCounter { get; set; }

        [DataMember(Name = "first_retrieve")]
        public string FirstRetrieve { get; set; }

        [DataMember(Name =  "length")]
        public string Length { get; set; }

        [DataMember(Name =  "is_original_language")]
        public string IsOriginalLanguage { get; set; }

        [DataMember(Name = "is_translated")]
        public string IsTranslated { get; set; }

        // TODO: 追加情報

        #endregion
    }
}
