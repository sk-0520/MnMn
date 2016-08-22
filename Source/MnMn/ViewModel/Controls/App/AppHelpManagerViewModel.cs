using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App
{
    public class AppHelpManagerViewModel: ManagerViewModelBase
    {
        public AppHelpManagerViewModel(Mediation mediation)
            : base(mediation)
        { }

        #region property

        WebBrowser HelpBrowser { get; set; }

        #endregion

        #region command
        #endregion

        #region ManagerViewModelBase

        public override Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan)
        {
            return GarbageCollectionDummyResult;
        }

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override void InitializeView(MainWindow view)
        {
            HelpBrowser = view.information.helpBrowser;
        }

        public override void UninitializeView(MainWindow view)
        {
        }

        protected override void ShowView()
        {
            base.ShowView();

            if(HelpBrowser.Source == null) {
                HelpBrowser.Source = new Uri(Constants.HelpFilePath);
            }
        }

        #endregion
    }
}
