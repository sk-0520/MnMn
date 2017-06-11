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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video
{
    /// <summary>
    ///
    /// </summary>
    public static class SmileVideoMsgUtility
    {
        #region define

        internal static readonly IReadOnlyList<Color> normalCommentColors;
        internal static readonly IReadOnlyList<Color> premiumCommentColors;

        static readonly IReadOnlyDictionary<string, Color> normalCommentColorMap;
        static readonly IReadOnlyDictionary<string, Color> premiumCommentColorMap;

        #endregion

        static SmileVideoMsgUtility()
        {
            // 定義データの読み込み
            //SerializeUtility
            var model = SerializeUtility.LoadXmlSerializeFromFile<SmileVideoMsgModel>(Constants.SmileVideoMsgPath);

            var colorItems = model.Colors
                .Select(e => new {
                    Color = (Color)ColorConverter.ConvertFromString(e.Key),
                    IsPremium = Convert.ToBoolean(e.Extends["premium"]),
                    Element = e,
                })
                .ToList()
            ;

            //static Dictionary<string, Color> GetColorMap()

            var normalColors = new List<Color>();
            var premiumColors = new List<Color>();

            var normalColorMap = new Dictionary<string, Color>();
            var premiumColorMap = new Dictionary<string, Color>();

            foreach(var colorItem in colorItems) {
                var commands = colorItem.Element.Extends["commands"]
                    .Split(',')
                    .Select(s => s.Trim())
                ;
                foreach(var command in commands) {
                    if(colorItem.IsPremium) {
                        premiumColors.Add(colorItem.Color);
                        premiumColorMap[command] = colorItem.Color;
                    } else {
                        normalColors.Add(colorItem.Color);
                        normalColorMap[command] = colorItem.Color;
                    }
                }
            }

            normalCommentColors = normalColors.Distinct().ToList();
            normalCommentColorMap = normalColorMap;

            premiumCommentColors = premiumColors.Distinct().ToList();
            premiumCommentColorMap = premiumColorMap;

        }

        #region function



        public static bool GetIsAnonymous(IEnumerable<string> commands)
        {
            foreach(var command in commands) {
                if(command == "184") {
                    return true;
                }
            }

            return false;
        }

        public static string ConvertRawIsAnonymous(bool isAnonymous)
        {
            if(isAnonymous) {
                return "184";
            }

            return string.Empty;
        }

        public static SmileVideoUserKind ConvertUserKind(string s)
        {
            if(s == null) {
                return SmileVideoUserKind.Noraml;
            }

            var map = new Dictionary<string, SmileVideoUserKind>() {
                { "0", SmileVideoUserKind.Noraml },
                { "1", SmileVideoUserKind.Premium },
                { "2", SmileVideoUserKind.Alert },
                { "3", SmileVideoUserKind.Real },
                { "6", SmileVideoUserKind.Official },
            };

            SmileVideoUserKind result;
            if(map.TryGetValue(s, out result)) {
                return result;
            }

            return SmileVideoUserKind.Noraml;
        }

        public static string ConvertRawUserKind(SmileVideoUserKind kind)
        {
            var map = new Dictionary<SmileVideoUserKind, string>() {
                { SmileVideoUserKind.Noraml, "0" },
                { SmileVideoUserKind.Premium, "1" },
                { SmileVideoUserKind.Alert, "2" },
                { SmileVideoUserKind.Real, "3" },
                { SmileVideoUserKind.Official, "6" },
            };

            return map[kind];
        }

        public static string ConvertRawIsPremium(bool isPremium)
        {
            return isPremium ? "1" : "0";
        }

        /// <summary>
        /// 1/100秒に変換。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static TimeSpan ConvertElapsedTime(string s)
        {
            var time = RawValueUtility.ConvertLong(s);
            return TimeSpan.FromMilliseconds(time * 10);
        }

        public static string ConvertRawElapsedTime(TimeSpan time)
        {
            var ms = (int)(time.TotalMilliseconds) / 10;
            return ms.ToString();
        }

        public static SmileVideoCommentVertical GetVerticalAlign(IEnumerable<string> commands)
        {
            var map = new Dictionary<string, SmileVideoCommentVertical>() {
                { "naka", SmileVideoCommentVertical.Normal },
                { "ue", SmileVideoCommentVertical.Top },
                { "shita", SmileVideoCommentVertical.Bottom },
            };

            foreach(var command in commands) {
                foreach(var pair in map) {
                    SmileVideoCommentVertical resultValue;
                    if(map.TryGetValue(command, out resultValue)) {
                        return resultValue;
                    }
                }
            }

            return SmileVideoCommentVertical.Normal;
        }

        public static string ConvertRawVerticalAlign(SmileVideoCommentVertical verticalAlign)
        {
            var map = new Dictionary<SmileVideoCommentVertical, string>() {
                { SmileVideoCommentVertical.Normal, "naka" },
                { SmileVideoCommentVertical.Top, "ue" },
                { SmileVideoCommentVertical.Bottom, "shita" },
            };

            return map[verticalAlign];
        }

        public static SmileVideoCommentSize GetFontSize(IEnumerable<string> commands)
        {
            var map = new Dictionary<string, SmileVideoCommentSize>() {
                { "medium", SmileVideoCommentSize.Medium },
                { "small", SmileVideoCommentSize.Small},
                { "big", SmileVideoCommentSize.Big},
            };

            foreach(var command in commands) {
                foreach(var pair in map) {
                    SmileVideoCommentSize resultValue;
                    if(map.TryGetValue(command, out resultValue)) {
                        return resultValue;
                    }
                }
            }

            return SmileVideoCommentSize.Medium;
        }

        public static string ConvertRawFontSize(SmileVideoCommentSize fontSize)
        {
            var map = new Dictionary<SmileVideoCommentSize, string>() {
                { SmileVideoCommentSize.Medium, "medium" },
                { SmileVideoCommentSize.Small, "small" },
                { SmileVideoCommentSize.Big, "big" },
            };

            return map[fontSize];
        }

        public static Color GetForeColor(IEnumerable<string> commands, bool isPremium)
        {
            var colorMap = new Dictionary<string, Color>() {
                { "white",  Colors.White },
                { "red",    Colors.Red },
                { "pink",   Colors.Pink },
                { "orange", Colors.Orange },
                { "yellow", Colors.Yellow },
                { "green",  Colors.Green },
                { "cyan",   Colors.Cyan },
                { "blue",   Colors.Blue },
                { "purple", Colors.Purple },
                { "black",  Colors.Black },
            };

            Regex regColorCode = null;
            if(isPremium) {
                var plusColorMap = new Dictionary<string, Color>() {
                    { "white2", (Color)ColorConverter.ConvertFromString("#CCCC99")},
                    { "niconicowhite", (Color)ColorConverter.ConvertFromString("#CCCC99") },

                    { "red2", (Color)ColorConverter.ConvertFromString("#CC0033") },
                    { "truered", (Color)ColorConverter.ConvertFromString("#CC0033") },

                    { "pink2", (Color)ColorConverter.ConvertFromString("#FF33CC") },

                    { "orange2", (Color)ColorConverter.ConvertFromString("#FF6600") },
                    { "passionorange", (Color)ColorConverter.ConvertFromString("#FF6600") },

                    { "yellow2", (Color)ColorConverter.ConvertFromString("#999900") },
                    { "madyellow", (Color)ColorConverter.ConvertFromString("#999900") },

                    { "green2", (Color)ColorConverter.ConvertFromString("#00CC66") },
                    { "elementalgreen", (Color)ColorConverter.ConvertFromString("#00CC66") },

                    { "cyan2", (Color)ColorConverter.ConvertFromString("#00CCCC") },

                    { "blue2", (Color)ColorConverter.ConvertFromString("#33FFFC") },
                    { "marineblue", (Color)ColorConverter.ConvertFromString("#33FFFC") },

                    { "purple2", (Color)ColorConverter.ConvertFromString("#6633CC") },
                    { "nobleviolet", (Color)ColorConverter.ConvertFromString("#6633CC") },

                    { "black2", (Color)ColorConverter.ConvertFromString("#666666") },
                };
                foreach(var pair in plusColorMap) {
                    colorMap.Add(pair.Key, pair.Value);
                }

                regColorCode = new Regex("^(?<COLOR>#[a-z0-9]{6})$", RegexOptions.ExplicitCapture);
            }

            foreach(var command in commands) {
                Color resultColor;
                if(colorMap.TryGetValue(command, out resultColor)) {
                    return resultColor;
                }
                if(regColorCode != null) {
                    var match = regColorCode.Match(command);
                    if(match.Success) {
                        var rawColor = match.Groups["COLOR"].Value;

                        return (Color)ColorConverter.ConvertFromString(rawColor);
                    }
                }
            }

            return Colors.White;
        }

        public static string ConvertRawForeColor(Color color)
        {
            //var colorMap = new Dictionary<Color, string>() {
            //    { Colors.White,  "white"  },
            //    { Colors.Red,    "red"    },
            //    { Colors.Pink,   "pink"   },
            //    { Colors.Orange, "orange" },
            //    { Colors.Yellow, "yellow" },
            //    { Colors.Green,  "green"  },
            //    { Colors.Cyan,   "cyan"   },
            //    { Colors.Blue,   "blue"   },
            //    { Colors.Purple, "purple" },
            //    { Colors.Black,  "black"  },

            //    { (Color)ColorConverter.ConvertFromString("#CCCC99"), "niconicowhite" },

            //    { (Color)ColorConverter.ConvertFromString("#CC0033"), "truered" },

            //    { (Color)ColorConverter.ConvertFromString("#FF33CC"), "pink2" },

            //    { (Color)ColorConverter.ConvertFromString("#FF6600"), "passionorange" },

            //    { (Color)ColorConverter.ConvertFromString("#999900"), "madyellow" },

            //    { (Color)ColorConverter.ConvertFromString("#00CC66"), "elementalgreen" },

            //    { (Color)ColorConverter.ConvertFromString("#00CCCC"), "cyan2" },

            //    { (Color)ColorConverter.ConvertFromString("#33FFFC"), "marineblue" },

            //    { (Color)ColorConverter.ConvertFromString("#6633CC"), "nobleviolet" },

            //    { (Color)ColorConverter.ConvertFromString("#666666"), "black2"},
            //};

            var colorMap = new Dictionary<Color, string>();
            foreach(var colorPair in normalCommentColorMap.Concat(premiumCommentColorMap)) {
                if(!colorMap.ContainsKey(colorPair.Value)) {
                    colorMap.Add(colorPair.Value, colorPair.Key);
                }
            }

            string result;
            if(colorMap.TryGetValue(color, out result)) {
                return result;
            }

            return BitConverter.ToString(new byte[] { color.R, color.G, color.B }).Replace("-", "");
        }

        public static string[] ConvertCommands(string rawCommand)
        {
            if(string.IsNullOrEmpty(rawCommand)) {
                return new string[0];
            }

            return rawCommand.Split(null);
        }

        public static int ConvertScore(string rawScore)
        {
            var result = RawValueUtility.ConvertInteger(rawScore);
            if(result == RawValueUtility.UnknownInteger) {
                return 0;
            }

            return result;
        }

        public static SmileVideoCommentResultStatus ConvertResultStatus(string rawStatus)
        {
            var map = new Dictionary<string, SmileVideoCommentResultStatus>() {
                { "0", SmileVideoCommentResultStatus.Success  },
                { "1", SmileVideoCommentResultStatus.Failure  },
                { "2", SmileVideoCommentResultStatus.InvalidThread  },
                { "3", SmileVideoCommentResultStatus.InvalidTicket  },
                { "4", SmileVideoCommentResultStatus.InvalidPostkey  },
                { "5", SmileVideoCommentResultStatus.Locked  },
                { "6", SmileVideoCommentResultStatus.Readonly  },
                { "7", SmileVideoCommentResultStatus.TooLong  },
            };
            SmileVideoCommentResultStatus result;
            if(map.TryGetValue(rawStatus, out result)) {
                return result;
            }
            return SmileVideoCommentResultStatus.Unknown;
        }

        #endregion
    }
}
