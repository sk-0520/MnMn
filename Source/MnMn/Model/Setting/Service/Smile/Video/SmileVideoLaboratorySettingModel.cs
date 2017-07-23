using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define.Laboratory;
using ContentTypeTextNet.MnMn.MnMn.Define.Laboratory.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video
{
    [Serializable, DataContract]
    public class SmileVideoLaboratorySettingModel: SettingModelBase
    {
        #region property

        [DataMember]
        public string PlayInputVideoSourceFilePath { get; set; }

        [DataMember]
        public string PlayInputMessageMessageFilePath { get; set; }

        [DataMember]
        public SmileVideoPlayerSettingModel Player { get; set; } = new SmileVideoPlayerSettingModel();

        [DataMember]
        public TimeSpan DummyLength { get; set; } = TimeSpan.FromSeconds(10);
        [DataMember]
        public string DummyOutputDirectoryPath { get; set; }
        [DataMember]
        public bool DummyOutputVideo { get; set; } = true;
        [DataMember]
        public bool DummyOutputComment { get; set; } = true;

        [DataMember]
        public CommentCreateType DummyCommentCreateType { get; set; } = CommentCreateType.Sequence;
        [DataMember]
        public bool DummyCommentIsJson_Issue665AP { get; set; } = true;
        [DataMember]
        public int DummyCommentNormalCount { get; set; } = 100;
        [DataMember]
        public int DummyCommentOriginalPostCount { get; set; } = 10;

        [DataMember]
        public VideoCreateType DummyVideoCreateType { get; set; } = VideoCreateType.Sequence;
        [DataMember]
        public decimal DummyVideoFramesPerSecond { get; set; } = 30;
        [DataMember]
        public int DummyVideoWidth { get; set; } = (int)Constants.ServiceSmileVideoPlayerOfficial16x9Width;
        [DataMember]
        public int DummyVideoHeight { get; set; } = (int)Constants.ServiceSmileVideoPlayerOfficial16x9Height;

        #endregion
    }
}
