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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Setting;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player
{
    /// <summary>
    /// コメント表示のフィルタリング。
    /// </summary>
    public class SmileVideoFilteringViweModel: ViewModelBase
    {
        public SmileVideoFilteringViweModel(SmileVideoCommentFilteringSettingModel commentSetting, SmileVideoFinderFilteringSettingModel finderSetting, SmileVideoFilteringModel filteringDefine)
        {
            CommentSetting = commentSetting;
            FinderSetting = finderSetting;

            FilteringDefine = filteringDefine;

            CommentFilterList = new MVMPairCreateDelegationCollection<SmileVideoCommentFilteringItemEditViewMode, SmileVideoCommentFilteringItemEditViewModel>(CommentSetting.Items, default(object), SmileVideoCommentUtility.CreateVideoCommentFilter);
            if(FinderSetting != null) {
                FinderFilterList = new MVMPairCreateDelegationCollection<SmileVideoFinderFilteringItemSettingModel, SmileVideoFinderFilteringItemEditViewModel>(FinderSetting.Items, default(object), (m, o) => new SmileVideoFinderFilteringItemEditViewModel(m));
            }
            CommentFilterItems = CollectionViewSource.GetDefaultView(CommentFilterList.ViewModelList);

            CommentDefineItems = filteringDefine.Elements;
        }

        #region property

        SmileVideoFilteringModel FilteringDefine { get; }

        public CollectionModel<DefinedElementModel> CommentDefineItems { get; }

        #region comment

        SmileVideoCommentFilteringSettingModel CommentSetting { get; }

        internal MVMPairCreateDelegationCollection<SmileVideoCommentFilteringItemEditViewMode, SmileVideoCommentFilteringItemEditViewModel> CommentFilterList { get; }
        public ICollectionView CommentFilterItems { get; }

        public bool IgnoreOverlapWord
        {
            get { return CommentSetting.IgnoreOverlapWord; }
            set { SetPropertyValue(CommentSetting, value); }
        }

        public TimeSpan IgnoreOverlapTime
        {
            get { return CommentSetting.IgnoreOverlapTime; }
            set { SetPropertyValue(CommentSetting, value); }
        }

        public CollectionModel<string> DefineKeys
        {
            get { return CommentSetting.DefineKeys; }
        }

        #endregion

        #region finder

        SmileVideoFinderFilteringSettingModel FinderSetting { get; }

        public MVMPairCreateDelegationCollection<SmileVideoFinderFilteringItemSettingModel, SmileVideoFinderFilteringItemEditViewModel> FinderFilterList { get; }

        //public bool IsEnabledFinderFiltering
        //{
        //    get { return FinderSetting.IsEnabledFinderFiltering; }
        //    set { SetPropertyValue(FinderSetting, value); }
        //}

        #endregion

        #endregion

        #region command

        #region comment

        public ICommand RemoveCommentFilterCommand
        {
            get { return CreateCommand(o => RemoveCommentFilter((SmileVideoCommentFilteringItemEditViewModel)o)); }
        }

        public ICommand AddCommentFilterCommand
        {
            get
            {
                return CreateCommand(
              o => {
                  var model = new SmileVideoCommentFilteringItemEditViewMode();
                  AddCommentFilter(model);
              });
            }
        }

        #endregion

        #endregion

        #region function

        void RemoveCommentFilter(SmileVideoCommentFilteringItemEditViewModel target)
        {
            CommentFilterList.Remove(target);
        }

        private void AddCommentFilter(SmileVideoCommentFilteringItemEditViewMode model)
        {
            CommentFilterList.Add(model, null);
        }


        #endregion
    }
}
