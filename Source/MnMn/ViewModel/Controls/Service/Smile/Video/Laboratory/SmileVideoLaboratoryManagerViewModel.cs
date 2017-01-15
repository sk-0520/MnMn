using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms;
using ContentTypeTextNet.Library.SharedLibrary.Data;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Laboratory;
using ContentTypeTextNet.MnMn.MnMn.Define.Laboratory.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Wrapper;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Laboratory;
using Microsoft.Win32;
using SharpAvi.Output;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Local
{
    public class SmileVideoLaboratoryManagerViewModel: SmileVideoCustomManagerViewModelBase
    {
        public SmileVideoLaboratoryManagerViewModel(Mediation mediation)
            : base(mediation)
        {
            PlayDragAndDrop = new DelegateDragAndDrop() {
                DragEnterAction = PlayDragEnterAndOver,
                DragOverAction = PlayDragEnterAndOver,
                DropAction = PlayDragDop,
            };
        }

        #region property

        public string PlayInputVideoSourceFilePath
        {
            get { return Setting.Laboratory.PlayInputVideoSourceFilePath; }
            set { SetPropertyValue(Setting.Laboratory, value, nameof(Setting.Laboratory.PlayInputVideoSourceFilePath)); }
        }
        public string PlayInputMessageSourceFilePath
        {
            get { return Setting.Laboratory.PlayInputMessageMessageFilePath; }
            set { SetPropertyValue(Setting.Laboratory, value, nameof(Setting.Laboratory.PlayInputMessageMessageFilePath)); }
        }

        public IDragAndDrop PlayDragAndDrop { get; }

        public TimeSpan DummyLength
        {
            get { return Setting.Laboratory.DummyLength; }
            set { SetPropertyValue(Setting.Laboratory, value, nameof(Setting.Laboratory.DummyLength)); }
        }
        public string DummyOutputDirectoryPath
        {
            get { return Setting.Laboratory.DummyOutputDirectoryPath; }
            set { SetPropertyValue(Setting.Laboratory, value, nameof(Setting.Laboratory.DummyOutputDirectoryPath)); }
        }
        public bool DummyOutputVideo
        {
            get { return Setting.Laboratory.DummyOutputVideo; }
            set { SetPropertyValue(Setting.Laboratory, value, nameof(Setting.Laboratory.DummyOutputVideo)); }
        }
        public bool DummyOutputComment
        {
            get { return Setting.Laboratory.DummyOutputComment; }
            set { SetPropertyValue(Setting.Laboratory, value, nameof(Setting.Laboratory.DummyOutputComment)); }
        }

        public CommentCreateType DummyCommentCreateType
        {
            get { return Setting.Laboratory.DummyCommentCreateType; }
            set { SetPropertyValue(Setting.Laboratory, value, nameof(Setting.Laboratory.DummyCommentCreateType)); }
        }
        public int DummyCommentNormalCount
        {
            get { return Setting.Laboratory.DummyCommentNormalCount; }
            set { SetPropertyValue(Setting.Laboratory, value, nameof(Setting.Laboratory.DummyCommentNormalCount)); }
        }
        public int DummyCommentOriginalPostCount
        {
            get { return Setting.Laboratory.DummyCommentOriginalPostCount; }
            set { SetPropertyValue(Setting.Laboratory, value, nameof(Setting.Laboratory.DummyCommentOriginalPostCount)); }
        }

        public VideoCreateType DummyVideoCreateType
        {
            get { return Setting.Laboratory.DummyVideoCreateType; }
            set { SetPropertyValue(Setting.Laboratory, value, nameof(Setting.Laboratory.DummyVideoCreateType)); }
        }
        public decimal DummyVideoFramesPerSecond
        {
            get { return Setting.Laboratory.DummyVideoFramesPerSecond; }
            set { SetPropertyValue(Setting.Laboratory, value, nameof(Setting.Laboratory.DummyVideoFramesPerSecond)); }
        }
        public int DummyVideoWidth
        {
            get { return Setting.Laboratory.DummyVideoWidth; }
            set { SetPropertyValue(Setting.Laboratory, value, nameof(Setting.Laboratory.DummyVideoWidth)); }
        }
        public int DummyVideoHeight
        {
            get { return Setting.Laboratory.DummyVideoHeight; }
            set { SetPropertyValue(Setting.Laboratory, value, nameof(Setting.Laboratory.DummyVideoHeight)); }
        }

        public FewViewModel<bool> NowOutput { get; } = new FewViewModel<bool>();

        #endregion

        #region command

        public ICommand OpenPlayInputVideoSourceFilePathCommand
        {
            get
            {
                return CreateCommand(o => {
                    var filters = new DialogFilterList() {
                        new DialogFilterItem("video", CreatePatterns(Constants.AppSmileVideoLaboratoryPlayVideoExtensions)),
                    };
                    PlayInputVideoSourceFilePath = OpenDialogInputPath(PlayInputVideoSourceFilePath, filters);
                });
            }
        }

        public ICommand OpenPlayInputMessageSourceFilePathCommand
        {
            get
            {
                return CreateCommand(o => {
                    var filters = new DialogFilterList() {
                        new DialogFilterItem("msg", CreatePatterns(Constants.AppSmileVideoLaboratoryPlayMsgExtensions)),
                    };
                    PlayInputMessageSourceFilePath = OpenDialogInputPath(PlayInputMessageSourceFilePath, filters);
                });
            }
        }

        public ICommand PlayInputFileCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var videoFilePath = PlayInputVideoSourceFilePath;
                        var msgFilePath = PlayInputMessageSourceFilePath;

                        PlayInputFileAsync(videoFilePath, msgFilePath);
                    },
                    o => {
                        return
                            !string.IsNullOrWhiteSpace(PlayInputVideoSourceFilePath)
                            ||
                            !string.IsNullOrWhiteSpace(PlayInputMessageSourceFilePath)
                        ;
                    }
                );
            }
        }

        public ICommand ExportDummyFileCommand
        {
            get
            {
                return base.CreateCommand(
                    o => {
                        this.NowOutput.Value = true;
                        var outputDirectoryPath = Environment.ExpandEnvironmentVariables(DummyOutputDirectoryPath);
                        ExportDummyFileAsync(outputDirectoryPath).ContinueWith((Task<bool> t) => {
                            this.NowOutput.Value = false;
                            if(!t.IsFaulted && t.Result && Directory.Exists(outputDirectoryPath)) {
                                try {
                                    Process.Start(outputDirectoryPath);
                                } catch(Exception ex) {
                                    Mediation.Logger.Error(ex);
                                }
                            }
                        });
                    },
                    o => {
                        if(string.IsNullOrEmpty(DummyOutputDirectoryPath) || DummyOutputDirectoryPath.Length <= @"X:".Length) {
                            return false;
                        }
                        return DummyOutputComment || DummyOutputVideo;
                    }
                );
            }
        }

        public ICommand SelectDummyOutputDirectoryPathCommand
        {
            get
            {
                return CreateCommand(o => {
                    DummyOutputDirectoryPath = SelectDirectoryPath(DummyOutputDirectoryPath);
                });
            }
        }
        #endregion

        #region function

        IEnumerable<string> CreatePatterns(IEnumerable<string> exts)
        {
            return exts.Select(ext => "*" + ext);
        }

        string OpenDialogInputPath(string path, DialogFilterList filters)
        {
            var existFile = File.Exists(path);

            var dialog = new OpenFileDialog() {
                FileName = existFile ? path : string.Empty,
                InitialDirectory = existFile ? Path.GetDirectoryName(path) : Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Filter = filters.FilterText,
                CheckFileExists = true,
            };
            if(dialog.ShowDialog().GetValueOrDefault()) {
                return dialog.FileName;
            }

            return path;
        }

        string SelectDirectoryPath(string initialDirectoryPath)
        {
            using(var dialog = new FolderBrowserDialog()) {
                dialog.SelectedPath = initialDirectoryPath;
                if(dialog.ShowDialog().GetValueOrDefault()) {
                    return dialog.SelectedPath;
                }
            }

            return initialDirectoryPath;
        }


        Task PlayInputFileAsync(string videoFilePath, string commentFilePath)
        {
            var videoFile = new FileInfo(videoFilePath);
            var commentFile = new FileInfo(commentFilePath);

            var vm = new SmileVideoLaboratoryPlayerViewModel(Mediation);
            var task = vm.LoadAsync(videoFile, commentFile);
            Mediation.Request(new ShowViewRequestModel(RequestKind.ShowView, ServiceType.SmileVideo, vm, ShowViewState.Foreground));

            return Task.CompletedTask;
        }

        bool HasExtension(string path, IEnumerable<string> extensions)
        {
            var dotExtension = Path.GetExtension(path);
            if(dotExtension.Length <= ".".Length) {
                return false;
            }

            var ext = dotExtension.Substring(1);

            return extensions.Any(s => string.Equals(s, ext, StringComparison.OrdinalIgnoreCase));
        }

        bool IsEnabledVideoFilePath(string path)
        {
            return HasExtension(path, Constants.AppSmileVideoLaboratoryPlayVideoExtensions);
        }

        bool IsEnabledMsgFilePath(string path)
        {
            return HasExtension(path, Constants.AppSmileVideoLaboratoryPlayMsgExtensions);
        }

        void PlayDragEnterAndOver(UIElement sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;

            if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
                var filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                if(filePaths.Length == 1) {
                    var filePath = filePaths[0];
                    if(Directory.Exists(filePath)) {
                        e.Effects = DragDropEffects.Move;
                    } else {
                        if(IsEnabledVideoFilePath(filePath) || IsEnabledMsgFilePath(filePath)) {
                            e.Effects = DragDropEffects.Move;
                        }
                    }
                } else if(filePaths.Length == 2) {
                    if(!filePaths.All(s => Directory.Exists(s))) {
                        var f1 = filePaths[0];
                        var f2 = filePaths[1];

                        var f1v = IsEnabledVideoFilePath(f1);
                        var f1m = IsEnabledMsgFilePath(f1);
                        var f2v = IsEnabledVideoFilePath(f2);
                        var f2m = IsEnabledMsgFilePath(f2);

                        if((f1v && f2m) || (f2v && f1m)) {
                            e.Effects = DragDropEffects.Move;
                        }
                    }
                }
            }

            Debug.WriteLine(e.Effects);
        }

        void PlayDragDop(UIElement sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;

            var filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
            ApplyPlayFilePaths(filePaths);
        }

        void ApplyPlayFilePaths(string[] filePaths)
        {
            if(filePaths.Length == 1) {
                var filePath = filePaths[0];
                if(Directory.Exists(filePath)) {
                    var files = Directory.EnumerateFiles(filePath)
                        .Where(f => IsEnabledVideoFilePath(f) || IsEnabledMsgFilePath(f))
                        .ToArray()
                    ;

                    var video = files.FirstOrDefault(f => IsEnabledVideoFilePath(f));

                    var msg = files
                        .Where(f => f != video)
                        .FirstOrDefault(f => f.EndsWith(Constants.AppSmileVideoLaboratoryPlayMsgExtensions.First(), StringComparison.OrdinalIgnoreCase))
                    ;
                    if(msg == null) {
                        msg = files.FirstOrDefault(f => IsEnabledMsgFilePath(f));
                    }

                    if(!string.IsNullOrWhiteSpace(video)) {
                        PlayInputVideoSourceFilePath = video;
                    }
                    if(!string.IsNullOrWhiteSpace(msg)) {
                        PlayInputMessageSourceFilePath = msg;
                    }
                } else {
                    if(IsEnabledVideoFilePath(filePath)) {
                        PlayInputVideoSourceFilePath = filePath;
                    } else if(IsEnabledMsgFilePath(filePath)) {
                        PlayInputMessageSourceFilePath = filePath;
                    }
                }
            } else if(filePaths.Length == 2) {
                var f1 = filePaths[0];
                var f2 = filePaths[1];

                var f1v = IsEnabledVideoFilePath(f1);
                var f1m = IsEnabledMsgFilePath(f1);
                var f2v = IsEnabledVideoFilePath(f2);
                var f2m = IsEnabledMsgFilePath(f2);

                if(f1v && f2m) {
                    PlayInputVideoSourceFilePath = f1;
                    PlayInputMessageSourceFilePath = f2;
                } else if(f2v && f1m) {
                    PlayInputVideoSourceFilePath = f2;
                    PlayInputMessageSourceFilePath = f1;
                }
            }
        }

        RawSmileVideoMsgChatModel CreateDummyChat(int number, int length, CommentCreateType type, bool isOriginalPost, Random random)
        {
            var contentMap = new Dictionary<CommentCreateType, Func<string>>() {
                [CommentCreateType.Sequence] = () => $"{(isOriginalPost ? "o" : "n")}:{number}",
                [CommentCreateType.Random] = () => $"{(isOriginalPost ? "o" : "n")}:{number}:{random.Next().ToString()}",
            };
            var userId = number / 10 * length;
            var result = new RawSmileVideoMsgChatModel() {
                Fork = isOriginalPost ? "1" : "0",
                Content = contentMap[type](),
                No = number.ToString(),
                UserId = userId.ToString(),
                Premium = (int.Parse(userId.ToString().Substring(0, 1)) % 2 == 0) ? "1" : "0",
                VPos = SmileVideoMsgUtility.ConvertRawElapsedTime(TimeSpan.FromSeconds(number)),
                Date = RawValueUtility.ConvertRawUnixTime(DateTime.Now.AddMinutes(number)).ToString(),
            };

            return result;
        }

        void ExportDummyMsgFile(DirectoryInfo outputDirectory, TimeSpan length, CommentCreateType commentType, int commentNormalLength, int commentOpLength)
        {
            var rawMessagePacket = new RawSmileVideoMsgPacketModel();

            var random = new Random();

            var normalChats = Enumerable
                .Range(1, commentNormalLength)
                .Select(i => CreateDummyChat(i, commentNormalLength, commentType, false, random))
            ;
            var opChats = Enumerable
                .Range(1, commentOpLength)
                .Select(i => CreateDummyChat(i, commentOpLength, commentType, true, random))
            ;

            rawMessagePacket.Chat.AddRange(normalChats);
            rawMessagePacket.Chat.AddRange(opChats);

            var outputFilePath = Path.Combine(outputDirectory.FullName, $"comment{Constants.AppSmileVideoLaboratoryPlayMsgExtensions.First()}");
            SerializeUtility.SaveXmlSerializeToFile(outputFilePath, rawMessagePacket);
        }

        Task<bool> ExportDummyVideoFileAsync(DirectoryInfo outputDirectory, TimeSpan length, VideoCreateType videoTye, decimal videFps, int videoWidth, int videoHeight)
        {
            var temporaryFilePath = Path.Combine(outputDirectory.FullName, $"video.avi");
            var outputFilePath = Path.ChangeExtension(temporaryFilePath, "mp4");

            return Task.Run(() => {
                using(var writer = new AviWriter(temporaryFilePath) {
                    FramesPerSecond = videFps,
                    EmitIndex1 = true,
                }) {
                    var videoStream = writer.AddVideoStream(videoWidth, videoHeight);
                    videoStream.BitsPerPixel = SharpAvi.BitsPerPixel.Bpp32;
                    videoStream.Codec = SharpAvi.KnownFourCCs.Codecs.Uncompressed;

                    var binaryLength = videoStream.Width * videoStream.Height * 4;
                    using(var buffer = GlobalManager.MemoryStream.GetStreamWidthAutoTag(binaryLength)) {
                        foreach(var i in Enumerable.Range(0, binaryLength)) {
                            buffer.WriteByte(0);
                        }
                        var binaryBuffer = buffer.GetBuffer();

                        using(var bitmap = new System.Drawing.Bitmap(videoStream.Width, videoStream.Height))
                        using(var canvas = System.Drawing.Graphics.FromImage(bitmap))
                        using(var format = new System.Drawing.StringFormat() {
                            Alignment = System.Drawing.StringAlignment.Center,
                            LineAlignment = System.Drawing.StringAlignment.Center,
                        }) {
                            canvas.ScaleTransform(1.0F, -1.0F);
                            canvas.TranslateTransform(0.0F, -videoHeight);
                            var rect = new System.Drawing.RectangleF(0, 0, videoStream.Width, videoStream.Height);
                            var lastFrame = (int)(length.TotalSeconds * (double)videFps);
                            foreach(var frame in Enumerable.Range(0, lastFrame)) {
                                canvas.FillRectangle(System.Drawing.Brushes.Black, rect);
                                canvas.DrawString($"{frame} / {lastFrame}", System.Drawing.SystemFonts.MessageBoxFont, System.Drawing.Brushes.White, rect, format);

                                var bits = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, videoStream.Width, videoStream.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                                System.Runtime.InteropServices.Marshal.Copy(bits.Scan0, binaryBuffer, 0, binaryLength);
                                bitmap.UnlockBits(bits);

                                videoStream.WriteFrame(true, binaryBuffer, 0, binaryLength);
                            }
                        }
                    }
                }
            }).ContinueWith(_ => {
                if(File.Exists(outputFilePath)) {
                    File.Delete(outputFilePath);
                }
                var ffmpeg = new Ffmpeg();
                var s = $"-y -i \"{temporaryFilePath}\" \"{outputFilePath}\"";
                return ffmpeg.ExecuteAsync(s);
            }, TaskContinuationOptions.NotOnFaulted).Unwrap().ContinueWith(t => {
                var exitCode = t.Result;
                Mediation.Logger.Information($"result: {exitCode}");
                if(exitCode == 0) {
                    if(File.Exists(temporaryFilePath)) {
                        File.Delete(temporaryFilePath);
                    }
                    return true;
                }

                return false;
            }, TaskContinuationOptions.NotOnFaulted);
        }


        Task<bool> ExportDummyFileAsync(string outputDirectoryPath)
        {
            var commentOutput = DummyOutputComment;
            var videoOutput = DummyOutputVideo;

            if(!commentOutput && !videoOutput) {
                return Task.FromResult(false);
            }

            var length = DummyLength;

            if(length < TimeSpan.FromSeconds(1)) {
                return Task.FromResult(false);
            }

            var outputDirectory = new DirectoryInfo(outputDirectoryPath);
            outputDirectory.Refresh();
            if(!outputDirectory.Exists) {
                outputDirectory.Create();
            }

            var commentType = DummyCommentCreateType;
            var commentNormalCount = DummyCommentNormalCount;
            var commentOriginalPost = DummyCommentOriginalPostCount;

            if(commentOutput && 0 < commentNormalCount && 0 < commentOriginalPost) {
                ExportDummyMsgFile(outputDirectory, length, commentType, commentNormalCount, commentOriginalPost);
            }

            var videoTye = DummyVideoCreateType;
            var videFps = DummyVideoFramesPerSecond;
            var videoWidth = DummyVideoWidth;
            var videoHeight = DummyVideoHeight;
            if(videoOutput) {
                return ExportDummyVideoFileAsync(outputDirectory, length, videoTye, videFps, videoWidth, videoHeight);
            } else {
                return Task.FromResult(true);
            }
        }

        #endregion

        #region SmileVideoCustomManagerViewModelBase

        public override Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan, bool force)
        {
            return GarbageCollectionDummyResult;
        }

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override Task UninitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override void InitializeView(MainWindow view)
        { }

        public override void UninitializeView(MainWindow view)
        { }

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return Enumerable.Empty<ManagerViewModelBase>();
        }

        protected override void HideViewCore()
        { }

        protected override void ShowViewCore()
        { }

        #endregion
    }
}
