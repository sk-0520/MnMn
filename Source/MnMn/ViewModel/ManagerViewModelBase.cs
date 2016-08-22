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
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;


namespace ContentTypeTextNet.MnMn.MnMn.ViewModel
{
    public abstract class ManagerViewModelBase: ViewModelBase
    {
        #region variable

        //bool _visible;
        bool _isVisible;
        ManagerViewModelBase _selectedManager;

        #endregion

        public ManagerViewModelBase(Mediation mediation)
        {
            Mediation = mediation;
        }

        #region property

        protected Mediation Mediation { get; set; }

        public bool IsVisible
        {
            get { return this._isVisible; }
            private set { SetVariableValue(ref this._isVisible, value); }
        }

        public ManagerViewModelBase SelectedManager
        {
            get { return this._selectedManager; }
            set
            {
                var prevSelectedManager = this._selectedManager;
                if(SetVariableValue(ref this._selectedManager, value)) {
                    if(this._selectedManager != null) {
                        this._selectedManager.ShowView();
                    }
                    if(prevSelectedManager != null) {
                        prevSelectedManager.HideView();
                    }
                }
            }
        }

        //[Obsolete]
        //public bool Visible
        //{
        //    get { return this._visible; }
        //    set
        //    {
        //        if(SetVariableValue(ref this._visible, value)) {
        //            if(this._visible) {
        //                ShowView();
        //            } else {
        //                HideView();
        //            }
        //        }
        //    }
        //}

        #endregion

        #region function

        protected virtual void ShowView()
        {
            IsVisible = true;
        }

        protected virtual void HideView()
        {
            IsVisible = false;
        }

        public abstract Task InitializeAsync();

        public abstract void InitializeView(MainWindow view);
        public abstract void UninitializeView(MainWindow view);

        /// <summary>
        /// キャッシュなどのゴミ処理を行う。
        /// </summary>
        public abstract Task GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan);

        #endregion
    }
}
