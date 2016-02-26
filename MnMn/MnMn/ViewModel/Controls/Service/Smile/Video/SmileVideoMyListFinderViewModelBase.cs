/*
This file is part of MnMn.

MnMn is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MnMn is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MnMn.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Feed.Rss2;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public abstract class SmileVideoMyListFinderViewModelBase: SmileVideoFeedFinderViewModelBase
    {
        public SmileVideoMyListFinderViewModelBase(Mediation mediation, bool isUserMyList)
            : base(mediation)
        {
            Session = MediationUtility.GetResultFromRequestResponse<SmileSessionViewModel>(Mediation, new RequestModel(RequestKind.Session, ServiceType.Smile));

            IsUserMyList = isUserMyList;
        }

        #region property

        protected SmileSessionViewModel Session { get; }
        public abstract string MyListId { get; }
        public abstract string MyListName { get; }
        public virtual int MyListItemCount { get { return VideoInformationList.Count; } }
        public bool IsUserMyList {get;}

        #endregion

        #region SmileVideoFeedFinderViewModelBase

        protected override void SetItems(IEnumerable<SmileVideoInformationViewModel> items)
        {
            base.SetItems(items);
            CallOnPropertyChange(nameof(MyListItemCount));
        }

        protected override SmileVideoInformationFlags InformationFlags => SmileVideoInformationFlags.Length;

        protected override PageLoader CreatePageLoader()
        {
            throw new NotSupportedException();
        }

        protected override Task<FeedSmileVideoModel> LoadFeedAsync()
        {
            var mylist = new MyList(Mediation, Session);
            mylist.SessionSupport = true;

            return mylist.GetGroupAsync(MyListId);
        }

        #endregion
    }
}
