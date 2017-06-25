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
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ReadOnly;
using ContentTypeTextNet.MnMn.Library.Bridging.Model;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.History
{
    public abstract class SmileVideoHistoryFinderViewModelBase: SmileVideoFeedFinderViewModelBase
    {
        public SmileVideoHistoryFinderViewModelBase(Mediation mediation, string key)
            : base(mediation, 0)
        {
            Key = key;
            var titleMap = new StringsModel() {
                { SmileVideoMediationKey.historyPage, global::ContentTypeTextNet.MnMn.MnMn.Properties.Resources.String_Service_Smile_SmileVideo_History_AccountHistory_Title },
                { SmileVideoMediationKey.historyApp, global::ContentTypeTextNet.MnMn.MnMn.Properties.Resources.String_Service_Smile_SmileVideo_History_ApplicationHistory_Title },
            };
            Title = titleMap[Key];
        }

        #region property

        string Key { get; }
        public string Title { get; }

        public virtual bool NeedSession { get { return false; } }

        /// <summary>
        /// 削除後に再読み込みを行うか。
        /// </summary>
        protected abstract bool IsRemovedReload { get; }

        #endregion

        #region command

        public ICommand RemoveCheckedItemsCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        RemoveCheckedItemsAsync().ContinueWith(task => {
                            if(IsRemovedReload) {
                                if(task.Result.IsSuccess) {
                                    System.Threading.Thread.Sleep(Constants.ServiceSmileVideoHistoryReloadWaitTime);
                                }
                                return task.Result;
                            }

                            return CheckModel.Failure();
                        }).ContinueWith(task => {
                            if(IsRemovedReload) {
                                if(task.Result.IsSuccess) {
                                    return LoadDefaultCacheAsync();
                                }
                            }
                            return Task.CompletedTask;
                        }, TaskScheduler.FromCurrentSynchronizationContext()).ConfigureAwait(false);
                    }
                );
            }
        }

        #endregion

        #region function

        protected abstract Task<IReadOnlyCheck> RemoveCheckedItemsAsync();

        #endregion

        #region SmileVideoFeedFinderViewModelBase

        public override bool IsEnabledCheckItLaterMenu { get; } = false;

        #endregion
    }
}
