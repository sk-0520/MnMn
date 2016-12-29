using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.User
{
    public class SmileUserVersionViewModel: DefinedElementViewModelBase
    {
        public SmileUserVersionViewModel(DefinedElementModel model)
            : base(model)
        { }

        #region property

        public DateTime StartTime
        {
            get
            {
                return RawValueUtility.ConvertDateTime(Extends["start"]);
            }
        }

        #endregion

        #region DefinedElementViewModelBase

        public override string ToString()
        {
            return Key;
        }

        #endregion
    }
}
