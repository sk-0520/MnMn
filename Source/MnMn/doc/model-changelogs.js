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
		'version': '0.71.1',
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
						'subject': '#652: あとで見るのデータ構造に「どこから」を持たせる'
					},
					{
						'revision': '',
						'subject': '#653: あとで見るがどこから設定されたのか視覚表示する'
					},
					{
						'revision': '',
						'subject': '#643: 使用許諾になぜ表示しているかの旨を表示する',
						'comments': [
							'誰も読んでない気がする',
							'元々小難しいこと書いてないけど GPL だから使用者の責任下でって意味を強調して最下部に「本文書内容を理解した」を追加した',
							'ちょい面倒だけどチェックしないと使えないようにしておいた',
							'君が責任を負いたくないように私も責任を負いたくない擦り合い社会'
						]
					},
					{
						'revision': '',
						'subject': '#630: スクロール可能タブに一覧メニューを表示する'
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
						'class': 'reopen',
						'subject': '#655: タグから大百科開くボタンの活性タイミングが何かしらのイベント後で気持ち悪い',
						'comments': [
							'こんなしょうもない問題で再オープンという失態'
						]
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
						'subject': '#599: ビルド手順の明文化'
					},
					{
						'revision': '',
						'class': 'nuget',
						'subject': 'HtmlAgilityPack 1.4.9.5 -> 1.5.0'
					},
					{
						'revision': '',
						'subject': '#657: 環境情報に app.config の内容が必要'
					},
					{
						'revision': '',
						'subject': '#658: あとで見るの各タブにアイコン付与して冗長的な文言破棄'
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
	},
	{
		'date': '2017/06/20',
		'version': '0.69.1',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'subject': '#641 により緊急リリース',
						'comments': [
							'試験してないから頼むべ',
							'追々 #642 で調査する'
						]
					},
					{
						'revision': '',
						'subject': '検索ワードのピン止めを廃止しました',
						'comments': [
							'ブックマークになりました',
							'ピン止めデータは一応残ってるけど短い命です(SearchPinItems)',
							'ロジックがピン止めよりややこしくなりました'
						]
					}
				]
			},
			{
				'type': 'features',
				'logs': [
					{
						'revision': '264e081c0e94a1339d03fd1d6bd75a7eb7f0de05',
						'subject': '#605: ファインダーコンテキストメニューから視聴ページURIをクリップボードにコピーする'
					},
					{
						'revision': 'cc24dad23535f0676a754f55a6e369de683d1e36',
						'subject': '#606: ファインダーからプレイリストへ追加',
						'comments': [
							'酷使するプレイヤー人追加する処理のみ実装',
							'他のプレイヤーに追加するのはちょっとなんというか、手間だったのでコメントアウト(SmileVideoFinderControl.xaml:464)した',
							'やりたきゃ自分でやってくれ、そいでそのソースくれ'
						]
					},
					{
						'revision': 'd60a6b31b0283afe672a44682493bd8c46b534db',
						'subject': '#615: ユーザータブ内の各タブをマウススクロール可能にする'
					},
					{
						'revision': '06373c07c96d5b9c9e2ee5c784a7d48990c0c309',
						'subject': '#614: フィルタタブ内のタブをマウススクロール可能にする'
					},
					{
						'revision': '5481f20591ae7a8f8f6ac992003fba983a28bcdc',
						'subject': '#612: コメントリストの自動スクロールを一時的に抑制する'
					},
					{
						'revision': '4ecddc25a7657d669cbdda61fde9df78b2166f90',
						'subject': '#620: 検索ワードをブックマークする'
					},
					{
						'revision': 'a6902c7756e52d199fe8c128850c9dc3b96f85bb',
						'subject': '#621: ブックマークした検索ワードの差分更新チェックを行う',
						'comments': [
							'ピン止めみたいに検索結果のタブのコンテキストメニューから「ブックマーク」を選択して設定',
							'ブックマークした各項目は検索タブ内の左上にあるブックマークアイコン付きのトグルボタン方ら表示・非表示を切り替え',
							'タグ検索ワードのブックマークが差分更新対象となる',
							"検索自体は、あー、これ、<MnMn>\\etc\\define\\service\\smile\\video\\uri-list.xml: //item[@key='video-tag-feed']",
							'内部状態半端なくやべぇ'
						]
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': 'e650885067c37c1c5cf4ecd7f84bc2b9355843a0',
						'subject': '#641: DMC形式のDLが出来ない',
						'comments': [
							'んもう！'
						]
					},
					{
						'revision': '2d6464bfe1b0836af874b4381b601398bdcaa18f',
						'subject': '#608: コメントの色定義が甘い',
						'comments': [
							'色定義を外部化',
							"<MnMn>\\etc\\define\\service\\smile\\video\\msg.xml"
						]
					},
					{
						'revision': '448253747f149b220e881c93ee96f6f992f7412d',
						'subject': '#610: UI 拡縮機能はタイトルバーからステータスバーに移動させる',
						'comments': [
							'ついでにプレイヤーのステータスバーの高さを少し低くして情報欄表示ボタンのスタイル変更'
						]
					},
					{
						'revision': 'b6ac91526f3e326c5e736d464413876c48194a03',
						'subject': '#600: ブックマークに同一アイテムを設定できないようにする',
						'comments': [
							'既存の重複データ起動時に破棄され、該当処理は #611 で数世代保守する予定',
							'2-3 世代で保守終わらせたいなぁ'
						]
					},
					{
						'revision': '8b8adc5dccc7efd01ce6d3084503b81031492ebd',
						'subject': '#601: プレイリストに同一アイテムを設定できないようにする'
					},
					{
						'revision': '0a80aae66f6ccab75362db3ad69107f76312766a',
						'subject': '#616: 視聴履歴を内部的に1000件くらい持っててもいいんじゃないですかね',
						'comments': [
							'視聴履歴としては内部的に 5000 件保持するようにした',
							'履歴表示としては今までと同じで 500 件にした',
							'これ内部的にはいくら保持してもいいんだけど表示側を考えると制限せざるを得ないのですよ',
							'その理由はかなり昔(2016年夏セミ全滅期くらい)に 1000 件から 500 件に変えた時と同じで件数多いとサムネイルやらタグやらで待ち時間がぱねぇ',
							'なので今回は上辺の表示と内部の保持数を分離した(デバッグしてないからミスってて逆だったらめんご)',
							'ここをページャ形式で分割するとしても、既存処理使っても実装がだるい ;-)'
						]
					},
					{
						'revision': '38d3d130987e96c409a735b3b235c1001a3a2037',
						'subject': '#214: ブックマークなどの追加処理に対する方向を固定する',
						'comments': [
							'新しいものが上、古いものが上になるように共通処理実装',
							'ただしプレイリストのブックマークは最後部に追加するのでここだけ処理イメージが違う',
							'本件とはあんまり関係ないけど履歴系も処理共通化'
						]
					},
					{
						'revision': '80432fae679fba0192848e364873eb11d15f46cc',
						'subject': '#626: #419 のキーバインドがヘルプに記載されてない',
						'comments': [
							'F7 で音声ミュート切り替えの件'
						]
					},
					{
						'revision': 'e3891fd256356428e84a2e2f59cc30c429328fe8',
						'subject': '#628: 無効ファイルの設定されたラボプレイヤーでキャッシュを開こうとすると死ぬ'
					},
					{
						'revision': '2ef8b173e42b3da794d3d8f1123b9e37032d026a',
						'subject': '#609: プレイヤー管轄の再生状態をローカライズする'
					},
					{
						'revision': '3d6810ac12dd6e2b9b5be22f65164c61351ef2c9',
						'subject': '#619: 検索ワードのピン止め機能を廃止する'
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': '584c7829c7a36400da55c6f5521f6e3474a1b3dc',
						'subject': '#617: コメントサイズの外部化',
						'comments': [
							'てっきり外部化してると思ったけど完全に定数埋め込みで恥の多い実装を送って来ました',
							'通常コメントサイズに対して以下キーを掛けて最終サイズを決定する',
							'`service-smile-smilevideo-player-comment-big`',
							'`service-smile-smilevideo-player-comment-small`'
						]
					},
					{
						'revision': '7d3dc5033b93217ef80dc753ac158fd438bc8b0f',
						'subject': '#631: App.config に入っていない定数を追い出す',
						'comments': [
							'`service-smile-user-data-cache-time`: ニコニコユーザー情報キャッシュ時間',
							'`service-smile-user-image-cache-time`: ニコニコユーザー画像キャッシュ時間',
							'`service-smile-mylist-cache-time`: ニコニコマイリストキャッシュ時間',
							'`service-smile-market-image-cache-time`: ニコニコ市場サムネイルキャッシュ時間',
							'`service-smile-smilevideo-thumb-cache-time`: ニコニコ動画情報キャッシュ時間',
							'`service-smile-smilevideo-msg-cache-time`: ニコニコ動画コメントキャッシュ時間',
							'`service-smile-smilevideo-relation-cache-time`: ニコニコ動画関連動画情報キャッシュ時間',
							'`service-smile-smilevideo-check_it_later-cache-time`: ニコニコ動画あとで見る の保持有効期間',
							'`service-smile-live-information-cache-time`: ニコニコ生放送動画情報キャッシュ時間'
						]
					}
				]
			}
		]
	}
];
