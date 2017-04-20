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
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF.Control;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Bookmark
{
    public class SmileVideoBookmarkNodeViewModel: SingleModelWrapperViewModelBase<SmileVideoBookmarkItemSettingModel>, IDropable
    {
        #region variable

        bool _isSelected = false;
        int _level;

        //string _editingName;
        bool _isDragOver;
        bool _isDragging;

        #endregion

        public SmileVideoBookmarkNodeViewModel(SmileVideoBookmarkItemSettingModel model)
            : base(model)
        {
            NodeList = new MVMPairCreateDelegationCollection<SmileVideoBookmarkItemSettingModel, SmileVideoBookmarkNodeViewModel>(Model.Nodes, default(object), CreateItem);
            NodeItems = NodeList.ViewModelList;
        }

        #region property

        public virtual bool IsSystemNode { get; } = false;

        public bool IsSelected
        {
            get { return this._isSelected; }
            set { SetVariableValue(ref this._isSelected, value); }
        }
        public bool IsExpanded
        {
            get { return Model.IsExpanded; }
            set { SetModelValue(value); }
        }

        public int Level
        {
            get { return this._level; }
            set { SetVariableValue(ref this._level, value); }
        }

        public MVMPairCreateDelegationCollection<SmileVideoBookmarkItemSettingModel, SmileVideoBookmarkNodeViewModel> NodeList { get; }
        public CollectionModel<SmileVideoBookmarkNodeViewModel> NodeItems { get; }

        public CollectionModel<SmileVideoVideoItemModel> VideoItems { get { return Model.Items; } }

        public string Name
        {
            get { return Model.Name; }
            set { SetModelValue(value); }
        }

        #endregion

        #region command

        #endregion

        #region function

        static SmileVideoBookmarkNodeViewModel CreateItem(SmileVideoBookmarkItemSettingModel model, object data)
        {
            return new SmileVideoBookmarkNodeViewModel(model);
        }

        #endregion

        #region IDropable

        public bool IsDragOver
        {
            get { return this._isDragOver; }
            set { SetVariableValue(ref this._isDragOver, value); }
        }

        #endregion

        #region IDraggable

        public bool IsDragging
        {
            get { return this._isDragging; }
            set { SetVariableValue(ref this._isDragging, value); }
        }

        #endregion

    }
}
