var changelogs = [
	/*
						'class': 'compatibility' 'notice' 'nuget' 'myget' 'warning' 'open' 'reopen',
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
		'version': '0.77.1',
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
	{
		'date': '2017/07/29',
		'version': '0.77.0',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'subject': '私が見ている間はクラッシュレポートあんまり飛んでこないのに仕事行ったり遊んだり寝たりした後に確認したらやたらクラッシュレポート飛んできている謎'
					}
				]
			},
			{
				'type': 'features',
				'logs': [
					{
						'revision': '7e276ea35aaafa5a1399a483050ff7986ca7d5d3',
						'subject': '#696: 動画プレイヤーのタスクバーボタン実装'
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '970a163cbba29a5e5ca4267d70ecff8930cadb54',
						'subject': '#694: 色々な想定外でデバッグ版(ベータも含む)とリリース版が同時起動できない',
						'comments': [
							'消極的解決法',
							'私の知ってる名前付きパイプと違う……'
						]
					},
					{
						'revision': 'd4f0a37e4c40f83d70319200cd324017fdc63ea8',
						'subject': '#693: チャンネルIDに対するファインダーのフィルタが ch の関係でボロボロ',
						'comments': [
							'とりあえず先頭の ch 有無は問わないようにしたけどあんまりいい処理じゃないので今後としちゃあ "ch" 付きの ID がチャンネル ID としたい',
							'そもそもサービスが ch 付けてくれたらいいんだけどなぁと思ったけど動画に関係ないユーザー ID もマイリスト ID もプレフィックスつかない現状こちらの設計ミスですね、そうですねハイハイ',
							'結局のところサービスに対応するデザインというかどうあるべきかというかそんなところに着陸する悲しみ',
							'プレフィックス統一したいなぁ'
						]
					},
					{
						'revision': 'abdb69cb3505d1e9bb4023819049cb9a6fca5743',
						'subject': '#692: クラッシュレポートより: System.InvalidOperationException: このオブジェクトは別のスレッドに所有されているため、呼び出しスレッドはこのオブジェクトにアクセスできません',
						'comments': [
							'ひとまず Window から Player の Loaded に処理回したけどわっかんねーよばーかばーか',
							'再現できないんじゃ難しいねばーか'
						]
					},
					{
						'revision': '34661fffcc8513e53790e2a2a7abc8ca6bb8c67f',
						'subject': '#691: 同一秒内に未検出例外があった場合、クラッシュレポート用プログラムが裏で死んだままの場合がある',
						'comments': [
							'普段はまぁ別にいいんだけどアップデート前に裏で生きたままだとあれよね'
						]
					},
					{
						'revision': 'c8c360e891373bc4c398ee1edf3e8498d6c88ab1',
						'subject': '#699: クラッシュレポートより: 動画情報の動画種別取得時に NullReferenceException',
						'comments': [
							'死ぬんじゃなくて気絶するようにした'
						]
					},
					{
						'revision': '776a0d6bd546bd949c57ddf6931106d79fe2434e',
						'subject': '#702: フィルタ登録 UI を整理したいね、少なくとも新規作成ボタンが下の方に消えていくのは何とかしてあげよう'
					},
					{
						'revision': '053d242fe5bffc4d8147a6e337b7a8ca6aa42f4d',
						'subject': '#703: 動画再生が公式ですら死んでるやつあるね、怖いね',
						'comments': [
							'公式で DMC 形式が正式実装された後の動画にも関わらず HTML5 版だと再生に必要な情報がないにも関わらず再生しようとして失敗して後方互換のための Flash 版に手動遷移を促すにも関わらず Flash 版だと DMC 形式でデータが降ってくるにも関わらずプレミアムと一般でこの辺りの切り替えが必要・不要な動画が生息してるとか怖い以前に理解が追い付かないよね',
							'公式ェ……',
							'#665 で消した処理とか復帰させたからもうグッチャグチャ、この辺りの後方互換の保守はさすがに厳しい',
							'DMC 形式配信版は全部新しい方にきれいに移ったと思ってたんだけどこんなもん知るかよばーか',
							'そういった場合は強制的に Flash 版に遷移するようにしたんだけどスレッド無視で Cookie 共有してるから何かのタイミングで競合したらゴメンよ'
						]
					},
					{
						'revision': 'a8864311278aa149c628e6a5cd9a0979e41d1cf0',
						'subject': '#704: クラッシュレポートより: 再生ウィンドウからマイリスト追加時にエラー(System.NullReferenceException )',
						'comments': [
							'クラッシュレポートに操作内容書いててくれたから調べるの楽だった。ありがとさん'
						]
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': 'f1274ecd7ec7a56c5d704f2d34cc976610528a99',
						'subject': '#700: 共通処理/サービスの正規表現を外部に退避'
					}
				]
			}
		]
	},
	{
		'date': '2017/07/23',
		'version': '0.76.0',
		'isRc': false,
		'contents': [
			{
				'type': 'features',
				'logs': [
					{
						'revision': 'e602ada0ad65ccf853fbf622724902b2f54767bd',
						'subject': '#687: プレイヤーに投稿者のサムネイルも表示する'
					},
					{
						'revision': '0e354b3b11bc9a7b0b84614637c2a5c894bba69a',
						'subject': '#148: コマンドラインオプションから動画再生やその他操作を行う',
						'comments': [
							'まぁダメなんじゃないかな',
							'非同期操作を同期操作で UI スレッドデッドロック無視するためのブン投げドロドロで処理するからめっちゃくちゃ',
							'詳細はヘルプを参照のこと'
						]
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '5bd4bfb51f654277694581c47790cd15ab46db2f',
						'subject': '#686: ターゲット配列の長さが足りません',
						'comments': [
							'調査した限り大問題で今までうまくいってたのがむしろ奇跡',
							'あとで見るの件数取得という感覚的には安全極まりない処理に悪魔が潜んでいた'
						]
					},
					{
						'revision': '5274c2e3d3502a2921df59d5bb1f4a5809d73517',
						'subject': '#688: 灰色テーマのスクロールバーが視認出来たもんじゃない',
						'comments': [
							'背景を少し濃くした投げやり対応'
						]
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': '8bd3433b711dd48d82ee67774d9531bd851e8a5c',
						'subject': '#690: ソース整理'
					}
				]
			}
		]
	},
	{
		'date': '2017/07/22',
		'version': '0.75.0',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'subject': 'このバージョンアップで 0.74.0 で配布した修正版アップデート機能が正式に走る！',
						'comments': [
							'試験も結構したしβ版も試したしいけるさ',
							'大丈夫さ',
							'いってくれ'
						]
					}
				]
			},
			{
				'type': 'features',
				'logs': [
					{
						'revision': '2ad9d085c2dbf120b02c120e9bbaaba2ba6345b7',
						'subject': '#674: ファインダーのコピー機能でユーザーとチャンネルの区別は不要'
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '73715d72f69f577e49c99286d4a03a16322aa2a9',
						'subject': '#670: チャンネルブックマークの検証・調整があまあま',
						'comments': [
							'もうワケわからん',
							'設計せずに TabControl と WindowsFormsHost 合わせるとダメだね'
						]
					},
					{
						'revision': '73715d72f69f577e49c99286d4a03a16322aa2a9',
						'subject': '#673: 元々頭おかしい内蔵ブラウザがチャンネルタブを閉じた際に頭狂う',
						'comments': [
							'#670 と同時'
						]
					},
					{
						'revision': '85fec1cb66b195924b1a3a76964041dd9f407de5',
						'subject': '#683: 酷使するプレーヤー → メインプレイヤー に改名する'
					},
					{
						'revision': '6dc9704b1b14b927677a1dae46d6a0d6863c26cd',
						'subject': '#684: チャンネルIDコピー時の操作でID番号に "ch" を付与すべき'
					},
					{
						'revision': 'b5cf843feb412538e8fafb4351240aa765a403ad',
						'subject': '#682: 秒間ダウンロード表示部がガックガクに動くの鬱陶しい',
						'comments': [
							'6:##0.00 これで気持ち抑えられたかな！'
						]
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': '0e14f717ae7bf436899069fb08c0f1623c7e1c29',
						'subject': '#504: DMC形式の処理を独立',
						'comments': [
							'＼(^_^)／ばんざーい',
							'DMC 周りの不安定さが解消できたと信じてる'
						]
					},
					{
						'revision': '1f789113e6b03d28437b2ffedbfbd0c371988023',
						'class': 'nuget',
						'subject': '#681: HtmlAgilityPack 1.5.0 -> 1.5.1'
					},
					{
						'revision': '4ecf8d3bb6a85a9b310fc86f34195d885206954e',
						'class': 'nuget',
						'subject': '#680: Geckofx 45.0.31 -> 45.0.32'
					},
					{
						'revision': '9b229dbdab6c15f9037c6e47d72d8009e92351d8',
						'class': 'nuget',
						'subject': '#679: Extended WPF Toolkit 3.0.0 -> 3.1.0'
					}
				]
			}
		]
	}
];
