using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Live
{
    public class SmileLiveInformationCaching: InformationCacherBase<SmileVideoInformationViewModel>
    {
        public SmileLiveInformationCaching(Mediation mediation)
            : base(mediation)
        {}



    }
}
