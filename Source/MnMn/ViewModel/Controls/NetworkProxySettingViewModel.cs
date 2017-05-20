using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls
{
    public class NetworkProxySettingViewModel : SingleModelWrapperViewModelBase<NetworkProxySettingModel>
    {
        public NetworkProxySettingViewModel(NetworkProxySettingModel model)
            : base(model)
        {
            RefreshTimer.Tick += RefreshTimer_Tick;
        }

        #region proeprty

        DispatcherTimer RefreshTimer { get; } = new DispatcherTimer() {
            Interval = Constants.NetworkProxySettingRefreshTime,
        };

        public bool UsingCustomProxy
        {
            get { return Model.UsingCustomProxy; }
            set
            {
                if(SetModelValue(value)) {
                    StartChangeParameter();
                }
            }
        }

        public string ServerAddress
        {
            get { return Model.ServerAddress; }
            set
            {
                if(SetModelValue(value)) {
                    StartChangeParameter();
                }
            }
        }

        public int ServerPort
        {
            get { return Model.ServerPort; }
            set
            {
                if(SetModelValue(value)) {
                    StartChangeParameter();
                }
            }
        }
        public bool UsingAuth
        {
            get { return Model.UsingAuth; }
            set
            {
                if(SetModelValue(value)) {
                    StartChangeParameter();
                }
            }
        }

        public string UserName
        {
            get { return Model.UserName; }
            set
            {
                if(SetModelValue(value)) {
                    StartChangeParameter();
                }
            }
        }

        public string Password
        {
            get { return Model.Password; }
            set
            {
                if(SetModelValue(value)) {
                    StartChangeParameter();
                }
            }
        }

        #endregion

        #region function

        void StartChangeParameter()
        {
            RefreshTimer.Stop();
            RefreshTimer.Start();
        }

        void UpdateChangedParameter()
        {
            var timerIsEnabled = RefreshTimer.IsEnabled;
            if(timerIsEnabled) {
                RefreshTimer.Stop();
            }

            SetModelValue(DateTime.Now, nameof(Model.ChangedTimestamp));

            if(timerIsEnabled) {
                RefreshTimer.Start();
            }
        }

        #endregion

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            UpdateChangedParameter();
        }
    }
}
