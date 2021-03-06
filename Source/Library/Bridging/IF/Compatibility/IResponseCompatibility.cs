﻿/*
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
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ReadOnly;
using ContentTypeTextNet.MnMn.Library.Bridging.Model;

namespace ContentTypeTextNet.MnMn.Library.Bridging.IF.Compatibility
{
    /// <summary>
    /// 受信データの互換処理。
    /// </summary>
    public interface IResponseCompatibility
    {
        /// <summary>
        /// 受信後に呼び出されるヘッダーチェック処理。
        /// </summary>
        /// <param name="uri">受信データの送り元。</param>
        /// <param name="headers">レスポンスヘッダ。</param>
        /// <param name="serviceType">呼び出し元の使用目的。</param>
        /// <returns></returns>
        IReadOnlyCheck CheckResponseHeader(string key, Uri uri, HttpHeaders headers, ServiceType serviceType);
        /// <summary>
        /// 受信後に呼び出されるバイナリ変換処理。
        /// </summary>
        /// <param name="uri">受信データの送り元。</param>
        /// <param name="stream">バイナリデータ。</param>
        /// <param name="serviceType">呼び出し元の使用目的。</param>
        void ConvertBinary(string key, Uri uri, Stream stream, ServiceType serviceType);
        /// <summary>
        /// 受信後に呼び出されるバイナリから文字コード取得処理。
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="stream"></param>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        Encoding GetEncoding(string key, Uri uri, Stream stream, ServiceType serviceType);
        /// <summary>
        /// 受信後に呼び出される文字列変換処理。
        /// </summary>
        /// <param name="uri">受信データの送り元。</param>
        /// <param name="text">テキストデータ。</param>
        /// <param name="serviceType">呼び出し元の使用目的。</param>
        /// <returns></returns>
        string ConvertString(string key, Uri uri, string text, ServiceType serviceType);
    }
}
