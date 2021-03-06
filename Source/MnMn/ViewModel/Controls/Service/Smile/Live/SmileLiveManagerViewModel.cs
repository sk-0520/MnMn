﻿/*
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Live.Official;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Live
{
    public class SmileLiveManagerViewModel: ManagerViewModelBase
    {
        public SmileLiveManagerViewModel(Mediator mediator)
            : base(mediator)
        {
            Session = Mediator.GetResultFromRequest<SmileSessionViewModel>(new RequestModel(RequestKind.Session, ServiceType.Smile));

            OfficialBroadcastManager = new SmileLiveOfficialBroadcastManagerViewModel(Mediator);
            CategoryManager = new SmileLiveCategoryManagerViewModel(Mediator);

        }

        #region property

        public SmileSessionViewModel Session { get; }

        public SmileLiveOfficialBroadcastManagerViewModel OfficialBroadcastManager { get; }
        public SmileLiveCategoryManagerViewModel CategoryManager { get; }


        #endregion

        #region ManagerViewModelBase

        public override Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan, bool force)
        {
            return GarbageCollectionDummyResult;
        }

        public async override Task InitializeAsync()
        {
            foreach(var manager in ManagerChildren) {
                await manager.InitializeAsync();
            }
        }

        public async override Task UninitializeAsync()
        {
            foreach(var manager in ManagerChildren) {
                await manager.UninitializeAsync().ConfigureAwait(false);
            }
        }

        public override void InitializeView(MainWindow view)
        {
            foreach(var manager in ManagerChildren) {
                manager.InitializeView(view);
            }
        }

        public override void UninitializeView(MainWindow view)
        {
            foreach(var manager in ManagerChildren) {
                manager.UninitializeView(view);
            }
        }

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return new ManagerViewModelBase[] {
                OfficialBroadcastManager,
                CategoryManager,
            };
        }

        protected override void HideViewCore()
        { }

        protected override void ShowViewCore()
        { }

        #endregion

    }
}
