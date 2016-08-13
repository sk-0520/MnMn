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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App
{
    public class AppInformationManagerViewModel: ManagerViewModelBase
    {
        #region variable

        Uri _archiveUri;
        Version _archiveVersion;
        UpdateCheckState _updateCheckState;

        #endregion

        public AppInformationManagerViewModel(Mediation mediation)
            : base(mediation)
        { }

        #region property

        WebBrowser HelpBrowser { get; set; }

        public Uri ArchiveUri
        {
            get { return this._archiveUri; }
            set { SetVariableValue(ref this._archiveUri, value); }
        }

        public Version ArchiveVersion
        {
            get { return this._archiveVersion; }
            set { SetVariableValue(ref this._archiveVersion, value); }
        }

        public UpdateCheckState UpdateCheckState
        {
            get { return this._updateCheckState; }
            set { SetVariableValue(ref this._updateCheckState, value); }
        }

        #endregion

        #region command
        #endregion

        #region function

        async Task CheckVersionAsync()
        {
            UpdateCheckState = UpdateCheckState.UnChecked;
            ArchiveVersion = null;
            ArchiveUri = null;

            var client = new HttpClient();
            Mediation.Logger.Trace("update check: " + Constants.UriUpdate);
            var response = await client.GetAsync(Constants.UriUpdate);

            Mediation.Logger.Trace("update response state: " + response.StatusCode);
            if(!response.IsSuccessStatusCode) {
                UpdateCheckState = UpdateCheckState.Error;
                return;
            }

            var resultText = await response.Content.ReadAsStringAsync();

            var xml = XElement.Parse(resultText);

            var item = xml
                .Elements()
                .Select(
                    x => new {
                        Version = new Version(x.Attribute("version").Value),
                        //IsRC = x.Attribute("type").Value == "rc",
                        ArchiveElements = x.Elements(),
                    }
                )
                .Where(x => Constants.ApplicationVersionNumber <= x.Version)
                .OrderByDescending(x => x.Version)
                .FirstOrDefault()
            ;

            if(item == null) {
                UpdateCheckState = UpdateCheckState.CurrentIsNew;
                return;
            }

            var archive = item.ArchiveElements
                .Select(x => new {
                    Uri = new Uri(x.Attribute("uri").Value),
                    Platform = x.Attribute("platform").Value,
                    Version = item.Version,
                })
                .FirstOrDefault(x => x.Platform == (Environment.Is64BitProcess ? "x64" : "x86"))
            ;

            if(archive == null) {
                UpdateCheckState = UpdateCheckState.CurrentIsNew;
                return;
            }

            ArchiveUri = archive.Uri;
            ArchiveVersion = archive.Version;

            UpdateCheckState = UpdateCheckState.CurrentIsOld;
        }

        #endregion

        #region ManagerViewModelBase

        public override Task InitializeAsync()
        {
            return CheckVersionAsync();
        }

        public override void InitializeView(MainWindow view)
        {
            HelpBrowser = view.about.helpBrowser;
        }

        public override void UninitializeView(MainWindow view)
        { }

        public override Task GarbageCollectionAsync()
        {
            return Task.CompletedTask;
        }

        protected override void ShowView()
        {
            base.ShowView();

            if(HelpBrowser.Source == null) {
                HelpBrowser.Source = new Uri(Constants.HelpFilePath);
            }
        }

        #endregion
    }
}
