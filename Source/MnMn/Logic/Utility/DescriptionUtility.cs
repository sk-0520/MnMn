using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Parameter;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility
{
    public static class DescriptionUtility
    {
        #region function

        public static void SetDescriptionDocument(FlowDocumentScrollViewer documentViewer, string flowDocumentSource, ILogger logger)
        {
            var document = documentViewer.Document;

            document.Blocks.Clear();

            using(var stringReader = new StringReader(flowDocumentSource))
            using(var xmlReader = System.Xml.XmlReader.Create(stringReader)) {
                try {
                    var flowDocument = XamlReader.Load(xmlReader) as FlowDocument;
                    document.Blocks.AddRange(flowDocument.Blocks.ToArray());
                } catch(XamlParseException ex) {
                    logger.Error(ex);
                    var error = new Paragraph();
                    error.Inlines.Add(ex.ToString());

                    var raw = new Paragraph();
                    raw.Inlines.Add(flowDocumentSource);

                    document.Blocks.Add(error);
                    document.Blocks.Add(raw);
                }
            }

            document.FontSize = documentViewer.FontSize;
            document.FontFamily = documentViewer.FontFamily;
            document.FontStretch = documentViewer.FontStretch;
        }

        internal static void CopyText(string text, ILogger logger)
        {
            ShellUtility.SetClipboard(text, logger);
        }

        /// <summary>
        /// <see cref="IDescription.OpenUriCommand"/>
        /// </summary>
        /// <param name="link"></param>
        /// <param name="logger"></param>
        static void OpenUriCore(string link, ILogger logger)
        {
            ShellUtility.OpenUriInDefaultBrowser(link, logger);
        }

        public static void OpenUri(object parameter, ILogger logger)
        {
            OpenUriCore((string)parameter, logger);
        }

        static void OpenUriInAppBrowserCore(Uri uri, ICommunication communication)
        {
            var parameter = new AppBrowserParameterModel() {
                Uri = uri,
            };
            communication.Request(new ShowViewRequestModel(RequestKind.ShowView, ServiceType.Application, parameter, ShowViewState.Foreground));
        }

        public static void OpenUriInAppBrowser(Uri uri, ICommunication communication)
        {
            OpenUriInAppBrowserCore(uri, communication);
        }

        public static void OpenUriInAppBrowser(object parameter, ICommunication communication)
        {
            Uri uri;
            if(Uri.TryCreate((string)parameter, UriKind.RelativeOrAbsolute, out uri)) {
                OpenUriInAppBrowserCore(uri, communication);
            }
        }

        public static void CopyUri(object parameter, ILogger logger)
        {
            CopyText((string)parameter, logger);
        }

        #endregion
    }
}
