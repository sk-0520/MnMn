using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    return MakeLinkCore(link, target, nameof(ISmileDescription.OpenMyListLinkCommand));
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
                    return MakeLinkCore(link, target, nameof(ISmileDescription.OpenUserLinkCommand));
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
                    return MakeLinkCore(link, target, nameof(ISmileDescription.OpenVideoLinkCommand));
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
            var convertedFlowDocumentSource = ConvertLinkFromPlainText(flowDocumentSource, nameof(ISmileDescription.OpenWebLinkCommand));
            convertedFlowDocumentSource = ConvertLinkFromMyList(convertedFlowDocumentSource);
            convertedFlowDocumentSource = ConvertLinkFromUserId(convertedFlowDocumentSource);
            convertedFlowDocumentSource = ConvertLinkFromVideoId(convertedFlowDocumentSource);

            convertedFlowDocumentSource = ConvertSafeXaml(convertedFlowDocumentSource);

            return convertedFlowDocumentSource;
        }
        #endregion

    }
}
