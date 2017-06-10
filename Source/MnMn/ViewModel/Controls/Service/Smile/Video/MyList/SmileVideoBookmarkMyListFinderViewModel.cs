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
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.MyList
{
    public class SmileVideoBookmarkMyListFinderViewModel: SmileVideoItemsMyListFinderViewModel
    {
        public SmileVideoBookmarkMyListFinderViewModel(Mediation mediation, SmileMyListBookmarkItemModel item)
            : base(mediation, item)
        {
            BookmarkItem = item;
        }

        #region property

        SmileMyListBookmarkItemModel BookmarkItem { get; }

        public string MyListCustomName
        {
            get { return BookmarkItem.MyListCustomName ?? base.MyListName; }
            set
            {
                if(SetPropertyValue(BookmarkItem, value, nameof(BookmarkItem.MyListCustomName))) {
                    CallOnPropertyChange(nameof(MyListName));
                }
            }
        }

        public string TagNames
        {
            get { return BookmarkItem.TagNames; }
            set
            {
                var originTags = value.Split(Constants.SmileMyListBookmarkTagTokenSplitter)
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToEvaluatedSequence()
                ;
                var distinctNoOrderTags = new List<string>(originTags.Count);
                foreach(var tag in originTags) {
                    if(!distinctNoOrderTags.Contains(tag)) {
                        distinctNoOrderTags.Add(tag);
                    }
                }
                var splitter = $"{Constants.SmileMyListBookmarkTagTokenSplitter} ";
                var joinTags = string.Join(splitter, distinctNoOrderTags);
                SetPropertyValue(BookmarkItem, joinTags);
            }
        }

        public IEnumerable<string> TagNameItems
        {
            get
            {
                if(BookmarkItem.TagNames != null) {
                    return BookmarkItem.TagNames.Split(Constants.SmileMyListBookmarkTagTokenSplitter)
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                    ;
                } else {
                    return Enumerable.Empty<string>();
                }
            }
        }

        #endregion

        #region command

        public ICommand RefreshBookmarkItemCommand
        {
            get
            {
                return CreateCommand(o => RefreshBookmarkItemAsync().ConfigureAwait(false));
            }
        }

        #endregion

        #region function

        Task RefreshBookmarkItemAsync()
        {
            MyListCustomName = null;
            BookmarkItem.UpdateTimestamp = DateTime.Now;

            // 実は変わってるかもしれないので裏で読み直しておく
            var mylist = new Logic.Service.Smile.Api.V1.MyList(Mediation);

            return mylist.LoadGroupAsync(MyListId).ContinueWith(t => {
                if(t.IsFaulted) {
                    return;
                }
                var rawMyList = t.Result;

                var newTitle = rawMyList.Channel.Title;
                if(BookmarkItem.MyListName != newTitle) {
                    Mediation.Logger.Information($"changed mylist name: {BookmarkItem.MyListName} -> {newTitle}");
                    BookmarkItem.MyListName = newTitle;
                    BookmarkItem.UpdateTimestamp = DateTime.Now;

                    CallOnPropertyChange(nameof(MyListName));
                    CallOnPropertyChange(nameof(MyListCustomName));
                }
            });
        }

        #endregion

        #region SmileVideoItemsMyListFinderViewModel

        public override string MyListName
        {
            get { return BookmarkItem?.MyListCustomName ?? base.MyListName; }
        }

        #endregion

    }
}
