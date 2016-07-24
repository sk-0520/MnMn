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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using MnMn.View.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Bookmark
{
    public class SmileVideoBookmarkManagerViewModel: SmileVideoCustomManagerViewModelBase
    {
        #region variable

        SmileVideoBookmarkNodeFinderViewModel _selectedBookmarkNodeFinder;
        SmileVideoBookmarkNodeViewModel _selectedBookmarkNode;

        #endregion

        public SmileVideoBookmarkManagerViewModel(Mediation mediation)
            : base(mediation)
        {
            Node = new SmileVideoBookmarkNodeViewModel(Setting.Bookmark);
            NodeItems = Node.NodeList.ViewModelList;
        }

        #region property

        public SmileVideoBookmarkNodeViewModel Node { get; }

        public CollectionModel<SmileVideoBookmarkNodeViewModel> NodeItems { get; }

        public SmileVideoBookmarkNodeViewModel SelectedBookmarkNode
        {
            get { return this._selectedBookmarkNode; }
            set {
                if(SetVariableValue(ref this._selectedBookmarkNode, value)) {
                    if(SelectedBookmarkNode != null) {
                        SelectedBookmarkNode.ClearEditingValue();
                    }
                }
            }
        }

        public SmileVideoBookmarkNodeFinderViewModel SelectedBookmarkNodeFinder
        {
            get { return this._selectedBookmarkNodeFinder; }
            set {
                if(SetVariableValue(ref this._selectedBookmarkNodeFinder, value)) {
                    if(SelectedBookmarkNodeFinder.CanLoad) {
                        SelectedBookmarkNodeFinder.LoadDefaultCacheAsync().ConfigureAwait(false);
                    }
                }
            }
        }


        #endregion

        #region command

        public ICommand AddNodeCommand
        {
            get
            {
                return CreateCommand(o => {
                    var parentNodeViewModel = GetSelectedNode();
                    AddNode(parentNodeViewModel);
                });
            }
        }
        public ICommand RemoveNodeCommand
        {
            get {
                return CreateCommand(o => {
                    var nodeViewModel = GetSelectedNode();
                    RemoveNode(nodeViewModel);
                });
            }
        }

        #endregion

        #region function

        IEnumerable<SmileVideoBookmarkNodeViewModel> GetNodesCore(SmileVideoBookmarkNodeViewModel node, Func<SmileVideoBookmarkNodeViewModel, bool> expr)
        {
            foreach(var child in node.NodeItems.Where(expr)) {
                foreach(var item in GetNodesCore(child, expr)) {
                    yield return item;
                }
            }

            if(expr(node)) {
                yield return node;
            }
        }
        IEnumerable<SmileVideoBookmarkNodeViewModel> GetNodes(Func<SmileVideoBookmarkNodeViewModel, bool> expr)
        {
            return NodeItems.Select(n => GetNodesCore(n, expr)).SelectMany(ns => ns);
        }

        /// <summary>
        /// 選択中ノードの取得。
        /// </summary>
        /// <returns>選択されているノード。選択されていなければ null 。</returns>
        SmileVideoBookmarkNodeViewModel GetSelectedNode()
        {
            var item = GetNodes(n => n.IsSelected).FirstOrDefault();
            return item;
        }

        void AddNode(SmileVideoBookmarkNodeViewModel parentNodeViewModel)
        {
        }

        void RemoveNode(SmileVideoBookmarkNodeViewModel nodeViewModel)
        {
        }

        #endregion

        #region SmileVideoCustomManagerViewModelBase

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override void InitializeView(MainWindow view)
        {
            view.smile.bookmark.treeNodes.SelectedItemChanged += TreeNodes_SelectedItemChanged;
        }

        public override void UninitializeView(MainWindow view)
        {
            view.smile.bookmark.treeNodes.SelectedItemChanged -= TreeNodes_SelectedItemChanged;
        }

        #endregion

        void TreeNodes_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            var node = e.NewValue as SmileVideoBookmarkNodeViewModel;
            if(node != null) {
                SelectedBookmarkNode = node;

                var finder = new SmileVideoBookmarkNodeFinderViewModel(Mediation, SelectedBookmarkNode);
                SelectedBookmarkNodeFinder = finder;
            }
        }

    }
}
