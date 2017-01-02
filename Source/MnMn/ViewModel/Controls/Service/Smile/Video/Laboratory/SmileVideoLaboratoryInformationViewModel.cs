using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Laboratory
{
    public class SmileVideoLaboratoryInformationViewModel: SmileVideoInformationViewModel
    {
        public SmileVideoLaboratoryInformationViewModel(Mediation mediation)
            :base(mediation, 0, Define.Service.Smile.Video.SmileVideoInformationFlags.None)
        {

        }

        #region SmileVideoInformationViewModel

        public override string VideoId
        {
            get { return "MnMn"; }
        }

        public override string Title
        {
            get { return "MNMN"; }
        }

        public override DirectoryInfo CacheDirectory { get { return new DirectoryInfo("NUL"); } }

        protected override FileInfo IndividualVideoSettingFile { get { return new FileInfo("NUL"); } }

        public override bool SaveSetting(bool force)
        {
            return false;
        }

        #endregion
    }
}
