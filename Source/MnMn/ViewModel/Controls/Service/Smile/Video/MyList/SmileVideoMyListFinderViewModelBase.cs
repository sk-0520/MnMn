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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api.V1;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Feed.Rss2;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.MyList
{
    public abstract class SmileVideoMyListFinderViewModelBase: SmileVideoFeedFinderViewModelBase
    {
        public SmileVideoMyListFinderViewModelBase(Mediation mediation, bool isAccountMyList)
            : base(mediation)
        {
            Session = Mediation.GetResultFromRequest<SmileSessionViewModel>(new RequestModel(RequestKind.Session, ServiceType.Smile));

            IsAccountMyList = isAccountMyList;

            InitializeCacheData();
        }

        #region property

        protected SmileSessionViewModel Session { get; }
        public abstract string MyListId { get; }
        public abstract string MyListName { get; }
        public virtual int MyListItemCount { get { return FinderItemList.Count; } }
        public bool IsAccountMyList { get; }

        protected DirectoryInfo CacheDirectory { get; private set; }
        protected FileInfo CacheFile { get; private set; }

        public virtual bool IsPublic { get; } = false;
        public virtual bool CanEdit { get; } = false;

        public virtual bool CanRemove { get; } = false;

        public virtual Color MyListFolderColor { get { return Constants.SettingServiceSmileVideoMyListFolderColor; } }
        public virtual bool HasMyListFolder { get; } = false;

        /// <summary>
        /// キャッシュを無視するか。
        /// </summary>
        public bool IgnoreCache { get; set; }
        protected bool IgnoreAddHistory { get; set; } = true;
        #endregion

        #region function

        protected void InitializeCacheData()
        {
            var dirInfo = Mediation.GetResultFromRequest<DirectoryInfo>(new RequestModel(RequestKind.CacheDirectory, ServiceType.Smile));
            var cachedDirPath = Path.Combine(dirInfo.FullName, Constants.SmileMyListCacheDirectoryName);
            if(Directory.Exists(cachedDirPath)) {
                CacheDirectory = new DirectoryInfo(cachedDirPath);
            } else {
                CacheDirectory = Directory.CreateDirectory(cachedDirPath);
            }
            CacheFile = new FileInfo(Path.Combine(CacheDirectory.FullName, PathUtility.CreateFileName(MyListId, "xml")));
        }

        protected FeedSmileVideoModel GetCache()
        {
            if(CacheImageUtility.ExistImage(CacheFile.FullName, Constants.ServiceSmileMyListCacheSpan)) {
                using(var stream = new FileStream(CacheFile.FullName, FileMode.Open, FileAccess.Read)) {
                    var cacheResult = SerializeUtility.LoadXmlSerializeFromStream<FeedSmileVideoModel>(stream);
                    return cacheResult;
                }
            }

            return null;
        }

        protected virtual void AddHistory(SmileMyListItemModel model)
        {
            //SmileMyListHistoryCount
            var setting = Mediation.GetResultFromRequest<SmileSettingModel>(new RequestModel(RequestKind.Setting, ServiceType.Smile));
            var existModel = setting.MyList.History.FirstOrDefault(m => m.MyListId == model.MyListId);
            if(existModel != null) {
                setting.MyList.History.Remove(existModel);
                model = existModel;
            }
            model.UpdateTimestamp = DateTime.Now;
            setting.MyList.History.Insert(0, model);
        }

        protected virtual Task<FeedSmileVideoModel> LoadFeedCoreAsync()
        {
            var mylist = new Logic.Service.Smile.Api.V1.MyList(Mediation);

            return mylist.LoadGroupAsync(MyListId);
        }

        #endregion

        #region SmileVideoFeedFinderViewModelBase

        protected override Task SetItemsAsync(IEnumerable<SmileVideoInformationViewModel> items)
        {
            return base.SetItemsAsync(items).ContinueWith(t => {
                CallOnPropertyChange(nameof(MyListItemCount));
            });
        }

        protected override SmileVideoInformationFlags InformationFlags => SmileVideoInformationFlags.Length;

        protected override Task<FeedSmileVideoModel> LoadFeedAsync()
        {
            if(!IgnoreCache) {
                var cacheResult = GetCache();
                if(cacheResult != null) {
                    if(!IgnoreAddHistory) {
                        AddHistory(new SmileMyListItemModel() { MyListId = this.MyListId, MyListName = this.MyListName });
                    }
                    return Task.Run(() => cacheResult);
                }
            }

            return LoadFeedCoreAsync().ContinueWith(t => {
                var result = t.Result;

                InitializeCacheData();
                SerializeUtility.SaveXmlSerializeToFile(CacheFile.FullName, result);
                if(!IgnoreAddHistory) {
                    AddHistory(new SmileMyListItemModel() { MyListId = this.MyListId, MyListName = this.MyListName });
                }

                return result;
            });
        }

        #endregion
    }
}
