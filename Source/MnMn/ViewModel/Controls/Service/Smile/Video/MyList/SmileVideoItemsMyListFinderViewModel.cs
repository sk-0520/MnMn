using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.MyList
{
    public class SmileVideoItemsMyListFinderViewModel: SmileVideoMyListFinderViewModelBase
    {
        public SmileVideoItemsMyListFinderViewModel(Mediation mediation, SmileMyListItemModel item)
            : base(mediation, false)
        {
            IgnoreAddHistory = false;

            Item = item;
        }

        #region property

        SmileMyListItemModel Item { get; }

        #endregion

        #region SmileVideoMyListFinderViewModelBase

        public override string MyListId
        {
            get { return Item?.MyListId; }
        }

        public override string MyListName
        {
            get { return Item?.MyListName; }
        }

        #endregion

    }
}
