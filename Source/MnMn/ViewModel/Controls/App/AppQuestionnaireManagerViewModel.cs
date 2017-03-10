using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.MnMn.MnMn.Data;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App
{
    public class AppQuestionnaireManagerViewModel: ManagerViewModelBase
    {
        #region variable

        QuestionnaireKind _questionnaireKind;

        string _kindOther;
        string _subject;
        string _message;

        #endregion

        public AppQuestionnaireManagerViewModel(Mediation mediation)
            : base(mediation)
        { }

        #region property

        WebNavigator QuestionnaireBrowser { get; set; }

        public QuestionnaireKind QuestionnaireKind
        {
            get { return this._questionnaireKind; }
            set { SetVariableValue(ref this._questionnaireKind, value); }
        }

        public string KindOther
        {
            get { return this._kindOther; }
            set { SetVariableValue(ref this._kindOther, value); }
        }

        public string Subject
        {
            get { return this._subject; }
            set { SetVariableValue(ref this._subject, value); }
        }

        public string Message
        {
            get { return this._message; }
            set { SetVariableValue(ref this._message, value); }
        }

        #endregion

        #region command

        public ICommand NewWindowCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var data = (WebNavigatorEventDataBase)o;
                        WebNavigatorUtility.OpenNewWindowWrapper(data, Mediation.Logger);
                    }
                );
            }
        }

        public ICommand SubmitCommand
        {
            get
            {
                return CreateCommand(
                    o => SubmitAsync()
                );
            }
        }

        #endregion

        #region function

        Task SubmitCoreAsync()
        {
            var info = new AppInformationCollection();
            var cpu = info.GetCPU();
            var memory = info.GetMemory();

            var cpuName = $"{cpu.Items["Name"]}, {cpu.Items["Description"]}";
            var totalMemoryByte = (ulong)memory.Items["TotalVisibleMemorySize"] * 1024;

            var map = new Dictionary<string, string>() {
                ["version"] = Constants.ApplicationVersionNumberText,
                ["revision"] = Constants.ApplicationVersionRevision,
                ["cpu"] = cpuName,
                ["platform"] = (Environment.Is64BitOperatingSystem ? "64" : "32"),
                ["memory"] = totalMemoryByte.ToString(),
                ["os"] = Environment.OSVersion.ToString(),
                ["clr"] = System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion(),
                ["subject"] = Subject,
                ["kind"] = QuestionnaireKind.ToString().ToLower(),
                ["kind_other"] = QuestionnaireKind == QuestionnaireKind.Other ? KindOther : string.Empty,
                ["message"] = Message
            };

            var encoding = Encoding.UTF8;
            var parameters = map
                .Select(p => new { Key = HttpUtility.UrlEncode(p.Key, encoding), Value = HttpUtility.UrlEncode(p.Value, encoding) })
                .Select(i => i.Key + "=" + i.Value)
            ;
            var content = new StringContent(string.Join("&", parameters), encoding, "application/x-www-form-urlencoded");
            var host = new HttpUserAgentHost();
            var client = host.CreateHttpUserAgent();
            return client.PostAsync(Constants.AppUriQuestionnaire, content).ContinueWith(t => {
                Mediation.Logger.Trace(t.Result.StatusCode.ToString(), t.Result.ToString());
                QuestionnaireBrowser.HomeCommand.TryExecute(null);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        Task SubmitAsync()
        {
            if(QuestionnaireKind == QuestionnaireKind.Other) {
                if (string.IsNullOrWhiteSpace(KindOther)) {
                    return Task.CompletedTask;
                }
            }
            if (string.IsNullOrWhiteSpace(Subject)) {
                return Task.CompletedTask;
            }
            if (string.IsNullOrWhiteSpace(Message)) {
                return Task.CompletedTask;
            }

            return SubmitCoreAsync();
        }

        #endregion

        #region ManagerViewModelBase

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return Enumerable.Empty<ManagerViewModelBase>();
        }

        protected override void ShowViewCore()
        {
            if(QuestionnaireBrowser.IsEmptyContent) {
                QuestionnaireBrowser.Navigate(QuestionnaireBrowser.HomeSource);
            }
        }

        protected override void HideViewCore()
        { }

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
        {
            QuestionnaireBrowser = view.information.questionnaireBrowser;
            QuestionnaireBrowser.HomeSource = Constants.AppUriQuestionnaire;
        }

        public override void UninitializeView(MainWindow view)
        {
            QuestionnaireBrowser = null;
        }

        #endregion

    }
}
