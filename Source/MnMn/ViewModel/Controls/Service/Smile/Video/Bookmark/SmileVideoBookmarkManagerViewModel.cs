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
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Exceptions.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.IF.Control;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player;
using Package.stackoverflow.com;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Bookmark
{
    public class SmileVideoBookmarkManagerViewModel : SmileVideoCustomManagerViewModelBase, IDropable
    {
        #region variable

        SmileVideoBookmarkNodeFinderViewModel _selectedBookmarkNodeFinder;
        SmileVideoBookmarkNodeViewModel _selectedBookmarkNode;
        SmileVideoBookmarkNodeViewModel _dragUnderBookmarkNode;
        bool _isDragOver;

        #endregion

        public SmileVideoBookmarkManagerViewModel(Mediator mediator)
            : base(mediator)
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
                        var finder = new SmileVideoBookmarkNodeFinderViewModel(Mediator, SelectedBookmarkNode);
                        SelectedBookmarkNodeFinder = finder;
                    }
                }
            }
        }

        /// <summary>
        /// ドラッグ中アイテムの下にあるノード。
        /// </summary>
        SmileVideoBookmarkNodeViewModel DragOverBookmarkNode
        {
            get { return this._dragUnderBookmarkNode; }
            set
            {
                var prev = DragOverBookmarkNode;

                if(SetVariableValue(ref this._dragUnderBookmarkNode, value)) {
                    if(DragOverBookmarkNode != null) {
                        DragOverBookmarkNode.IsDragOver = true;
                    }
                    if(prev != null) {
                        prev.IsDragOver = false;
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
                    o => SelectedBookmarkNode != null || !UserNodes.Any()
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

        public ICommand ExpandAllNodeCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var nodes = GetNodes(n => true);
                        foreach(var node in nodes) {
                            node.IsExpanded = true;
                        }
                    },
                    o => UserNodes.Any()
                );
            }
        }


        public ICommand SequentialPlayCommand
        {
            get
            {
                return CreateCommand(
                    o => PlayAllVideosAsync((SmileVideoBookmarkNodeViewModel)o, false).ConfigureAwait(false),
                    o => 0 < ((SmileVideoBookmarkNodeViewModel)o)?.VideoItems.Count
                );
            }
        }

        public ICommand RandomPlayCommand
        {
            get
            {
                return CreateCommand(
                    o => PlayAllVideosAsync((SmileVideoBookmarkNodeViewModel)o, true).ConfigureAwait(false),
                    o => 0 < ((SmileVideoBookmarkNodeViewModel)o)?.VideoItems.Count
                );
            }
        }

        #endregion

        #region function

        IEnumerable<SmileVideoBookmarkNodeViewModel> GetNodesCore(SmileVideoBookmarkNodeViewModel node, Predicate<SmileVideoBookmarkNodeViewModel> expr)
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
        IEnumerable<SmileVideoBookmarkNodeViewModel> GetNodes(Predicate<SmileVideoBookmarkNodeViewModel> expr)
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
                Name = TextUtility.ToUniqueDefault(global::ContentTypeTextNet.MnMn.MnMn.Properties.Resources.String_Service_Smile_SmileVideo_Bookmark_NewName, GetNodes(_ => true).Select(n => n.Name), StringComparison.Ordinal),
            };
            var pair = parentNodeViewModel.NodeList.Add(model, null);
            pair.ViewModel.IsSelected = true;
        }

        /// <summary>
        /// ブックマーク追加。
        /// </summary>
        /// <param name="targetNodeViewModel">親とする対象ブックマークノード。</param>
        /// <param name="videoItems">登録する動画。</param>
        /// <returns>登録した動画。</returns>
        public IEnumerable<SmileVideoVideoItemModel> AddBookmarkItems(SmileVideoBookmarkNodeViewModel targetNodeViewModel, IEnumerable<SmileVideoVideoItemModel> videoItems)
        {
            //var index = targetNodeViewModel.VideoItems.Count;
            //targetNodeViewModel.VideoItems.AddRange(videoItems);

            //return targetNodeViewModel.VideoItems.Skip(index);
            var addItems = videoItems
                .Where(i => targetNodeViewModel.VideoItems.All(i2 => !SmileVideoVideoItemUtility.IsEquals(i, i2)))
                .Reverse()
                .ToEvaluatedSequence()
            ;

            if(addItems.Any()) {
                //targetNodeViewModel.VideoItems.AddRange(addItems);
                foreach(var addItem in addItems) {
                    AppUtility.PlusItem(targetNodeViewModel.VideoItems, addItem);
                }
                return addItems;
            }

            return Enumerable.Empty<SmileVideoVideoItemModel>();
        }

        /// <summary>
        /// ブックマーク初期化。
        /// </summary>
        /// <param name="targetNodeViewModel">親とする対象ブックマークノード。</param>
        /// <param name="videoItems">登録する動画。</param>
        /// <returns>登録した動画。</returns>
        public IEnumerable<SmileVideoVideoItemModel> InitializeBookmarkItems(SmileVideoBookmarkNodeViewModel targetNodeViewModel, IEnumerable<SmileVideoVideoItemModel> videoItems)
        {
            targetNodeViewModel.VideoItems.Clear();
            targetNodeViewModel.VideoItems.AddRange(videoItems);

            return targetNodeViewModel.VideoItems;
        }

        /// <summary>
        /// ブックマークを新規作成。
        /// </summary>
        /// <param name="parentNode">親となるブックマークノード。 null の場合は最上位となる。</param>
        /// <param name="newNode">登録する動画。</param>
        /// <returns>登録したノード</returns>
        public SmileVideoBookmarkNodeViewModel CreateBookmark(SmileVideoBookmarkNodeViewModel parentNode, SmileVideoBookmarkItemSettingModel newNode)
        {
            if(parentNode == null) {
                parentNode = Node;
            }

            var pair = parentNode.NodeList.Add(newNode, null);
            return pair.ViewModel;
        }

        /// <summary>
        /// 未整理のブックマークへ登録。
        /// </summary>
        /// <param name="videoItem"></param>
        /// <returns>登録した動画。 登録済みの場合は null が返る(というか登録できない)。</returns>
        public SmileVideoVideoItemModel AddUnorganizedBookmark(SmileVideoVideoItemModel videoItem)
        {
            if(Node.VideoItems.All(i => !SmileVideoVideoItemUtility.IsEquals(videoItem, i))) {
                //Node.VideoItems.Add(videoItem);
                AppUtility.PlusItem(Node.VideoItems, videoItem);
                return Node.VideoItems.Last();
            }

            return null;
        }

        void InsertNode(SmileVideoBookmarkNodeViewModel parentNodeViewModel)
        {
            var model = new SmileVideoBookmarkItemSettingModel() {
                Name = TextUtility.ToUniqueDefault(global::ContentTypeTextNet.MnMn.MnMn.Properties.Resources.String_Service_Smile_SmileVideo_Bookmark_NewName, GetNodes(_ => true).Select(n => n.Name), StringComparison.Ordinal),
            };
            var pair = parentNodeViewModel.NodeList.Add(model, null);
            pair.ViewModel.IsSelected = true;
        }

        void RemoveNode(SmileVideoBookmarkNodeViewModel nodeViewModel)
        {
            var parentNode = GetParentNode(nodeViewModel);
            parentNode.NodeList.Remove(nodeViewModel);
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

        IReadOnlyCheckResult<DragParameterModel> GetDragParameterNode(UIElement sender, MouseEventArgs e)
        {
            IsDragOver = true;

            SelectedBookmarkNode.IsDragging = true;

            var data = new DataObject(SelectedBookmarkNode.GetType(), SelectedBookmarkNode);
            var param = new DragParameterModel() {
                Data = data,
                Effects = DragDropEffects.Move,
                Element = sender,
            };

            return CheckResultModel.Success(param);
        }

        void DragEnterAndOverNode(UIElement sender, DragEventArgs e)
        {
            var prev = IsDragOver;
            if(!prev) {
                IsDragOver = true;
            }
            if(e.Data.GetDataPresent(typeof(SmileVideoBookmarkNodeViewModel))) {
                var srcNode = (SmileVideoBookmarkNodeViewModel)e.Data.GetData(typeof(SmileVideoBookmarkNodeViewModel));
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
                        } else {
                            e.Effects = DragDropEffects.Move;
                            DragOverBookmarkNode = dstNode;
                        }
                    } else {
                        e.Effects = DragDropEffects.None;
                    }
                }
            } else if(e.Data.GetDataPresent(typeof(SmileVideoFinderItemViewModel)) || e.Data.GetDataPresent(typeof(IEnumerable<SmileVideoFinderItemViewModel>))) {
                var dstNode = GetNodeFromPosition(TreeNodes, e.GetPosition(TreeNodes));
                if(dstNode != null && dstNode != SelectedBookmarkNode) {
                    e.Effects = DragDropEffects.Move;
                    DragOverBookmarkNode = dstNode;
                } else {
                    e.Effects = DragDropEffects.None;
                }
            } else {
                e.Effects = DragDropEffects.None;
            }

            if(e.Effects == DragDropEffects.None) {
                DragOverBookmarkNode = null;
            } else if(IsDragOver && !prev) {
                TreeNodes.ScrollToCenterOfView(DragOverBookmarkNode, true, false);
            }

            e.Handled = true;
        }

        void DragLeaveNode(UIElement sender, DragEventArgs e)
        {
            IsDragOver = false;
            DragOverBookmarkNode = null;
            foreach(var node in GetNodes(n => n.IsDragOver).ToEvaluatedSequence()) {
                node.IsDragOver = false;
            }
        }

        void DropNode(UIElement sender, DragEventArgs e)
        {
            e.Handled = true;
            e.Effects = DragDropEffects.None;
            DragOverBookmarkNode = null;
            IsDragOver = false;

            if(e.Data.GetDataPresent(typeof(SmileVideoBookmarkNodeViewModel))) {
                var srcNode = (SmileVideoBookmarkNodeViewModel)e.Data.GetData(typeof(SmileVideoBookmarkNodeViewModel));
                srcNode.IsDragging = false;
                var dstNode = GetNodeFromPosition(TreeNodes, e.GetPosition(TreeNodes));
                if(dstNode != null && srcNode != dstNode) {
                    var srcModel = srcNode.Model;
                    RemoveNode(srcNode);
                    var item = dstNode.NodeList.Add(srcModel, null);
                    item.ViewModel.IsSelected = true;
                }
            } else {
                if(e.Data.GetDataPresent(typeof(SmileVideoFinderItemViewModel))) {
                    var finderItem = (SmileVideoFinderItemViewModel)e.Data.GetData(typeof(SmileVideoFinderItemViewModel));
                    var dstNode = GetNodeFromPosition(TreeNodes, e.GetPosition(TreeNodes));
                    if(dstNode != null) {
                        var videoItem = finderItem.Information.ToVideoItemModel();
                        dstNode.VideoItems.Add(videoItem);
                        SelectedBookmarkNodeFinder.RemoveItem(SelectedBookmarkNodeFinder.SelectedFinderItem);
                        SelectedBookmarkNodeFinder.SelectedFinderItem = SelectedBookmarkNodeFinder.FinderItems.Cast<SmileVideoFinderItemViewModel>().FirstOrDefault();
                    }
                } else if(e.Data.GetDataPresent(typeof(IEnumerable<SmileVideoFinderItemViewModel>))) {
                    var finderItems = (IEnumerable<SmileVideoFinderItemViewModel>)e.Data.GetData(typeof(IEnumerable<SmileVideoFinderItemViewModel>));
                    var dstNode = GetNodeFromPosition(TreeNodes, e.GetPosition(TreeNodes));
                    if(dstNode != null) {
                        var videoItems = finderItems.Select(i => i.Information.ToVideoItemModel());
                        dstNode.VideoItems.AddRange(videoItems);
                        foreach(var item in finderItems.ToEvaluatedSequence()) {
                            SelectedBookmarkNodeFinder.RemoveItem(item);
                        }
                        SelectedBookmarkNodeFinder.SelectedFinderItem = SelectedBookmarkNodeFinder.FinderItems.Cast<SmileVideoFinderItemViewModel>().FirstOrDefault();
                    }
                }
            }
        }

        async Task PlayAllVideosAsync(SmileVideoBookmarkNodeViewModel node, bool isRandom)
        {
            var playList = new List<SmileVideoInformationViewModel>(node.VideoItems.Count);

            foreach(var video in node.VideoItems) {
                var request = new SmileVideoInformationCacheRequestModel(new SmileVideoInformationCacheParameterModel(video.VideoId, Constants.ServiceSmileVideoThumbCacheSpan));
                try {
                    var info = await Mediator.GetResultFromRequest<Task<SmileVideoInformationViewModel>>(request);
                    playList.Add(info);
                } catch(SmileVideoGetthumbinfoFailureException ex) {
                    Mediator.Logger.Warning(ex);
                }
            }

            if(playList.Any()) {
                var playerViewModel = new SmileVideoPlayerViewModel(Mediator);
                playerViewModel.IsRandomPlay = isRandom;
                playerViewModel.SelectedBookmark = node;

                try {
                    var task = playerViewModel.LoadAsync(playList, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan);
                    Mediator.Request(new ShowViewRequestModel(RequestKind.ShowView, ServiceType.SmileVideo, playerViewModel, ShowViewState.Foreground));
                } catch(SmileVideoCanNotPlayItemInPlayListException ex) {
                    Mediator.Logger.Warning(ex);
                    playerViewModel.Dispose();
                }
            } else {
                Mediator.Logger.Warning($"{node.Name}: {nameof(playList)}: empty");
            }
        }

        void FilterIssue611(SmileVideoBookmarkNodeViewModel node)
        {
            var items = new List<SmileVideoVideoItemModel>(node.VideoItems.Count);
            foreach(var nodeItem in node.VideoItems) {
                if(items.All(i => !SmileVideoVideoItemUtility.IsEquals(i, nodeItem))) {
                    items.Add(nodeItem);
                }
            }
            node.VideoItems.InitializeRange(items);

            foreach(var nodeItem in node.NodeItems) {
                FilterIssue611(nodeItem);
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
            // #611
            FilterIssue611(Node);

            return Task.CompletedTask;
        }

        public override Task UninitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override void InitializeView(MainWindow view)
        {
            TreeNodes = view.smile.bookmark.treeNodes;

            view.smile.bookmark.treeNodes.SelectedItemChanged += TreeNodes_SelectedItemChanged;
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

        #region IDropable

        public bool IsDragOver
        {
            get { return this._isDragOver; }
            set { SetVariableValue(ref this._isDragOver, value); }
        }

        #endregion

        void TreeNodes_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            var node = e.NewValue as SmileVideoBookmarkNodeViewModel;
            if(node != null) {
                SelectedBookmarkNode = node;
            } else {
                var viewNode = e.NewValue as TreeViewItem;
            }
        }
    }
}
