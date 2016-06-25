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
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Search
{
    /// <summary>
    /// <para>配列的な操作はこのクラス内で完結させたい思い</para>
    /// </summary>
    public class SmileVideoSearchGroupFinderViewModel: SmileVideoFinderViewModelBase
    {
        #region define

        static IEnumerable<string> ChangePagePropertyNames => new[] {
            nameof(VideoInformationItems),
            nameof(FinderLoadState),
            nameof(CanLoad),
            nameof(NowLoading),
            nameof(PageItems),
            nameof(PageChangeCommand),
        };

        #endregion
        #region variable

        DefinedElementModel _selectedMethod;
        DefinedElementModel _selectedSort;

        //ICollectionView _selectedVideoInformationItems;

        int _totalCount;
        PageViewModel<SmileVideoSearchItemFinderViewModel> _selectedPage;

        bool _notfound;

        #endregion

        public SmileVideoSearchGroupFinderViewModel(Mediation mediation, SmileVideoSearchModel searchModel, DefinedElementModel method, DefinedElementModel sort, SearchType type, string query)
            : base(mediation)
        {
            SearchModel = searchModel;
            Query = query;
            Type = type;

            SetContextElements(method, sort);
        }

        #region property

        SmileVideoSearchModel SearchModel { get; }

        SmileVideoSearchItemFinderViewModel SearchFinder { get; set; }

        public IList<DefinedElementModel> MethodItems => SearchModel.Methods;
        public IList<DefinedElementModel> SortItems => SearchModel.Sort;

        public string Query { get; }
        public SearchType Type { get; }

        public DefinedElementModel LoadingMethod { get; private set; }
        public DefinedElementModel LoadingSort { get; private set; }

        public DefinedElementModel SelectedMethod
        {
            get { return this._selectedMethod; }
            set { SetVariableValue(ref this._selectedMethod, value); }
        }
        public DefinedElementModel SelectedSort
        {
            get { return this._selectedSort; }
            set { SetVariableValue(ref this._selectedSort, value); }
        }

        public CollectionModel<PageViewModel<SmileVideoSearchItemFinderViewModel>> PageItems { get; set; } = new CollectionModel<PageViewModel<SmileVideoSearchItemFinderViewModel>>();

        public CollectionModel<SmileVideoTagViewModel> RelationTagItems { get; } = new CollectionModel<SmileVideoTagViewModel>();

        public PageViewModel<SmileVideoSearchItemFinderViewModel> SelectedPage
        {
            get { return this._selectedPage; }
            set {
                var oldSelectedPage = this._selectedPage;
                if(SetVariableValue(ref this._selectedPage, value)) {
                    if(this._selectedPage != null) {
                        this._selectedPage.IsChecked = true;
                    }
                    if(oldSelectedPage!= null) {
                        oldSelectedPage.ViewModel.PropertyChanged -= PageVm_PropertyChanged;
                        oldSelectedPage.IsChecked = false;
                    }
                    CallPageItemOnPropertyChange();
                }
            }
        }

        public override ICollectionView VideoInformationItems
        {
            get
            {
                if(SelectedPage == null) {
                    if(SearchFinder != null) {
                        return SearchFinder.VideoInformationItems;
                    } else {
                        return base.VideoInformationItems;
                    }
                }
                return SelectedPage.ViewModel.VideoInformationItems;
            }
        }

        public int TotalCount
        {
            get { return this._totalCount; }
            set { SetVariableValue(ref this._totalCount, value); }
        }

        public bool NotFound
        {
            get { return this._notfound; }
            set { SetVariableValue(ref this._notfound, value); }
        }

        public override SourceLoadState FinderLoadState
        {
            get
            {
                if(SelectedPage == null) {
                    if(SearchFinder != null) {
                        return SearchFinder.FinderLoadState;
                    } else {
                        return base.FinderLoadState;
                    }
                }
                return SelectedPage.ViewModel.FinderLoadState;
            }

            set
            {
                if(SelectedPage == null) {
                    if(SearchFinder != null) {
                        SearchFinder.FinderLoadState = value;
                    } else {
                        base.FinderLoadState = value;
                    }
                } else {
                    SelectedPage.ViewModel.FinderLoadState = value;
                }
            }
        }

        #endregion

        #region command

        public ICommand PageChangeCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var pageVm = (PageViewModel<SmileVideoSearchItemFinderViewModel>)o;
                        if(pageVm.LoadState != LoadState.Loaded) {
                            var thumbCacheSpan = Constants.ServiceSmileVideoThumbCacheSpan;
                            var imageCacheSpan = Constants.ServiceSmileVideoImageCacheSpan;

                            SelectedPage = pageVm;
                            pageVm.ViewModel.PropertyChanged += PageVm_PropertyChanged;
                            pageVm.ViewModel.LoadAsync(thumbCacheSpan, imageCacheSpan).ConfigureAwait(true);
                        } else {
                            SelectedPage = pageVm;
                        }
                    }
                );
            }
        }

        public override ICommand ReloadCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        LoadAsync(Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan, true).ConfigureAwait(true);
                    }
                );
            }
        }

        #endregion

        #region function

        void CallPageItemOnPropertyChange()
        {
            CallOnPropertyChange(ChangePagePropertyNames);
        }

        DefinedElementModel GetContextElemetFromChangeElement(IEnumerable<DefinedElementModel> items, DefinedElementModel element)
        {
            if(items.Any(i => i == element)) {
                return element;
            } else {
                return items.FirstOrDefault(i => i.Key == element.Key);
            }
        }

        public void SetContextElements(DefinedElementModel method, DefinedElementModel sort)
        {
            LoadingMethod = SelectedMethod = GetContextElemetFromChangeElement(MethodItems, method);
            LoadingSort = SelectedSort = GetContextElemetFromChangeElement(SortItems, sort);
        }

        protected override Task LoadCoreAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan, object extends)
        {
            var isReload = (bool)extends;

            DefinedElementModel nowMethod;
            DefinedElementModel nowSort;
            if(isReload) {
                nowMethod = SelectedMethod;
                nowSort = SelectedSort;
                SelectedPage = null;
            } else {
                nowMethod = LoadingMethod;
                nowSort = LoadingSort;
            }


            SearchFinder = new SmileVideoSearchItemFinderViewModel(Mediation, SearchModel, nowMethod, nowSort, Type, Query, 0, Setting.Search.Count);
            SearchFinder.PropertyChanged += PageVm_PropertyChanged;

            var query = Query;

            if(isReload) {
                var tag = new Logic.Service.Smile.Video.Api.V1.Tag(Mediation);
                var tagTask = tag.LoadRelationTagListAsync(query).ContinueWith(task => {
                    var list = task.Result;
                    var items = list.Tags
                        .Where(t => t.Text != query)
                        .Select(t => new SmileVideoTagViewModel(Mediation, t))
                    ;
                    RelationTagItems.InitializeRange(items);
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }

            return SearchFinder.LoadAsync(thumbCacheSpan, imageCacheSpan).ContinueWith(task => {
                if(isReload) {
                    TotalCount = SearchFinder.TotalCount;
                    if(TotalCount > Setting.Search.Count) {
                        var pageCount = Math.Min(TotalCount / Setting.Search.Count, (SearchModel.MaximumIndex + SearchModel.MaximumCount) / Setting.Search.Count);
                        var correctionPage = TotalCount > (SearchModel.MaximumIndex + SearchModel.MaximumCount) ? 1 : 0;
                        var preList = Enumerable.Range(1, pageCount - correctionPage)
                            .Select((n, i) => new SmileVideoSearchItemFinderViewModel(Mediation, SearchModel, nowMethod, nowSort, Type, query, (i + 1) * Setting.Search.Count, Setting.Search.Count))
                            .Select((v, i) => new PageViewModel<SmileVideoSearchItemFinderViewModel>(v, i + 2))
                            .ToList()
                        ;
                        var pageVm = new PageViewModel<SmileVideoSearchItemFinderViewModel>(SearchFinder, 1) {
                            LoadState = LoadState.Loaded,
                        };
                        preList.Insert(0, pageVm);
                        return (IEnumerable<PageViewModel<SmileVideoSearchItemFinderViewModel>>)preList;
                    } else if(TotalCount > 0) {
                        var pageVm = new PageViewModel<SmileVideoSearchItemFinderViewModel>(SearchFinder, 1) {
                            LoadState = LoadState.Loaded,
                        };
                        return new[] { pageVm };
                    }
                }

                return Enumerable.Empty<PageViewModel<SmileVideoSearchItemFinderViewModel>>();
            }, TaskScheduler.FromCurrentSynchronizationContext()).ContinueWith(task => {
                var preList = task.Result;
                PageItems.InitializeRange(preList);
                NotFound = !PageItems.Any();
                if(!NotFound) {
                    SelectedPage = PageItems.First();
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public Task LoadAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan, bool isReload)
        {
            return LoadCoreAsync(thumbCacheSpan, imageCacheSpan, isReload);
        }

        public new Task LoadAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan)
        {
            throw new NotSupportedException();
        }

        KeyValuePair<MemberInfo, SmileVideoFinderViewModelBase> GetMemberInfo(bool getMethod, string memberName)
        {
            SmileVideoFinderViewModelBase target;
            if(SelectedPage == null) {
                if(SearchFinder != null) {
                    target = SearchFinder;
                } else {
                    target = this;
                }
            } else {
                target = SelectedPage.ViewModel;
            }
            var type = target.GetType();
            MemberInfo[] members = getMethod 
                ? type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod).Where(m => m.Name == memberName).ToArray()
                : type.GetMember(memberName)
            ;
            MemberInfo member;
            if(target == this) {
                member = members.First(m => m.DeclaringType == typeof(SmileVideoFinderViewModelBase));
            } else {
                member = members.First();
            }

            return new KeyValuePair<MemberInfo, SmileVideoFinderViewModelBase>(member, target);
        }

        KeyValuePair<PropertyInfo, SmileVideoFinderViewModelBase> GetPropertyInfo([CallerMemberName] string propertyName = "")
        {
            var pair = GetMemberInfo(false, propertyName);

            return new KeyValuePair<PropertyInfo, SmileVideoFinderViewModelBase>((PropertyInfo)pair.Key, pair.Value);
        }

        KeyValuePair<MethodInfo, SmileVideoFinderViewModelBase> GetMethodInfo([CallerMemberName] string propertyName = "")
        {
            var pair = GetMemberInfo(true, propertyName);

            return new KeyValuePair<MethodInfo, SmileVideoFinderViewModelBase>((MethodInfo)pair.Key, pair.Value);
        }

        TResult GetSearchProperty<TResult>([CallerMemberName] string propertyName = "")
        {
            var pair = GetPropertyInfo(propertyName);
            Debug.Assert(pair.Key.PropertyType == typeof(TResult));

            return (TResult)pair.Key.GetValue(pair.Value);
        }
        void SetSearchProperty<TValue>(TValue value, [CallerMemberName] string propertyName = "")
        {
            var pair = GetPropertyInfo(propertyName);
            Debug.Assert(pair.Key.PropertyType == typeof(TValue));

            pair.Key.SetValue(pair.Value, value);
        }

        void DoSearchAction(string methodName, params object[] parameters)
        {
            var pair = GetMethodInfo(methodName);
            Debug.Assert(pair.Key.ReturnType == typeof(void));

            pair.Key.Invoke(pair.Value, parameters);
        }

        #endregion

        #region SmileVideoFinderViewModelBase

        public override bool CanLoad
        {
            get
            {
                if(SelectedPage == null) {
                    return false;
                }

                return SelectedPage.ViewModel.CanLoad;
            }
        }

        public override string InputFilter
        {
            get
            {
                //if(SelectedPage == null) {
                //    if(SearchFinder != null) {
                //        return SearchFinder.InputFilter;
                //    } else {
                //        return base.InputFilter;
                //    }
                //}

                //return SelectedPage.ViewModel.InputFilter;
                return GetSearchProperty<string>();
            }
            set
            {
                //if(SelectedPage == null) {
                //    if(SearchFinder != null) {
                //        SearchFinder.InputFilter = value;
                //    } else {
                //        base.InputFilter = value;
                //    }
                //} else {
                //    SelectedPage.ViewModel.InputFilter = value;
                //}
                SetSearchProperty(value);
            }
        }
        public override bool IsBlacklist
        {
            get
            {
                if(SelectedPage == null) {
                    if(SearchFinder != null) {
                        return SearchFinder.IsBlacklist;
                    } else {
                        return base.IsBlacklist;
                    }
                }

                return SelectedPage.ViewModel.IsBlacklist;
            }
            set
            {
                if(SelectedPage == null) {
                    if(SearchFinder != null) {
                        SearchFinder.IsBlacklist = value;
                    } else {
                        base.IsBlacklist = value;
                    }
                } else {
                    SelectedPage.ViewModel.IsBlacklist = value;
                }
            }
        }

        internal override void ToggleAllCheck()
        {
            //if(SelectedPage == null) {
            //    if(SearchFinder != null) {
            //        SearchFinder.ToggleAllCheck();
            //    } else {
            //        base.ToggleAllCheck();
            //    }
            //} else {
            //    SelectedPage.ViewModel.ToggleAllCheck();
            //}
            DoSearchAction(nameof(ToggleAllCheck));
        }

        internal override Task ContinuousPlayback()
        {
            if(SelectedPage == null) {
                if(SearchFinder != null) {
                    return SearchFinder.ContinuousPlayback();
                } else {
                    return base.ContinuousPlayback();
                }
            } else {
                return SelectedPage.ViewModel.ContinuousPlayback();
            }
        }

        #endregion

        private void PageVm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Mediation.Logger.Information(e.PropertyName);
            if(ChangePagePropertyNames.Any(n => n == e.PropertyName)) {
                CallPageItemOnPropertyChange();
            }
        }

    }
}
