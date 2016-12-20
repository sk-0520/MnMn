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
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video.Parameter;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter;
using ContentTypeTextNet.MnMn.MnMn.IF.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Parameter;
using System.IO;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.MyList;
using System.Windows;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile
{
    public static class SmileDescriptionUtility
    {
        public static Task<SmileVideoInformationViewModel> GetVideoInformationAsync(string videoId, ICommunication communicator)
        {
            var request = new SmileVideoInformationCacheRequestModel(new SmileVideoInformationCacheParameterModel(videoId, Constants.ServiceSmileVideoThumbCacheSpan));
            return communicator.GetResultFromRequest<Task<SmileVideoInformationViewModel>>(request).ContinueWith(t => {
                if(t.Status == TaskStatus.Faulted) {
                    return null;
                }

                return t.Result;
            });
        }

        static async Task MenuOpenVideoLinkInNewWindowCoreAsync(string videoId, ICommunication communicator)
        {
            var videoInformation = await GetVideoInformationAsync(videoId, communicator);
            if(videoInformation != null) {
                await videoInformation.OpenVideoDefaultAsync(false);
            }
        }

        public static Task MenuOpenVideoLinkInNewWindowAsync(object parameter, ICommunication communicator)
        {
            var videoId = (string)parameter;
            if(string.IsNullOrWhiteSpace(videoId)) {
                return Task.CompletedTask;
            }

            return MenuOpenVideoLinkInNewWindowCoreAsync(videoId, communicator);
        }

        public static void CopyVideoId(object parameter, ILogger logger)
        {
            var myListId = (string)parameter;
            DescriptionUtility.CopyText(myListId, logger);
        }

        /// <summary>
        ///<see cref="ISmileDescription.OpenMyListIdLinkCommand"/>
        /// <para>TODO: Videoにべったり依存。</para>
        /// </summary>
        /// <param name="communicator"></param>
        /// <param name="myListId"></param>
        static void OpenMyListIdCore(string myListId, ICommunication communicator)
        {
            var parameter = new SmileVideoSearchMyListParameterModel() {
                Query = myListId,
                QueryType = SmileVideoSearchMyListQueryType.MyListId,
            };

            communicator.Request(new ShowViewRequestModel(RequestKind.ShowView, ServiceType.SmileVideo, parameter, ShowViewState.Foreground));
        }

        /// <summary>
        ///<see cref="ISmileDescription.OpenMyListIdLinkCommand"/>
        /// <para>TODO: Videoにべったり依存。</para>
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="myListId"></param>
        public static void OpenMyListId(object parameter, ICommunication communicator)
        {
            OpenMyListIdCore((string)parameter, communicator);
        }

        static async Task AddMyListBookmarkCoreAsync(string myListId, Mediation mediation)
        {
            var dirInfo = mediation.GetResultFromRequest<DirectoryInfo>(new RequestModel(RequestKind.CacheDirectory, ServiceType.Smile));
            var cachedDirPath = Path.Combine(dirInfo.FullName, Constants.SmileMyListCacheDirectoryName);
            var cacheDirectory = RestrictUtility.Is(Directory.Exists(cachedDirPath), () => new DirectoryInfo(cachedDirPath), () => Directory.CreateDirectory(cachedDirPath));
            var cacheFile = new FileInfo(Path.Combine(cacheDirectory.FullName, PathUtility.CreateFileName(myListId, "xml")));

            var mylist = new Logic.Service.Smile.Api.V1.MyList(mediation);
            FeedSmileVideoModel group = null;
            if(CacheImageUtility.ExistImage(cacheFile.FullName, Constants.ServiceSmileMyListCacheSpan)) {
                using(var stream = new FileStream(cacheFile.FullName, FileMode.Open, FileAccess.Read)) {
                    group = SerializeUtility.LoadXmlSerializeFromStream<FeedSmileVideoModel>(stream);
                }
            }
            if(group == null) {
                group = await mylist.LoadGroupAsync(myListId);
            }
            if(group != null) {
                SerializeUtility.SaveXmlSerializeToFile(cacheFile.FullName, group);
                var finder = new SmileVideoMatchMyListFinderViewModel(mediation, group, myListId, false);
                await finder.LoadDefaultCacheAsync();
                mediation.Smile.VideoMediation.ManagerPack.MyListManager.AddBookmarkUserMyList(finder);
            }
        }

        public static Task AddMyListBookmarkAsync(object parameter, Mediation mediation)
        {
            var myListId = (string)parameter;
            return AddMyListBookmarkCoreAsync(myListId, mediation);
        }

        public static void CopyMyListId(object parameter, ILogger logger)
        {
            var myListId = (string)parameter;
            DescriptionUtility.CopyText(myListId, logger);
        }

        /// <summary>
        /// <see cref="ISmileDescription.OpenUserIdLinkCommand"/>
        /// </summary>
        /// <param name="communicator"></param>
        /// <param name="userId"></param>
        static void OpenUserIdCore(string userId, ICommunication communicator)
        {
            var parameter = new SmileOpenUserIdParameterModel() {
                UserId = userId,
            };

            communicator.Request(new ShowViewRequestModel(RequestKind.ShowView, ServiceType.Smile, parameter, ShowViewState.Foreground));
        }

        /// <summary>
        /// <see cref="ISmileDescription.OpenUserIdLinkCommand"/>
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="userId"></param>
        public static void OpenUserId(string parameter, ICommunication communicator)
        {
            OpenUserIdCore((string)parameter, communicator);
        }
    }
}
