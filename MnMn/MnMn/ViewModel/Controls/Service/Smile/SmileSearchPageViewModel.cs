using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile
{
    public class SmileSearchPageViewModel<TViewModel>: ViewModelBase
        where TViewModel: ViewModelBase
    {
        #region variable

        LoadState _loadState;

        #endregion

        public SmileSearchPageViewModel(TViewModel viewModel, int pageNumber)
        {
            ViewModel = viewModel;
            PageNumber = pageNumber;
        }

        #region property

        public TViewModel ViewModel { get; }

        public int PageNumber { get; }

        public LoadState LoadState
        {
            get { return this._loadState; }
            set { SetVariableValue(ref this._loadState, value); }
        }

        #endregion

    }
}
