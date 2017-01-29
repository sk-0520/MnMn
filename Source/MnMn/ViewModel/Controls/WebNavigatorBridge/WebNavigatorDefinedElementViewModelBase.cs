using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Model.WebNavigatorBridge;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.WebNavigatorBridge
{
    public abstract class WebNavigatorDefinedElementViewModelBase<TWebNavigatorDefinedElementModel>: DefinedElementViewModelBase<TWebNavigatorDefinedElementModel>
        where TWebNavigatorDefinedElementModel : WebNavigatorDefinedElementModelBase
    {
        public WebNavigatorDefinedElementViewModelBase(TWebNavigatorDefinedElementModel model) 
            : base(model)
        { }

        #region property

        public ServiceType AllowService { get { return DefinedModel.AllowService; } }

        public ServiceType SendService { get { return DefinedModel.SendService; } }

        #endregion
    }
}
