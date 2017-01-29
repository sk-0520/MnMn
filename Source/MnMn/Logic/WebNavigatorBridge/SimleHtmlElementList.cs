using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Data.WebNavigatorBridge;
using ContentTypeTextNet.MnMn.MnMn.Model.WebNavigatorBridge;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.WebNavigatorBridge;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.WebNavigatorBridge
{
    public class SimleHtmlElementList
    {
        public SimleHtmlElementList(Regex tagNameRegex, IEnumerable<SimpleHtmlElement> nodes)
        {
            TagNameRegex = tagNameRegex;
            Nodes = nodes.ToList();
            NodesPath = GetNodesPath(Nodes);

            HitTagNameInNodesPath = TagNameRegex.IsMatch(NodesPath);
        }

        public SimleHtmlElementList(Regex tagNameRegex, IEnumerable<SimpleHtmlElement> nodes, SimpleHtmlElement element)
            :this(tagNameRegex, nodes.Concat(new[] { element }))
        { }

        #region property

        public Regex TagNameRegex { get; }
        public IReadOnlyList<SimpleHtmlElement> Nodes { get; }
        public string NodesPath { get; }
        public bool HitTagNameInNodesPath { get; }

        #endregion

        #region function

        static string GetNodesPath(IEnumerable<SimpleHtmlElement> nodes)
        {
            return string.Join("/", nodes.Select(n => n.Name));
        }

        public MatchSmileHtmlElement MatchElement<TModel>(WebNavigatorElementConditionTagViewModelBase<TModel> targetElement)
            where TModel : WebNavigatorElementConditionTagModel
        {
            for(var i = 0; i < Nodes.Count; i++) {
                var currentElements = Nodes
                    .Take(i + 1)
                    .ToList()
                ;
                var elementsCurrentNodePath = GetNodesPath(currentElements);
                if(TagNameRegex.IsMatch(elementsCurrentNodePath)) {
                    var tagetElement = currentElements.Last();
                    if(tagetElement.Attributes.ContainsKey(targetElement.Attribute)) {
                        var attributeValue = tagetElement.Attributes[targetElement.Attribute];
                        var match = targetElement.ValueRegex.Match(attributeValue);
                        if(match.Success) {
                            return new MatchSmileHtmlElement(true, tagetElement, match);
                        }
                    }
                }
            }

            return new MatchSmileHtmlElement(false, null, null);
        }


        #endregion
    }
}
