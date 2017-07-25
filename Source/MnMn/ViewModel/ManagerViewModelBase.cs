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
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Setting;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;


namespace ContentTypeTextNet.MnMn.MnMn.ViewModel
{
    /// <summary>
    /// 各 UI を管理する基底クラス。
    /// </summary>
    public abstract class ManagerViewModelBase: ViewModelBase
    {
        #region define

        /// <summary>
        /// <see cref="GarbageCollectionAsync(GarbageCollectionLevel, CacheSpan)"/>時に何もしない<see cref="Task"/>。
        /// </summary>
        protected static Task<long> GarbageCollectionDummyResult { get; } = Task.FromResult(0L);

        #endregion

        #region variable

        bool _isVisible;
        ManagerViewModelBase _selectedManager;
        CollectionModel<ManagerViewModelBase> _managerChildren = null;

        #endregion

        public ManagerViewModelBase(Mediator mediator)
        {
            Mediation = mediator;

            NetworkSetting = Mediation.GetNetworkSetting();
        }

        #region property

        /// <summary>
        /// 通信役。
        /// </summary>
        protected Mediator Mediation { get; set; }
        protected IReadOnlyNetworkSetting NetworkSetting { get; }

        /// <summary>
        /// 表示されているか。
        /// </summary>
        public bool IsVisible
        {
            get { return this._isVisible; }
            private set { SetVariableValue(ref this._isVisible, value); }
        }

        /// <summary>
        /// 選択(表示)中マネージャ。
        /// </summary>
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

        /// <summary>
        /// 自身の保持する子マネージャ一覧。
        /// </summary>
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

        /// <summary>
        /// UI 表示処理。
        /// </summary>
        protected void ShowView()
        {
            IsVisible = true;

            ShowViewCore();

            if(ManagerChildren.Any()) {
                ManagerChildren.First().ShowView();
            }
        }

        /// <summary>
        /// UI 非表示処理。
        /// </summary>
        protected void HideView()
        {
            IsVisible = false;
            HideViewCore();
        }

        /// <summary>
        /// UI 表示で実施される処理。
        /// </summary>
        protected abstract void ShowViewCore();

        /// <summary>
        /// UI 非表示で実施される処理。
        /// </summary>
        protected abstract void HideViewCore();

        /// <summary>
        /// 初期化処理。
        /// <para>Viewが云々は関係。</para>
        /// </summary>
        /// <returns></returns>
        public abstract Task InitializeAsync();

        /// <summary>
        /// 破棄処理。
        /// </summary>
        /// <returns></returns>
        public abstract Task UninitializeAsync();

        /// <summary>
        /// Viewの初期化処理。
        /// </summary>
        /// <param name="view"></param>
        public abstract void InitializeView(MainWindow view);
        /// <summary>
        /// Viewの破棄処理。
        /// </summary>
        /// <param name="view"></param>
        public abstract void UninitializeView(MainWindow view);

        /// <summary>
        /// キャッシュなどのゴミ処理を行う。
        /// </summary>
        /// <param name="garbageCollectionLevel">ゴミ処理レベル。</param>
        /// <param name="cacheSpan">基準にするキャッシュ時間。順守する必要はない。</param>
        /// <param name="force">強制実行するか。</param>
        /// <returns>処理したデータのバイト数。</returns>
        public abstract Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan, bool force);

        #endregion
    }
}
