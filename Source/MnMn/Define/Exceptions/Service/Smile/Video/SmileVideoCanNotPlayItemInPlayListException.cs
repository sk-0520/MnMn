using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Define.Exceptions.Service.Smile.Video
{
    public class SmileVideoCanNotPlayItemInPlayListException: ApplicationException
    {
        public SmileVideoCanNotPlayItemInPlayListException(IEnumerable<SmileVideoInformationViewModel> informations)
            : base(string.Join(",", informations.Select(v => v.VideoId)))
        { }
    }
}
