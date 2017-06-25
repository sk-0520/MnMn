using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly
{
    public interface IReadOnlyCData
    {
        #region property

        /// <summary>
        /// ユーザーコードから設定する CData の中身。
        /// </summary>
        string Text { get; }

        /// <summary>
        /// シリアライズ処理で使用する CData の中身。
        /// <para>ユーザーコードで使用しないこと。</para>
        /// </summary>
        XmlNode[] CDataText { get; }

        #endregion
    }
}
