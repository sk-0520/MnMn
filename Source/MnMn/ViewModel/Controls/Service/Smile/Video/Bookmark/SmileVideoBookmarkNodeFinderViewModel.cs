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
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.History;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Bookmark
{
    public class SmileVideoBookmarkNodeFinderViewModel: SmileVideoFeedFinderViewModelBase
    {
        #region variable

        bool _isUpDownEnabled;

        #endregion

        public SmileVideoBookmarkNodeFinderViewModel(Mediation mediation, SmileVideoBookmarkNodeViewModel node)
            : base(mediation, 0)
        {
            Node = node;
        }

        #region property

        SmileVideoBookmarkNodeViewModel Node { get; }

        public bool IsUpDownEnabled
        {
            get { return this._isUpDownEnabled; }
            set { SetVariableValue(ref this._isUpDownEnabled, value); }
        }

        #endregion

        #region command

        public ICommand UpSelectedVideoCommand
        {
            get
            {
                return CreateCommand(o => {
                    var information = SelectedFinderItem?.Information;
                    if(information != null) {
                        UpDownVideo(information, IsAscending);
                    }
                });
            }
        }

        public ICommand DownSelectedVideoCommand
        {
            get
            {
                return CreateCommand(o => {
                    var information = SelectedFinderItem?.Information;
                    if(information != null) {
                        UpDownVideo(information, !IsAscending);
                    }
                });
            }
        }

        public ICommand RemoveCheckedVideosCommand
        {
            get
            {
                return CreateCommand(o => {
                    var videos = GetCheckedItems();
                    if(videos.Any()) {
                        RemoveCheckedVideos(videos);
                    }
                });
            }
        }


        #endregion

        #region function

        void UpDownVideo(SmileVideoInformationViewModel videoInformation, bool isUp)
        {
            var srcIndex = FinderItemList.FindIndex(i => i.Information == videoInformation);
            var nextIndex = isUp ? srcIndex - 1 : srcIndex + 1;
            if(isUp && srcIndex == 0) {
                return;
            } else if(!isUp && FinderItemList.Count - 1 == srcIndex) {
                return;
            }
            var srcVideo = FinderItemList[srcIndex];
            var nextVideo = FinderItemList[nextIndex];

            var srcNumber = srcVideo.Number;
            srcVideo.Number = nextVideo.Number;
            nextVideo.Number = srcNumber;

            FinderItemList.SwapIndex(srcIndex, nextIndex);
            Node.VideoItems.SwapIndex(srcIndex, nextIndex);
        }

        void RemoveCheckedVideos(IEnumerable<SmileVideoFinderItemViewModel> finderItems)
        {
            foreach(var finderItem in finderItems.ToArray()) {
                var index = FinderItemList.IndexOf(finderItem);
                FinderItemList.RemoveAt(index);
                Node.VideoItems.RemoveAt(index);
            }
        }

        public void RemoveItem(SmileVideoFinderItemViewModel finderItem)
        {
            var index = FinderItemList.IndexOf(finderItem);
            if(index != -1) {
                FinderItemList.RemoveAt(index);
                Node.VideoItems.RemoveAt(index);
            }
        }

        #endregion

        #region SmileVideoFeedFinderViewModelBase

        protected override SmileVideoInformationFlags InformationFlags { get; } = SmileVideoInformationFlags.MylistCounter | SmileVideoInformationFlags.CommentCounter | SmileVideoInformationFlags.ViewCounter;

        public override bool IsUsingFinderFilter { get; } = false;

        public override bool IsEnabledCheckItLaterMenu { get; } = false;
        public override bool IsEnabledUnorganizedBookmarkMenu { get; } = false;

        protected override Task<FeedSmileVideoModel> LoadFeedAsync()
        {
            var result = new FeedSmileVideoModel();
            foreach(var model in Node.VideoItems) {
                var item = new FeedSmileVideoItemModel();

                item.Title = model.VideoTitle;
                item.Link = "/" + model.WatchUrl.OriginalString;

                var detailModel = new RawSmileVideoFeedDetailModel();
                detailModel.Title = model.VideoTitle;
                detailModel.VideoId = model.VideoId;
                detailModel.Length = SmileVideoFeedUtility.ConvertM3H2TimeFromTimeSpan(model.Length);

                item.Description = SmileVideoFeedUtility.ConvertDescriptionFromFeedDetailModel(detailModel);
                result.Channel.Items.Add(item);
            }

            return Task.FromResult(result);
        }

        internal override void ChangeSortItems()
        {
            base.ChangeSortItems();

            IsUpDownEnabled = SelectedSortType == SmileVideoSortType.Number && FinderItems.Cast<object>().Count() == FinderItemList.Count;

        }

        protected override bool CanDragStartFromFinder(UIElement sender, MouseEventArgs e)
        {
            if(SelectedFinderItem != null) {
                var position = e.GetPosition(sender);
                var item = sender.InputHitTest(position);
                var hitTestResults = VisualTreeHelper.HitTest(sender, position);
                if(hitTestResults != null) {
                    var element = hitTestResults.VisualHit as FrameworkElement;
                    if(element != null) {
                        return element.DataContext is SmileVideoFinderItemViewModel;
                    }
                }
            }

            return false;
        }

        protected override CheckResultModel<DragParameterModel> GetDragParameterFromFinder(UIElement sender, MouseEventArgs e)
        {
            DataObject data;
            if(SelectedFinderItem.IsChecked.GetValueOrDefault()) {
                var items = GetCheckedItems().ToList();
                data = new DataObject(typeof(IEnumerable<SmileVideoFinderItemViewModel>), items);
            } else {
                data = new DataObject(SelectedFinderItem.GetType(), SelectedFinderItem);
            }

            var param = new DragParameterModel() {
                Data = data,
                Effects = DragDropEffects.Move,
                Element = sender,
            };

            return CheckResultModel.Success(param);
        }

        #endregion

    }
}
