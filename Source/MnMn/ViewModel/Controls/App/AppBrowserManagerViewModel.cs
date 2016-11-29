using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.MnMn.MnMn.Data;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.App;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App
{
    public class AppBrowserManagerViewModel: ManagerViewModelBase
    {
        public AppBrowserManagerViewModel(Mediation mediation)
            : base(mediation)
        { }


        #region property

        WebNavigator Browser { get; set; }

        #endregion

        #region commadn

        public ICommand NewWindowCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var data = (WebNavigatorEventDataBase)o;
                        WebNavigatorUtility.OpenNewWindowWrapper(data, Mediation.Logger);
                    }
                );
            }
        }

        #endregion

        #region ManagerViewModelBase

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
            Browser = view.general.browser;
            Browser.HomeSource = Constants.AppUriGeneralBrowserHome;
        }

        public override void UninitializeView(MainWindow view)
        { }

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return Enumerable.Empty<ManagerViewModelBase>();
        }

        protected override void HideViewCore()
        { }

        protected override void ShowViewCore()
        {
            if(Browser.IsEmptyContent) {
                Browser.Navigate(Browser.HomeSource);
            }
        }

        #endregion
    }
}
