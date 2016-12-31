using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ContentTypeTextNet.MnMn.Library.SpaghettiAssembly.Logic
{
    /// <summary>
    /// <see cref="IDisposable.Dispose"/>をサポートする基底クラス。
    /// </summary>
    public abstract class DisposeFinalizeBase
    {
        #region variable

        bool _isDisposed;

        #endregion

        protected DisposeFinalizeBase()
        {
            this._isDisposed = false;
        }

        ~DisposeFinalizeBase()
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

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>時に呼び出されるイベント。
        /// <para>呼び出し時点では<see cref="IsDisposed"/>は偽のまま。</para>
        /// </summary>
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

            IsDisposed = true;
            GC.SuppressFinalize(this);
        }

        #region IDisposable

        /// <summary>
        /// 解放。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        #endregion
    }
}
