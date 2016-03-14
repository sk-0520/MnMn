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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.App;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel
{
    public class AppManagerViewModel: ManagerViewModelBase
    {
        public AppManagerViewModel()
            :base(new Mediation())
        {
            Setting = LoadSetting();

            Mediation = new Mediation(Setting);
            
            SmileManager = new SmileManagerViewModel(Mediation);
            AppSettingManager = new AppSettingManagerViewModel(Mediation);

            Mediation.SetManager(ServiceType.Application, new ApplicationManagerPackModel(AppSettingManager, SmileManager));
        }

        #region property

        AppSettingModel Setting { get; }

        public SmileManagerViewModel SmileManager { get; private set; }
        public AppSettingManagerViewModel AppSettingManager { get; }

        #endregion

        #region command

        [Obsolete]
        public ICommand Temp_OpenSmilePlayerCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        SmileVideoInformationViewModel.CreateFromVideoIdAsync(Mediation, "sm15218544", Constants.ServiceSmileVideoThumbCacheSpan).ContinueWith(task => {
                            task.Result.OpenPlayerAsync();
                        }, TaskScheduler.FromCurrentSynchronizationContext());
                        
                    }
                );
            }
        }

        #endregion

        #region function

        AppSettingModel LoadSetting()
        {
            return new AppSettingModel();
        }

        #endregion

        #region ManagerViewModelBase

        public override Task InitializeAsync()
        {
            return SmileManager.InitializeAsync();
        }

        #endregion

    }
}
