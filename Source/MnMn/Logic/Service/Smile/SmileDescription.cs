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
using ContentTypeTextNet.MnMn.MnMn.Data;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.IF.Compatibility;
using ContentTypeTextNet.MnMn.MnMn.IF.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile
{
    public class SmileDescription: DescriptionBase
    {
        protected SmileDescription(IConvertCompatibility convertCompatibility, ServiceType serviceType)
            : base(convertCompatibility, serviceType)
        { }

        public SmileDescription(IConvertCompatibility convertCompatibility)
            : this(convertCompatibility, ServiceType.Smile)
        { }

        #region function

        string ConvertLinkFromMyList(string flowDocumentSource)
        {
            var replacedSource = ConvertRunTarget(flowDocumentSource, m => {
                var target = m.Groups["TARGET"].Value;
                object outputValue;
                if(ConvertCompatibility.ConvertValue(out outputValue, typeof(string), SmileMediationKey.inputGetMyListId, target, typeof(string), ServiceType)) {
                    var link = (string)outputValue;

                    var menuItems = new[] {
                        new DescriptionContextMenuItem(true, Properties.Resources.String_Service_Smile_ISmileDescription_MenuOpenMyListId, nameof(ISmileDescription.MenuOpenMyListIdLinkCommand), null),
                        //new DescriptionContextMenuItem(false, Properties.Resources.String_Service_Smile_ISmileDescription_MenuAddMyList, nameof(ISmileDescription.MenuAddMyListLinkCommand), null),
                        new DescriptionContextMenuItem(false, Properties.Resources.String_Service_Smile_ISmileDescription_MenuCopyMyListId, nameof(ISmileDescription.MenuCopyMyListIdCommand), null, Constants.xamlImage_Copy, Constants.xamlStyle_SmallDefaultIconPath),
                    };

                    return MakeLinkCore(link, target, nameof(ISmileDescription.OpenMyListIdLinkCommand), menuItems);
                } else {
                    return m.Groups[0].Value;
                }
            });

            return replacedSource;
        }

        string ConvertLinkFromUserId(string flowDocumentSource)
        {
            var replacedSource = ConvertRunTarget(flowDocumentSource, m => {
                var target = m.Groups["TARGET"].Value;
                object outputValue;
                if(ConvertCompatibility.ConvertValue(out outputValue, typeof(string), SmileMediationKey.inputGetUserId, target, typeof(string), ServiceType)) {
                    var link = (string)outputValue;
                    return MakeLinkCore(link, target, nameof(ISmileDescription.OpenUserIdLinkCommand), Enumerable.Empty<DescriptionContextMenuItem>());
                } else {
                    return m.Groups[0].Value;
                }
            });

            return replacedSource;
        }

        string ConvertLinkFromVideoId(string flowDocumentSource)
        {
            var replacedSource = ConvertRunTarget(flowDocumentSource, m => {
                var target = m.Groups["TARGET"].Value;
                object outputValue;
                if(ConvertCompatibility.ConvertValue(out outputValue, typeof(string), SmileMediationKey.inputGetVideoId, target, typeof(string), ServiceType)) {
                    var link = (string)outputValue;

                    var menuItems = new[] {
                        new DescriptionContextMenuItem(true, "open", nameof(ISmileDescription.MenuOpenVideoIdLinkCommand), null),
                        new DescriptionContextMenuItem(false, "new window", nameof(ISmileDescription.MenuOpenVideoIdLinkInNewWindowCommand), null),
                        new DescriptionContextMenuItem(false, "add playlist", nameof(ISmileDescription.MenuAddPlayListVideoIdLinkCommand), null),
                        new DescriptionContextMenuItem(false, "copy", nameof(ISmileDescription.MenuCopyVideoIdCommand), null, Constants.xamlImage_Copy, Constants.xamlStyle_SmallDefaultIconPath),
                    };

                    return MakeLinkCore(link, target, nameof(ISmileDescription.OpenVideoIdLinkCommand), menuItems);
                } else {
                    return m.Groups[0].Value;
                }
            });

            return replacedSource;
        }

        #endregion

        #region DescriptionBase

        protected override string ConvertFlowDocumentFromHtmlCore(string flowDocumentSource)
        {
            var convertedFlowDocumentSource = ConvertLinkFromPlainText(flowDocumentSource, nameof(ISmileDescription.OpenUriCommand));
            convertedFlowDocumentSource = ConvertLinkFromMyList(convertedFlowDocumentSource);
            convertedFlowDocumentSource = ConvertLinkFromUserId(convertedFlowDocumentSource);
            convertedFlowDocumentSource = ConvertLinkFromVideoId(convertedFlowDocumentSource);

            convertedFlowDocumentSource = ConvertSafeXaml(convertedFlowDocumentSource);

            return convertedFlowDocumentSource;
        }
        #endregion

    }
}
