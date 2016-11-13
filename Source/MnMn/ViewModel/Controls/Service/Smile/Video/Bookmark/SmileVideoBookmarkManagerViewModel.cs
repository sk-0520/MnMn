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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Data;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;

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

        //bool _isDragging = false;
        //Point _dragStartPosition;

        bool _nodeAllowDrop = true;
        bool _nodeIsEnabledDrag = true;

        #endregion

        public SmileVideoBookmarkManagerViewModel(Mediation mediation)
            : base(mediation)
        {
            Node = new SmileVideoBookmarkSystemNodeViewModel(Setting.Bookmark.Root);
            Node.Name = Properties.Resources.String_Service_Smile_SmileVideo_Bookmark_Unorganized;
            SystemNodes = new CollectionModel<SmileVideoBookmarkSystemNodeViewModel>() {
                Node,
            };
            UserNodes = Node.NodeList.ViewModelList;

            NodeDragAndDrop = new DelegateDragAndDrop() {
                CanDragStartFunc = CanDragStartNode,
                GetDragParameterFunc = GetDragParameterNode,
                DragEnterAction = DragEnterAndOverNode,
                DragOverAction = DragEnterAndOverNode,
                DragLeaveAction = DragLeaveNode,
                DropAction = DropNode,
            };
        }

        #region property

        TreeView TreeNodes { get; set; }

        public IDragAndDrop NodeDragAndDrop { get; }

        public SmileVideoBookmarkSystemNodeViewModel Node { get; }

        public CollectionModel<SmileVideoBookmarkSystemNodeViewModel> SystemNodes { get; }
        public CollectionModel<SmileVideoBookmarkNodeViewModel> UserNodes { get; }

        public SmileVideoBookmarkNodeViewModel SelectedBookmarkNode
        {
            get { return this._selectedBookmarkNode; }
            set
            {
                if(SetVariableValue(ref this._selectedBookmarkNode, value)) {
                    if(SelectedBookmarkNode != null) {
                        //SelectedBookmarkNode.ClearEditingValue();
                        var finder = new SmileVideoBookmarkNodeFinderViewModel(Mediation, SelectedBookmarkNode);
                        SelectedBookmarkNodeFinder = finder;
                    }
                }
            }
        }

        public SmileVideoBookmarkNodeFinderViewModel SelectedBookmarkNodeFinder
        {
            get { return this._selectedBookmarkNodeFinder; }
            set
            {
                if(SetVariableValue(ref this._selectedBookmarkNodeFinder, value)) {
                    if(SelectedBookmarkNodeFinder != null && SelectedBookmarkNodeFinder.CanLoad) {
                        SelectedBookmarkNodeFinder.LoadDefaultCacheAsync().ConfigureAwait(false);
                    }
                }
            }
        }

        public GridLength GroupWidth
        {
            get { return new GridLength(Setting.Bookmark.GroupWidth, GridUnitType.Star); }
            set { SetPropertyValue(Setting.Bookmark, value.Value, nameof(Setting.Bookmark.GroupWidth)); }
        }
        public GridLength ItemsWidth
        {
            get { return new GridLength(Setting.Bookmark.ItemsWidth, GridUnitType.Star); }
            set { SetPropertyValue(Setting.Bookmark, value.Value, nameof(Setting.Bookmark.ItemsWidth)); }
        }

        //public bool IsDragging
        //{
        //    get { return this._isDragging; }
        //    set { SetVariableValue(ref this._isDragging, value); }
        //}

        public bool NodeAllowDrop
        {
            get { return this._nodeAllowDrop; }
            set { SetVariableValue(ref this._nodeAllowDrop, value); }
        }
        public bool NodeIsEnabledDrag
        {
            get { return this._nodeIsEnabledDrag; }
            set { SetVariableValue(ref this._nodeIsEnabledDrag, value); }
        }


        #endregion

        #region command

        public ICommand AddNodeCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var node = SelectedBookmarkNode;
                        if(node != null) {
                            node = GetParentNode(node);
                        } else {
                            node = Node;
                        }
                        AddNode(node);
                    },
                    o => SelectedBookmarkNode != null
                );
            }
        }
        public ICommand InsertNodeCommand
        {
            get
            {
                return CreateCommand(
                    o => InsertNode(SelectedBookmarkNode),
                    o => SelectedBookmarkNode != null && !SelectedBookmarkNode.IsSystemNode
                );
            }
        }
        public ICommand RemoveNodeCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        RemoveNode(SelectedBookmarkNode);
                        SelectedBookmarkNode = GetSelectedNode();
                    },
                    o => SelectedBookmarkNode != null && !SelectedBookmarkNode.IsSystemNode
                );
            }
        }

        public ICommand UpNodeCommand
        {
            get
            {
                return CreateCommand(
                    o => UpNode(SelectedBookmarkNode),
                    o => {
                        if(SelectedBookmarkNode == null) {
                            return false;
                        }
                        if(SelectedBookmarkNode.IsSystemNode) {
                            return false;
                        }
                        var parentNode = GetParentNode(SelectedBookmarkNode);
                        return 0 < parentNode.NodeItems.IndexOf(SelectedBookmarkNode);
                    }
                );
            }
        }

        public ICommand DownNodeCommand
        {
            get
            {
                return CreateCommand(
                    o => DownNode(SelectedBookmarkNode),
                    o => {
                        if(SelectedBookmarkNode == null) {
                            return false;
                        }
                        if(SelectedBookmarkNode.IsSystemNode) {
                            return false;
                        }
                        var parentNode = GetParentNode(SelectedBookmarkNode);
                        return parentNode.NodeItems.Count - 1 > parentNode.NodeItems.IndexOf(SelectedBookmarkNode);
                    }
                );
            }
        }

        public ICommand UpParentNodeCommand
        {
            get
            {
                return CreateCommand(
                    o => UpParentNode(SelectedBookmarkNode),
                    o => SelectedBookmarkNode != null && !SelectedBookmarkNode.IsSystemNode && GetParentNode(SelectedBookmarkNode) != Node
                );
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
            foreach(var node in UserNodes) {
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

        SmileVideoBookmarkNodeViewModel GetParentNode(SmileVideoBookmarkNodeViewModel node)
        {
            return GetNodes(_ => true)
                .FirstOrDefault(n => n.NodeItems.Any(nn => nn == node))
                ?? Node
            ;
        }

        void AddNode(SmileVideoBookmarkNodeViewModel parentNodeViewModel)
        {
            var model = new SmileVideoBookmarkItemSettingModel() {
                Name = TextUtility.ToUniqueDefault(global::ContentTypeTextNet.MnMn.MnMn.Properties.Resources.String_Service_Smile_SmileVideo_Bookmark_NewName, GetNodes(_ => true).Select(n => n.Name)),
            };
            var pair = parentNodeViewModel.NodeList.Add(model, null);
            pair.ViewModel.IsSelected = true;
        }

        void InsertNode(SmileVideoBookmarkNodeViewModel parentNodeViewModel)
        {
            var model = new SmileVideoBookmarkItemSettingModel() {
                Name = TextUtility.ToUniqueDefault(global::ContentTypeTextNet.MnMn.MnMn.Properties.Resources.String_Service_Smile_SmileVideo_Bookmark_NewName, GetNodes(_ => true).Select(n => n.Name)),
            };
            //var target = parentNodeViewModel == null
            //    ? Node.NodeList
            //    : parentNodeViewModel.NodeList
            //;
            var pair = parentNodeViewModel.NodeList.Add(model, null);
            pair.ViewModel.IsSelected = true;
        }

        void RemoveNode(SmileVideoBookmarkNodeViewModel nodeViewModel)
        {
            var parentNode = GetParentNode(nodeViewModel);
            //if(parentNode != null) {
            parentNode.NodeList.Remove(nodeViewModel);
            //} else {
            //    Node.NodeList.Remove(nodeViewModel);
            //}
        }

        /// <summary>
        /// 座標からデータを取得。
        /// </summary>
        /// <param name="treeView">指定ツリービュー。</param>
        /// <param name="position">ツリービューの原点を基点とした座標。</param>
        /// <returns></returns>
        SmileVideoBookmarkNodeViewModel GetNodeFromPosition(TreeView treeView, Point position)
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

        private void UpNode(SmileVideoBookmarkNodeViewModel node)
        {
            var parentNode = GetParentNode(node);
            var srcIndex = parentNode.NodeItems.IndexOf(node);
            if(srcIndex != 0) {
                parentNode.NodeList.SwapIndex(srcIndex, srcIndex - 1);
            }
        }

        private void DownNode(SmileVideoBookmarkNodeViewModel node)
        {
            var parentNode = GetParentNode(node);
            var srcIndex = parentNode.NodeItems.IndexOf(node);
            if(srcIndex != parentNode.NodeItems.Count - 1) {
                parentNode.NodeList.SwapIndex(srcIndex, srcIndex + 1);
            }
        }

        private void UpParentNode(SmileVideoBookmarkNodeViewModel node)
        {
            var parentNode = GetParentNode(node);
            if(parentNode != Node) {
                var ancestorNode = GetParentNode(parentNode);
                RemoveNode(node);
                var parentIndex = ancestorNode.NodeItems.IndexOf(parentNode);
                var nextIndex = parentIndex + 1;
                var newNode = ancestorNode.NodeList.Insert(nextIndex, node.Model, null);
                newNode.ViewModel.IsSelected = true;
            }
        }

        protected virtual bool CanDragStartNode(UIElement sender, MouseEventArgs e)
        {
            var isScrollDrag = GetNodeFromPosition(TreeNodes, e.GetPosition(TreeNodes)) == null;
            // スクロールバーD&DはアイテムD&Dしない
            if(isScrollDrag) {
                return false;
            }

            return SelectedBookmarkNode != null;
        }

        CheckResultModel<DragParameterModel> GetDragParameterNode(UIElement sender, MouseEventArgs e)
        {
            var data = new DataObject(DragNodeFormat, SelectedBookmarkNode);
            var param = new DragParameterModel() {
                Data = data,
                Effects = DragDropEffects.Move,
                Element = sender,
            };

            return CheckResultModel.Success(param);
        }

        void DragEnterAndOverNode(UIElement sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DragNodeFormat)) {
                var srcNode = (SmileVideoBookmarkNodeViewModel)e.Data.GetData(DragNodeFormat);
                if(srcNode.IsSystemNode) {
                    e.Effects = DragDropEffects.None;
                } else {
                    var dstNode = GetNodeFromPosition(TreeNodes, e.GetPosition(TreeNodes));
                    if(dstNode != null && srcNode != dstNode && !dstNode.IsSystemNode) {
                        var isChildNode = GetNodesCore(srcNode, _ => true).Any(n => n == dstNode);
                        if(isChildNode) {
                            e.Effects = DragDropEffects.None;
                        } else if(dstNode == GetParentNode(srcNode)) {
                            e.Effects = DragDropEffects.None;
                        }
                    } else {
                        e.Effects = DragDropEffects.None;
                    }
                }
            } else if(e.Data.GetDataPresent(typeof(SmileVideoFinderItemViewModel))) {
                var dstNode = GetNodeFromPosition(TreeNodes, e.GetPosition(TreeNodes));
                if(dstNode != SelectedBookmarkNode) {
                    e.Effects = DragDropEffects.Move;
                } else {
                    e.Effects = DragDropEffects.None;
                }
            } else {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        void DragLeaveNode(UIElement sender, DragEventArgs e)
        { }

        void DropNode(UIElement sender, DragEventArgs e)
        {
            e.Handled = true;
            e.Effects = DragDropEffects.None;

            if(e.Data.GetDataPresent(DragNodeFormat)) {
                var srcNode = (SmileVideoBookmarkNodeViewModel)e.Data.GetData(DragNodeFormat);
                var dstNode = GetNodeFromPosition(TreeNodes, e.GetPosition(TreeNodes));
                if(dstNode != null && srcNode != dstNode) {
                    var srcModel = srcNode.Model;
                    RemoveNode(srcNode);
                    var item = dstNode.NodeList.Add(srcModel, null);
                    item.ViewModel.IsSelected = true;
                }
            } else if(e.Data.GetDataPresent(typeof(SmileVideoFinderItemViewModel))) {
                var finderItem = (SmileVideoFinderItemViewModel)e.Data.GetData(typeof(SmileVideoFinderItemViewModel));
                var dstNode = GetNodeFromPosition(TreeNodes, e.GetPosition(TreeNodes));
                var videoItem = finderItem.Information.ToVideoItemModel();
                dstNode.VideoItems.Add(videoItem);
                SelectedBookmarkNodeFinder.RemoveItem(SelectedBookmarkNodeFinder.SelectedFinderItem);
                SelectedBookmarkNodeFinder.SelectedFinderItem = SelectedBookmarkNodeFinder.FinderItems.Cast<SmileVideoFinderItemViewModel>().FirstOrDefault();
            }
        }

        #endregion

        #region SmileVideoCustomManagerViewModelBase

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return Enumerable.Empty<ManagerViewModelBase>();
        }

        protected override void ShowViewCore()
        {
            if(SelectedBookmarkNode != null && SelectedBookmarkNodeFinder.CanLoad) {
                SelectedBookmarkNodeFinder.LoadDefaultCacheAsync().ConfigureAwait(false);
            } else {
                if(UserNodes.Any() && !Node.VideoItems.Any()) {
                    SelectedBookmarkNode = UserNodes.FirstOrDefault();
                } else {
                    SelectedBookmarkNode = SystemNodes.First();
                }
            }
            if(SelectedBookmarkNode != null) {
                SelectedBookmarkNode.IsSelected = true;
            }
        }

        protected override void HideViewCore()
        { }

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
        public override void InitializeView(MainWindow view)
        {
            TreeNodes = view.smile.bookmark.treeNodes;
            //NodeUnorganized = view.smile.bookmark.nodeUnorganized;

            view.smile.bookmark.treeNodes.SelectedItemChanged += TreeNodes_SelectedItemChanged;
            //view.smile.bookmark.treeNodes.PreviewMouseLeftButtonDown += TreeNodes_PreviewMouseLeftButtonDown;
            //view.smile.bookmark.treeNodes.MouseMove += TreeNodes_MouseMove;
            //view.smile.bookmark.treeNodes.DragEnter += TreeNodes_DragEnterAndOver;
            //view.smile.bookmark.treeNodes.DragOver += TreeNodes_DragEnterAndOver;
            //view.smile.bookmark.treeNodes.Drop += TreeNodes_Drop;
        }

        public override void UninitializeView(MainWindow view)
        {
            view.smile.bookmark.treeNodes.SelectedItemChanged -= TreeNodes_SelectedItemChanged;
        }

        public override Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan, bool force)
        {
            return GarbageCollectionDummyResult;
        }

        #endregion

        void TreeNodes_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            var node = e.NewValue as SmileVideoBookmarkNodeViewModel;
            if(node != null) {
                SelectedBookmarkNode = node;
            } else {
                var viewNode = e.NewValue as TreeViewItem;
                //if(viewNode == NodeUnorganized) {
                //    SelectedBookmarkNode = Node;
                //}
            }
        }

        //private void TreeNodes_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    this._dragStartPosition = e.GetPosition(null);
        //}

        //private void TreeNodes_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if(e.LeftButton != MouseButtonState.Pressed) {
        //        return;
        //    }

        //    if(IsDragging) {
        //        return;
        //    }

        //    var isScrollDrag = GetNodeFromPosition(TreeNodes, e.GetPosition(TreeNodes)) == null;
        //    if(isScrollDrag) {
        //        // スクロールバーD&DはアイテムD&Dしない
        //        return;
        //    }

        //    var treeView = (TreeView)sender;
        //    var node = treeView.SelectedItem as SmileVideoBookmarkNodeViewModel;

        //    var nowPosition = e.GetPosition(null);
        //    var size = new Size(10, 10);

        //    var isDragX = Math.Abs(nowPosition.X - this._dragStartPosition.X) > size.Width;
        //    var isDragY = Math.Abs(nowPosition.Y - this._dragStartPosition.Y) > size.Height;
        //    if(isDragX || isDragY) {
        //        //var treeView = (TreeView)sender;
        //        treeView.AllowDrop = true;
        //        CastUtility.AsAction<SmileVideoBookmarkNodeViewModel>(treeView.SelectedItem, selectedNode => {
        //            var item = new DataObject(DragNodeFormat, selectedNode);
        //            IsDragging = true;
        //            DragDrop.DoDragDrop(treeView, item, DragDropEffects.Move);
        //            IsDragging = false;
        //            treeView.AllowDrop = false;
        //        });
        //    }
        //}

        //void TreeNodes_DragEnterAndOver(object sender, DragEventArgs e)
        //{
        //    if(e.Data.GetDataPresent(DragNodeFormat)) {
        //        var srcNode = (SmileVideoBookmarkNodeViewModel)e.Data.GetData(DragNodeFormat);
        //        if(srcNode.IsSystemNode) {
        //            e.Effects = DragDropEffects.None;
        //        } else {
        //            var dstNode = GetNodeFromPosition(TreeNodes, e.GetPosition(TreeNodes));
        //            if(dstNode != null && srcNode != dstNode) {
        //                var isChildNode = GetNodesCore(srcNode, _ => true).Any(n => n == dstNode);
        //                if(isChildNode) {
        //                    e.Effects = DragDropEffects.None;
        //                } else if(dstNode == GetParentNode(srcNode)) {
        //                    e.Effects = DragDropEffects.None;
        //                }
        //            } else {
        //                e.Effects = DragDropEffects.None;
        //            }
        //        }
        //    } else {
        //        e.Effects = DragDropEffects.None;
        //    }
        //    e.Handled = true;
        //}

        //private void TreeNodes_Drop(object sender, DragEventArgs e)
        //{
        //    e.Handled = true;
        //    e.Effects = DragDropEffects.None;

        //    if(e.Data.GetDataPresent(DragNodeFormat)) {
        //        var srcNode = (SmileVideoBookmarkNodeViewModel)e.Data.GetData(DragNodeFormat);
        //        var dstNode = GetNodeFromPosition(TreeNodes, e.GetPosition(TreeNodes));
        //        if(dstNode != null && srcNode != dstNode) {
        //            var srcModel = srcNode.Model;
        //            RemoveNode(srcNode);
        //            var item = dstNode.NodeList.Add(srcModel, null);
        //            item.ViewModel.IsSelected = true;
        //        }
        //    }
        //}


    }
}
