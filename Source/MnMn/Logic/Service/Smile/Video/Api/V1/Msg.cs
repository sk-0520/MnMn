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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1
{
    /// <summary>
    ///
    /// </summary>
    public class Msg : SessionApiBase<SmileSessionViewModel>
    {
        public Msg(Mediation mediation)
            : base(mediation, ServiceType.Smile)
        { }

        #region function

        public static RawSmileVideoMsgPacket_Issue665NA_Model ConvertFromRawPacketData_Issue665NA(Stream stream)
        {
            return SerializeUtility.LoadXmlSerializeFromStream<RawSmileVideoMsgPacket_Issue665NA_Model>(stream);
        }

        public static RawSmileVideoMsgPacket_Issue665NA_ResultModel ConvertFromRawPacketResultData_Issue665NA(Stream stream)
        {
            return SerializeUtility.LoadXmlSerializeFromStream<RawSmileVideoMsgPacket_Issue665NA_ResultModel>(stream);
        }

        public static SmileVideoMsgSettingModel ConvertMsgSettingModel(FileInfo file)
        {
            return SerializeUtility.LoadJsonDataFromFile<SmileVideoMsgSettingModel>(file.FullName);
        }

        string ReplaceJsonApiUrl(Uri msgServerUri)
        {
            // watch_dll.js で謎処理
            return msgServerUri.OriginalString.Replace("/api/", "/api.json/");
        }

        public async Task<RawSmileVideoMsgPacket_Issue665NA_Model> Load_Issue665NA_Async(Uri msgServer, string threadId, string userId, int getCount, int rangeHeadMinutes, int rangeTailMinutes, int rangeGetCount, int rangeGetAllCount, RawSmileVideoGetthreadkeyModel threadkeyModel)
        {
            using(var page = new PageLoader(Mediation, Session, SmileVideoMediationKey.msg_Issue665NA, ServiceType.SmileVideo)) {
                //page.ParameterType = ParameterType.Mapping;
                page.ReplaceUriParameters["msg-uri"] = msgServer.OriginalString;
                page.ReplaceRequestParameters["thread-id"] = threadId;
                page.ReplaceRequestParameters["user-id"] = userId;
                page.ReplaceRequestParameters["res_from"] = $"-{Math.Abs(getCount)}";
                if(rangeHeadMinutes < rangeTailMinutes && 0 < rangeGetCount) {
                    page.ReplaceRequestParameters["time-size"] = $"{rangeHeadMinutes}-{rangeTailMinutes}:{rangeGetCount}";
                    page.ReplaceRequestParameters["all-size"] = $"{Math.Abs(rangeGetAllCount)}";
                }
                if(threadkeyModel != null) {
                    page.ReplaceRequestParameters["threadkey"] = threadkeyModel.Threadkey;
                    page.ReplaceRequestParameters["force_184"] = threadkeyModel.Force184;
                }

                var rawMessage = await page.GetResponseTextAsync(Define.PageLoaderMethod.Post);
                //Debug.WriteLine(rawMessage.Result);
                using(var stream = StreamUtility.ToUtf8Stream(rawMessage.Result)) {
                    var result = ConvertFromRawPacketData_Issue665NA(stream);
                    return result;
                }
            }
        }

        public async Task<SmileVideoMsgSettingModel> LoadAsync(Uri msgServer, string threadId, int getCount, SmileVideoMsgRangeModel range, string userId, string userKey, bool hasOriginalPosterComment, bool isCommunityThread, string communityThreadId, RawSmileVideoGetthreadkeyModel communityThreadKey)
        {
            var packetMap = new Dictionary<SmileVideoMsgPacketId, int>() {
                [SmileVideoMsgPacketId.Normal] = 0,
                [SmileVideoMsgPacketId.NormalLeaves] = 1,
            };
            var packetCount = packetMap.Max(p => p.Value);

            using(var page = new PageLoader(Mediation, Session, SmileVideoMediationKey.msg, ServiceType.SmileVideo)) {
                //page.ReplaceUriParameters["msg-uri"] = msgServer.OriginalString;
                page.ReplaceUriParameters["msg-uri"] = ReplaceJsonApiUrl(msgServer);
                page.ReplaceRequestParameters["thread-id"] = threadId;
                page.ReplaceRequestParameters["res_from"] = $"-{Math.Abs(getCount)}";
                page.ReplaceRequestParameters["content"] = range.ToString();
                page.ReplaceRequestParameters["user-id"] = userId;
                page.ReplaceRequestParameters["userkey"] = userKey;
                page.ReplaceRequestParameters["community-thread-id"] = communityThreadId;

                // 頑張ったマッピングのパケット番号調整
                // 投稿者
                page.ReplaceRequestParameters["filter-op"] = hasOriginalPosterComment.ToString();
                if(hasOriginalPosterComment) {
                    packetCount += 1;
                    packetMap[SmileVideoMsgPacketId.OriginalPoster] = packetCount;
                    page.ReplaceRequestParameters["packet-op"] = packetCount.ToString();
                }

                // スレッドキーが必要な人
                page.ReplaceRequestParameters["filter-community"] = isCommunityThread.ToString();
                if(isCommunityThread) {
                    page.ReplaceRequestParameters["threadkey"] = communityThreadKey.Threadkey;
                    page.ReplaceRequestParameters["force_184"] = communityThreadKey.Force184;

                    packetCount += 1;
                    packetMap[SmileVideoMsgPacketId.Community] = packetCount;
                    page.ReplaceRequestParameters["packet-community-thread"] = packetCount.ToString();

                    packetCount += 1;
                    packetMap[SmileVideoMsgPacketId.CommunityLeaves] = packetCount;
                    page.ReplaceRequestParameters["packet-community-thread_leaves"] = packetCount.ToString();
                }

                var rawMessage = await page.GetResponseTextAsync(Define.PageLoaderMethod.Post);
                //Debug.WriteLine(rawMessage.Result);
                var items = Newtonsoft.Json.JsonConvert.DeserializeObject<CollectionModel<RawSmileVideoMsgResultItemModel>>(rawMessage.Result);

                var result = new SmileVideoMsgSettingModel() {
                    PacketId = packetMap,
                    Items = items,
                };

                return result;
            }
        }

        public async Task<RawSmileVideoPostKeyModel> LoadPostKeyAsync(string threadId, int commentCount)
        {
            using(var page = new PageLoader(Mediation, Session, SmileVideoMediationKey.msgPostKey, ContentTypeTextNet.MnMn.Library.Bridging.Define.ServiceType.SmileVideo)) {
                page.ReplaceUriParameters["block-no"] = (Math.Floor((commentCount + 1) / 100.0)).ToString();
                page.ReplaceUriParameters["thread-id"] = threadId;

                var rawMessage = await page.GetResponseTextAsync(Define.PageLoaderMethod.Get);
                if(rawMessage.IsSuccess) {
                    var result = RawValueUtility.ConvertNameModelFromWWWFormData<RawSmileVideoPostKeyModel>(rawMessage.Result);
                    return result;
                }

                return null;
            }
        }

        public async Task<RawSmileVideoMsgPacket_Issue665NA_ResultModel> Post_Issue665NA_Async(Uri msgServer, string threadId, TimeSpan vpos, string ticket, string postkey, IEnumerable<string> commands, string comment)
        {
            await LoginIfNotLoginAsync();

            using(var page = new PageLoader(Mediation, Session, SmileVideoMediationKey.msgPost_Issue665NA, ServiceType.SmileVideo)) {
                page.ReplaceUriParameters["msg-uri"] = msgServer.OriginalString;

                page.ReplaceRequestParameters["thread-id"] = threadId;
                page.ReplaceRequestParameters["ticket"] = ticket;
                page.ReplaceRequestParameters["postkey"] = postkey;
                page.ReplaceRequestParameters["vpos"] = SmileVideoMsgUtility.ConvertRawElapsedTime(vpos);
                page.ReplaceRequestParameters["premium"] = SmileVideoMsgUtility.ConvertRawIsPremium(Session.IsPremium);
                page.ReplaceRequestParameters["user_id"] = Session.UserId;
                page.ReplaceRequestParameters["commands"] = string.Join(" ", commands);
                page.ReplaceRequestParameters["comment"] = comment;

                var response = await page.GetResponseTextAsync(Define.PageLoaderMethod.Post);
                if(response.IsSuccess) {
                    Mediation.Logger.Trace(response.Result);
                    using(var stream = StreamUtility.ToUtf8Stream(response.Result)) {
                        var result = ConvertFromRawPacketResultData_Issue665NA(stream);
                        return result;
                    }
                }

                return null;
            }
        }

        public async Task PostAsync(Uri msgServer, string threadId, TimeSpan vpos, string ticket, string postkey, IEnumerable<string> commands, string comment)
        {
            await LoginIfNotLoginAsync();

            using(var page = new PageLoader(Mediation, Session, SmileVideoMediationKey.msgPost, ServiceType.SmileVideo)) {
                page.ReplaceUriParameters["msg-uri"] = ReplaceJsonApiUrl(msgServer);

                page.ReplaceRequestParameters["thread-id"] = threadId;
                page.ReplaceRequestParameters["ticket"] = ticket;
                page.ReplaceRequestParameters["postkey"] = postkey;
                page.ReplaceRequestParameters["vpos"] = SmileVideoMsgUtility.ConvertRawElapsedTime(vpos);
                page.ReplaceRequestParameters["premium"] = SmileVideoMsgUtility.ConvertRawIsPremium(Session.IsPremium);
                page.ReplaceRequestParameters["user_id"] = Session.UserId;
                page.ReplaceRequestParameters["commands"] = string.Join(" ", commands);
                page.ReplaceRequestParameters["comment"] = comment;

                var response = await page.GetResponseTextAsync(Define.PageLoaderMethod.Post);
                if(response.IsSuccess) {
                    Mediation.Logger.Trace(response.Result);
                    using(var stream = StreamUtility.ToUtf8Stream(response.Result)) {
                        var result = ConvertFromRawPacketResultData_Issue665NA(stream);
                        //return null;
                    }
                }

                //return null;
            }
        }

        #endregion
    }
}
