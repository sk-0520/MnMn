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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Event;
using ContentTypeTextNet.MnMn.MnMn.IF.Control;
using ContentTypeTextNet.MnMn.MnMn.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls
{
    public class PagerFinderProvider<TBaseFinderVideModel, TChildFinderViewModel, TInformationViewModel, TFinderItemViewModel>: ViewModelBase, IPagerFinder<TChildFinderViewModel, TInformationViewModel, TFinderItemViewModel>
        where TBaseFinderVideModel : FinderViewModelBase<TInformationViewModel, TFinderItemViewModel>
        where TChildFinderViewModel : FinderViewModelBase<TInformationViewModel, TFinderItemViewModel>
        where TInformationViewModel : InformationViewModelBase
        where TFinderItemViewModel : FinderItemViewModelBase<TInformationViewModel>
    {
        #region define

        static IEnumerable<string> DefaultChangePagePropertyNames => new[] {
            nameof(FinderViewModelBase<TInformationViewModel, TFinderItemViewModel>.FinderItemsViewer),
            nameof(FinderViewModelBase<TInformationViewModel, TFinderItemViewModel>.FinderItems),
            nameof(FinderViewModelBase<TInformationViewModel, TFinderItemViewModel>.FinderLoadState),
            nameof(FinderViewModelBase<TInformationViewModel, TFinderItemViewModel>.CanLoad),
            nameof(FinderViewModelBase<TInformationViewModel, TFinderItemViewModel>.NowLoading),
            nameof(IPagerFinder<TChildFinderViewModel, TInformationViewModel, TFinderItemViewModel>.PageItems),
            nameof(IPagerFinder<TChildFinderViewModel, TInformationViewModel, TFinderItemViewModel>.PageChangeCommand),
            nameof(FinderViewModelBase<TInformationViewModel, TFinderItemViewModel>.IsAscending),
            //nameof(TFinderViewModel.SelectedSortType),
            //nameof(TFinderViewModel.IsEnabledFinderFiltering),
            //nameof(TFinderViewModel.ShowFilterSetting),
        };

        #endregion

        #region event

        public event EventHandler<ChangedSelectedPageEventArgs<TChildFinderViewModel, TInformationViewModel, TFinderItemViewModel>> ChangedSelectedPage;

        #endregion

        #region variable

        TChildFinderViewModel _currentFinder;
        PageViewModel<TChildFinderViewModel> _selectedPage;

        #endregion

        public PagerFinderProvider(Mediation mediation, IPagerFinder<TChildFinderViewModel, TInformationViewModel, TFinderItemViewModel> parentFinder, IEnumerable<string> changePropertyNames)
        {
            Mediation = mediation;
            ParentFinder = parentFinder;
            if(changePropertyNames.Any()) {
                CustomChangePagePropertyNames.AddRange(changePropertyNames);
            }

            PropertyChangedListener = new WeakEventListener<PropertyChangedEventManager, PropertyChangedEventArgs>(PageVm_PropertyChanged);
        }

        public PagerFinderProvider(Mediation mediation, IPagerFinder<TChildFinderViewModel, TInformationViewModel, TFinderItemViewModel> parentFinder)
            : this(mediation, parentFinder, Enumerable.Empty<string>())
        { }

        #region property

        Mediation Mediation { get; }

        WeakEventListener<PropertyChangedEventManager, PropertyChangedEventArgs> PropertyChangedListener { get; }

        CollectionModel<string> CustomChangePagePropertyNames { get; } = new CollectionModel<string>(DefaultChangePagePropertyNames);
        public IReadOnlyList<string> ChangePagePropertyNames => CustomChangePagePropertyNames;

        IPagerFinder<TChildFinderViewModel, TInformationViewModel, TFinderItemViewModel> ParentFinder { get; }
        public TChildFinderViewModel CurrentFinder
        {
            get { return this._currentFinder; }
            set
            {
                if(SetVariableValue(ref this._currentFinder, value)) {
                    if(CurrentFinder != null) {
                        DetachmentChildProprtyChange(CurrentFinder);
                        AttachmentChildProprtyChange(CurrentFinder);
                    }
                }
            }
        }

        #endregion

        #region property

        public TChildFinderViewModel SelectedFinder => SelectedPage.ViewModel;

        public ICollectionView FinderItems
        {
            get
            {
                if(SelectedPage == null) {
                    if(CurrentFinder != null) {
                        return CurrentFinder.FinderItems;
                    } else {
                        return ParentFinder.FinderItems;
                    }
                }
                return SelectedPage.ViewModel.FinderItems;
            }
        }

        public SourceLoadState FinderLoadState
        {
            get
            {
                if(SelectedPage == null) {
                    if(CurrentFinder != null) {
                        return CurrentFinder.FinderLoadState;
                    } else {
                        return ParentFinder.FinderLoadState;
                    }
                }
                return SelectedPage.ViewModel.FinderLoadState;
            }
            set
            {
                if(SelectedPage == null) {
                    if(CurrentFinder != null) {
                        CurrentFinder.FinderLoadState = value;
                    } else {
                        ParentFinder.FinderLoadState = value;
                    }
                } else {
                    SelectedPage.ViewModel.FinderLoadState = value;
                }
            }
        }

        #endregion

        #region function

        protected void OnChangedSelectedPage(PageViewModel<TChildFinderViewModel> oldSelectedPage, PageViewModel<TChildFinderViewModel> newSelectedPage)
        {
            if(ChangedSelectedPage == null) {
                return;
            }

            //var e = new ChangedSelectedPageEventArgs(oldSelectedPage, newSelectedPage);
            var e = new ChangedSelectedPageEventArgs<TChildFinderViewModel, TInformationViewModel, TFinderItemViewModel>(oldSelectedPage, newSelectedPage);
            ChangedSelectedPage(this, e);
        }

        public void AttachmentChildProprtyChange(INotifyPropertyChanged target)
        {
            //target.PropertyChanged += PageVm_PropertyChanged;
            PropertyChangedEventManager.AddListener(target, PropertyChangedListener, string.Empty);
        }

        public void DetachmentChildProprtyChange(INotifyPropertyChanged target)
        {
            //target.PropertyChanged -= PageVm_PropertyChanged;
            PropertyChangedEventManager.RemoveListener(target, PropertyChangedListener, string.Empty);
        }

        KeyValuePair<MemberInfo, TBaseFinderVideModel> GetMemberInfo(bool getMethod, string memberName)
        {
            object target;
            if(SelectedPage == null) {
                if(CurrentFinder != null) {
                    target = CurrentFinder;
                } else {
                    target = ParentFinder;
                }
            } else {
                target = SelectedPage.ViewModel;
            }
            var type = target.GetType();
            MemberInfo[] members = getMethod
                ? type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(m => m.Name == memberName).ToArray()
                : type.GetMember(memberName)
            ;
            MemberInfo member;
            if(target == this) {
                member = members.First(m => m.DeclaringType == typeof(TBaseFinderVideModel));
            } else {
                member = members.First();
            }

            return new KeyValuePair<MemberInfo, TBaseFinderVideModel>(member, (TBaseFinderVideModel)target);
        }

        KeyValuePair<PropertyInfo, TBaseFinderVideModel> GetPropertyInfo([CallerMemberName] string propertyName = "")
        {
            var pair = GetMemberInfo(false, propertyName);

            return new KeyValuePair<PropertyInfo, TBaseFinderVideModel>((PropertyInfo)pair.Key, pair.Value);
        }

        KeyValuePair<MethodInfo, TBaseFinderVideModel> GetMethodInfo([CallerMemberName] string propertyName = "")
        {
            var pair = GetMemberInfo(true, propertyName);

            return new KeyValuePair<MethodInfo, TBaseFinderVideModel>((MethodInfo)pair.Key, pair.Value);
        }

        public TResult GetFinderProperty<TResult>([CallerMemberName] string propertyName = "")
        {
            var pair = GetPropertyInfo(propertyName);
            Debug.Assert(pair.Key.PropertyType == typeof(TResult));

            return (TResult)pair.Key.GetValue(pair.Value);
        }
        public void SetFinderProperty<TValue>(TValue value, [CallerMemberName] string propertyName = "")
        {
            var pair = GetPropertyInfo(propertyName);
            Debug.Assert(pair.Key.PropertyType == typeof(TValue));

            pair.Key.SetValue(pair.Value, value);
            CallPageItemOnPropertyChange();
        }

        public void DoFinderAction(string methodName, params object[] parameters)
        {
            var pair = GetMethodInfo(methodName);
            Debug.Assert(pair.Key.ReturnType == typeof(void));

            pair.Key.Invoke(pair.Value, parameters);
        }

        public TResult DoFinderFunction<TResult>(string methodName, params object[] parameters)
        {
            var pair = GetMethodInfo(methodName);
            Debug.Assert(pair.Key.ReturnType == typeof(TResult));

            return (TResult)pair.Key.Invoke(pair.Value, parameters);
        }

        #endregion

        #region IPagerFinder

        /// <summary>
        /// 保持するページャ。
        /// </summary>
        public CollectionModel<PageViewModel<TChildFinderViewModel>> PageItems { get; } = new CollectionModel<PageViewModel<TChildFinderViewModel>>();

        /// <summary>
        /// 選択中ページ。
        /// </summary>
        public PageViewModel<TChildFinderViewModel> SelectedPage
        {
            get { return this._selectedPage; }
            set
            {
                var oldSelectedPage = SelectedPage;
                if(SetVariableValue(ref this._selectedPage, value)) {
                    if(SelectedPage != null) {
                        SelectedPage.IsChecked = true;
                    }
                    if(oldSelectedPage != null) {
                        DetachmentChildProprtyChange(oldSelectedPage.ViewModel);
                        //oldSelectedPage.ViewModel.PropertyChanged -= SearchFinder_PropertyChanged_TotalCount;
                        oldSelectedPage.IsChecked = false;
                    }

                    if(SelectedPage != null && oldSelectedPage != null) {
                        SelectedPage.ViewModel.InputTitleFilter = oldSelectedPage.ViewModel.InputTitleFilter;
                        //SelectedPage.ViewModel.SelectedSortType = oldSelectedPage.ViewModel.SelectedSortType;
                        // #168
                        SelectedPage.ViewModel.IsAscending = oldSelectedPage.ViewModel.IsAscending;
                        SelectedPage.ViewModel.IsBlacklist = oldSelectedPage.ViewModel.IsBlacklist;
                        SelectedPage.ViewModel.IsEnabledFinderFiltering = oldSelectedPage.ViewModel.IsEnabledFinderFiltering;

                        SelectedPage.ViewModel.FinderItems.Refresh();
                    }

                    OnChangedSelectedPage(oldSelectedPage, SelectedPage);
                    CallPageItemOnPropertyChange();
                }
            }
        }

        public ICommand PageChangeCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var pageVm = (PageViewModel<TChildFinderViewModel>)o;
                        if(pageVm.LoadState != LoadState.Loaded) {
                            SelectedPage = pageVm;
                            //pageVm.ViewModel.PropertyChanged += PageVm_PropertyChanged;
                            AttachmentChildProprtyChange(pageVm.ViewModel);
                            pageVm.ViewModel.LoadDefaultCacheAsync().ConfigureAwait(true);
                        } else {
                            SelectedPage = pageVm;
                        }
                    }
                );
            }
        }

        public void CallPageItemOnPropertyChange()
        {
            ParentFinder.CallPageItemOnPropertyChange();
        }

        #endregion

        private void PageVm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Mediation.Logger.Information(e.PropertyName);
            if(CustomChangePagePropertyNames.Any(n => n == e.PropertyName)) {
                CallPageItemOnPropertyChange();
            }
        }

    }
}
