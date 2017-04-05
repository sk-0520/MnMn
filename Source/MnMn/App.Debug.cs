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
using ContentTypeTextNet.MnMn.Library.Bridging.Define.CodeExecutor;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api.V1;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Live.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
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
            //liverss();
            //dmc();
            //src();
            //livestatus();
            //market();
            //user_id();
            //exp();
        }

        async void login()
        {
            var mediation = new Mediation(new Model.Setting.AppSettingModel(), new Logger());
            var cmd = new CommandLine();
            var account = cmd.GetValue("smile-login-name");
            var password = cmd.GetValue("smile-login-pass");

            var model = new SmileUserAccountModel() {
                Name = account,
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
            var path = @"etc\define\service\smile\uri-list.xml";
            var list = SerializeUtility.LoadXmlSerializeFromFile<UrisModel>(path);
        }

        void param()
        {
            var path = @"etc\define\service\smile\uri-params.xml";
            var list = SerializeUtility.LoadXmlSerializeFromFile<ParametersModel>(path);
        }

        async void getthumbinfo()
        {
            var gt = new Getthumbinfo(new Mediation(new Model.Setting.AppSettingModel(), new Logger()));
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
            var mediation = new Mediation(new Model.Setting.AppSettingModel(), new Logger());
            var cmd = new CommandLine();
            var account = cmd.GetValue("smile-login-name");
            var password = cmd.GetValue("smile-login-pass");

            var model = new SmileUserAccountModel() {
                Name = account,
                Password = password,
            };
            var session = new SmileSessionViewModel(mediation, model);

            var mylist = new MyList(mediation);

            var def = await mylist.LoadAccountDefaultAsync();
            var grp = await mylist.LoadAccountGroupAsync();
        }

        private void liverss()
        {
            var rss = new FeedSmileLiveModel();
            rss.Channel.TotalCount = "aaa";
            using(var stream = new MemoryStream()) {
                SerializeUtility.SaveXmlSerializeToStream(stream, rss);
                var binary = stream.ToArray();
                var s = Encoding.UTF8.GetString(binary);
            }
        }

        void dmc()
        {
            var path1 = @"X:\dmc-recv1.xml";
            var path2 = @"X:\dmc-recv2.xml";

            var model = SerializeUtility.LoadXmlSerializeFromFile<RawSmileVideoDmcObjectModel>(path1);
            SerializeUtility.SaveXmlSerializeToFile(path2, model);
        }

        void src()
        {
            var model = new SpaghettiSourceModel() {
                CodeLanguage = CodeLanguage.CSharp,
                Code = "usinf System; class asd(){  }",
            };
            model.Parameter.AssemblyNames.Add("123");
            var path = @"X:\src.xml";
            SerializeUtility.SaveXmlSerializeToFile(path, model);
        }

        async void livestatus()
        {
            //var mediation = new Mediation(new Model.Setting.AppSettingModel(), new Logger());
            //var cmd = new CommandLine();
            //var account = cmd.GetValue("smile-login-name");
            //var password = cmd.GetValue("smile-login-pass");

            //var model = new SmileUserAccountModel() {
            //    Name = account,
            //    Password = password,
            //};
            //var vm = new SmileSessionViewModel(mediation, model);
            //await vm.LoginAsync();

            //var getPlayerStatus = new GetPlayerStatus(mediation);
            await Task.Delay(0);
            var mmm = GetPlayerStatus.ConvertFromRawData(File.ReadAllText(@"x:\stat.xml"));
        }

        async void market()
        {
            var mediation = new Mediation(new Model.Setting.AppSettingModel(), new Logger());
            var market = new Market(mediation);
            //var model = await market.LoadVideoRelationAsync("sm9");
            var model = await market.LoadVideoRelationAsync("sm14027065");
            var items = SmileMarketUtility.GetVideoRelationItems(model).ToList();
        }

        void user_id()
        {
            var logger = new Logger();
            logger.LoggerConfig.PutsDebug = true;
            var userid = AppUtility.CreateUserId(logger);
        }

        void exp()
        {
            var i = new ExpressionItemModel();
            i.Data.Text = "zxc";
            i.Kind = Define.ExpressionItemKind.Regex;

            var e = new ExpressionElementModel();
            e.Key = "asd";
            e.Items.Add(i);
            var es = new ExpressionsModel();
            es.Elements.Add(e);

            SerializeUtility.SaveXmlSerializeToFile(@"X:\exps.xml", es);
        }
    }
#endif
}
