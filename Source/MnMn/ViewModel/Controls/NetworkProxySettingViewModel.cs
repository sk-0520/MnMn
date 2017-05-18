using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls
{
    public class NetworkProxySettingViewModel : SingleModelWrapperViewModelBase<NetworkProxySettingModel>
    {
        public NetworkProxySettingViewModel(NetworkProxySettingModel model)
            : base(model)
        { }

        #region proeprty

        public bool UsingCustomProxy
        {
            get { return Model.UsingCustomProxy; }
            set { SetModelValue(value); }
        }

        public string ServerAddress
        {
            get { return Model.ServerAddress; }
            set { SetModelValue(value); }
        }

        public bool UsingAuth
        {
            get { return Model.UsingAuth; }
            set { SetModelValue(value); }
        }

        public string UserName
        {
            get { return Model.UserName; }
            set { SetModelValue(value); }
        }

        public string Password
        {
            get { return Model.Password; }
            set { SetModelValue(value); }
        }

        #endregion
    }
}
