using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Channel
{
    public class SmileChannelInformationViewModel: InformationViewModelBase
    {
        public SmileChannelInformationViewModel(Mediation mediation, string channelId)
        {
            Mediation = mediation;
            ChannelId = channelId;
        }

        #region property

        Mediation Mediation { get; }

        public string ChannelId { get; }

        #endregion

        #region InformationViewModelBase

        public override string Title => throw new NotImplementedException();

        protected override Task<bool> LoadInformationCoreAsync(CacheSpan cacheSpan, HttpClient client)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> LoadThumbnaiImageCoreAsync(CacheSpan cacheSpan, HttpClient client)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
