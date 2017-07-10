using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Channel
{
    public class SmileChannelManagerViewModel: ManagerViewModelBase
    {
        #region variable

        SmileChannelInformationViewModel _selectedChannel;

        #endregion


        public SmileChannelManagerViewModel(Mediation mediation)
            : base(mediation)
        {
            Setting = Mediation.GetResultFromRequest<SmileSettingModel>(new RequestModel(RequestKind.Setting, ServiceType.Smile));
        }

        #region property

        SmileSettingModel Setting { get; }

        public SmileChannelInformationViewModel SelectedChannel
        {
            get { return this._selectedChannel; }
            set { SetVariableValue(ref this._selectedChannel, value); }
        }

        public GridLength GroupWidth
        {
            get { return new GridLength(Setting.Channel.GroupWidth, GridUnitType.Star); }
            set { SetPropertyValue(Setting.Channel, value.Value, nameof(Setting.Channel.GroupWidth)); }
        }
        public GridLength ItemsWidth
        {
            get { return new GridLength(Setting.Channel.ItemsWidth, GridUnitType.Star); }
            set { SetPropertyValue(Setting.Channel, value.Value, nameof(Setting.Channel.ItemsWidth)); }
        }

        public CollectionModel<SmileChannelInformationViewModel> ChannelItems { get; } = new CollectionModel<SmileChannelInformationViewModel>();

        #endregion

        #region function

        public Task LoadFromParameterAsync(SmileOpenChannelIdParameterModel parameter)
        {
            return LoadAsync(parameter.ChannelId, parameter.IsLoginUser, parameter.AddHistory);
        }

        public Task LoadAsync(string channelId, bool isLoginUser, bool addHistory)
        {
            var existChannel = ChannelItems.FirstOrDefault(i => i.ChannelId == channelId);
            if(existChannel != null) {
                SelectedChannel = existChannel;
                return Task.CompletedTask;
            } else {
                var channel = new SmileChannelInformationViewModel(Mediation, channelId);
                ChannelItems.Add(channel);
                SelectedChannel = channel;
                return channel.LoadDefaultAsync();
            }
        }

        #endregion

        #region SmileChannelManagerViewModel

        public override Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan, bool force)
        {
            return GarbageCollectionDummyResult;
        }

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override Task UninitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override void InitializeView(MainWindow view)
        { }

        public override void UninitializeView(MainWindow view)
        { }

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return Enumerable.Empty<ManagerViewModelBase>();
        }

        protected override void HideViewCore()
        { }

        protected override void ShowViewCore()
        { }

        #endregion
    }
}
