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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.IF.Control;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Setting
{
    public class SmileVideoCommentFilteringItemEditViewModel: SingleModelWrapperViewModelBase<SmileVideoCommentFilteringItemSettingModel>
    {
        #region variable

        //FilteringType _editingType;
        //SmileVideoCommentFilteringTarget _editingTarget;
        //string _editingSource;
        //bool _editingIgnoreCase;

        //bool _isSelected;

        #endregion

        public SmileVideoCommentFilteringItemEditViewModel(SmileVideoCommentFilteringItemSettingModel model)
            : base(model)
        {
            //EditingType = model.Type;
            //EditingTarget = model.Target;
            //EditingSource = model.Source;
            //EditingIgnoreCase = model.IgnoreCase;
        }

        #region property

        public bool IsEnabled
        {
            get { return Model.IsEnabled; }
            set { SetModelValue(value); }
        }

        public FilteringType Type
        {
            get { return Model.Type; }
            set { SetModelValue(value); }
        }
        public SmileVideoCommentFilteringTarget Target
        {
            get { return Model.Target; }
            set { SetModelValue(value); }
        }
        public string Source
        {
            get { return Model.Source; }
            set { SetModelValue(value); }
        }
        public bool IgnoreCase
        {
            get { return Model.IgnoreCase; }
            set { SetModelValue(value); }
        }
        public string Name
        {
            get { return Model.Name; }
            set { SetModelValue(value); }
        }

        //public FilteringType EditingType
        //{
        //    get { return this._editingType; }
        //    set { SetVariableValue(ref this._editingType, value); }
        //}
        //public SmileVideoCommentFilteringTarget EditingTarget
        //{
        //    get { return this._editingTarget; }
        //    set { SetVariableValue(ref this._editingTarget, value); }
        //}
        //public string EditingSource
        //{
        //    get { return this._editingSource; }
        //    set { SetVariableValue(ref this._editingSource, value); }
        //}
        //public bool EditingIgnoreCase
        //{
        //    get { return this._editingIgnoreCase; }
        //    set { SetVariableValue(ref this._editingIgnoreCase, value); }
        //}

        #endregion

        #region function

        //internal void Update()
        //{
        //    Type = EditingType;
        //    Target = EditingTarget;
        //    Source = EditingSource;
        //    IgnoreCase = EditingIgnoreCase;

        //    ResetChangeFlag();
        //}

        //internal void Reset()
        //{
        //    EditingIgnoreCase = IgnoreCase;
        //    EditingSource = Source;
        //    EditingTarget = Target;
        //    Type = Type;
        //    ResetChangeFlag();
        //}

        #endregion

        //#region ISelectable

        //public bool IsSelected
        //{
        //    get { return this._isSelected; }
        //    set { SetVariableValue(ref this._isSelected, value); }
        //}

        //#endregion
    }
}
