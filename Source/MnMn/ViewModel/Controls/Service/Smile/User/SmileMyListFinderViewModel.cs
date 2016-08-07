using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.MyList;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.User
{
    /// <summary>
    /// やっぱサービス分けミスってんなぁ。
    /// </summary>
    public class SmileMyListFinderViewModel: SmileVideoMyListFinderViewModelBase
    {
        public SmileMyListFinderViewModel(Mediation mediation, RawSmileUserMyListGroupModel group)
            : base(mediation, false)
        {
            MyListGroup = group;
            InitializeCacheData();
        }

        #region property

        RawSmileUserMyListGroupModel MyListGroup { get; }

        #endregion

        #region SmileVideoMyListFinderViewModelBase

        public override string MyListId
        {
            get { return MyListGroup?.MyListId; }
        }

        public override string MyListName
        {
            get { return MyListGroup?.Title; }
        }

        public override int MyListItemCount
        {
            get { return MyListGroup.Count; }
        }

        #endregion
    }
}
