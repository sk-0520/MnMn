using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.IF.Control;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Control
{
    public abstract class TreeNodeItemViewModelBase: ViewModelBase, ITreeNodeItem
    {
        #region ITreeNodeItem

        public abstract bool IsSelected { get; set; }
        public abstract bool IsExpanded { get; set; }
        public abstract bool CanMove { get; set; }

        #endregion
    }
}
