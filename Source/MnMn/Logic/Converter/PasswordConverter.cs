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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Converter
{
    public class PasswordConverter: JsonConverter
    {
        #region JsonConverter

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var textProtectPassword = reader.Value as string;

            if(string.IsNullOrEmpty(textProtectPassword)) {
                return string.Empty;
            }

            try {
                var binaryProtectPassword = Convert.FromBase64String(textProtectPassword);
                var binaryRawPassword = ProtectedData.Unprotect(binaryProtectPassword, null, DataProtectionScope.CurrentUser);

                var input = Encoding.UTF8.GetString(binaryRawPassword);

                return input;
            } catch(Exception ex) {
                Debug.WriteLine(ex);
                return string.Empty;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var rawPassword = value as string;
            if(string.IsNullOrEmpty(rawPassword)) {
                serializer.Serialize(writer, string.Empty);
            } else {
                var binaryRawPassword = Encoding.UTF8.GetBytes(rawPassword);
                var binaryProtectPassword = ProtectedData.Protect(binaryRawPassword, null, DataProtectionScope.CurrentUser);

                var output = Convert.ToBase64String(binaryProtectPassword, Base64FormattingOptions.None);
                serializer.Serialize(writer, output);
            }
        }

        #endregion
    }
}
