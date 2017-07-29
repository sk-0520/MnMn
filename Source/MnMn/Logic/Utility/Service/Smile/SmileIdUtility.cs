using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.IF;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile
{
    /// <summary>
    /// 各種IDに関する処理。
    /// <para><see cref="ContentTypeTextNet.MnMn.Library.Bridging.IF.Compatibility.ConvertValue"/>から呼び出される想定で直接使用すべきではない。</para>
    /// <para><see cref="ContentTypeTextNet.MnMn.Library.Bridging.IF.Compatibility.ConvertValue"/>廃止しよう！</para>
    /// </summary>
    internal static class SmileIdUtility
    {
        /// <summary>
        /// スクレイピングする必要のある動画IDか。
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        public static bool IsScrapingVideoId(string videoId, IExpressionGetter expressionGetter)
        {
            var expression = expressionGetter.GetExpression(SmileMediatorKey.isScrapingVideoId, ServiceType.Smile);
            return expression.Regex.IsMatch(videoId);
        }

        /// <summary>
        /// 文字列中から動画IDを取得する。
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns>null: とれんかった</returns>
        public static string GetVideoId(string inputValue, IExpressionGetter expressionGetter)
        {
            if(string.IsNullOrWhiteSpace(inputValue)) {
                return null;
            }

            var videoIdPrefix = expressionGetter.GetExpression(SmileMediatorKey.getVideoId, SmileMediatorKey.Id.getVideoId_prefixId, ServiceType.Smile);
            var match = videoIdPrefix.Regex.Match(inputValue);
            if(match.Success) {
                return match.Groups["VIDEO_ID"].Value;
            } else {
                var videoIdNumber = expressionGetter.GetExpression(SmileMediatorKey.getVideoId, SmileMediatorKey.Id.getVideoId_numberId, ServiceType.Smile);
                var numberMatch = videoIdNumber.Regex.Match(inputValue);
                if(numberMatch.Success) {
                    return numberMatch.Groups["VIDEO_ID"].Value;
                }

                return null;
            }
        }

        /// <summary>
        /// 文字列中からマイリストIDを取得する。
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns>null: とれんかった</returns>
        public static string GetMyListId(string inputValue, IExpressionGetter expressionGetter)
        {
            if(string.IsNullOrWhiteSpace(inputValue)) {
                return null;
            }

            var expression = expressionGetter.GetExpression(SmileMediatorKey.getMylistId, ServiceType.Smile);
            var match = expression.Regex.Match(inputValue);
            if(match.Success) {
                return match.Groups["MYLIST_ID"].Value;
            } else {
                return null;
            }
        }

        /// <summary>
        /// 文字列中からユーザーIDを取得する。
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns>null: とれんかった</returns>
        public static string GetUserId(string inputValue, IExpressionGetter expressionGetter)
        {
            if(string.IsNullOrWhiteSpace(inputValue)) {
                return null;
            }

            var expression = expressionGetter.GetExpression(SmileMediatorKey.getUserId, ServiceType.Smile);
            var match = expression.Regex.Match(inputValue);
            if(match.Success) {
                return match.Groups["USER_ID"].Value;
            } else {
                return null;
            }
        }

        /// <summary>
        /// 指定動画IDは補正処理が必要か。
        /// <para>nnnn 形式から xxnnnn 形式に変換が必要かを返す。</para>
        /// <para>こいつは単独呼び出しOKとする(妥協)。</para>
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        public static bool NeedCorrectionVideoId(string videoId, IExpressionGetter expressionGetter)
        {
            if(!string.IsNullOrEmpty(videoId)) {
                var expression = expressionGetter.GetExpression(SmileMediatorKey.needCorrectionVideoId, ServiceType.Smile);
                return expression.Regex.IsMatch(videoId);
            }

            return false;
        }

        public static string ConvertChannelId(string rawChannelId, IExpressionGetter expressionGetter)
        {
            var normalization = expressionGetter.GetExpression(SmileMediatorKey.convertChannelId, SmileMediatorKey.Id.convertChannelId_normalizationId, ServiceType.Smile);
            if(normalization.Regex.IsMatch(rawChannelId)) {
                return rawChannelId;
            }

            var numberOnly = expressionGetter.GetExpression(SmileMediatorKey.convertChannelId, SmileMediatorKey.Id.convertChannelId_numberOnly, ServiceType.Smile);
            if(numberOnly.Regex.IsMatch(rawChannelId)) {
                // TODO: 即値
                return "ch" + rawChannelId;
            }

            // もしかしたらIDとして有効かもしんない
            return rawChannelId;
        }

    }
}
