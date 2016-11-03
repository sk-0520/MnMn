﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App
{
    public class AppForumManagerViewModel: ManagerViewModelBase
    {
        public AppForumManagerViewModel(Mediation mediation)
            : base(mediation)
        { }

        #region property

        WebNavigator ForumBrowser { get; set; }

        #endregion

        #region ManagerViewModelBase

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return Enumerable.Empty<ManagerViewModelBase>();
        }

        protected override void ShowViewCore()
        {
            if(ForumBrowser.IsEmptyContent) {
                ForumBrowser.Navigate(Constants.AppUriForum);
            }
        }

        protected override void HideViewCore()
        { }

        public override Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan, bool force)
        {
            return GarbageCollectionDummyResult;
        }

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override void InitializeView(MainWindow view)
        {
            ForumBrowser = view.information.forumBrowser;
            ForumBrowser.HomeSource = Constants.AppUriIssueResolved;
        }

        public override void UninitializeView(MainWindow view)
        {
            ForumBrowser = null;
        }

        #endregion

    }
}