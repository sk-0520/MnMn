﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
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

        SmileUserInformationModel GetUserInformationFromHtmlDocument(HtmlDocument htmlDocument)
        {
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

            var descriptionElement = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='description_full']");
            if(descriptionElement != null) {
                var lines = descriptionElement.InnerText
                    .SplitLines()
                    .Select(s => s.Trim())
                    .Where(s => 0 < s.Length)
                ;
                result.Description = string.Join(Environment.NewLine, lines);
            }

            var sidebarElement = htmlDocument.DocumentNode.SelectSingleNode("//*[@class='sidebar']");

            var mylist = SmileUserUtility.GetPageLink(sidebarElement, "mylistTab");
            result.IsPublicMyList = mylist.IsSuccess;
            var post = SmileUserUtility.GetPageLink(sidebarElement, "videoTab");
            result.IsPublicPost = post.IsSuccess;
            var report = SmileUserUtility.GetPageLink(sidebarElement, "nicorepoTab");
            result.IsPublicReport = report.IsSuccess;

            return result;
        }

        SmileUserInformationModel GetUserInformationFromHtmlSource(string htmlSource)
        {
            var htmlDocument = new HtmlDocument() {
                OptionAutoCloseOnEnd = true,
            };
            htmlDocument.LoadHtml(htmlSource);

            return GetUserInformationFromHtmlDocument(htmlDocument);
        }

        public Task<SmileUserInformationModel> LoadUserInformationAsync(string userId)
        {
            var page = new PageLoader(Mediation, Session, SmileMediationKey.userPage, ServiceType.Smile);
            page.ReplaceUriParameters["user-id"] = userId;
            return page.GetResponseTextAsync(PageLoaderMethod.Get).ContinueWith(task => {
                page.Dispose();
                var response = task.Result;
                return GetUserInformationFromHtmlSource(response.Result);
            });
        }

        RawSmileUserMyListModel GetUserMyListFromHtmlDocument(HtmlDocument htmlDocument)
        {
            var result = new RawSmileUserMyListModel();

            var myListElement = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='mylist']");

            var groupElements = myListElement.SelectNodes(".//*[@class='outer']");
            if(groupElements == null) {
                return null;
            }
            foreach(var groupElement in groupElements) {
                var group = new RawSmileUserMyListGroupModel();

                var headerElement = groupElement.SelectSingleNode(".//h4");
                var titleElement = headerElement.SelectSingleNode(".//a");
                group.MyListId = titleElement.Attributes["href"].Value.Split('/').Last();
                group.Title = titleElement.InnerText;

                var countElement = headerElement.SelectSingleNode(".//*[@class='mylistNum']");
                group.Count = SmileUserUtility.GetMyListCount(countElement.InnerText);

                var descElement = groupElement.SelectNodes(".//*[@class='mylistDescription']");
                if(descElement != null) {
                    var descFullElement = descElement.FirstOrDefault(n => n.Attributes.Any(a => a.Value == "data-nico-mylist-desc-full"));
                    if(descFullElement != null) {
                        group.Description = descFullElement.InnerText;
                    }
                }

                result.Groups.Add(group);
            }

            return result;
        }

        RawSmileUserMyListModel GetUserMyListFromHtmlSource(string htmlSource)
        {
            var htmlDocument = new HtmlDocument() {
                OptionAutoCloseOnEnd = true,
            };
            htmlDocument.LoadHtml(htmlSource);

            return GetUserMyListFromHtmlDocument(htmlDocument);
        }

        public Task<RawSmileUserMyListModel> LoadUserMyListAsync(string userId)
        {
            var page = new PageLoader(Mediation, Session, SmileMediationKey.userMyListPage, ServiceType.Smile);
            page.ReplaceUriParameters["user-id"] = userId;
            return page.GetResponseTextAsync(PageLoaderMethod.Get).ContinueWith(task => {
                page.Dispose();
                var response = task.Result;
                return GetUserMyListFromHtmlSource(response.Result);
            });
        }

        public Task<FeedSmileVideoModel> LoadPostVideoAsync(string userId)
        {
            var page = new PageLoader(Mediation, Session, SmileMediationKey.userPostVideo, ServiceType.Smile);
            page.ReplaceUriParameters["user-id"] = userId;
            page.ReplaceUriParameters["lang"] = Constants.CurrentLanguageCode;
            return page.GetResponseTextAsync(PageLoaderMethod.Get).ContinueWith(task => {
                var feedResult = task.Result;
                if(!feedResult.IsSuccess) {
                    return null;
                } else {
                    using(var stream = new MemoryStream(Encoding.UTF8.GetBytes(feedResult.Result))) {
                        return SerializeUtility.LoadXmlSerializeFromStream<FeedSmileVideoModel>(stream);
                    }
                }
            });
        }

        #endregion
    }
}