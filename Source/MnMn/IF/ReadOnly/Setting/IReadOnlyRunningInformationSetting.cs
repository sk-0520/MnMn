using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Setting
{
    interface IReadOnlyRunningInformationSetting
    {
        #region property

        /// <summary>
        /// 使用許諾OK。
        /// </summary>
        bool Accept { get; }

        /// <summary>
        /// 前回終了時のバージョン。
        /// </summary>
        Version LastExecuteVersion { get; }

        /// <summary>
        /// 使用回数。
        /// </summary>
        int ExecuteCount { get; }
        /// <summary>
        /// 初回起動バージョン。
        /// </summary>
        Version FirstVersion { get; }
        /// <summary>
        /// 初回起動時間。
        /// </summary>
        DateTime FirstTimestamp { get; }

        ///// <summary>
        ///// アップデートチェックを行うか。
        ///// </summary>
        //[DataMember]
        //bool CheckUpdateRelease { get;}
        ///// <summary>
        ///// RCアップデートチェックを行うか。
        ///// </summary>
        //[DataMember]
        //bool CheckUpdateRC { get;}
        /// <summary>
        /// アップデートチェックで無視するバージョン。
        /// </summary>
        Version IgnoreUpdateVersion { get; }

        /// <summary>
        /// ユーザー識別子。
        /// <para>#474: ユーザー環境に対する識別子の設定</para>
        /// </summary>
        string UserId { get; }

        /// <summary>
        /// 簡易アップデート実施日時。
        /// </summary>
        DateTime LightweightUpdateTimestamp { get; }

        #endregion
    }
}
