using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Channel;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Channel
{
    public class SmileChannelHistoryItemViewModel : SingleModelWrapperViewModelBase<SmileChannelItemModel>
    {
        public SmileChannelHistoryItemViewModel(SmileChannelItemModel model)
            : base(model)
        { }

        #region property

        public string ChannelId { get { return Model.ChannelId; } }

        public string ChannelName { get { return Model.ChannelName; } }

        #endregion
    }
}
