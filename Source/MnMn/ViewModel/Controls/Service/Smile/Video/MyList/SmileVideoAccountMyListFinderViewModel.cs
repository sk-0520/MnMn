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
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model;
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
        DefinedElementModel _editingMyListFolderIdElement;
        bool _editingMyListIsPublic;
        string _editingMyListDescription;
        DefinedElementModel _editingMyListSortElement;

        bool _isEditing;

        #endregion

        public SmileVideoAccountMyListFinderViewModel(Mediator mediation, RawSmileAccountMyListGroupItemModel groupModel)
            : base(mediation, true)
        {
            GroupModel = groupModel;

            MyList = Mediation.GetResultFromRequest<SmileVideoMyListModel>(new RequestModel(RequestKind.PlayListDefine, ServiceType.SmileVideo));

            ResetEditingValue();
        }

        #region property

        public override bool CanEdit { get; } = true;

        RawSmileAccountMyListGroupItemModel GroupModel { get; }

        protected SmileVideoMyListModel MyList { get; }

        public virtual string MyListFolderId { get { return GroupModel.IconId; } }

        public virtual string MyListSort { get { return GroupModel.DefaultSort; } }

        public string EditingMyListName
        {
            get { return this._editingMyListName; }
            set
            {
                var isEdited = SetVariableValue(ref this._editingMyListName, value);
                if(isEdited) {
                    IsEditing = isEdited;
                }
            }
        }
        public DefinedElementModel EditingMyListFolderIdElement
        {
            get { return this._editingMyListFolderIdElement; }
            set
            {
                var isEdited = SetVariableValue(ref this._editingMyListFolderIdElement, value);
                if(isEdited) {
                    IsEditing = isEdited;
                }
            }
        }
        public bool EditingMyListIsPublic
        {
            get { return this._editingMyListIsPublic; }
            set
            {
                var isEdited = SetVariableValue(ref this._editingMyListIsPublic, value);
                if(isEdited) {
                    IsEditing = isEdited;
                }
            }
        }
        public string EditingMyListDescription
        {
            get { return this._editingMyListDescription; }
            set
            {
                var isEdited = SetVariableValue(ref this._editingMyListDescription, value);
                if(isEdited) {
                    IsEditing = isEdited;
                }
            }
        }
        public DefinedElementModel EditingMyListSortElement
        {
            get { return this._editingMyListSortElement; }
            set
            {
                var isEdited = SetVariableValue(ref this._editingMyListSortElement, value);
                if(isEdited) {
                    IsEditing = isEdited;
                }
            }
        }

        public bool IsEditing
        {
            get { return this._isEditing; }
            set { SetVariableValue(ref this._isEditing, value); }
        }

        public override bool IsPublic
        {
            get { return RawValueUtility.ConvertBoolean(GroupModel.Public); }
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

        internal void ResetEditingValue()
        {
            if(GroupModel != null) {
                EditingMyListName = MyListName;
                EditingMyListFolderIdElement = MyList.Folder.FirstOrDefault(f => f.Key == MyListFolderId) ?? MyList.Folder.First();
                EditingMyListIsPublic = IsPublic;
                EditingMyListDescription = GroupModel.Description;
                EditingMyListSortElement = MyList.Sort.FirstOrDefault(f => f.Key == MyListSort) ?? MyList.Sort.First();
                IsEditing = false;
            }
        }

        #endregion

        #region SmileVideoMyListFinderViewModel

        public override string MyListId { get { return GroupModel?.Id; } }
        public override string MyListName { get { return GroupModel.Name; } }

        public override bool CanRemove { get; } = true;

        protected override SmileVideoInformationFlags InformationFlags => SmileVideoInformationFlags.Length;

        public override Color MyListFolderColor
        {
            get
            {
                var colorElement = RestrictUtility.IsNull(
                    MyList.Folder.FirstOrDefault(s => s.Key == MyListFolderId),
                    () => MyList.Folder.First(),
                    fe => fe
                );
                var colors = SmileMyListUtility.GetColorsFromExtends(colorElement.Extends);
                if(colors.Any()) {
                    return colors.First();
                }

                return Colors.Silver;
            }
        }

        public override bool HasMyListFolder { get; } = true;

        public override bool IsUsingFinderFilter { get; } = false;

        public override bool IsEnabledCheckItLaterMenu { get; } = false;
        public override bool IsEnabledUnorganizedBookmarkMenu { get; } = false;

        #endregion
    }
}
