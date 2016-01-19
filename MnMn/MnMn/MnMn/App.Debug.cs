﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.NicoNico;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.NicoNico;

namespace ContentTypeTextNet.MnMn.MnMn
{
#if DEBUG
    partial class App
    {
        void DoDebug()
        {
            //login();
            load_uri();
        }

        async void login()
        {
            var mediation = new Mediation();
            var cmd = new CommandLine();
            var account = cmd.GetValue("account");
            var password = cmd.GetValue("password");

            var model = new UserAccountModel() {
                User = account,
                Password = password,
            };
            var vm = new UserSessionViewModel(model, mediation);
            var state = await vm.LoginAsync();
        }

        void load_uri()
        {
            var path = @"etc\niconico\uri-list.xml";
            var list = SerializeUtility.LoadXmlSerializeFromFile<UriListModel>(path);
        }
    }
#endif
}
