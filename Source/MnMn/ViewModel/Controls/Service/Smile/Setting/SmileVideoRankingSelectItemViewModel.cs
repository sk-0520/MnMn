using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Setting
{
    public class SmileVideoRankingSelectItemViewModel : DefinedElementViewModelBase
    {
        #region variable

        bool _isChecked;

        #endregion

        public SmileVideoRankingSelectItemViewModel(DefinedElementModel model)
            : base(model)
        { }

        #region property

        public bool IsChecked
        {
            get { return this._isChecked; }
            set { SetVariableValue(ref this._isChecked, value); }
        }

        #endregion
    }
}
