using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.History
{
    public　abstract class SmileVideoHistoryFinderViewModelBase: SmileVideoFeedFinderViewModelBase
    {
        public SmileVideoHistoryFinderViewModelBase(Mediation mediation, string key)
            : base(mediation)
        {
            Key = key;
            var titleMap = new StringsModel() {
                { SmileVideoMediationKey.historyPage, global::ContentTypeTextNet.MnMn.MnMn.Properties.Resources.String_Service_Smile_SmileVideo_History_AccountHistory_Title },
            };
            Title = titleMap[Key];
        }

        #region property

        string Key { get; }
        public string Title { get; }

        #endregion

        #region command

        public ICommand RemoveCheckedItemsCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        RemoveCheckedItemsAsync().ContinueWith(task => {
                            if(task.Result.IsSuccess) {
                                // TODO: 即値
                                var sleepTime = TimeSpan.FromMilliseconds(500);
                                System.Threading.Thread.Sleep(sleepTime);
                            }
                            return task.Result;
                        }).ContinueWith(task => {
                            if(task.Result.IsSuccess) {
                                return LoadDefaultCacheAsync();
                            } else {
                                return Task.CompletedTask;
                            }
                        }, TaskScheduler.FromCurrentSynchronizationContext()).ConfigureAwait(false);
                    }
                );
            }
        }

        #endregion

        #region function

        protected abstract Task<CheckModel> RemoveCheckedItemsAsync();

        #endregion

        #region SmileVideoFeedFinderViewModelBase

        #endregion
    }
}
