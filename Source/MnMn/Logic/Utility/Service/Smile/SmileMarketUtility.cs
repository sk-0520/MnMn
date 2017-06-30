using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile
{
    public class SmileMarketUtility
    {
        #region function

        static string TrimCountNoise(string s)
        {
            return s?.Replace("人", string.Empty);
        }

        static public IEnumerable<SmileMarketVideoItemModel> GetVideoRelationItems(RawSmileMarketVideoRelationModel model)
        {
            var htmlDocument = HtmlUtility.CreateHtmlDocument(model.Main);

            var itemElements = htmlDocument.DocumentNode.SelectNodes("//*[@class='ichiba_mainitem']");
            if(itemElements == null) {
                yield break;
            }

            foreach(var itemElement in itemElements) {
                var item = new SmileMarketVideoItemModel();

                var thumbnailElement = itemElement.Descendants().First(n => n.Attributes.Contains("class") && n.Attributes["class"].Value.Contains("thumbnail"));
                var cashRegisterElement = thumbnailElement.SelectSingleNode(".//a");
                var imageElement = thumbnailElement.SelectSingleNode(".//img");
                var standbyElement = thumbnailElement.SelectSingleNode(".//span[last()][not(@id)]");

                item.CashRegisterUrl = cashRegisterElement.GetAttributeValue("href", string.Empty);
                item.ThumbnailUrl = imageElement.GetAttributeValue("src", string.Empty);
                item.Standby = standbyElement?.InnerText;
                item.Title = imageElement.GetAttributeValue("alt", imageElement.GetAttributeValue("title", string.Empty));

                var makerElement = itemElement.SelectSingleNode(".//*[@class='maker']");
                if(makerElement != null) {
                    item.Maker = makerElement.InnerText;
                }

                var actionElement = itemElement.SelectSingleNode(".//*[@class='action']");
                var buyElement = actionElement?.SelectSingleNode(".//*[@class='buy']");
                var clickElement = actionElement?.SelectSingleNode(".//*[@class='click']");
                var videoElement = actionElement?.SelectSingleNode(".//span[last()][not(@class)]"); // この動画でクリック

                if(buyElement != null) {
                    item.BuyCount = TrimCountNoise(buyElement.InnerText);
                }
                if(clickElement != null) {
                    item.ViewCount = TrimCountNoise(clickElement.InnerText);
                }
                if(videoElement != null) {
                    item.ReferenceViewCount = TrimCountNoise(videoElement.InnerText);
                }

                var marketElement = itemElement.SelectSingleNode(".//*[@class='goIchiba']/a");
                item.MarketUrl = marketElement.GetAttributeValue("href", string.Empty);

                yield return item;
            }
        }

        #endregion
    }
}
