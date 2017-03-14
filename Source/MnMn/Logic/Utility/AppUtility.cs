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
using System.DirectoryServices.AccountManagement;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;
using MahApps.Metro;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility
{
    public static class AppUtility
    {
        #region function

        public static string ReplaceString(string s, IDictionary<string, string> map)
        {
            return s.ReplaceRangeFromDictionary("${", "}", (Dictionary<string, string>)map);
        }

        /// <summary>
        /// 現在の言語を取得。
        /// </summary>
        /// <returns></returns>
        public static string GetCultureLanguage()
        {
            var culture = CultureInfo.CurrentCulture;
            if(culture.IsNeutralCulture) {
                return culture.Name;
            } else {
                return culture.Parent.Name;
            }
        }

        /// <summary>
        /// 現在の国を取得。
        /// </summary>
        /// <returns></returns>
        public static string GetCultureCountry()
        {
            return RegionInfo.CurrentRegion.TwoLetterISORegionName;
        }

        /// <summary>
        /// 現在のカルチャ名を取得。
        /// </summary>
        /// <returns>言語/国</returns>
        public static string GetCultureName()
        {
            return CultureInfo.CurrentCulture.Name;
        }

        /// <summary>
        /// テーマの初期化。
        /// <para>人が触ることは考慮しない。</para>
        /// </summary>
        public static void InitializeTheme(ILogger logger)
        {
            var theme = SerializeUtility.LoadXmlSerializeFromFile<ThemeDefineModel>(Constants.ApplicationThemeDefinePath);

            var customBaseItems = theme.BaseItems
                .Where(t => RawValueUtility.ConvertBoolean(t.Extends["custom"]))
            ;
            foreach(var baseItem in customBaseItems) {
                logger.Information($"base theme: {baseItem.Key}, {baseItem.Extends["resource"]}");
                ThemeManager.AddAppTheme(baseItem.Key, new Uri(baseItem.Extends["resource"]));
            }
        }

        static void SetThemeDefine(string applicationTheme, string baseTheme, string accent)
        {
            Tuple<AppTheme, Accent> appStyle = ThemeManager.DetectAppStyle(Application.Current);

            ThemeManager.ChangeAppStyle(
                Application.Current,
                ThemeManager.GetAccent(accent),
                ThemeManager.GetAppTheme(baseTheme)
            );
        }

        public static void SetRandomTheme()
        {
            var theme = SerializeUtility.LoadXmlSerializeFromFile<ThemeDefineModel>(Constants.ApplicationThemeDefinePath);

            var random = new Random();

            var applicationThemeIndex = random.Next(0, theme.ApplicationItems.Count);
            var baseThemeIndex = random.Next(0, theme.BaseItems.Count);
            var accentIndex = random.Next(0, theme.AccentItems.Count);

            var applicationTheme = theme.ApplicationItems[applicationThemeIndex];
            var baseTheme = theme.BaseItems[baseThemeIndex];
            var accent = theme.AccentItems[accentIndex];

            SetThemeDefine(applicationTheme.Key, baseTheme.Key, accent.Key);
        }

        public static void SetTheme(ThemeSettingModel theme)
        {
            if(theme.IsRandom) {
                SetRandomTheme();
            } else {
                SetThemeDefine(theme.ApplicationTheme, theme.BaseTheme, theme.Accent);
            }
        }

        /// <summary>
        /// XAML(XML)から第一要素取得。
        /// <para>リソースとして定義したXAMLの子要素を取得する。</para>
        /// </summary>
        /// <param name="xamlSource"></param>
        /// <returns></returns>
        public static XmlElement ExtractResourceXamlElement(string xamlSource, Action<XmlDocument, XmlElement> action)
        {
            using(var stream = new StringReader(xamlSource)) {
                var xml = new XmlDocument();
                xml.Load(stream);

                var element = xml.DocumentElement.ChildNodes
                    .Cast<XmlNode>()
                    .FirstOrDefault(n => n.NodeType == XmlNodeType.Element) as XmlElement
                ;
                if(action != null) {
                    action(xml, element);
                }

                return element;
            }
        }

        /// <summary>
        /// ユーザーIDの作成。
        /// </summary>
        /// <returns></returns>
        public static string CreateUserId(ILogger logger)
        {
            var materials = new List<string>();

            // アカウント情報のユーザー名(取れなくてもいい)
            try {
                var userPrincipal = UserPrincipal.Current;
                materials.Add(userPrincipal.DisplayName);
            } catch(Exception ex) {
                logger.Warning(ex);
            }

            // Windows ログオンユーザー名
            materials.Add(Environment.UserName);

            using(var appInfo = new AppInformationCollection()) {
                // CPU の名称と説明
                var cpuInfo = appInfo.GetCPU();
                materials.Add((string)cpuInfo.Items["Name"]);
                materials.Add((string)cpuInfo.Items["Description"]);

                // メモリの合計サイズ(KB)
                var memInfo = appInfo.GetMemory();
                materials.Add(memInfo.Items["TotalVisibleMemorySize"].ToString());
            }

            // OS のメジャー・マイナーバージョン
            var osVersion = Environment.OSVersion.Version;
            materials.Add(osVersion.Major.ToString());
            materials.Add(osVersion.Minor.ToString());

            var material = string.Join(string.Empty, materials);

            using(var hashCreator = new SHA512CryptoServiceProvider())
            using(var stream = StreamUtility.ToUtf8Stream(material)) {
                var hash = hashCreator.ComputeHash(stream);
                var hashText = BitConverter.ToString(hash, 0);
                var result = hashText.ToLower().Replace("-", string.Empty);
                return result;
            }
        }

        #endregion
    }
}
