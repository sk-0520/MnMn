using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Laboratory
{
    public sealed class SmileVideoLaboratoryPlayerViewModel: SmileVideoPlayerViewModel
    {
        public SmileVideoLaboratoryPlayerViewModel(Mediation mediation, FileInfo videoFile, FileInfo commentFile) 
            : base(mediation)
        {
            Information = new SmileVideoLaboratoryInformationViewModel(Mediation);
        }

        #region SmileVideoPlayerViewModel

        protected override SmileVideoPlayerSettingModel PlayerSetting { get { return Setting.Laboratory.Player; } }


        #endregion
    }
}
