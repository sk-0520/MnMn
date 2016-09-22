using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.IF.Control;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls
{
    public class PagerFinderProvider<TFinderViewModel, TInformationViewModel, TFinderItemViewModel>: ViewModelBase, IPagerFinder<TFinderViewModel, TInformationViewModel, TFinderItemViewModel>
        where TFinderViewModel : FinderViewModelBase<TInformationViewModel, TFinderItemViewModel>
        where TInformationViewModel : InformationViewModelBase
        where TFinderItemViewModel : FinderItemViewModelBase<TInformationViewModel>
    {
        #region variable

        PageViewModel<TFinderViewModel> _selectedPage;

        #endregion

        public PagerFinderProvider(IPagerFinder<TFinderViewModel, TInformationViewModel, TFinderItemViewModel> parentFinder)
        {
            ParentFinder = parentFinder;
        }

        #region property

        IPagerFinder<TFinderViewModel, TInformationViewModel, TFinderItemViewModel> ParentFinder { get; }

        #endregion

        #region property

        public TFinderViewModel SelectedFinder => SelectedPage.ViewModel;

        #endregion

        #region function
        #endregion

        #region IPagerFinder

        /// <summary>
        /// 保持するページャ。
        /// </summary>
        public CollectionModel<PageViewModel<TFinderViewModel>> PageItems { get; } = new CollectionModel<PageViewModel<TFinderViewModel>>();

        /// <summary>
        /// 選択中ページ。
        /// </summary>
        public PageViewModel<TFinderViewModel> SelectedPage
        {
            get { return this._selectedPage; }
            set { SetVariableValue(ref this._selectedPage, value); }
        }

        #endregion

    }
}
