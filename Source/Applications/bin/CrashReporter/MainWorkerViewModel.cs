﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ReadOnly;
using ContentTypeTextNet.MnMn.Library.Bridging.Model;
using Newtonsoft.Json.Linq;

namespace ContentTypeTextNet.MnMn.Applications.CrashReporter
{
    public class MainWorkerViewModel: ViewModelBase
    {
        #region variable

        string _reportMessage;
        string _reportFilePath;
        string _reportFileData;
        string _reportInformation;
        string _contactAddress;

        bool _isBusy;
        string _message;
        bool _sending;
        bool _sendSuccess;

        string _rebootApplicationPath;
        string _rebootApplicationCommandLine;

        bool _autoSend;
        double _waitAutoSend;

        #endregion

        public MainWorkerViewModel()
        { }

        #region property

        MainWindow View { get; set; }

        public string ReportMessage
        {
            get { return this._reportMessage; }
            set { SetVariableValue(ref this._reportMessage, value); }
        }

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

        bool IsDebug { get; set; }

        public string RebootApplicationPath
        {
            get { return this._rebootApplicationPath; }
            set { SetVariableValue(ref this._rebootApplicationPath, value); }
        }

        public string RebootApplicationCommandLine
        {
            get { return this._rebootApplicationCommandLine; }
            set { SetVariableValue(ref this._rebootApplicationCommandLine, value); }
        }

        public bool AutoSend
        {
            get { return this._autoSend; }
            set { SetVariableValue(ref this._autoSend, value); }
        }

        public double WaitAutoSend
        {
            get { return this._waitAutoSend; }
            set { SetVariableValue(ref this._waitAutoSend, value); }
        }

        DispatcherTimer AutoSendTimer { get; set; }
        DateTime AutoSendStartTime { get; set; }
        DateTime AutoSendEndTime { get; set; }
        TimeSpan AutoSendEndTimeSpan { get; } = TimeSpan.Parse(ConfigurationManager.AppSettings.Get("auto-send-wait-time"));

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

        public ICommand OpenHelpCommand
        {
            get
            {
                return CreateCommand(o => {
                    try {
                        var receiveProgramUrl = ConfigurationManager.AppSettings.Get("help-url");
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

        public ICommand RebootApplicationCommand
        {
            get
            {
                return CreateCommand(
                    o => RebootApplication(),
                    o => !string.IsNullOrWhiteSpace(RebootApplicationPath) && File.Exists(Environment.ExpandEnvironmentVariables(RebootApplicationPath))
                );
            }
        }

        public ICommand CancelAutoSendCommand
        {
            get
            {
                return CreateCommand(
                    o => CancelAutoSend()
                );
            }
        }

        #endregion

        #region function

        public void SetView(MainWindow view)
        {
            View = view;

            View.Loaded += View_Loaded;
        }

        void InitializeCrash(CommandLine commandLine)
        {
            var messageOption = "message";
            if(commandLine.HasValue(messageOption)) {
                var reportPath = commandLine.GetValue(messageOption);
                ReportMessage = reportPath;
            }

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

            IsDebug = commandLine.HasOption("debug");

            var rebootOption = "reboot";
            if(commandLine.HasValue(rebootOption)) {
                var rebootAppPath = commandLine.GetValue(rebootOption);
                RebootApplicationPath = rebootAppPath;
            }

            var rebootArgOption = "reboot-arg";
            if(commandLine.HasValue(rebootArgOption)) {
                var rebootAppArg = commandLine.GetValue(rebootArgOption);
                RebootApplicationCommandLine = rebootAppArg;
            }

            AutoSend = commandLine.HasOption("auto-send");
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

        Task<IReadOnlyCheck> SendCoreAsync()
        {
            var map = new Dictionary<string, string>() {
                [ConfigurationManager.AppSettings.Get("send-param-data")] = ReportFileData,
                [ConfigurationManager.AppSettings.Get("send-param-addr")] = ContactAddress,
                [ConfigurationManager.AppSettings.Get("send-param-info")] = ReportInformation,
                [ConfigurationManager.AppSettings.Get("send-param-debug")] = IsDebug.ToString().ToLower(),
            };

            var encoding = Encoding.GetEncoding(ConfigurationManager.AppSettings.Get("send-encoding"));
            var items = map
                .Select(p => new { Key = HttpUtility.UrlEncode(p.Key, encoding), Value = HttpUtility.UrlEncode(p.Value, encoding) })
                .Select(i => $"{i.Key}={i.Value}");
            ;
            var param = string.Join("&", items);
            var content = new StringContent(param, encoding, "application/x-www-form-urlencoded");
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

        void RebootApplication()
        {
            var path = Environment.ExpandEnvironmentVariables(RebootApplicationPath);
            Process.Start(path, RebootApplicationCommandLine);
            View.Close();
        }

        void CancelAutoSend()
        {
            AutoSend = false;
            AutoSendTimer.Stop();
            AutoSendTimer.Tick -= AutoSendTimer_Tick;
        }

        #endregion

        void View_Loaded(object sender, RoutedEventArgs e)
        {
            View.Loaded -= View_Loaded;

            if(AutoSend) {
                AutoSendTimer = new DispatcherTimer() {
                    Interval = TimeSpan.Parse(ConfigurationManager.AppSettings.Get("auto-send-polling"))
                };
                AutoSendTimer.Tick += AutoSendTimer_Tick;

                AutoSendStartTime = DateTime.Now;
                AutoSendEndTime = AutoSendStartTime + AutoSendEndTimeSpan;
                AutoSendTimer.Start();
            }
        }

        private void AutoSendTimer_Tick(object sender, EventArgs e)
        {
            var current = DateTime.Now;
            if(AutoSendEndTime < current) {
                WaitAutoSend = 1;
                AutoSend = false;
                AutoSendTimer.Stop();
                AutoSendTimer.Tick -= AutoSendTimer_Tick;

                SendAsync().ConfigureAwait(false);
            } else {
                WaitAutoSend = (current - AutoSendStartTime).TotalMilliseconds / AutoSendEndTimeSpan.TotalMilliseconds;
            }
        }
    }
}
