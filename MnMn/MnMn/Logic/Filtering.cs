﻿/*
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    public class Filtering
    {
        public Filtering(FilteringSettingModel setting)
        {
            Setting = setting;
        }

        #region property

        FilteringSettingModel Setting { get; }

        #endregion

        #region function

        protected bool CheckPartial(string target, string filterSource,bool ignoreCase)
        {
            Debug.Assert(target != null);

            return target.IndexOf(filterSource, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) != 0;
        }
        protected bool CheckForward(string target, string filterSource, bool ignoreCase)
        {
            Debug.Assert(target != null);

            return target.StartsWith(filterSource, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
        }
        protected bool CheckPerfect(string target, string filterSource, bool ignoreCase)
        {
            Debug.Assert(target != null);

            return target.Equals(filterSource, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
        }
        protected bool CheckRegex(string target, string filterSource, bool ignoreCase)
        {
            Debug.Assert(target != null);

            var option = RegexOptions.Multiline;
            if(ignoreCase) {
                option |= RegexOptions.IgnoreCase;
            }
            try {
                var reg = new Regex(filterSource, option);
                return reg.IsMatch(target);
            } catch(ArgumentException ex) {
                // 解析エラー
                Debug.WriteLine(ex);
                return false;
            }
        }

        public bool CheckHit(string target)
        {
            var notNullTarget = target ?? string.Empty;
            var notNullSource = Setting.Source ?? string.Empty;
            switch(Setting.Type) {
                case FilteringType.PartialMatch:
                    return CheckPartial(notNullTarget, notNullSource, Setting.IgnoreCase);

                case FilteringType.ForwardMatch:
                    return CheckForward(notNullTarget, notNullSource, Setting.IgnoreCase);

                case FilteringType.PerfectMatch:
                    return CheckPerfect(notNullTarget, notNullSource, Setting.IgnoreCase);

                case FilteringType.Regex:
                    return CheckRegex(notNullTarget, notNullSource, Setting.IgnoreCase);

                default:
                    throw new NotImplementedException();
            }

        }

        #endregion
    }
}
