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
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;


namespace ContentTypeTextNet.MnMn.MnMn.ViewModel
{
    public abstract class ManagerViewModelBase: ViewModelBase
    {
        #region define

        protected static Task<long> GarbageCollectionDummyResult { get; } = Task.FromResult(0L);

        #endregion

        #region variable

        bool _isVisible;
        ManagerViewModelBase _selectedManager;
        CollectionModel<ManagerViewModelBase> _managerChildren = null;

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

        public IEnumerable<ManagerViewModelBase> ManagerChildren
        {
            get
            {
                if(this._managerChildren == null) {
                    var items = GetManagerChildren();
                    this._managerChildren = new CollectionModel<ManagerViewModelBase>(items);
                }

                return this._managerChildren;
            }
        }


        #endregion

        #region function

        /// <summary>
        /// 自身の保持する子マネージャ一覧を取得。
        /// <para>プロパティでで使用する目的のため原則一回しか呼び出されない。</para>
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<ManagerViewModelBase> GetManagerChildren();

        protected void ShowView()
        {
            IsVisible = true;

            ShowViewCore();

            if(ManagerChildren.Any()) {
                ManagerChildren.First().ShowView();
            }
        }

        protected void HideView()
        {
            IsVisible = false;
            HideViewCore();
        }

        protected abstract void ShowViewCore();

        protected abstract void HideViewCore();

        public abstract Task InitializeAsync();

        public abstract void InitializeView(MainWindow view);
        public abstract void UninitializeView(MainWindow view);

        /// <summary>
        /// キャッシュなどのゴミ処理を行う。
        /// </summary>
        /// <param name="garbageCollectionLevel">ゴミ処理レベル。</param>
        /// <param name="cacheSpan">基準にするキャッシュ時間。順守する必要はない。</param>
        /// <returns>処理したデータのバイト数。</returns>
        public abstract Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan);

        #endregion
    }
}
