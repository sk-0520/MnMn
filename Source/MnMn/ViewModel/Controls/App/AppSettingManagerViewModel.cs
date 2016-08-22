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
using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App
{
    public class AppSettingManagerViewModel: ManagerViewModelBase
    {
        public AppSettingManagerViewModel(Mediation mediation)
            : base(mediation)
        {
            AppSetting = Mediation.GetResultFromRequest<AppSettingModel>(new Model.Request.RequestModel(Define.RequestKind.Setting, Define.ServiceType.Application));
        }

        #region property

        AppSettingModel AppSetting { get; }

        public string CacheDirectoryPath
        {
            get { return AppSetting.CacheDirectoryPath; }
            set { SetPropertyValue(AppSetting, value); }
        }

        public TimeSpan CacheLifeTime
        {
            get { return AppSetting.CacheLifeTime; }
            set { SetPropertyValue(AppSetting, value); }
        }

        #endregion

        #region command

        public ICommand SelectCacheDirectoryCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        CacheDirectoryPath = SelectDirectory(CacheDirectoryPath);
                    }
                );
            }
        }

        public ICommand SetDefaultCacheDirectoryCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        CacheDirectoryPath = string.Empty;
                    }
                );
            }
        }

        #endregion

        #region function

        string SelectDirectory(string initialDirectoryPath)
        {
            using(var dialog = new FolderBrowserDialog()) {
                dialog.SelectedPath = initialDirectoryPath;
                if(dialog.ShowDialog().GetValueOrDefault()) {
                    return dialog.SelectedPath;
                }
            }

            return initialDirectoryPath;
        }

        #endregion

        #region ManagerViewModelBase

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override void InitializeView(MainWindow view)
        { }
        public override void UninitializeView(MainWindow view)
        { }

        public override Task GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan)
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
