using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using Meta.Vlc;

[assembly: AssemblyVersion("0.83.6.*")]

#region あまり変わらない部分

[assembly: AssemblyInformationalVersion("xxxxxxxxxxxxxxDEVELOPMENTxxxxxxxxxxxxxxx")]

// アセンブリに関する一般情報は以下の属性セットをとおして制御されます。
// アセンブリに関連付けられている情報を変更するには、
// これらの属性値を変更してください。
[assembly: AssemblyTitle("MnMn")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("MnMn")]
[assembly: AssemblyCopyright("Copyright © 2016-<YEAR> sk <content-type-text.net>")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// ComVisible を false に設定すると、その型はこのアセンブリ内で COM コンポーネントから
// 参照不可能になります。COM からこのアセンブリ内の型にアクセスする場合は、
// その型の ComVisible 属性を true に設定してください。
[assembly: ComVisible(false)]

//ローカライズ可能なアプリケーションのビルドを開始するには、
//.csproj ファイルの <UICulture>CultureYouAreCodingWith</UICulture> を
//<PropertyGroup> 内部で設定します。たとえば、
//ソース ファイルで英語を使用している場合、<UICulture> を en-US に設定します。次に、
//下の NeutralResourceLanguage 属性のコメントを解除します。下の行の "en-US" を
//プロジェクト ファイルの UICulture 設定と一致するよう更新します。

//[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]


[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //テーマ固有のリソース ディクショナリが置かれている場所
                                     //(リソースがページ、
                                     //またはアプリケーション リソース ディクショナリに見つからない場合に使用されます)
    ResourceDictionaryLocation.SourceAssembly //汎用リソース ディクショナリが置かれている場所
                                              //(リソースがページ、
                                              //アプリケーション、またはいずれのテーマ固有のリソース ディクショナリにも見つからない場合に使用されます)
)]

[assembly: VlcSettings(@"lib\LibVlc")]

#endregion

