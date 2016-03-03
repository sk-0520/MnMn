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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api.V1;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn
{
#if DEBUG
    partial class App
    {
        void DoDebug()
        {
            //new Mediation();
            //login();
            //load_uri();
            //param();
            //getthumbinfo();
            //var a = GetthumbinfoUtility.ConvertTimeSpan("121:11");
            //mappng();
            //mylist();
        }

        async void login()
        {
            var mediation = new Mediation();
            var cmd = new CommandLine();
            var account = cmd.GetValue("smile-login-name");
            var password = cmd.GetValue("smile-login-pass");

            var model = new SmileUserAccountModel() {
                User = account,
                Password = password,
            };
            var vm = new SmileSessionViewModel(mediation, model);
            await vm.LoginAsync();
            var a = await vm.CheckLoginAsync();
            await vm.LogoutAsync();
            var b = await vm.CheckLoginAsync();
        }

        void load_uri()
        {
            var path = @"etc\niconico\uri-list.xml";
            var list = SerializeUtility.LoadXmlSerializeFromFile<UrisModel>(path);
        }

        void param()
        {
            var path = @"etc\niconico\uri-params.xml";
            var list = SerializeUtility.LoadXmlSerializeFromFile<ParametersModel>(path);
        }

        async void getthumbinfo()
        {
            var gt = new Getthumbinfo(new Mediation());
            var a = await gt.LoadAsync("sm9");
            var b = await gt.LoadAsync("sm9");
        }

        void mappng()
        {
            var ms = new MappingsModel();
            var mm = new MappingModel();
            var mi = new MappingItemModel();

            mm.Key = "test";
            mm.Content.Value = "content\r\ncontent";

            mi.Key = "item1";
            mm.Items.Add(mi);

            ms.Mappings.Add(mm);
            using(var stream = new MemoryStream()) {
                SerializeUtility.SaveXmlSerializeToStream(stream, ms);
                var s = Encoding.UTF8.GetString(stream.ToArray());
                Debug.WriteLine(s);
            }
        }

        async void mylist()
        {
            var mediation = new Mediation();
            var cmd = new CommandLine();
            var account = cmd.GetValue("smile-login-name");
            var password = cmd.GetValue("smile-login-pass");

            var model = new SmileUserAccountModel() {
                User = account,
                Password = password,
            };
            var session = new SmileSessionViewModel(mediation, model);

            var mylist = new MyList(mediation);
            
            var def = await mylist.LoadAccountDefaultAsync();
            var grp = await mylist.LoadAccountGroupAsync();
        }

    }
#endif
}
