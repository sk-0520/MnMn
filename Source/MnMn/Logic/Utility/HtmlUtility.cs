using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility
{
    public static class HtmlUtility
    {
        static HtmlDocument CreateHtmlDocumentCore()
        {
            var htmlDocument = new HtmlDocument() {
                OptionAutoCloseOnEnd = true,
            };

            return htmlDocument;
        }
        public static HtmlDocument CreateHtmlDocument(Stream stream)
        {
            var htmlDocument = CreateHtmlDocumentCore();
            htmlDocument.Load(stream);

            return htmlDocument;
        }

        public static HtmlDocument CreateHtmlDocument(string htmlSource)
        {
            var htmlDocument = CreateHtmlDocumentCore();
            htmlDocument.LoadHtml(htmlSource);

            return htmlDocument;
        }
    }
}
