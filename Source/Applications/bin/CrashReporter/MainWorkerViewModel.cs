using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Model;
using Newtonsoft.Json.Linq;

namespace ContentTypeTextNet.MnMn.Applications.CrashReporter
{
    public class MainWorkerViewModel: ViewModelBase
    {
        #region variable

        string _reportFilePath;
        string _reportFileData;
        string _reportInformation;
        string _contactAddress;

        bool _isBusy;
        string _message;
        bool _sending;
        bool _sendSuccess;

        #endregion

        public MainWorkerViewModel()
        { }

        #region property

        MainWindow View { get; set; }

        public string ReportFilePath
        {
            get { return this._reportFilePath; }
            set { SetVariableValue(ref this._reportFilePath, value); }
        }

        public string ReportFileData
        {
            get { return this._reportFileData; }
            set { SetVariableValue(ref this._reportFileData, value); }
        }

        public string ReportInformation
        {
            get { return this._reportInformation; }
            set { SetVariableValue(ref this._reportInformation, value); }
        }

        public string ContactAddress
        {
            get { return this._contactAddress; }
            set { SetVariableValue(ref this._contactAddress, value); }
        }

        public bool IsBusy
        {
            get { return this._isBusy; }
            set { SetVariableValue(ref this._isBusy, value); }
        }

        public string Message
        {
            get { return this._message; }
            set
            {
                if(SetVariableValue(ref this._message, value)) {
                    CallOnPropertyChange(nameof(HasMessage));
                }
            }
        }
        public bool HasMessage => !string.IsNullOrEmpty(Message);

        public bool Sending
        {
            get { return this._sending; }
            set { SetVariableValue(ref this._sending, value); }
        }

        public bool SendSuccess
        {
            get { return this._sendSuccess; }
            set { SetVariableValue(ref this._sendSuccess, value); }
        }

        #endregion

        #region command

        public ICommand OpenReceiveProgramCommand
        {
            get
            {
                return CreateCommand(o => {
                    try {
                        var receiveProgramUrl = ConfigurationManager.AppSettings.Get("receive-program-url");
                        Process.Start(receiveProgramUrl);
                    } catch(Exception ex) {
                        MessageBox.Show(ex.Message);
                    }
                });
            }
        }

        public ICommand SendCommand
        {
            get { return CreateCommand(o => SendAsync()); }
        }

        public ICommand ReInputCommand
        {
            get { return CreateCommand(o => IsBusy = false); }
        }

        #endregion

        #region function

        public void SetView(MainWindow view)
        {
            View = view;
        }

        void InitializeCrash(CommandLine commandLine)
        {
            var reportOption = "report";
            if(commandLine.HasValue(reportOption)) {
                var reportPath = commandLine.GetValue(reportOption);
                ReportFilePath = reportPath;
            }

            if(!string.IsNullOrWhiteSpace(ReportFilePath)) {
                var usingFilePath = Environment.ExpandEnvironmentVariables(ReportFilePath);
                if(File.Exists(usingFilePath)) {
                    ReportFileData = File.ReadAllText(usingFilePath);
                }
            }
        }

        public void Initialize()
        {
            var commandLine = new CommandLine();

            var crashOption = "crash";
            if(commandLine.HasOption(crashOption)) {
                InitializeCrash(commandLine);
            } else {
                throw new NotSupportedException(Properties.Resources.String_Execute_Fail_StartMode);
            }
        }

        Task<CheckModel> SendCoreAsync()
        {
            var map = new Dictionary<string, string>() {
                [ConfigurationManager.AppSettings.Get("send-param-data")] = ReportFileData,
                [ConfigurationManager.AppSettings.Get("send-param-addr")] = ContactAddress,
                [ConfigurationManager.AppSettings.Get("send-param-info")] = ReportInformation,
            };
            var content = new FormUrlEncodedContent(map);
            var client = new HttpClient();
            return client.PostAsync(ConfigurationManager.AppSettings.Get("send-url"), content).ContinueWith(t => {
                var response = t.Result;
                if(response.IsSuccessStatusCode) {
                    var resultJson = response.Content.ReadAsStringAsync().Result;
                    var json = JObject.Parse(resultJson);
                    if(json[ConfigurationManager.AppSettings.Get("send-result-state")] != null) {
                        var state = json[ConfigurationManager.AppSettings.Get("send-result-state")].Value<string>();
                        if(state == ConfigurationManager.AppSettings.Get("send-result-success")) {
                            return CheckModel.Success();
                        }

                        var detail = json[ConfigurationManager.AppSettings.Get("send-result-detail")].Value<string>();
                        return CheckModel.Failure(detail);
                    }

                    return CheckModel.Failure(resultJson);
                } else {
                    return CheckModel.Failure(response.ToString());
                }
            });
        }

        Task SendAsync()
        {
            IsBusy = true;
            Sending = true;
            SendSuccess = false;
            Message = string.Empty;

            return SendCoreAsync().ContinueWith(t => {
                Sending = false;
                if(t.IsFaulted) {
                    Message = t.Exception.ToString();
                } else {
                    var success = t.Result;
                    if(success.IsSuccess) {
                        SendSuccess = true;
                    } else {
                        Message = success.Message;
                    }
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion
    }
}
