using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Setting;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Setting.Service.Smile
{
    interface IReadOnlySmileUserAccount: IReadOnlyPassword
    {
        #region property

        /// <summary>
        /// アカウント名。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 自動ログインするか。
        /// </summary>
        bool EnabledStartupAutoLogin { get; }

        #endregion
    }
}
