using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;
using HtmlAgilityPack;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.HalfBakedApi
{
    public class User: SessionApiBase<SmileSessionViewModel>
    {
        public User(Mediation mediation) 
            : base(mediation, ServiceType.Smile)
        { }

        #region function

        RawSmileUserAccountSimpleModel GetSimpleUserAccountModelFromHtmlDocument(HtmlDocument htmlDocument)
        {
            var regUser = new Regex(
                @"
                var
                \s+
                User
                \s*
                =
                \s*
                \{
                    (?<VALUE>
                        .*
                    )
                \}
                \s*
                ;?
                ",
                RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace
            );

            var userElement = htmlDocument.DocumentNode.Descendants()
                .Where(n => n.Name == "script")
                .Select(e => e.InnerText)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .FirstOrDefault(s => regUser.IsMatch(s))
            ;

            if(userElement == null) {
                return null;
            }

            var match = regUser.Match(userElement);
            var rawUser = match.Groups["VALUE"];

            var jsonUser = "{" + rawUser + "}";

            using(var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonUser))) {
                return SerializeUtility.LoadJsonDataFromStream<RawSmileUserAccountSimpleModel>(stream);
            }
        }

        public RawSmileUserAccountSimpleModel GetSimpleUserAccountModelFromHtmlSource(string htmlSource)
        {
            var htmlDocument = new HtmlDocument() {
                OptionAutoCloseOnEnd = true,
            };
            htmlDocument.LoadHtml(htmlSource);

            return GetSimpleUserAccountModelFromHtmlDocument(htmlDocument);
        }

        SmileUserInformationModel GetUserInformationFromHtmlSource(string htmlSource)
        {
            var htmlDocument = new HtmlDocument() {
                OptionAutoCloseOnEnd = true,
            };
            htmlDocument.LoadHtml(htmlSource);

            var result = new SmileUserInformationModel();

            var nickNameElement = htmlDocument.DocumentNode.SelectSingleNode("//*[@class='userDetail']/*[@class='avatar']/img");
            var nickName = nickNameElement.Attributes["alt"].Value;
            var imageUrl = nickNameElement.Attributes["src"].Value;

            result.ThumbnailUri = new Uri(imageUrl);
            result.UserName = nickName;

            var profileElement = htmlDocument.DocumentNode.SelectSingleNode("//*[@class='profile']");
            var accountElement = profileElement.SelectSingleNode(".//*[@class='account']");

            var accountElementInnerText = accountElement.InnerText;

            result.UserId = SmileUserUtility.GetUserId(accountElementInnerText);
            result.IsPremium = SmileUserUtility.IsPremium(accountElementInnerText);
            result.ResistedVersion = SmileUserUtility.GetVersion(accountElementInnerText);
            var gender = SmileUserUtility.GetGender(accountElementInnerText);
            result.IsPublicGender = gender.IsSuccess;
            if(result.IsPublicGender) {
                result.Gender = gender.Result;
            } else {
                result.Gender = Gender.Unknown;
            }
            var location = SmileUserUtility.GetLocation(accountElementInnerText);
            result.IsPublicLocation = location.IsSuccess;
            if(result.IsPublicLocation) {
                result.Location = location.Result;
            }
            var birthday = SmileUserUtility.GetBirthday(accountElementInnerText);
            result.IsPublicBirthday = birthday.IsSuccess;
            if(result.IsPublicBirthday) {
                result.Birthday = birthday.Result;
            }

            return result;
        }

        public Task<SmileUserInformationModel> LoadUserInformationAsync(string userId)
        {
            var page = new PageLoader(Mediation, SessionBase, SmileMediationKey.userPage, ServiceType.Smile);
            page.ReplaceUriParameters["user-id"] = userId;
            return page.GetResponseTextAsync(PageLoaderMethod.Get).ContinueWith(task => {
                page.Dispose();
                var response = task.Result;
                return GetUserInformationFromHtmlSource(response.Result);
            });
        }

        #endregion
    }
}
