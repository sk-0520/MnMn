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
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile
{
    [DataContract]
    public class SmileUserInformationModel: ModelBase
    {
        #region property

        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public bool IsPremium { get; set; }

        [DataMember]
        public string ResistedVersion { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public bool IsBookmarked { get; set; }

        [DataMember]
        public Gender Gender { get; set; }

        [DataMember]
        public bool IsPublicGender { get; set; }

        [DataMember]
        public DateTime Birthday { get; set; }

        [DataMember]
        public bool IsPublicBirthday { get; set; }

        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public bool IsPublicLocation { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public Uri ThumbnailUri { get; set; }

        #endregion
    }
}
