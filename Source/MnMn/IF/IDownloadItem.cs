using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.IF
{
    /// <summary>
    /// ダウンロード情報表示データ。
    /// </summary>
    public interface IDownloadItem: INotifyPropertyChanged, IDisplayText
    {
        #region property

        Uri DownloadUri { get; }

        /// <summary>
        /// ダウンロード状態。
        /// </summary>
        DownloadState DownloadState { get; }

        /// <summary>
        /// ダウンロード単位。
        /// <para>基本的に <see cref="DownloadUnit.Size"/> でいい。</para>
        /// </summary>
        DownloadUnit DownloadUnit { get; }

        /// <summary>
        /// ダウンロードするサイズは判明しているか。
        /// </summary>
        bool EnabledTotalSize { get; }

        /// <summary>
        /// ダウンロード完了を示すサイズ。
        /// <para><see cref="EnabledTotalSize"/>が真であれば有効。</para>
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

        /// <summary>
        /// 表示する画像。
        /// <para>48pxくらいかなぁ。</para>
        /// </summary>
        ImageSource Image { get; }

        /// <summary>
        /// キャンセル後に再実行可能か。
        /// <para>内部制御がややこしい場合にfalseを設定するイメージ。</para>
        /// <para>基本的にはアップデート処理以外は可能だと思いたい。</para>
        /// </summary>
        bool CanRestart { get; }

        #endregion

        #region command

        ICommand OpenDirectoryCommand { get; }
        ICommand ExecuteTargetCommand { get; }
        ICommand AutoExecuteTargetCommand { get; }

        #endregion

        #region function

        Task StartAsync();
        void Cancel();

        #endregion
    }
}
