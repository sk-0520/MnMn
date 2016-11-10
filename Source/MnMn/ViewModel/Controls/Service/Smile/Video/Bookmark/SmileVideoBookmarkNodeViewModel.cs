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
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Bookmark
{
    public class SmileVideoBookmarkNodeViewModel: SingleModelWrapperViewModelBase<SmileVideoBookmarkItemSettingModel>
    {
        #region variable

        bool _isSelected = false;
        bool _isExpanded = true;
        int _level;

        //string _editingName;

        #endregion

        public SmileVideoBookmarkNodeViewModel(SmileVideoBookmarkItemSettingModel model)
            : base(model)
        {
            NodeList = new MVMPairCreateDelegationCollection<SmileVideoBookmarkItemSettingModel, SmileVideoBookmarkNodeViewModel>(Model.Nodes, default(object), CreateItem);
            NodeItems = NodeList.ViewModelList;
        }

        #region property

        public virtual bool IsRootNode => false;
        public virtual bool CanMoveNode => true;

        public bool IsSelected
        {
            get { return this._isSelected; }
            set { SetVariableValue(ref this._isSelected, value); }
        }
        public bool IsExpanded
        {
            get { return this._isExpanded; }
            set { SetVariableValue(ref this._isExpanded, value); }
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

        //public string EditingName
        //{
        //    get { return this._editingName; }
        //    set { SetVariableValue(ref this._editingName, value); }
        //}

        #endregion

        #region command

        //public ICommand SaveEditCommand
        //{
        //    get
        //    {
        //        return CreateCommand(o => SaveEdit());
        //    }
        //}

        #endregion

        #region function

        static SmileVideoBookmarkNodeViewModel CreateItem(SmileVideoBookmarkItemSettingModel model, object data)
        {
            return new SmileVideoBookmarkNodeViewModel(model);
        }

        //void SaveEdit()
        //{
        //    Name = EditingName;
        //    ResetChangeFlag();
        //}

        //public void ClearEditingValue()
        //{
        //    EditingName = Name;

        //    ResetChangeFlag();
        //}

        #endregion

    }
}
