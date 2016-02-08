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
using Newtonsoft.Json;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile
{
    public class RawSmileContentsSearchItemModel: ModelBase
    {
        #region property

        [DataMember(Name = "contentId")]
        public string ContentId { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "tags")]
        public string Tags { get; set; }
        [DataMember(Name = "categoryTags")]
        public string CategoryTags { get; set; }
        [DataMember(Name = "viewCounter")]
        public string ViewCounter { get; set; }
        [DataMember(Name = "mylistCounter")]
        public string MylistCounter { get; set; }
        [DataMember(Name = "commentCounter")]
        public string CommentCounter { get; set; }
        [DataMember(Name = "startTime")]
        public string StartTime { get; set; }
        [DataMember(Name = "thumbnailUrl")]
        public string ThumbnailUrl { get; set; }
        [DataMember(Name = "communityIcon")]
        public string CommunityIcon { get; set; }
        [DataMember(Name = "scoreTimeshiftReserved")]
        public string ScoreTimeshiftReserved { get; set; }
        [DataMember(Name = "liveStatus")]
        public string LiveStatus { get; set; }

        #endregion
    }
}
