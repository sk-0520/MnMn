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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Data;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF.Control;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Live;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;
using Gecko;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Live.Player
{
    public class SmileLivePlayerViewModel: ViewModelBase, ISetView, ICloseView, ICaptionCommand
    {
        #region define

        static readonly Thickness enabledResizeBorderThickness = SystemParameters.WindowResizeBorderThickness;
        static readonly Thickness maximumWindowBorderThickness = SystemParameters.WindowResizeBorderThickness;
        static readonly Thickness normalWindowBorderThickness = new Thickness(1);

        #endregion

        #region variable

        WindowState _state = WindowState.Normal;
        Thickness _resizeBorderThickness = enabledResizeBorderThickness;
        Thickness _windowBorderThickness = normalWindowBorderThickness;

        #endregion

        public SmileLivePlayerViewModel(Mediation mediation)
        {
            Mediation = mediation;

            Session = Mediation.GetResultFromRequest<SmileSessionViewModel>(new RequestModel(RequestKind.Session, ServiceType.Smile));
        }

        #region proeprty

        Mediation Mediation { get; }

        public SmileSessionViewModel Session { get; }

        public FewViewModel<bool> IsWorkingPlayer { get; } = new FewViewModel<bool>(false);

        public SmileLiveInformationViewModel Information { get; private set; }

        SmileLivePlayerWindow View { get; set; }
        WebNavigator NavigatorPlayer { get; set; }

        /// <summary>
        /// ビューが閉じられたか。
        /// </summary>
        bool IsViewClosed { get; set; }

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



        #endregion

        #region function

        Task LoadWatchPageAsync()
        {
            WebNavigatorCore.SetSessionEngine(Session, Information.WatchUrl);
            //NavigatorPlayer.Navigate(Information.WatchUrl);
            NavigatorPlayer.Navigate(new Uri(Constants.SmileLivePlayerContainerPath));

            return Task.CompletedTask;
        }

        internal Task LoadAsync(SmileLiveInformationViewModel information, bool forceEconomy, CacheSpan informationCacheSpan, CacheSpan imageCacheSpan)
        {
            // ブラウザまりの処理が必要なのでVideo側とは順序が異なる
            Debug.Assert(View != null);

            Information = information;

            LoadWatchPageAsync();

            return Task.CompletedTask;
        }

        void SourceLoadedGeckoFx(WebNavigatorEventData<DomEventArgs> eventData)
        {
            //var browser = (GeckoWebBrowser)eventData.Sender;
            //var flvplayerElement = browser.Document.GetElementById("flvplayer") as GeckoHtmlElement;
            //if(flvplayerElement == null) {
            //    return;
            //}

            //var htmlElement = browser.Document.GetElementsByTagName("html").FirstOrDefault();
            //var bodyElement = browser.Document.GetElementsByTagName("body").FirstOrDefault();
            //var firstElements = bodyElement.ChildNodes
            //    .OfType<GeckoHtmlElement>()
            //    .ToArray()
            //;

            //foreach(var element in firstElements) {
            //    element.Style.SetPropertyValue("display", "none");
            //}
            //var rootElements = new[] {
            //    htmlElement,
            //    bodyElement
            //};
            //foreach(var element in rootElements) {
            //    element.Style.SetPropertyValue("height", "100%");
            //    element.Style.SetPropertyValue("margin", "0");
            //}

            //var flvplayerContainerElement = flvplayerElement.ParentElement as GeckoHtmlElement;
            //flvplayerContainerElement.Style.SetPropertyValue("width", "100%");
            //flvplayerContainerElement.Style.SetPropertyValue("height", "100%");
            ////flvplayerElement.RemoveAttribute("width");
            ////flvplayerElement.RemoveAttribute("height");
            //flvplayerElement.Style.SetPropertyValue("width", "100%");
            //flvplayerElement.Style.SetPropertyValue("height", "100%");
            //flvplayerContainerElement.ParentElement.RemoveChild(flvplayerContainerElement);
            //bodyElement.AppendChild(flvplayerContainerElement);

            if(NavigatorPlayer.IsEmptyContent) {
                return;
            }

            var browser = (GeckoWebBrowser)eventData.Sender;
            //browser.Document.CurrentScript
            var frameElement = browser.Document.GetElementById("cttn_mnmn_service_smile_live_frame") as GeckoHtmlElement;
            browser.AddMessageEventListener("onload", s => LoadedSource(browser, s));
            frameElement.SetAttribute("src", Information.WatchUrl.OriginalString);
        }

        void LoadedSource(GeckoWebBrowser browser, string s)
        {
            browser.RemoveMessageEventListener("onload");
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


        #endregion

        #region ISetView

        public void SetView(FrameworkElement view)
        {
            View = (SmileLivePlayerWindow)view;
            NavigatorPlayer = View.navigatorPlayer;

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

        /// <summary>
        /// リサイズ幅。
        /// </summary>
        public Thickness ResizeBorderThickness
        {
            get { return this._resizeBorderThickness; }
            set { SetVariableValue(ref this._resizeBorderThickness, value); }
        }
        /// <summary>
        /// ウィンドウ枠幅。
        /// </summary>
        public Thickness WindowBorderThickness
        {
            get { return this._windowBorderThickness; }
            set { SetVariableValue(ref this._windowBorderThickness, value); }
        }

        public WindowState State
        {
            get { return this._state; }
            set
            {
                if(SetVariableValue(ref this._state, value)) {
                    if(State == WindowState.Maximized) {
                        WindowBorderThickness = maximumWindowBorderThickness;
                    } else {
                        //if(!IsNormalWindow) {
                        //    SetWindowMode(false);
                        //}
                        WindowBorderThickness = normalWindowBorderThickness;
                    }
                }
            }
        }

        //TODO: Videoと重複
        public ICommand ShowSystemMenuCommand
        {
            get
            {
                return CreateCommand(o => {
                    var hWnd = HandleUtility.GetWindowHandle(View);
                    var _WM_SYSTEM_MENU = 0x313;
                    var devicePoint = MouseUtility.GetDevicePosition();
                    var desktopPoint = PodStructUtility.Convert(devicePoint);
                    var lParam = new IntPtr(desktopPoint.X | desktopPoint.Y << 16);
                    NativeMethods.PostMessage(hWnd, (WM)_WM_SYSTEM_MENU, IntPtr.Zero, lParam);
                });
            }
        }

        public ICommand CaptionMinimumCommand { get { return CreateCommand(o => State = WindowState.Minimized); } }
        public ICommand CaptionMaximumCommand { get { return CreateCommand(o => State = WindowState.Maximized); } }
        public ICommand CaptionRestoreCommand { get { return CreateCommand(o => State = WindowState.Normal); } }
        public ICommand CaptionCloseCommand { get { return CreateCommand(o => View.Close()); } }

        #endregion

        void View_Closed(object sender, EventArgs e)
        {
            NavigatorPlayer.NavigateEmpty();
        }


    }
}
