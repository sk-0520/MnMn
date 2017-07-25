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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Setting;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls
{
    /// <summary>
    /// <para>独立できるやつは独立していこう。</para>
    /// </summary>
    public abstract class FinderViewModelBase : ViewModelBase
    {
        #region variable

        bool _isAscending = true;
        bool _nowLoading;
        SourceLoadState _finderLoadState;

        double _scrollPositionY;

        IDragAndDrop _dragAndDrop;

        string _inputTitleFilter;
        bool _isBlacklist;

        bool _isOpenContextMenu;

        #endregion

        public FinderViewModelBase(Mediator mediator, int baseNumber)
        {
            Mediation = mediator;
            BaseNumber = baseNumber;

            NetworkSetting = Mediation.GetNetworkSetting();
        }


        #region property

        protected int BaseNumber { get; }
        protected Mediator Mediation { get; }

        protected IReadOnlyNetworkSetting NetworkSetting { get; }

        public virtual ICollectionView FinderItems { get; protected set; }

        /// <summary>
        /// 昇順か。
        /// </summary>
        public virtual bool IsAscending
        {
            get { return this._isAscending; }
            set
            {
                if(SetVariableValue(ref this._isAscending, value)) {
                    ChangeSortItems();
                }
            }
        }

        /// <summary>
        /// 現在読み込み中か。
        /// </summary>
        public virtual bool NowLoading
        {
            get { return this._nowLoading; }
            set { SetVariableValue(ref this._nowLoading, value); }
        }

        /// <summary>
        /// 読込状態。
        /// </summary>
        public virtual SourceLoadState FinderLoadState
        {
            get { return this._finderLoadState; }
            set
            {
                if(SetVariableValue(ref this._finderLoadState, value)) {
                    var propertyNames = new[] {
                        nameof(CanLoad),
                        nameof(NowLoading),
                    };
                    CallOnPropertyChange(propertyNames);
                }
            }
        }

        /// <summary>
        /// 読込可能か。
        /// </summary>
        public virtual bool CanLoad
        {
            get
            {
                var loadSkips = new[] { SourceLoadState.SourceLoading, SourceLoadState.SourceChecking };
                return !loadSkips.Any(l => l == FinderLoadState);
            }
        }

        public virtual double ScrollPositionY
        {
            get { return this._scrollPositionY; }
            set { SetVariableValue(ref this._scrollPositionY, value); }
        }

        public IDragAndDrop DragAndDrop
        {
            get { return this._dragAndDrop; }
            set { SetVariableValue(ref this._dragAndDrop, value); }
        }

        /// <summary>
        /// タイトルフィルター文字列。
        /// </summary>
        public virtual string InputTitleFilter
        {
            get { return this._inputTitleFilter; }
            set
            {
                if(SetVariableValue(ref this._inputTitleFilter, value)) {
                    FinderItems.Refresh();
                }
            }
        }

        /// <summary>
        /// タイトルフィルタを除外として扱うか。
        /// </summary>
        public virtual bool IsBlacklist
        {
            get { return this._isBlacklist; }
            set
            {
                if(SetVariableValue(ref this._isBlacklist, value)) {
                    FinderItems.Refresh();
                }
            }
        }

        public virtual bool IsOpenContextMenu
        {
            get { return this._isOpenContextMenu; }
            set { SetVariableValue(ref this._isOpenContextMenu, value); }
        }

        #endregion

        #region function

        internal abstract void ChangeSortItems();

        #endregion

    }
}
