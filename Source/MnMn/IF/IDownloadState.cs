﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.IF
{
    /// <summary>
    /// ダウンロード情報表示データ。
    /// </summary>
    public interface IDownloadState: INotifyPropertyChanged, IDisplayText
    {
        #region property

        Uri DownloadUri { get; }

        /// <summary>
        /// ダウンロード状態。
        /// </summary>
        LoadState DownLoadState { get; }

        /// <summary>
        /// ダウンロードするサイズは判明しているか。
        /// </summary>
        bool EnabledCompleteSize { get; }

        /// <summary>
        /// ダウンロード完了を示すサイズ。
        /// <para><see cref="EnabledCompleteSize"/>が真であれば有効。</para>
        /// </summary>
        long DownloadTotalSize { get; }

        /// <summary>
        /// ダウンロード済みサイズ。
        /// </summary>
        long DownloadedSize { get; }

        /// <summary>
        /// 読み込み進捗。
        /// <para>0-1</para>
        /// </summary>
        IProgress<double> DownloadingProgress { get; set; }

        #endregion

        #region function

        Task StartAsync(CancellationToken cancellationToken);

        #endregion
    }
}
