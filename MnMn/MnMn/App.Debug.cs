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
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.NicoNico.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.NicoNico.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.NicoNico;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.NicoNico;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.NicoNico;

namespace ContentTypeTextNet.MnMn.MnMn
{
#if DEBUG
    partial class App
    {
        void DoDebug()
        {
            new Mediation();
            //login();
            //load_uri();
            //param();
            //getthumbinfo();
            //var a = GetthumbinfoUtility.ConvertTimeSpan("121:11");
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
            var vm = new NicoNicoSessionViewModel(mediation, model);
            await vm.LoginAsync();
            var a = await vm.CheckLoginAsync();
            await vm.LogoutAsync();
            var b = await vm.CheckLoginAsync();
        }

        void load_uri()
        {
            var path = @"etc\niconico\uri-list.xml";
            var list = SerializeUtility.LoadXmlSerializeFromFile<UriListModel>(path);
        }

        void param()
        {
            var path = @"etc\niconico\uri-params.xml";
            var list = SerializeUtility.LoadXmlSerializeFromFile<ParameterListModel>(path);
        }

        async void getthumbinfo()
        {
            var gt = new Getthumbinfo(new Mediation());
            var a = await gt.GetAsync("sm9");
            var b = await gt.GetAsync("sm9");
        }
    }
#endif
}
