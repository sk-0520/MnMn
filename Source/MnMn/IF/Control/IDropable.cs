using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.Control
{
    public interface IDropable
    {
        /// <summary>
        /// ドラッグ中アイテムが上に存在しているか。
        /// </summary>
        bool IsDragOver { get; set; }
    }
}
