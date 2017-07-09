var changelogs = [
	/*
						'class': 'compatibility' 'notice' 'nuget' 'myget' 'warning' 'open',
						'comments': [
							''
							['alt', 'image-path']
						]
	*/
	//---------------------------------------------
	/*
	{
		'date': 'YYYY/MM/DD',
		'version': '0.xx.1',
		'isRc': true,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'subject': ''
					},
					{
						'revision': '',
						'subject': ''
					},
					{
						'revision': '',
						'subject': ''
					}
				]
			},
			{
				'type': 'features',
				'logs': [
					{
						'revision': '',
						'subject': ''
					},
					{
						'revision': '',
						'subject': ''
					},
					{
						'revision': '',
						'subject': ''
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '',
						'subject': ''
					},
					{
						'revision': '',
						'subject': ''
					},
					{
						'revision': '',
						'subject': ''
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': '',
						'subject': ''
					},
					{
						'revision': '',
						'subject': ''
					},
					{
						'revision': '',
						'subject': ''
					}
				]
			}
		]
	},
	*/
	{
		'date': 'YYYY/MM/DD',
		'version': '0.72.1',
		'isRc': true,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'subject': ''
					},
					{
						'revision': '',
						'subject': ''
					},
					{
						'revision': '',
						'subject': ''
					}
				]
			},
			{
				'type': 'features',
				'logs': [
					{
						'revision': '',
						'subject': '#644: 外部プログラム設定などで使用する置き換え可能文字列を自動入力できるようにする'
					},
					{
						'revision': '',
						'subject': ''
					},
					{
						'revision': '',
						'subject': ''
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '',
						'subject': '#660: ユーザータブ一覧メニュー表示の項目表示が出来てない'
					},
					{
						'revision': '',
						'subject': ''
					},
					{
						'revision': '',
						'subject': ''
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': '',
						'subject': ''
					},
					{
						'revision': '',
						'subject': ''
					},
					{
						'revision': '',
						'subject': ''
					}
				]
			}
		]
	},
	{
		'date': '2017/07/08',
		'version': '0.72.0',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'subject': '0.71.0 で対応した新アップデート処理が動く。動いて'
					}
				]
			},
			{
				'type': 'features',
				'logs': [
					{
						'revision': 'd79a1672090cef3da455f428b77d5464b6648bf5',
						'subject': '#652: あとで見るのデータ構造に「どこから」を持たせる'
					},
					{
						'revision': 'cf18d04efb6a6015dc1c29056f54b0c109c39704',
						'subject': '#653: あとで見るがどこから設定されたのか視覚表示する',
						'comments': [
							'既存のあとで見るデータは表面上「手動」扱い',
							'内部的には Unknown 扱い'
						]
					},
					{
						'revision': 'cc030de859afb3c32b0d29db0f6a53a96dd19102',
						'subject': '#643: 使用許諾になぜ表示しているかの旨を表示する',
						'comments': [
							'誰も読んでない気がする',
							'元々小難しいこと書いてないけど GPL だから使用者の責任下でって意味を強調して最下部に「本文書内容を理解した」を追加した',
							'ちょい面倒だけどチェックしないと使えないようにしておいた',
							'君が責任を負いたくないように私も責任を負いたくない擦り合い社会'
						]
					},
					{
						'revision': '833f7ae6a837d16bcd0f4b8ac6751a2c858f3595',
						'subject': '#630: スクロール可能タブに一覧メニューを表示する'
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '12d21472e64fbdcf945e4fde58c1b61e1e50eced',
						'class': 'reopen',
						'subject': '#655: タグから大百科開くボタンの活性タイミングが何かしらのイベント後で気持ち悪い',
						'comments': [
							'こんなしょうもない問題で再オープンという失態'
						]
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': '86a4551f7855aefa2c73e3df2be3c0e7f7fd6d60',
						'subject': '#599: ビルド手順の明文化'
					},
					{
						'revision': 'fd211a9b8f6cd3f5e3a889683569ebd1e7f95e0d',
						'class': 'nuget',
						'subject': '#656: HtmlAgilityPack 1.4.9.5 -> 1.5.0'
					},
					{
						'revision': 'ce1e7c0df66d450731a8435b3dbbff50230444bc',
						'subject': '#657: 環境情報に app.config の内容が必要'
					},
					{
						'revision': '1071da88db3be0ef7df685290e21a8e648d17d58',
						'subject': '#658: あとで見るの各タブにアイコン付与して冗長的な文言破棄'
					}
				]
			}
		]
	},
	{
		'date': '2017/07/01',
		'version': '0.71.0',
		'isRc': false,
		'contents': [
			{
				'type': 'features',
				'logs': [
					{
						'revision': 'c4c2e38495bbb6316fca69d6eedd22431d514077',
						'subject': '#646: タグがランキングに存在すればランキングを表示できるようにする'
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '019ab94d23c001a6e0db851551f6dca7abe1dd25',
						'class': 'open',
						'subject': '#552: (新)アップデート処理がファイルハンドル掴んでて成功しない',
						'comments': [
							'環境変数の Path をね、除外したの',
							'次のアップデートで動くの'
						]
					},
					{
						'revision': '0331f27c82f2ec3546ab84e5ab8698ee5fcc340e',
						'subject': '#654: プレイヤーメニュー部の高さがでこぼこしてる'
					},
					{
						'revision': '9bdd5afcd7ef1e44d5d70896b2f7c1ef3da6a92f',
						'subject': '#655: タグから大百科開くボタンの活性タイミングが何かしらのイベント後で気持ち悪い'
					},
					{
						'revision': 'cb415862ef6380bc642e80b8c27a6374e6564a13',
						'subject': '#454: 市場の画像が取得できない'
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': '76ad96847bfee39ad972d94c14b631c338edd3b5',
						'subject': '#629: 精神衛生のための ReadOnly',
						'comments': [
							'さすがに全部はしんどい、以後 見つけて手が空いてれば対応する'
						]
					},
					{
						'revision': '8fe70025c8380f4e173cf690ff493f0607484528',
						'subject': '#607: 更新履歴を一定の件数でファイル分割したい'
					}
				]
			}
		]
	},
	{
		'date': '2017/06/25',
		'version': '0.70.0',
		'isRc': false,
		'contents': [
			{
				'type': 'features',
				'logs': [
					{
						'revision': '4526dcacc9a1f0bc0da5e39f0094005d9e1e2122',
						'subject': '#623: 設定ファイルのアーカイブを gzip から zip に置き換える',
						'comments': [
							'*.gz のローテート処理は #639 で数世代保守する',
							'間隔が30世代だから余裕見て 2-3 か月くらいは保守せにゃなぁ'
						]
					},
					{
						'revision': 'e24717b31000cf5d7f6424c09d9e6673a08216bd',
						'subject': '#624: ニコニコのアイコンをベクターにする',
						'comments': [
							'ダサい',
							'絵心がない',
							'Inkscape を使う技術もない',
							'だれか SVG か XAML で単独のパスデータくれくれ'
						]
					},
					{
						'revision': '29766ed09c7ebbe4dfd7bf2cf38cbcaa8f1de0ee',
						'subject': '#633: ログ出力用のバッチファイル作成',
						'comments': [
							"<MnMn>\\bat\\log.bat",
							'実行するとデスクトップにログを出力',
							'今回実装はバッチの作成と独自環境変数の展開ってだけで機能的には最初期から存在してた'
						]
					},
					{
						'revision': 'f075fd524625ff17b8e7f5c510ad383f6f35e370',
						'subject': '#645: 動画プレイヤー(ウィンドウ状態)の最前面表示設定を行う',
						'comments': [
							'プレイヤー -> 設定 -> 最前面設定',
							'再生中のみ最前面設定はクセがあるので細かい制御は `service-smile-smilevideo-player-active-topmost-playing-restart` でよろしく。正直何が正しいのか分からん',
							'つーか WPF でところどころバインドが変な挙動するの勘弁してほしいね、Forms ちっくに書いてしまった'
						]
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '4ce36a55d830bb937d0d3b119a01498f31ba5023',
						'subject': '#637: 検索フィードの更新待ちカウントがTimeSpanで狂ってる'
					},
					{
						'revision': '47d3526a1f54d305dc2add69e86e5143ca67094f',
						'subject': '#636: 検索ブックマークコンテキストメニュー初回表示が隠れる'
					},
					{
						'revision': '17cb9b0d8cddc3ca3beee3315e197b3ccaf5a0f3',
						'subject': '#535: クラッシュレポートより: System.InvalidCastException: 指定されたキャストは有効ではありません。'
					},
					{
						'revision': '23d9a656956780f6d81b8b94efcc694f47f5ab3a',
						'subject': '#634: i と u が近くて vuew-scale だから恥ずかしい'
					},
					{
						'revision': 'c192496e0ff0822c6cd5ced73a5d0ad5b62ffe9d',
						'subject': '#648: Twitterアイコンが気にくわん'
					},
					{
						'revision': 'fc4e0f7244c7ad83e7c4d5dc38ee50806940d515',
						'subject': '#649: プレイヤー上部メニューの「プレイヤー領域」を「領域」に変更'
					},
					{
						'revision': '54aed368ec7b59bc93730307773e5c0f3cfa506e',
						'subject': '#618: 酷使するプレイヤーの切り替えが多分これ開発者しか操作できてないのでわかりやすくする',
						'comments': [
							'プレイヤー -> メニュー -> 酷使するプレイヤー',
							'ステータスバーからメニュー部に移動'
						]
					},
					{
						'revision': '6aa7b264518231fbb108bd5609e40011c1ee160f',
						'subject': '#650: おみくじ状態でラボプレイヤーが落ちる',
						'comments': [
							'分からん',
							'一応手を入れたけどまだおみくじ状態継続'
						]
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': '6b0b191b68d59c17944f63c1e6e2e84d875e2811',
						'subject': '#635: twitter互換用コードの配置ディレクトリを作るの忘れてる'
					},
					{
						'revision': '1f24650cdbd36fd2ac3029d6b14d99398e7f526a',
						'class': 'nuget',
						'subject': '#638: Newtonsoft.Json 10.0.2 -> 10.0.2'
					},
					{
						'revision': 'b477b541e3f9cfbb40a4be577380eefa98ac2f63',
						'class': 'nuget',
						'subject': '#647: Meta.Vlc　16.11.19 -> 17.6.20'
					}
				]
			}
		]
	}
];
