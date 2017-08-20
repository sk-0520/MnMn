﻿/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.IF;

namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
    /// <summary>
    /// <see cref="IDisposable"/>をサポートする基底モデル。
    /// </summary>
    [Serializable]
    public abstract class DisposeFinalizeModelBase: ModelBase, IIsDisposed
    {
        #region variable

        bool _isDisposed;

        #endregion

        protected DisposeFinalizeModelBase()
        {
            this._isDisposed = false;
        }

        ~DisposeFinalizeModelBase()
        {
            Dispose(false);
        }

        #region function

        /// <summary>
        /// <see cref="IsDisposed"/>を再度無効にする場合に呼び出される。
        /// </summary>
        protected virtual void ResetDispose()
        {
            GC.ReRegisterForFinalize(this);
        }

        #endregion

        #region IIsDisposed

        [field: NonSerialized]
        public event EventHandler Disposing;

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>されたか。
        /// </summary>
        [IgnoreDataMember, XmlIgnore]
        public bool IsDisposed
        {
            get { return this._isDisposed; }
            protected set
            {
                if(this._isDisposed && !value) {
                    ResetDispose();
                }
                this._isDisposed = value;
            }
        }

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>の内部処理。
        /// <para>継承先クラスでは本メソッドを呼び出す必要がある。</para>
        /// </summary>
        /// <param name="disposing">CLRの管理下か。</param>
        protected virtual void Dispose(bool disposing)
        {
            if(IsDisposed) {
                return;
            }

            if(Disposing != null) {
                Disposing(this, EventArgs.Empty);
            }

            if(disposing) {
                GC.SuppressFinalize(this);
            }

            IsDisposed = true;
        }

        /// <summary>
        /// 解放。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}