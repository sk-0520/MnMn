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
using System.Windows.Media;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Define.UI.Player;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Bookmark;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.MyList;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player
{
    // 変数部分。
    partial class SmileVideoPlayerViewModel
    {
        #region variable

        bool _canVideoPlay;
        bool _isVideoPlayng;

        float _videoPosition;
        float _prevStateChangedPosition;

        WindowState _state = WindowState.Normal;
        double _left;
        double _top;
        double _width;
        double _height;
        [Obsolete]
        bool _topmost;

        TopmostKind _topmostKind;

        double _viewScale;

        bool _playerShowDetailArea;
        bool _showNormalWindowCommentList;
        bool _showFullScreenCommentList;
        bool _playerVisibleComment;
        bool _isAutoScroll;
        int _volume;
        bool _isMute;
        bool _replayVideo;
        bool _isEnabledDisplayCommentLimit;
        int _displayCommentLimitCount;

        bool _isEnabledSharedNoGood;
        int _sharedNoGoodScore;

        double _commentAreaWidth;
        double _commentAreaHeight;

        TimeSpan _totalTime;
        TimeSpan _playTime;

        SmileVideoCommentViewModel _selectedComment;

        LoadState _relationVideoLoadState;
        LoadState _marketLoadState;

        double _realVideoWidth;
        double _realVideoHeight;
        double _baseWidth;
        double _baseHeight;

        double _commentEnabledPercent = 100;
        double _commentEnabledHeight;
        bool _showEnabledCommentPreviewArea = false;
        bool _isEnabledOriginalPosterCommentArea = false;

        PlayerState _playerState;

        IReadOnlyList<SmileVideoAccountMyListFinderViewModel> _accountMyListItems;
        IReadOnlyList<SmileVideoBookmarkNodeViewModel> _bookmarkItems;

        SmileVideoFilteringCommentType _filteringCommentType;
        string _filteringUserId;
        int _commentListCount;
        int _originalPosterCommentListCount;

        bool _isOpenCommentDetail;
        bool _isSelectedInformation;
        bool _isSelectedMarket;
        bool _isSettedMedia;

        long _secondsDownloadingSize;

        SmileVideoCommentVertical _postCommandVertical = SmileVideoCommentVertical.Normal;
        SmileVideoCommentSize _postCommandSize = SmileVideoCommentSize.Medium;
        Color _postCommandColor = Colors.White;
        string _postBeforeCommand;
        string _postAfterCommand;
        string _postCommentBody;

        string _commentInformation;

        bool _isNormalWindow = true;

        bool _showCommentChart;

        bool _isEnabledOriginalPosterFilering;

        bool _fillBackgroundOriginalPoster;

        bool _isOpenWorkingPlayer;

        bool _forceNavigatorbarOperation;

        bool _showCommentTab;
        bool _showPlayListTab;
        SmileVideoBookmarkNodeViewModel _selectedBookmark;
        string _newBookmarkName;
        bool _isNewBookmark = true;

        SmileVideoInformationViewModel _selectedPlayListItem;

        #endregion
    }
}
