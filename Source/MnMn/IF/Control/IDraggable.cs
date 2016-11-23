using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.Control
{
    public interface IDraggable
    {
        /// <summary>
        /// ドラッグ中か。
        /// </summary>
        bool IsDragging { get; set; }
    }
}
