using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Service.Smile.Video
{
    public interface IReadOnlySmileVideoFinderFilteringParameter
    {
        #region property

        string VideoId { get; set; }
        string Title { get; set; }
        string UserId { get; set; }
        string UserName { get; set; }
        string ChannelId { get; set; }
        string ChannelName { get; set; }
        string Description { get; set; }
        IReadOnlyList<SmileVideoTagViewModel> Tags { get; set; }

        #endregion
    }
}
