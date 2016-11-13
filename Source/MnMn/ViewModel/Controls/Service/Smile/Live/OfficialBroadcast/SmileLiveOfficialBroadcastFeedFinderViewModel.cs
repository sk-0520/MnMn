using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Live.Official
{
    public class SmileLiveOfficialBroadcastFeedFinderViewModel: SmileLiveFinderViewModelBase
    {
        public SmileLiveOfficialBroadcastFeedFinderViewModel(Mediation mediation)
            : base(mediation, 0)
        { }

        #region SmileLiveFinderViewModelBase

        protected override Task LoadCoreAsync(CacheSpan informationCacheSpan, CacheSpan imageCacheSpan, object extends)
        {
            var broadcast = new Logic.Service.Smile.Live.Api.Broadcast(Mediation);
            return broadcast.LoadAsync().ContinueWith(t => {
                var list = t.Result.Channel.Items.Select(i => new SmileLiveInformationViewModel(Mediation, i));
                SetItemsAsync(list, informationCacheSpan);
            });
        }

        #endregion
    }
}
