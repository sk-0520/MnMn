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
		'version': '0.76.1',
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
						'subject': '#694: 色々な想定外でデバッグ版(ベータも含む)とリリース版が同時起動できない',
						'comments': [
							'消極的解決法',
							'私の知ってる名前付きパイプと違う……'
						]
					},
					{
						'revision': '',
						'subject': '#693: チャンネルIDに対するファインダーのフィルタが ch の関係でボロボロ',
						'comments': [
							'とりあえず先頭の ch 有無は問わないようにしたけどあんまりいい処理じゃないので今後としちゃあ "ch" 付きの ID がチャンネル ID としたい',
							'そもそもサービスが ch 付けてくれたらいいんだけどなぁと思ったけど動画に関係ないユーザー ID もマイリスト ID もプレフィックスつかない現状こちらの設計ミスですね、そうですねハイハイ',
							'結局のところサービスに対応するデザインというかどうあるべきかというかそんなところに着陸する悲しみ',
							'プレフィックス統一したいなぁ'
						]
					},
					{
						'revision': '',
						'subject': '#692: クラッシュレポートより: System.InvalidOperationException: このオブジェクトは別のスレッドに所有されているため、呼び出しスレッドはこのオブジェクトにアクセスできません',
						'comments': [
							'ひとまず Window から Player の Loaded に処理回したけどわっかんねーよばーかばーか',
							'再現できないんじゃ難しいねばーか'
						]
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
	},
	{
		'date': '2017/07/19',
		'version': '0.74.0',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'class': 'warning',
						'subject': '本バージョンでアップデート処理の不具合対応版を配布します',
						'comments': [
							'実際に使用されるのは次回バージョン以降になるので本バージョンアップデートに失敗する場合は下部の「旧アップデートを使用する」をチェックして運用回避してください'
						]
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': 'ae7a793922ec74e0c12d08a7cceb9469d1587755',
						'subject': '#672: チャンネルの動画ファインダーのエクスパンダ―が開発中ヘッダの文言のまま'
					},
					{
						'revision': 'd1612485c2865b378ad05ff95373b0947619b1f7',
						'subject': '#671: チャンネルのファインダーが読み込み済みでも特定操作で再読み込みを実施してしまう'
					},
					{
						'revision': '89875327c79738e3ae5c044669fb0c4e6947a1d9',
						'subject': '#676: HTML5 版で取得するデータの Json で謎い部分をとりあえず string から object にしとく',
						'comments': [
							'その項目が何かしらの (Jsonとしての)Object だったら (C#の)object でとるようにした',
							'データ保全っていうよりパース失敗対応',
							'パッと見たとこだけしか対応してないから発見次第対応していきたい',
							'見れない動画とか教えてもらえるとたいていこれ関係だろうから教えてくれるとありがたい'
						]
					},
					{
						'revision': '24e49057830abc7a3829c4f31b66ad04137c82e0',
						'subject': '#669: チャンネルのファインダーではフィルタを無効にすべし'
					},
					{
						'revision': 'bcfbe52774cf473f1fb51c33e49c73f5dd9d93d5',
						'subject': '#661: もっさい件数表示を今風にする'
					},
					{
						'revision': '32b168519603d5ea40d8d0fc2c47da7759feeb46',
						'subject': '#552: (新)アップデート処理がファイルハンドル掴んでて成功しない',
						'comments': [
							'まさかの DLL (セルフ)ハイジャック',
							'細かい調査は課題の方に書いといた'
						]
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': 'e5a158044f8f9ce38d58fa76c33d3c0ac0a63556',
						'subject': '0.69.1 の更新履歴がなんだかダブってた'
					},
					{
						'revision': 'b0db5d3f1fc259125691507d009fa7a68bb3553f',
						'class': 'reopen',
						'subject': '#667: 開発のメールアドレス変える',
						'comments': [
							'エスケープしてなかった'
						]
					}
				]
			}
		]
	}
];
