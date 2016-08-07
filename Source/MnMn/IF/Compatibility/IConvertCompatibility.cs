using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.IF.Compatibility
{
    public interface IConvertCompatibility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputValue">出力データ。</param>
        /// <param name="outputType">出力データ型。</param>
        /// <param name="inputKey">入力識別子。</param>
        /// <param name="inputValue">入力データ。</param>
        /// <param name="inputType">入力データ型。</param>
        /// <param name="serviceType">呼び出し元の使用目的。</param>
        /// <returns></returns>
        bool ConvertValue(out object outputValue, Type outputType, string inputKey, object inputValue, Type inputType, ServiceType serviceType);
    }
}
