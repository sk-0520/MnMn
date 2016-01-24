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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Attribute;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Attribute
{
    public static class NameAttributeUtility
    {
        public static string GetName(string propertyName, string setName, NameType nameType)
        {
            return setName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns>[プロパティ名: 設定名]</returns>
        public static IDictionary<string, string> GetNames(object obj)
        {
            var result = new Dictionary<string, string>();

            var propertyInfos = obj.GetType().GetProperties();
            return GetNames(propertyInfos);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyInfos"></param>
        /// <returns>[プロパティ名: 設定名]</returns>
        public static IDictionary<string, string> GetNames(IEnumerable<PropertyInfo> propertyInfos)
        {
            var result = new Dictionary<string, string>();

            foreach(var propertyInfo in propertyInfos) {
                var names = System.Attribute.GetCustomAttributes(propertyInfo, typeof(NameAttribute))
                    .OfType<NameAttribute>()
                ;
                if(names.Any()) {
                    var name = names.Single();
                    result[propertyInfo.Name] = GetName(propertyInfo.Name, name.Name, name.NameType);
                }
            }

            return result;
        }
    }
}
