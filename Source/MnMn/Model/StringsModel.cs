using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    /// <summary>
    /// 毎度毎度 Dictionary&lt;string, string&gt; 入力がしんどい。
    /// </summary>
    public sealed class StringsModel: Dictionary<string, string>
    {
        //
        // 概要:
        //     空で、既定の初期量を備え、キーの型の既定の等値比較子を使用する、System.Collections.Generic.Dictionary`2 クラスの新しいインスタンスを初期化します。
        public StringsModel()
            : base()
        { }

        //
        // 概要:
        //     空で、指定した初期量を備え、キーの型の既定の等値比較子を使用する、System.Collections.Generic.Dictionary`2 クラスの新しいインスタンスを初期化します。
        //
        // パラメーター:
        //   capacity:
        //     System.Collections.Generic.Dictionary`2 が格納できる要素数の初期値。
        //
        // 例外:
        //   T:System.ArgumentOutOfRangeException:
        //     capacity が 0 未満です。
        public StringsModel(int capacity)
            : base(capacity)
        { }

        //
        // 概要:
        //     空で、既定の初期量を備え、指定した System.Collections.Generic.Dictionary`2 を使用する、System.Collections.Generic.IEqualityComparer`1
        //     クラスの新しいインスタンスを初期化します。
        //
        // パラメーター:
        //   comparer:
        //     キーの比較時に使用する System.Collections.Generic.IEqualityComparer`1 実装。キーの型の既定の null を使用する場合は
        //     System.Collections.Generic.EqualityComparer`1。
        public StringsModel(IEqualityComparer<string> comparer)
            : base(comparer)
        { }

        //
        // 概要:
        //     指定した System.Collections.Generic.IDictionary`2 から要素をコピーして格納し、キーの型の既定の等値比較子を使用する、System.Collections.Generic.Dictionary`2
        //     クラスの新しいインスタンスを初期化します。
        //
        // パラメーター:
        //   dictionary:
        //     新しい System.Collections.Generic.IDictionary`2 に要素がコピーされた System.Collections.Generic.Dictionary`2。
        //
        // 例外:
        //   T:System.ArgumentNullException:
        //     dictionary は null です。
        //
        //   T:System.ArgumentException:
        //     dictionary に、1 つ以上の重複するキーが格納されています。
        public StringsModel(IDictionary<string, string> dictionary)
             : base(dictionary)
        { }
       //
        // 概要:
        //     空で、指定した初期量を備え、指定した System.Collections.Generic.Dictionary`2 を使用する、System.Collections.Generic.IEqualityComparer`1
        //     クラスの新しいインスタンスを初期化します。
        //
        // パラメーター:
        //   capacity:
        //     System.Collections.Generic.Dictionary`2 が格納できる要素数の初期値。
        //
        //   comparer:
        //     キーの比較時に使用する System.Collections.Generic.IEqualityComparer`1 実装。キーの型の既定の null を使用する場合は
        //     System.Collections.Generic.EqualityComparer`1。
        //
        // 例外:
        //   T:System.ArgumentOutOfRangeException:
        //     capacity が 0 未満です。
        public StringsModel(int capacity, IEqualityComparer<string> comparer)
             : base(capacity, comparer)
        { }
       //
        // 概要:
        //     指定した System.Collections.Generic.IDictionary`2 から要素をコピーして格納し、指定した System.Collections.Generic.IEqualityComparer`1
        //     を使用する、System.Collections.Generic.Dictionary`2 クラスの新しいインスタンスを初期化します。
        //
        // パラメーター:
        //   dictionary:
        //     新しい System.Collections.Generic.IDictionary`2 に要素がコピーされた System.Collections.Generic.Dictionary`2。
        //
        //   comparer:
        //     キーの比較時に使用する System.Collections.Generic.IEqualityComparer`1 実装。キーの型の既定の null を使用する場合は
        //     System.Collections.Generic.EqualityComparer`1。
        //
        // 例外:
        //   T:System.ArgumentNullException:
        //     dictionary は null です。
        //
        //   T:System.ArgumentException:
        //     dictionary に、1 つ以上の重複するキーが格納されています。
        public StringsModel(IDictionary<string, string> dictionary, IEqualityComparer<string> comparer)
            : base(dictionary, comparer)
        { }
        ////
        //// 概要:
        ////     シリアル化したデータを使用して、System.Collections.Generic.Dictionary`2 クラスの新しいインスタンスを初期化します。
        ////
        //// パラメーター:
        ////   info:
        ////     System.Runtime.Serialization.SerializationInfo をシリアル化するために必要な情報を格納している System.Collections.Generic.Dictionary`2
        ////     オブジェクト。
        ////
        ////   context:
        ////     System.Collections.Generic.Dictionary`2 に関連付けられているシリアル化ストリームの送信元および送信先を格納している
        ////     System.Runtime.Serialization.StreamingContext 構造体。
        //protected StringsModel(SerializationInfo info, StreamingContext context)
        //    : base(info, context)
        //{ }
    }
}
