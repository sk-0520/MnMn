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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF.Control;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.App;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;
using MnMn.View.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel
{
    public class AppManagerViewModel: ManagerViewModelBase, ISetView
    {
        #region variable

        WindowState _state = WindowState.Normal;

        #endregion

        public AppManagerViewModel(Mediation mediation)
            :base(mediation)
        {
            Setting = Mediation.GetResultFromRequest<AppSettingModel>(new Model.Request.RequestModel(RequestKind.Setting, ServiceType.Application));

            SmileManager = new SmileManagerViewModel(Mediation);
            AppSettingManager = new AppSettingManagerViewModel(Mediation);

            Mediation.SetManager(ServiceType.Application, new ApplicationManagerPackModel(AppSettingManager, SmileManager));

            SmileSession = Mediation.GetResultFromRequest<SessionViewModelBase>(new RequestModel(RequestKind.Session, ServiceType.Smile));

        }

        #region property

        MainWindow View { get; set; }
        AppSettingModel Setting { get; }

        public AppSettingManagerViewModel AppSettingManager { get; }

        public SmileManagerViewModel SmileManager { get; }

        public SessionViewModelBase SmileSession { get; }

        public WindowState State
        {
            get { return this._state; }
            set { SetVariableValue(ref this._state, value); }
        }
        public double Left
        {
            get { return Setting.Window.Left; }
            set
            {
                if(State == WindowState.Normal) {
                    SetPropertyValue(Setting.Window, value, nameof(Setting.Window.Left));
                }
            }
        }
        public double Top
        {
            get { return Setting.Window.Top; }
            set
            {
                if(State == System.Windows.WindowState.Normal) {
                    SetPropertyValue(Setting.Window, value, nameof(Setting.Window.Top));
                }
            }
        }
        public double Width
        {
            get { return Setting.Window.Width; }
            set
            {
                if(State == WindowState.Normal) {
                    SetPropertyValue(Setting.Window, value, nameof(Setting.Window.Width));
                }
            }
        }
        public double Height
        {
            get { return Setting.Window.Height; }
            set
            {
                if(State == System.Windows.WindowState.Normal) {
                    SetPropertyValue(Setting.Window, value, nameof(Setting.Window.Height));
                }
            }
        }


        #endregion

        #region command

        #endregion

        #region function

        void SaveSetting()
        {
            var dir = VariableConstants.GetSettingDirectory();
            var filePath = Path.Combine(dir.FullName, Constants.SettingFileName);

            SerializeUtility.SaveSetting(filePath, Setting, SerializeFileType.Json, true, Mediation.Logger);
        }

        #endregion

        #region ManagerViewModelBase

        public override Task InitializeAsync()
        {
            return SmileManager.InitializeAsync();
        }

        #endregion

        #region ISetView

        public  void SetView(FrameworkElement view)
        {
            View = (MainWindow)view;

            View.Closed += View_Closed;
        }

        #endregion

        private void View_Closed(object sender, EventArgs e)
        {
            View.Closed -= View_Closed;

            SaveSetting();
        }

    }
}
