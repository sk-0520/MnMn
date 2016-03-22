﻿/*
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
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.User
{
    /// <summary>
    /// ユーザー情報表示用VM。
    /// </summary>
    public class SmileUserInformationViewModel: ViewModelBase
    {
        #region variable

        SourceLoadState _userLoadState;
        LoadState _userInformationLoadState;
        LoadState _userThumbnailLoadState;

        BitmapSource _thumbnailImage;
        SmileVideoFinderViewModelBase _selectedMyListFinder;

        #endregion

        public SmileUserInformationViewModel(Mediation mediation, string userId, bool isMyAccount)
        {
            Mediation = mediation;
            UserId = userId;
            IsMyAccount = isMyAccount;

            var dirInfo = Mediation.GetResultFromRequest<DirectoryInfo>(new RequestModel(RequestKind.CacheDirectory, ServiceType.Smile));
            var cachedDirPath = Path.Combine(dirInfo.FullName, Constants.SmileUserCacheVideosDirectoryName, UserId);
            if(Directory.Exists(cachedDirPath)) {
                CacheDirectory = new DirectoryInfo(cachedDirPath);
            } else {
                CacheDirectory = Directory.CreateDirectory(cachedDirPath);
            }

        }


        #region property

        Mediation Mediation { get; set; }
        SmileUserInformationModel UserInformation { get; set; }
        public SmileVideoFinderViewModelBase SelectedMyListFinder
        {
            get { return this._selectedMyListFinder; }
            set { if(SetVariableValue(ref this._selectedMyListFinder, value)) {
                    if(this._selectedMyListFinder != null) {
                        if(this._selectedMyListFinder.FinderLoadState == SmileVideoFinderLoadState.None) {
                            this._selectedMyListFinder.LoadDefaultCacheAsync().ConfigureAwait(false);
                        }
                    }
                }
            }
        }

        public SmileVideoFinderViewModelBase PostFinder { get; private set; }

        public virtual bool IsMyAccount { get; }

        public ImageSource ThumbnailImage
        {
            get
            {
                switch(UserThumbnailLoadState) {
                    case LoadState.None:
                        return null;

                    case LoadState.Preparation:
                        return null;

                    case LoadState.Loading:
                        return null;

                    case LoadState.Loaded:
                        return this._thumbnailImage;

                    case LoadState.Failure:
                        return null;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public LoadState UserInformationLoadState
        {
            get { return this._userInformationLoadState; }
            set { if(SetVariableValue(ref this._userInformationLoadState, value)) {
                    var propertyNames = new[] {
                        nameof(UserName),
                        nameof(IsPublicLocation),
                        nameof(Location),
                        nameof(IsPublicGender),
                        nameof(Gender),
                        nameof(IsPublicBirthday),
                        nameof(Birthday),
                        nameof(ResistedVersion),
                        nameof(IsPremium),
                        nameof(Description),
                        nameof(IsPublicMyList),
                        nameof(IsPublicPost),
                        nameof(IsPublicReport),
                    };
                    CallOnPropertyChange(propertyNames);
                }
            }
        }

        public LoadState UserThumbnailLoadState
        {
            get { return this._userThumbnailLoadState; }
            set
            {
                if(SetVariableValue(ref this._userThumbnailLoadState, value)) {
                    CallOnPropertyChange(nameof(ThumbnailImage));
                }
            }
        }

        public DirectoryInfo CacheDirectory { get; private set; }

        public string UserId { get; }
        public string UserName { get
            {
                if(UserInformationLoadState == LoadState.Loaded) {
                    return UserInformation.UserName;
                }

                return string.Empty;
            }
        }
        public bool IsPublicLocation
        {
            get
            {
                if(UserInformationLoadState == LoadState.Loaded) {
                    return UserInformation.IsPublicLocation;
                }

                return false;
            }
        }
        public string Location
        {
            get
            {
                if(UserInformationLoadState == LoadState.Loaded) {
                    return UserInformation.Location;
                }

                return string.Empty;
            }
        }
        public bool IsPublicGender
        {
            get
            {
                if(UserInformationLoadState == LoadState.Loaded) {
                    return UserInformation.IsPublicGender ;
                }

                return false;
            }
        }
        public Gender Gender
        {
            get
            {
                if(UserInformationLoadState == LoadState.Loaded) {
                    return UserInformation.Gender;
                }

                return Gender.Unknown;
            }
        }
        public bool IsPublicBirthday
        {
            get
            {
                if(UserInformationLoadState == LoadState.Loaded) {
                    return UserInformation.IsPublicBirthday;
                }

                return false;
            }
        }
        public DateTime Birthday
        {
            get
            {
                if(UserInformationLoadState == LoadState.Loaded) {
                    return UserInformation.Birthday;
                }

                return RawValueUtility.UnknownDateTime;
            }
        }
        public string ResistedVersion
        {
            get
            {
                if(UserInformationLoadState == LoadState.Loaded) {
                    return UserInformation.ResistedVersion;
                }

                return string.Empty;
            }
        }
        public bool IsPremium
        {
            get
            {
                if(UserInformationLoadState == LoadState.Loaded) {
                    return UserInformation.IsPremium;
                }

                return false;
            }
        }
        public string Description
        {
            get
            {
                if(UserInformationLoadState == LoadState.Loaded) {
                    return UserInformation.Description;
                }

                return string.Empty;
            }
        }
        public bool IsPublicMyList
        {
            get
            {
                if(UserInformationLoadState == LoadState.Loaded) {
                    return UserInformation.IsPublicMyList;
                }

                return false;
            }
        }
        public bool IsPublicPost
        {
            get
            {
                if(UserInformationLoadState == LoadState.Loaded) {
                    return UserInformation.IsPublicPost;
                }

                return false;
            }
        }
        public bool IsPublicReport
        {
            get
            {
                if(UserInformationLoadState == LoadState.Loaded) {
                    return UserInformation.IsPublicReport;
                }

                return false;
            }
        }

        public SourceLoadState UserLoadState
        {
            get { return this._userLoadState; }
            set
            {
                SetVariableValue(ref this._userLoadState, value);
            }
        }

        public CollectionModel<SmileMyListFinderViewModel> MyListItems { get; } = new CollectionModel<SmileMyListFinderViewModel>();

        #endregion

        #region function

        //ImageSource GetImage(Stream stream)
        //{
        //    Application.Current.Dispatcher.Invoke(new Action(() => {
        //        var image = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
        //        FreezableUtility.SafeFreeze(image);
        //        this._thumbnailImage = image;
        //    }));
        //    return this._thumbnailImage;
        //}

        //Task LoadThumbnaiImageAsyncCore(string savePath, HttpClient client)
        //{
        //    var uri = UserInformation.ThumbnailUri;

        //    return RestrictUtility.Block(async () => {
        //        var maxCount = 3;
        //        var count = 0;
        //        do {
        //            try {
        //                Mediation.Logger.Trace($"img -> {uri}");
        //                return await client.GetByteArrayAsync(uri);
        //            } catch(HttpRequestException ex) {
        //                Mediation.Logger.Error($"error img -> {uri}");
        //                Mediation.Logger.Warning(ex);
        //                if(count != 0) {
        //                    var wait = TimeSpan.FromSeconds(1);
        //                    Thread.Sleep(wait);
        //                }
        //            }
        //        } while(count++ < maxCount);
        //        return null;
        //    }).ContinueWith(task => {
        //        var binary = task.Result;

        //        if(binary != null) {
        //            using(var stream = new MemoryStream(binary)) {
        //                GetImage(stream);
        //                UserThumbnailLoadState = LoadState.Loaded;
        //            }
        //            if(this._thumbnailImage != null && Application.Current != null) {
        //                // キャッシュ構築
        //                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
        //                    var frame = BitmapFrame.Create(this._thumbnailImage);
        //                    var encoder = new PngBitmapEncoder();
        //                    encoder.Frames.Add(frame);
        //                    FileUtility.MakeFileParentDirectory(savePath);
        //                    using(var stream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.Read)) {
        //                        encoder.Save(stream);
        //                    }
        //                }));
        //            }
        //        } else {
        //            UserThumbnailLoadState = LoadState.Failure;
        //        }
        //    });
        //}

        Task LoadThumbnaiImageAsync(CacheSpan userImageCacheSpan)
        {
            //UserThumbnailLoadState = LoadState.Preparation;
            //var imagePath = Path.Combine(CacheDirectory.FullName, UserId + ".png");
            //var imageFile = new FileInfo(imagePath);
            //if(imageFile.Exists && Constants.MinimumPngFileSize <= imageFile.Length &&  userImageCacheSpan.IsCacheTime(DateTime.Now)) {
            //    UserThumbnailLoadState = LoadState.Loading;

            //    using(var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
            //        GetImage(stream);
            //        UserThumbnailLoadState = LoadState.Loaded;
            //        return Task.CompletedTask;
            //    }
            //}

            //UserThumbnailLoadState = LoadState.Loading;
            //var client = new HttpClient();
            //return LoadThumbnaiImageAsyncCore(imagePath, client).ContinueWith(_ => {
            //    client.Dispose();
            //});

            UserThumbnailLoadState = LoadState.Preparation;
            var imagePath = Path.Combine(CacheDirectory.FullName, FileNameUtility.CreateFileName(UserId, "png"));
            if(CacheImageUtility.ExistImage(imagePath, userImageCacheSpan)) {
                UserThumbnailLoadState = LoadState.Loading;
                this._thumbnailImage = CacheImageUtility.LoadBitmapBinary(imagePath);
                UserThumbnailLoadState = LoadState.Loaded;
                return Task.CompletedTask;
            }

            UserThumbnailLoadState = LoadState.Loading;
            var client = new HttpClient();
            return CacheImageUtility.LoadBitmapBinaryDefaultAsync(client, UserInformation.ThumbnailUri, Mediation.Logger).ContinueWith(task => {
                client.Dispose();
                var image = task.Result;
                if(image != null) {
                    this._thumbnailImage = image;
                    UserThumbnailLoadState = LoadState.Loaded;
                    CacheImageUtility.SaveBitmapSourceToPngAsync(image, imagePath, Mediation.Logger);
                } else {
                    UserThumbnailLoadState = LoadState.Failure;
                }
            });
        }

        public Task LoadAsync(CacheSpan userDataCacheSpan, CacheSpan userImageCacheSpan)
        {
            var cancel = new CancellationTokenSource();

            UserLoadState = SourceLoadState.SourceLoading;
            var user = new Logic.Service.Smile.HalfBakedApi.User(Mediation);
            return user.LoadUserInformationAsync(UserId).ContinueWith(task => {
                UserLoadState = SourceLoadState.SourceChecking;
                UserInformation = task.Result;
                UserInformationLoadState = LoadState.Loaded;

            }).ContinueWith(task => {
                LoadThumbnaiImageAsync(userImageCacheSpan);
            }, cancel.Token, TaskContinuationOptions.AttachedToParent, TaskScheduler.Current).ContinueWith(_ => {
                var mylistTask = Task.CompletedTask;
                if(UserInformation.IsPublicMyList) {
                    user.LoadUserMyListAsync(UserId).ContinueWith(task => {
                        var userMyList = task.Result;
                        if(userMyList != null && userMyList.Groups.Any()) {
                            var items = userMyList.Groups.Select(g => new SmileMyListFinderViewModel(Mediation, g));
                            MyListItems.InitializeRange(items);
                        }
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }

                var postTask = Task.CompletedTask;
                if(UserInformation.IsPublicPost) {
                    PostFinder = new SmileUserPostFinderViewModel(Mediation, UserId);
                    CallOnPropertyChange(nameof(PostFinder));

                    postTask = PostFinder.LoadDefaultCacheAsync();
                }

                return Task.WhenAll(mylistTask, postTask);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public Task LoadDefaultAsync()
        {
            return LoadAsync(Constants.ServiceSmileUserDataCacheSpan, Constants.ServiceSmileUserImageCacheSpan);
        }

        #endregion
    }
}
