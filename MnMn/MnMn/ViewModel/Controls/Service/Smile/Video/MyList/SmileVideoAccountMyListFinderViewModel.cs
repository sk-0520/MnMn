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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Feed.Rss2;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.MyList
{
    public class SmileVideoAccountMyListFinderViewModel: SmileVideoMyListFinderViewModelBase
    {
        #region variable

        string _editingMyListName;
        SmileVideoElementModel _editingMyListFolderIdElement;
        bool _editingMyListIsPublic;
        string _editingMyListDescription;
        string _editingMyListSort;

        #endregion

        public SmileVideoAccountMyListFinderViewModel(Mediation mediation, RawSmileAccountMyListGroupItemModel groupModel)
            : base(mediation, true)
        {
            GroupModel = groupModel;

            MyList = MediationUtility.GetResultFromRequestResponse<SmileVideoMyListModel>(Mediation, new RequestModel(RequestKind.PlayListDefine, ServiceType.SmileVideo));

            if(GroupModel != null) {
                ResetEditingValue();
            }
        }

        #region property

        public virtual bool CanEdit { get; } = true;

        RawSmileAccountMyListGroupItemModel GroupModel { get; }

        SmileVideoMyListModel MyList { get; }

        public virtual string MyListFolderId { get { return GroupModel.IconId; } }

        public string EditingMyListName
        {
            get { return this._editingMyListName; }
            set { SetVariableValue(ref this._editingMyListName, value); }
        }
        public SmileVideoElementModel EditingMyListFolderIdElement
        {
            get { return this._editingMyListFolderIdElement; }
            set { SetVariableValue(ref this._editingMyListFolderIdElement, value); }
        }
        public bool EditingMyListIsPublic
        {
            get { return this._editingMyListIsPublic; }
            set { SetVariableValue(ref this._editingMyListIsPublic, value); }
        }
        public string EditingMyListDescription
        {
            get { return this._editingMyListDescription; }
            set { SetVariableValue(ref this._editingMyListDescription, value); }
        }
        public string EditingMyListSort
        {
            get { return this._editingMyListSort; }
            set { SetVariableValue(ref this._editingMyListSort, value); }
        }

        #endregion

        #region command

        public ICommand CancelEditCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        ResetEditingValue();
                    }
                );
            }
        }


        #endregion

        #region function

        void ResetEditingValue()
        {
            EditingMyListName = MyListName;
            EditingMyListFolderIdElement = MyList.Folders.First(f => f.Key == MyListFolderId);
            EditingMyListIsPublic = RawValueUtility.ConvertBoolean(GroupModel.Public);
            EditingMyListDescription = GroupModel.Description;
            EditingMyListSort = GroupModel.DefaultSort;
            ResetChangeFlag();
        }

        #endregion

        #region SmileVideoMyListFinderViewModel

        public override string MyListId { get { return GroupModel.Id; } }
        public override string MyListName { get { return GroupModel.Name; } }

        public override Color MyListFolderColor
        {
            get
            {
                var colorElement = RestrictUtility.IsNull(
                    MyList.Folders.FirstOrDefault(s => s.Key == MyListFolderId),
                    () => MyList.Folders.First(),
                    fe => fe
                );
                var colors = SmileMyListUtility.GetColorsFromExtends(colorElement.Extends);
                if(colors.Any()) {
                    return colors.First();
                }

                return Colors.Silver;
            }
        }

        protected override SmileVideoInformationFlags InformationFlags => SmileVideoInformationFlags.Length;

        #endregion
    }
}
