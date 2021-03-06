﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Live.Official
{
    public class SmileLiveOfficialBroadcastFeedFinderViewModel: SmileLiveFinderViewModelBase
    {
        public SmileLiveOfficialBroadcastFeedFinderViewModel(Mediator mediator)
            : base(mediator, 0)
        { }

        #region SmileLiveFinderViewModelBase

        protected override Task LoadCoreAsync(CacheSpan informationCacheSpan, CacheSpan imageCacheSpan, object extends)
        {
            var broadcast = new Logic.Service.Smile.Live.Api.Broadcast(Mediator);
            return broadcast.LoadAsync().ContinueWith(t => {
                var list = t.Result.Channel.Items.Select(i => new SmileLiveInformationViewModel(Mediator, i));
                SetItemsAsync(list, informationCacheSpan);
            });
        }

        #endregion
    }
}
