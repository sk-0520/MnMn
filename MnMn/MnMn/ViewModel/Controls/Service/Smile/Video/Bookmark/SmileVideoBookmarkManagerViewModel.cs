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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using MnMn.View.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Bookmark
{
    public class SmileVideoBookmarkManagerViewModel: SmileVideoCustomManagerViewModelBase
    {
        #region define

        static readonly string DragNodeFormat = Constants.ApplicationName + "**Node";

        #endregion

        #region variable

        SmileVideoBookmarkNodeFinderViewModel _selectedBookmarkNodeFinder;
        SmileVideoBookmarkNodeViewModel _selectedBookmarkNode;

        bool _isDragging = false;
        Point _dragStartPosition;
        TreeView _treeNodes;

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
                    var finder = new SmileVideoBookmarkNodeFinderViewModel(Mediation, SelectedBookmarkNode);
                    SelectedBookmarkNodeFinder = finder;
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

        public bool IsDragging
        {
            get { return this._isDragging; }
            set { SetVariableValue(ref this._isDragging, value); }
        }

        #endregion

        #region command

        public ICommand AddNodeCommand
        {
            get
            {
                return CreateCommand(o => {
                    InsertNode(null);
                });
            }
        }
        public ICommand InsertNodeCommand
        {
            get
            {
                return CreateCommand(o => {
                    var parentNodeViewModel = GetSelectedNode();
                    InsertNode(parentNodeViewModel);
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
            foreach(var child in node.NodeItems) {
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
            foreach(var node in NodeItems) {
                foreach(var item in GetNodesCore(node, expr)) {
                    yield return item;
                }
            }
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

        void InsertNode(SmileVideoBookmarkNodeViewModel parentNodeViewModel)
        {
            var model = new SmileVideoBookmarkItemSettingModel() {
                Name = global::ContentTypeTextNet.MnMn.MnMn.Properties.Resources.String_Service_Smile_SmileVideo_Bookmark_NewName,
            };
            var target = parentNodeViewModel == null
                ? Node.NodeList
                : parentNodeViewModel.NodeList
            ;
            var pair= target.Add(model, null);
            pair.ViewModel.IsSelected = true;
        }

        void RemoveNode(SmileVideoBookmarkNodeViewModel nodeViewModel)
        {
            var parentNode = GetNodes(n => n.NodeItems.Any(nv => nv == nodeViewModel)).FirstOrDefault();
            if(parentNode != null) {
                parentNode.NodeItems.Remove(nodeViewModel);
            } else {
                NodeItems.Remove(nodeViewModel);
            }
        }

        /// <summary>
        /// 座標からデータを取得。
        /// </summary>
        /// <param name="treeView">指定ツリービュー。</param>
        /// <param name="position">ツリービューの原点を基点とした座標。</param>
        /// <returns></returns>
        SmileVideoBookmarkNodeViewModel GetToolbarNode(TreeView treeView, Point position)
        {
            var node = treeView.InputHitTest(position);

            var hitTestResults = VisualTreeHelper.HitTest(treeView, position);
            SmileVideoBookmarkNodeViewModel result = null;
            if(hitTestResults != null) {
                CastUtility.AsAction<FrameworkElement>(hitTestResults.VisualHit, element => {
                    result = element.DataContext as SmileVideoBookmarkNodeViewModel;
                });
            }

            return result;
        }

        #endregion

        #region SmileVideoCustomManagerViewModelBase

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        protected override void ShowView()
        {
            if(SelectedBookmarkNode != null && SelectedBookmarkNodeFinder.CanLoad) {
                SelectedBookmarkNodeFinder.LoadDefaultCacheAsync().ConfigureAwait(false);
            } else {
                SelectedBookmarkNode = NodeItems.FirstOrDefault();
            }
            if(SelectedBookmarkNode != null) {
                SelectedBookmarkNode.IsSelected = true;
            }
            base.ShowView();
        }

        public override void InitializeView(MainWindow view)
        {
            this._treeNodes = view.smile.bookmark.treeNodes;
            view.smile.bookmark.treeNodes.SelectedItemChanged += TreeNodes_SelectedItemChanged;
            view.smile.bookmark.treeNodes.PreviewMouseLeftButtonDown += TreeNodes_PreviewMouseLeftButtonDown;
            view.smile.bookmark.treeNodes.MouseMove += TreeNodes_MouseMove;
            view.smile.bookmark.treeNodes.Drop += TreeNodes_Drop;
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
            }
        }

        private void TreeNodes_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this._dragStartPosition = e.GetPosition(null);
        }

        private void TreeNodes_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton != MouseButtonState.Pressed) {
                return;
            }

            if(IsDragging) {
                return;
            }

            var isScrollDrag = GetToolbarNode(this._treeNodes, e.GetPosition(this._treeNodes)) == null;
            if(isScrollDrag) {
                // スクロールバーD&DはアイテムD&Dしない
                return;
            }

            var treeView = (TreeView)sender;
            var node = treeView.SelectedItem as SmileVideoBookmarkNodeViewModel;

            var nowPosition = e.GetPosition(null);
            var size = new Size(10, 10);

            var isDragX = Math.Abs(nowPosition.X - this._dragStartPosition.X) > size.Width;
            var isDragY = Math.Abs(nowPosition.Y - this._dragStartPosition.Y) > size.Height;
            if(isDragX || isDragY) {
                //var treeView = (TreeView)sender;
                treeView.AllowDrop = true;
                CastUtility.AsAction<SmileVideoBookmarkNodeViewModel>(treeView.SelectedItem, selectedNode => {
                    var item = new DataObject(DragNodeFormat, selectedNode);
                    IsDragging = true;
                    DragDrop.DoDragDrop(treeView, item, DragDropEffects.Move);
                    IsDragging = false;
                    treeView.AllowDrop = false;
                });
            }
        }

        private void TreeNodes_Drop(object sender, DragEventArgs e)
        {
            e.Handled = true;
            e.Effects = DragDropEffects.None;

            if(e.Data.GetDataPresent(DragNodeFormat)) {
                var srcNode = (SmileVideoBookmarkNodeViewModel)e.Data.GetData(DragNodeFormat);
                var dstNode = GetToolbarNode(this._treeNodes, e.GetPosition(this._treeNodes));
                if(dstNode != null && srcNode != dstNode) {

                }
            }
        }


    }
}
