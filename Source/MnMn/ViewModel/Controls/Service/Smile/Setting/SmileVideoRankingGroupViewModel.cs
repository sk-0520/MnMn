using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Setting
{
    public class SmileVideoRankingGroupViewModel : SingleModelWrapperViewModelBase<SmileVideoCategoryGroupModel>
    {
        public SmileVideoRankingGroupViewModel(SmileVideoCategoryGroupModel model)
            : base(model)
        {
            Items = Model.Categories.Select(i => new SmileVideoRankingSelectItemViewModel(i)).ToEvalSequence();
            RootItem = Items.First();
            Children = Items.Skip(1).ToEvalSequence();
        }

        #region property

        internal IReadOnlyList<SmileVideoRankingSelectItemViewModel> Items { get; }

        public SmileVideoRankingSelectItemViewModel RootItem { get; }
        public IReadOnlyList<SmileVideoRankingSelectItemViewModel> Children { get; }

        public bool HasChildren => Children.Any();

        #endregion
    }
}
