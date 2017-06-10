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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility.UI;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Data;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Live;
using ContentTypeTextNet.MnMn.MnMn.IF.Control;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.IF.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Live;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Live;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;
using Gecko;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Live.Player
{
    public class SmileLivePlayerViewModel: ViewModelBase, ISetView, ICloseView, ISmileDescription
    {
        #region define

        //static readonly Thickness enabledResizeBorderThickness = SystemParameters.WindowResizeBorderThickness;
        //static readonly Thickness maximumWindowBorderThickness = SystemParameters.WindowResizeBorderThickness;
        //static readonly Thickness normalWindowBorderThickness = new Thickness(1);

        #endregion

        #region variable

        WindowState _state = WindowState.Normal;
        double _left;
        double _top;
        double _width;
        double _height;
        bool _topmost;
        bool _isNormalWindow = true;

        double _viewScale;

        string _descriptionHtmlSource;

        #endregion

        public SmileLivePlayerViewModel(Mediation mediation)
        {
            Mediation = mediation;

            Setting = Mediation.GetResultFromRequest<SmileLiveSettingModel>(new RequestModel(RequestKind.Setting, ServiceType.SmileLive));
            Session = Mediation.GetResultFromRequest<SmileSessionViewModel>(new RequestModel(RequestKind.Session, ServiceType.Smile));

            NetworkSetting = Mediation.GetNetworkSetting();

            PropertyChangedListener = new PropertyChangedWeakEventListener(ShowWebPlayer_PropertyChanged);
            PropertyChangedListener.Add(ShowWebPlayer);

            ImportSetting();
        }

        #region proeprty

        Mediation Mediation { get; }
        IReadOnlyNetworkSetting NetworkSetting { get; }

        public SmileSessionViewModel Session { get; }
        SmileLiveSettingModel Setting { get; }

        public string Id => Information.Id;

        PropertyChangedWeakEventListener PropertyChangedListener { get; }

        public FewViewModel<bool> IsWorkingPlayer { get; } = new FewViewModel<bool>(false);

        public SmileLiveInformationViewModel Information { get; private set; }


        SmileLivePlayerWindow View { get; set; }
        WebNavigator NavigatorPlayer { get; set; }

        public FewViewModel<bool> ShowWebPlayer { get; } = new FewViewModel<bool>(false);
        public FewViewModel<bool> ShowMask { get; } = new FewViewModel<bool>(true);

        public FewViewModel<bool> PlayerShowDetailArea { get; } = new FewViewModel<bool>();

        public FewViewModel<LoadState> PlayerLoadState { get; } = new FewViewModel<LoadState>();

        /// <summary>
        /// ビューが閉じられたか。
        /// </summary>
        bool IsViewClosed { get; set; }

        public bool IsNormalWindow
        {
            get { return this._isNormalWindow; }
            set { SetVariableValue(ref this._isNormalWindow, value); }
        }

        #region window

        /// <summary>
        /// ウィンドウ X 座標。
        /// </summary>
        public double Left
        {
            get { return this._left; }
            set
            {
                if(IsNormalWindow && State == WindowState.Normal) {
                    SetVariableValue(ref this._left, value);
                }
            }
        }
        /// <summary>
        /// ウィンドウ Y 座標。
        /// </summary>
        public double Top
        {
            get { return this._top; }
            set
            {
                if(IsNormalWindow && State == WindowState.Normal) {
                    SetVariableValue(ref this._top, value);
                }
            }
        }
        /// <summary>
        /// ウィンドウ横幅。
        /// </summary>
        public double Width
        {
            get { return this._width; }
            set
            {
                if(IsNormalWindow && State == WindowState.Normal) {
                    SetVariableValue(ref this._width, value);
                }
            }
        }
        /// <summary>
        /// ウィンドウ高さ。
        /// </summary>
        public double Height
        {
            get { return this._height; }
            set
            {
                if(IsNormalWindow && State == WindowState.Normal) {
                    SetVariableValue(ref this._height, value);
                }
            }
        }
        /// <summary>
        /// 最前面表示状態。
        /// </summary>
        public bool Topmost
        {
            get { return this._topmost; }
            set { SetVariableValue(ref this._topmost, value); }
        }

        public double ViewScale
        {
            get { return this._viewScale; }
            set
            {
                if(SetVariableValue(ref this._viewScale, value)) {
                    WebNavigatorUtility.ApplyWebNavigatorScale(View, ViewScale);
                }
            }
        }

        #endregion

        public DescriptionBase DescriptionProcessor => new SmileDescription(Mediation);

        public string DescriptionHtmlSource
        {
            get { return this._descriptionHtmlSource; }
            set { SetVariableValue(ref this._descriptionHtmlSource, value); }
        }

        #endregion

        #region command

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

        public ICommand SourceLoadedCommand
        {
            get { return CreateCommand(o => SourceLoaded((WebNavigatorEventDataBase)o)); }
        }

        public ICommand DomLoadedCommand
        {
            get { return CreateCommand(o => DomLoaded((WebNavigatorEventDataBase)o)); }
        }

        public ICommand ForceShowPlayerCommand
        {
            get
            {
                return CreateCommand(
                    o => ShowWebPlayer.Value = true,
                    o => ShowMask.Value
                );
            }
        }


        #endregion

        #region function

        void ImportSetting()
        {
            Left = Setting.Player.Window.Left;
            Top = Setting.Player.Window.Top;
            Width = Setting.Player.Window.Width;
            Height = Setting.Player.Window.Height;
            Topmost = Setting.Player.Window.Topmost;

            ViewScale = Setting.Player.ViewScale;

            PlayerShowDetailArea.Value = Setting.Player.ShowDetailArea;
        }

        void ExportSetting()
        {
            Setting.Player.Window.Left = Left;
            Setting.Player.Window.Top = Top;
            Setting.Player.Window.Width = Width;
            Setting.Player.Window.Height = Height;
            Setting.Player.Window.Topmost = Topmost;

            Setting.Player.ViewScale = ViewScale;

            Setting.Player.ShowDetailArea = PlayerShowDetailArea.Value;
        }

        Task LoadWatchPageAsync()
        {
            ShowWebPlayer.Value = false;
            PlayerLoadState.Value = LoadState.Preparation;

            //ShowMask.Value = true;
            WebNavigatorCore.SetSessionEngine(Session, Information.WatchUrl);
            return NavigatorPlayer.Dispatcher.BeginInvoke(new Action(() => {
                NavigatorPlayer.Navigate(Information.WatchUrl);
                PlayerLoadState.Value = LoadState.Loading;
            })).Task;
        }

        internal Task LoadAsync(SmileLiveInformationViewModel information, bool forceEconomy, CacheSpan informationCacheSpan, CacheSpan imageCacheSpan)
        {
            // ブラウザまりの処理が必要なのでVideo側とは順序が異なる
            Debug.Assert(View != null);

            Information = information;
            CallOnPropertyChange(nameof(Information));

            var page = new PageLoader(Mediation, new HttpUserAgentHost(NetworkSetting, Mediation.Logger), SmileLiveMediationKey.watchPage, ServiceType.SmileLive);
            page.ForceUri = Information.WatchUrl;
            return page.GetResponseTextAsync(PageLoaderMethod.Get).ContinueWith(t => {
                var response = t.Result;

                page.Dispose();

                if(response.IsSuccess) {
                    var htmlSource = response.Result;

                    DescriptionHtmlSource = GetDescription(htmlSource);
                }

                return LoadWatchPageAsync();
            });
        }

        void TuneFlvplayer(GeckoWebBrowser browser, GeckoHtmlElement flvplayerElement)
        {
            var htmlElement = browser.Document.GetElementsByTagName("html").FirstOrDefault();
            var bodyElement = browser.Document.Body;
            var firstElements = bodyElement.ChildNodes
                .OfType<GeckoHtmlElement>()
                .ToEvaluatedSequence()
            ;

            foreach(var element in firstElements) {
                element.Style.SetPropertyValue("display", "none");
            }
            var rootElements = new[] {
                htmlElement,
                bodyElement,
            };
            foreach(var element in rootElements) {
                element.Style.SetPropertyValue("height", "100%");
                element.Style.SetPropertyValue("margin", "0");
                element.Style.SetPropertyValue("padding", "0");
                element.Style.SetPropertyValue("width", "100%");
                element.Style.SetPropertyValue("height", "100%");
            }

            var flvplayerContainerElement = flvplayerElement.ParentElement as GeckoHtmlElement;
            flvplayerContainerElement.Style.SetPropertyValue("width", "100%");
            flvplayerContainerElement.Style.SetPropertyValue("height", "100%");
            flvplayerElement.RemoveAttribute("width");
            flvplayerElement.RemoveAttribute("height");
            flvplayerElement.Style.SetPropertyValue("width", "100%");
            flvplayerElement.Style.SetPropertyValue("height", "100%");
            bodyElement.InsertBefore(flvplayerContainerElement, bodyElement.FirstChild);
        }

        private void TunePlayerContainer(GeckoWebBrowser browser, GeckoHtmlElement playerContainerElement)
        {
            var htmlElement = browser.Document.GetElementsByTagName("html").FirstOrDefault();
            var bodyElement = browser.Document.Body;
            var firstElements = bodyElement.ChildNodes
                .OfType<GeckoHtmlElement>()
                .ToEvaluatedSequence()
            ;

            foreach(var element in firstElements) {
                element.Style.SetPropertyValue("display", "none");
            }
            var rootElements = new[] {
                htmlElement,
                bodyElement,
            };
            foreach(var element in rootElements) {
                //element.Style.SetPropertyValue("height", "100%");
                element.Style.SetPropertyValue("margin", "0");
                element.Style.SetPropertyValue("padding", "0");
                //element.Style.SetPropertyValue("width", "100%");
                //element.Style.SetPropertyValue("height", "100%");
            }

            bodyElement.InsertBefore(playerContainerElement, bodyElement.FirstChild);
        }


        void SourceLoadedGeckoFx(WebNavigatorEventData<DomEventArgs> eventData)
        {
            Mediation.Logger.Debug($"{Information.Id}: {nameof(NavigatorPlayer.IsEmptyContent)} = {NavigatorPlayer.IsEmptyContent}");

            var browser = (GeckoWebBrowser)eventData.Sender;

            if(NavigatorPlayer.IsEmptyContent) {
                // TODO: 本来不要
                PlayerLoadState.Value = LoadState.Loaded;

                ShowWebPlayer.Value = true;
                return;
            }

            // 旧来の
            var flvplayerElement = browser.Document.GetElementById("flvplayer") as GeckoHtmlElement;
            // 新しいの
            var playerContainerElement = browser.Document.GetElementById("player-container") as GeckoHtmlElement;

            if(flvplayerElement == null && playerContainerElement == null) {
                Mediation.Logger.Debug($"{Information.Id}: {nameof(flvplayerElement)} is null && {nameof(flvplayerElement)} is null");
                // TODO: 本来不要
                PlayerLoadState.Value = LoadState.Loaded;

                ShowWebPlayer.Value = true;
                return;
            }
            if(flvplayerElement != null) {
                TuneFlvplayer(browser, flvplayerElement);
            } else {
                TunePlayerContainer(browser, playerContainerElement);
            }

            PlayerLoadState.Value = LoadState.Loaded;
            ShowWebPlayer.Value = true;
            Mediation.Logger.Trace($"{Information.Id}: {PlayerLoadState.Value}");
        }

        string GetDescription(string htmlSource)
        {
            var html = new HtmlAgilityPack.HtmlDocument() {
                OptionAutoCloseOnEnd = true,
            };
            //TODO: 不勉強故か <img> が死なないので古き良き文字列置き換え
            //var images = html.DocumentNode.SelectNodes("//img").ToArray();
            //foreach(var img in images) {
            //    img.Remove();
            //}
            htmlSource = Regex.Replace(htmlSource, @"<img(\s.*?)?\s*/?>", string.Empty);
            html.LoadHtml(htmlSource);
            var baseElement = html.GetElementbyId("jsFollowingAdMain");
            var removeTargets = new[] {
                baseElement?.SelectSingleNode(".//*[@class='creator_btn_are']"),
                baseElement?.SelectSingleNode(".//*[@id='livetags']"),
                baseElement?.SelectSingleNode(".//*[@class='chan']"),
                baseElement?.SelectSingleNode(".//*[@id='tooltip']"),
                baseElement?.SelectSingleNode(".//*[@class='com']"),
                baseElement?.SelectSingleNode(".//*[@class='community-info-score']"),
            };
            foreach(var target in removeTargets.Where(n => n != null)) {
                target.Remove();
            }

            return baseElement?.InnerHtml ?? string.Empty;
        }

        void SourceLoaded(WebNavigatorEventDataBase eventData)
        {
            switch(eventData.Engine) {
                case WebNavigatorEngine.GeckoFx:
                    SourceLoadedGeckoFx((WebNavigatorEventData<DomEventArgs>)eventData);
                    break;

                default: //TODO: IE側余なし
                    break;
            }
        }

        private void DomLoadedGeckoFx(WebNavigatorEventData<DomEventArgs> eventData)
        {
        }

        private void DomLoaded(WebNavigatorEventDataBase eventData)
        {
            switch(eventData.Engine) {
                case WebNavigatorEngine.GeckoFx:
                    DomLoadedGeckoFx((WebNavigatorEventData<DomEventArgs>)eventData);
                    break;

                default: //TODO: IE側余なしっていうか、そもそも飛んでこない
                    break;
            }
        }

        Task OpenVideoLinkAsync(string videoId)
        {
            Mediation.Logger.Error($"not impl: {videoId}");
            return Task.CompletedTask;
        }


        void OpenUserLink(string userId)
        {
            SmileDescriptionUtility.OpenUserId(userId, Mediation);
        }

        #endregion

        #region ISetView

        public void SetView(FrameworkElement view)
        {
            View = (SmileLivePlayerWindow)view;
            NavigatorPlayer = View.navigatorPlayer;

            WebNavigatorUtility.ApplyWebNavigatorScale(View, ViewScale);
            //DocumentDescription = View.documentDescription;

            //
            View.Closed += View_Closed;
        }

        #endregion

        #region ICloseView

        public void Close()
        {
            if(!IsViewClosed) {
                View.Close();
            }
        }

        #endregion

        #region ICaptionCommand

        public WindowState State
        {
            get { return this._state; }
            set { SetVariableValue(ref this._state, value); }
        }

        #endregion

        #region ISmileDescription

        public ImageSource DefaultBrowserIcon { get; } = WebNavigatorCore.DefaultBrowserIcon;

        public ICommand OpenUriCommand
        {
            get { return CreateCommand(o => DescriptionUtility.OpenUri(o, Mediation.Logger)); }
        }
        public ICommand MenuOpenUriCommand => OpenUriCommand;
        public ICommand MenuOpenUriInAppBrowserCmmand
        {
            get { return CreateCommand(o => DescriptionUtility.OpenUriInAppBrowser(o, Mediation)); }
        }
        public ICommand MenuCopyUriCmmand { get { return CreateCommand(o => DescriptionUtility.CopyUri(o, Mediation.Logger)); } }

        public ICommand OpenVideoIdLinkCommand { get { return CreateCommand(o => OpenVideoLinkAsync((string)o), o => !string.IsNullOrWhiteSpace((string)o)); } }
        public ICommand MenuOpenVideoIdLinkCommand => OpenVideoIdLinkCommand;
        public ICommand MenuOpenVideoIdLinkInNewWindowCommand { get { return CreateCommand(o => SmileDescriptionUtility.MenuOpenVideoLinkInNewWindowAsync(o, Mediation).ConfigureAwait(false), o => !string.IsNullOrWhiteSpace((string)o)); } }
        public ICommand MenuCopyVideoIdCommand { get { return CreateCommand(o => SmileDescriptionUtility.CopyVideoId(o, Mediation.Logger)); } }
        public ICommand MenuAddPlayListVideoIdLinkCommand { get { return CreateCommand(o => { Mediation.Logger.Trace("not impl"); }); } }
        public ICommand MenuAddCheckItLaterVideoIdCommand
        {
            get { return CreateCommand(o => SmileDescriptionUtility.AddCheckItLaterVideoIdAsync(o, Mediation, Mediation.Smile.VideoMediation.ManagerPack.CheckItLaterManager).ConfigureAwait(false)); }
        }
        public ICommand MenuAddUnorganizedBookmarkVideoIdCommand
        {
            get { return CreateCommand(o => SmileDescriptionUtility.AddUnorganizedBookmarkAsync(o, Mediation, Mediation.Smile.VideoMediation.ManagerPack.BookmarkManager).ConfigureAwait(false)); }
        }

        public ICommand OpenMyListIdLinkCommand { get { return CreateCommand(o => SmileDescriptionUtility.OpenMyListId(o, Mediation)); } }
        public ICommand MenuOpenMyListIdLinkCommand => OpenMyListIdLinkCommand;
        public ICommand MenuAddMyListIdLinkCommand { get { return CreateCommand(o => SmileDescriptionUtility.AddMyListBookmarkAsync(o, Mediation).ConfigureAwait(false)); } }
        public ICommand MenuCopyMyListIdCommand { get { return CreateCommand(o => SmileDescriptionUtility.CopyMyListId(o, Mediation.Logger)); } }

        public ICommand OpenUserIdLinkCommand { get { return CreateCommand(o => OpenUserLink((string)o)); } }

        #endregion

        #region ViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(ShowWebPlayer != null) {
                PropertyChangedListener.Remove(ShowWebPlayer);
            }

            base.Dispose(disposing);
        }

        #endregion

        void View_Closed(object sender, EventArgs e)
        {
            ExportSetting();

            NavigatorPlayer.NavigateEmpty();

            PropertyChangedListener.Remove(ShowWebPlayer);

            Information.IsPlaying = false;
        }

        void ShowWebPlayer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ShowMask.Value = !ShowWebPlayer.Value;
        }


    }
}
