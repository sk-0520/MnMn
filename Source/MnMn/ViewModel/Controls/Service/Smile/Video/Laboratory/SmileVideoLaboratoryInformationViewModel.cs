using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Laboratory
{
    public class SmileVideoLaboratoryInformationViewModel: SmileVideoInformationViewModel
    {
        public SmileVideoLaboratoryInformationViewModel(Mediation mediation)
            :base(mediation, 0, Define.Service.Smile.Video.SmileVideoInformationFlags.None)
        {

        }

        #region SmileVideoInformationViewModel

        public override string VideoId { get; } = Constants.ApplicationName;
        public override string Title { get; } = Constants.ApplicationName + ":" + Constants.BuildTypeInformation;
        public override Uri WatchUrl { get; } = new Uri(Constants.AppUriAbout);
        public override string UserName { get; } = nameof(UserName);
        public override string UserId { get; } = nameof(UserId);
        public override string ChannelId { get; } = nameof(ChannelId);
        public override string ChannelName { get; } = nameof(ChannelName);
        public override bool IsChannelVideo { get; } = false;
        public override DateTime FirstRetrieve { get; } = DateTime.Now;
        public override int ViewCounter { get; } = -1;
        public override int CommentCounter { get; } = -1;
        public override int MylistCounter { get; } = -1;
        public override SmileVideoMovieType MovieType { get; } = SmileVideoMovieType.Unknown;
        public override TimeSpan Length { get; } = TimeSpan.Zero;

        public override DirectoryInfo CacheDirectory { get { return new DirectoryInfo("NUL"); } }

        protected override FileInfo IndividualVideoSettingFile { get { return new FileInfo("NUL"); } }

        public override CollectionModel<SmileVideoTagViewModel> TagList { get; } = new CollectionModel<SmileVideoTagViewModel>();

        public override bool SaveSetting(bool force)
        {
            return false;
        }

        protected override Task<bool> LoadInformationCoreAsync(CacheSpan cacheSpan, HttpClient client)
        {
            return Task.FromResult(true);
        }

        public override SmileVideoCommentFilteringSettingModel Filtering { get; } = new SmileVideoCommentFilteringSettingModel();


        protected override Task<bool> LoadThumbnaiImageCoreAsync(CacheSpan cacheSpan, HttpClient client)
        {
            ThumbnailLoadState = LoadState.Preparation;

            return Task.Run(() => {
                var resizeWidth = 130;
                var resizeHeight = 100;

                var left = (int)Screen.PrimaryScreen.DeviceBounds.X;
                var top = (int)Screen.PrimaryScreen.DeviceBounds.Y;
                var width = (int)Screen.PrimaryScreen.DeviceBounds.Width;
                var height = (int)Screen.PrimaryScreen.DeviceBounds.Height;

                Application.Current.Dispatcher.Invoke(() => {
                    ThumbnailLoadState = LoadState.Loading;

                    using(var screenBmp = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb)) {
                        using(var bmpGraphics = System.Drawing.Graphics.FromImage(screenBmp)) {
                            bmpGraphics.CopyFromScreen(left, top, 0, 0, new System.Drawing.Size(width, height));
                        }

                        using(var resizeBmp = new System.Drawing.Bitmap(resizeWidth, resizeHeight)) {
                            using(var bmpGraphics = System.Drawing.Graphics.FromImage(resizeBmp)) {
                                bmpGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                bmpGraphics.DrawImage(screenBmp, 0, 0, resizeWidth, resizeHeight);
                            }

                            var image = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                                resizeBmp.GetHbitmap(),
                                IntPtr.Zero,
                                Int32Rect.Empty,
                                BitmapSizeOptions.FromEmptyOptions()
                            );
                            FreezableUtility.SafeFreeze(image);

                            SetThumbnaiImage(image);
                        }
                    }
                });

                return true;
            });
        }

        public override void LoadLocalPageHtmlAsync()
        {
            var html = "test";
            SetPageHtmlAsync(html, false);
        }

        protected override void SetPageHtmlAsync(string html, bool isSave)
        {
            PageHtmlLoadState = LoadState.Loaded;
        }


        #endregion
    }
}
