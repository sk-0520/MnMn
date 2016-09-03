var changelogs = [
	/*
						'class': 'compatibility' 'notice' 'nuget' 'myget'
						'comments': [
							''
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
		'version': '0.15.1',
		'isRc': true,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'class': 'compatibility',
						'subject': '#105 実装に伴い 0.16.0 未満のコメント設定(フォントやサイズ等)が失われます'
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
						'subject': '#112: 動画個別設定に最終再生日時を保持させる'
					},
					{
						'revision': '',
						'subject': '#11: マイリストの並び替え'
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
						'subject': '#122: せきゅ®ひてぃ保護'
					},
					{
						'revision': '',
						'subject': '#105: プレイヤー情報はプレイヤーウィンドウ破棄時に保存させる',
						'comments': [
							'コメント設定周りを一新したため過去の設定部分を破棄',
							'バージョンが 1.0.0 以下だと気楽に互換切れて開発側に優しい！'
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
						'subject': '0.15.0 の更新履歴で #42 の後続課題が #120 でなかった部分を修正',
						'comments': [
							'単独更新履歴の方は手で修正'
						]
					}
				]
			}
		]
	},
	{
		'date': '2016/09/04',
		'version': '0.15.0',
		'isRc': false,
		'contents': [
			{
				'type': 'features',
				'logs': [
					{
						'revision': '5793ef45f3f0ea5a1253a5fd151e213ad5fbf2a2',
						'subject': '#113: プレイヤーにリサイズグリップをつける'
					},
					{
						'revision': '00aabc3cad8c661fad08cd4dbc8f2221e7e1ff56',
						'subject': '#104: コメントフィルタタブをまとめましょう',
						'comments': [
							'そう頻繁にさわるものじゃないと思うので1タブの中に2タブ入れておいた',
							'全体フィルタ設定の使用・不使用をトグルボタンからチェックボックスに変更した'
						]
					},
					{
						'revision': '5f206bd67cf5eeb9c2c760de57aad5c1724e0dec',
						'subject': '#102: コメント一覧の表示状態を通常ウィンドウとフルスクリーンで分ける'
					},
					{
						'revision': 'f356e251333d5e542b15f3123916f3c954918be9',
						'subject': '#42: WebBrowserコントロールの制御',
						'comments': [
							'ヘルプは本対応では厳しかったので後回し(#120)',
							'WebBrowserほんと嫌なんだけど',
							'百歩譲って Forms はいいよ。でもせっかく WPF 使ってるのに Trident 強制って言うやるせなさよ'
						]
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '7d81b8fa7a0deb748cbec50d4512daa7c5b643a3',
						'subject': 'ヘルプのメールアドレスが古かった'
					},
					{
						'revision': 'd0bdceb6966f4ea08f45cb47f5a6b62fb3e4a208',
						'subject': '#107: ログ表示機能のスクロールとか表示とかの制御'
					},
					{
						'revision': '4e0272849f1375c3abca527cfa4cc2c6bb24520d',
						'subject': '#119: マイリスト・動画のブックマーク位置移動で落ちる'
					},
					{
						'revision': 'd55877b933b9a2bb041f8ff9983d175d6b2089ff',
						'subject': '#121: マイリストをブックマークに追加した時に該当ブックマーク内の動画がすべて「あとで見る」動画としてマークされる',
						'comments': [
							'動画一覧が読み込めていない、つまりは動画IDの一覧が取得できていない状態でブックマークに登録した後にあとで見るチェックが走ると全件がブックマーク設定時からの差分判定されてた',
							'なので動画一覧が読み込めた状態でブックマーク登録出来るようにした'
						]
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': '6d37015a6cf816f4ba9685c601a4da8b1eb99978',
						'subject': '#110: 動画情報・ダウンローダー・プレイヤーの歪な役割分担を誰かに押し付けたい'
					},
					{
						'revision': '96817004c59acf6ec4904fda65f12f4044cb904e',
						'subject': 'マイリストの定義ファイルからプロパティに文字列を移した'
					},
					{
						'revision': 'e8f03b26b6f4ba1fbe730771bca949b5f7c5cee3',
						'subject': '関連動画のソート関連を App.config に移動'
					},
					{
						'revision': '8871220543c38cade28b5dd7e81568e38021a475',
						'subject': '#88: 各種マネージャの子マネージャの初期表示処理が呼び出されないからわざわざ回避してる手順をなくしたい',
						'comments': [
							'テストしてないけど動くっしょ',
							'動くっしょ',
							'動いてね'
						]
					}
				]
			}
		]
	},
	{
		'date': '2016/08/29',
		'version': '0.14.0',
		'isRc': false,
		'contents': [
			{
				'type': 'features',
				'logs': [
					{
						'revision': '280384a2ad854ae96438a526e2851d4cdd2e14bd',
						'subject': '#93: プレイヤーの動画部分でカーソルを非表示にする',
						'comments': [
							'ちょっとだけ制限として #82 にもあったけどコメントとカーソルがかぶる場合は未考慮',
							'コメント側の当たり判定消せばそれで終いなんだけど将来的にコメントクリックしてどうこうとかあるかもしれないから今はまだ判定消せない'
						]
					},
					{
						'revision': '2a8fdd548d407aa028c2f9f5b3aca41e57862f40',
						'subject': '#99: 次回リリースの表示内容を更新できるようにする',
						'comments': [
							'うーん、リリース前の俺しか使わんしブックマークしてるしで無意味感半端ない'
						]
					},
					{
						'revision': '2ee3654609aef393d66f2bdbd87ec7ea6c24a552',
						'subject': '#97: アプリケーション側履歴削除時はリスト全体の再読み込みは行わない',
						'comments': [
							'手塩にかけて構築した読込手順外の処理で泥臭い感じだけど操作性は上がった'
						]
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '1ed5fa819a137468e93bf76f68e7e05fca844279',
						'subject': '#100: メモリ消費を抑える',
						'comments': [
							'プレイヤーウィンドウ破棄時にメモリ解放処理を追加',
							'GC: LOHを含む',
							'Meta.VLC: Dispose呼び出し。これに関しては過去の挙動があれなもんなんで try ... catch(Exception) した'
						]
					},
					{
						'revision': '1fd163c05bee6fb0c7ae28fddaae82137bc11618',
						'subject': '#94: 投稿者コメント位置制御のアイコンが他の子たちと微妙にサイズが違う'
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': 'ae5355efc197b6e858bc6024e174c61375eeb3dd',
						'subject': '#90: ソースが重い。特にプレイヤー周りが腐り過ぎてる',
						'comments': [
							'むりだった！'
						]
					},
					{
						'revision': 'bc1ed9316c3a915e8bb243ccf172632af978813d',
						'subject': 'コメント表示の際のデバッグメッセージ破棄'
					}
				]
			}
		]
	},
	{
		'date': '2016/08/28',
		'version': '0.13.0',
		'isRc': false,
		'contents': [
			{
				'type': 'features',
				'logs': [
					{
						'revision': 'b7378fa1d7b7e68a07be338908276c771b6505a7',
						'subject': '#82: フルスクリーン表示',
						'comments': [
							'本実装に伴いプレイヤーのウィンドウ UI が Windows 標準のものと変わります',
							'UI 変更とはいうものの UI 実装はまだきちんとする気がないのですごく適当です',
							'フルスクリーンの解除は再度フルスクリーンボタンを押下する以外には、ESCキー押下・プレイヤー部分のダブルクリックになります',
							'(バグみたいなもんだけど仕様として)ただしコメントをダブルクリックした場合なにも起こりません'
						]
					},
					{
						'revision': '520140577832aefbbb91c4fba553747c06d54cff',
						'subject': '#87: 開発中内容を一覧表示する'
					},
					{
						'revision': '764a5502a4d878c5b087df46506bf6797da19fc7',
						'subject': '#83: ログ出力を人に優しくする'
					},
					{
						'revision': 'b381dff4a4b54dccb1a0e653d499ef8d3fa31706',
						'subject': '#89: 再生中動画のコメントの投稿数グラフ表示',
						'comments': [
							'実装が思ったより面白くなかったって言うかストレスだけが溜まった',
							'表示タイミングの1秒間隔で適当にグラフにしだけなんだけど #84 の影響もあっていまいちよくわからんことになってる'
						]
					},
					{
						'revision': '35d7b31755ce17d5783d497d2c19df068b39be6f',
						'subject': '#91: ブックマークマイリスト更新チェックを定期的に行う'
					},
					{
						'revision': '854a2be7b7bd8f370d9418b5f5a93515ef44ef59',
						'subject': '#92: アップデートがあるときにもっと自己主張する'
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '2d74ecba89484e506fbab1ed926bb4933d2e6cd4',
						'subject': '#80: マイリストのタブ遷移時にファインダーのリスト内容が変わらない'
					},
					{
						'revision': '021caea5de64e09f1b16b781d36c2b4ecae2b284',
						'subject': 'アカウント切り替え時にマイリストの状態をぐいっとした'
					},
					{
						'revision': '7b04b92ea57c12b45154a30bafc2f63dd7350b67',
						'subject': '#75: アップデートチェックを行った際に結果を表示する'
					},
					{
						'revision': 'cde4f39722118dbd495088cac3a6115c2464b34a',
						'subject': '#86: Windows10 でファインダーフィルタの有効・無効チェックボックスのチェック部分があれ'
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': '766ac5a3bb0bd5535de4294392fd8c4ca4ff5163',
						'subject': 'ログ出力をコマンドラインから使用',
						'comments': [
							'/log=出力ディレクトリ'
						]
					}
				]
			}
		]
	},
	{
		'date': '2016/08/25',
		'version': '0.12.0',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'subject': 'むだに頑張って無駄につかれた'
					},
					{
						'revision': 'd399b69851eb506d5f21218a934efae5d3182d5c',
						'subject': '再オープンで #59',
						'comments': [
							'0.11.0で完全に死んでたから0.12.0として再リリース',
							'多分大丈夫なはず。。。はず。。。'
						]
					}
				]
			},
			{
				'type': 'features',
				'logs': [
					{
						'revision': '25be418fa62b5cc1a96e2fa160e7ffe0cca40090',
						'subject': '#59: ゴミ収集を定期的に行う。',
						'comments': [
							'ファイルの排他がスーパーあやしい！',
							'あやしいとかそういうレベルじゃなかった'
						]
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '737a2fe187a6ca8fc66121ec36156bebd595de59',
						'subject': '#77: ファインダーフィルタで無効対象にした後、フィルタを削除したら色が黒い'
					},
					{
						'revision': '637ec648c4d8bed83cc70cbdbe3d7c5960bab4a5',
						'subject': '#76: あとで見るを見た後に件数表示が変わらない'
					},
					{
						'revision': '407c9a77a69f48177ace2230cfce08784882d83c',
						'subject': '#13: キャッシュクリア時にキャッシュ情報を初期化する'
					},
					{
						'revision': '361b2784cf9835bc20e098ab435952aed7d4bd4c',
						'subject': '#78: チャンネル動画の名前とかがプレイヤーに表示されない',
						'comments': [
							'チャンネルページの構造は調べてないのでクリックしてもチャンネルページには遷移しません',
							'一応課題(#81)は作ったので気が向いたらやるし、気が向かなければブラウザに任せる',
						]
					},
					{
						'revision': '8393a3941dba6bc92fd6db2454e58c21dcf18292',
						'subject': '動画連続再生時にユーザー名(と#78で修正したチャンネル名)が再生中動画のものに反映されない'
					}
				]
			}
		]
	},
	{
		'date': '2016/08/21',
		'version': '0.10.1',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'subject': 'ファインダーのフィルタリングON/OFFが変だったので修正+緊急リリース'
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '37a3ec310f3c63d3feef5ae29b53bf3005cccfe6',
						'subject': '#17: プレイヤーのナビゲーター使い辛くて逆にすごい',
						'comments': [
							'一応世間一般のプレイヤー操作感覚に合わせた',
							'移動後の問題は #20 に委ねる'
						]
					},
					{
						'revision': '936a349e865fb3f47fce73f6b3c5fefc2eafd1f3',
						'subject': '#72: ファインダーのフィルタリング初期状態切替とViewModelの憂鬱',
						'comments': [
							'対して考えてもいないViewModelで状態遷移もややこしいし基本的に有効で問題ないはずなので設定としては持ち歩かないようにした'
						]
					},
					{
						'revision': '0e4d5392c5fde1d6ed26eecbe09b9fa3b7e23a11',
						'subject': '#19: 動画リプレイ時に流れていたコメントは破棄する'
					},
					{
						'revision': '849aac9a2a0c0b5900fd7ae4beedb95cb8f8426b',
						'subject': '#27: コメント投稿後の各種制御'
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': 'eb1488a15f2320ba943ff4b53fcd76b93c8ccace',
						'subject': 'ファインダーのキャッシュ状態表示を済・未から有・無に変更'
					},
					{
						'revision': 'fda2cad72057a84b040bae37ae9ace9265c5c407',
						'subject': '#57: ウィンドウのタイトル書式を統一可能な仕組みを作っておく',
						'comments': [
							'未だに Mode=OneWayとプロパティのSetter有無がようわからんけど未来に禍根を残す懸念材料は潰せた'
						]
					},
					{
						'revision': 'fa143a6a1a9957c6c2305a4606757766448793e5',
						'subject': 'アップデートチェックは問答無用だから設定項目から外した'
					}
				]
			}
		]
	},
	{
		'date': '2016/08/21',
		'version': '0.09.0',
		'isRc': false,
		'contents': [
			{
				'type': 'features',
				'logs': [
					{
						'revision': '1dbf62729ab94c47670d2eff7f98a71d912085d7',
						'subject': '#65: コメントリストの改行表示を控えめにする',
						'comments': [
							'改行は ↵ に置き換え',
							'連続する空白は ˽ に置き換え'
						]
					},
					{
						'revision': 'f9373d77bd36bc17e83f9f8aac71421fdd7a8425',
						'subject': '#67: 検索結果一覧のフィルタリング登録',
						'comments': [
							'各ファインダー内で設定するけど全てのファインダーに影響します',
							'アカウントマイリスト・あとで見る・ブックマーク等の自身が見るためのファインダーはフィルタリング設定は影響しません'
						]
					},
					{
						'revision': '92b45e94d27c601268e0f62d18605d30462f86af',
						'subject': '#4: ログイン設定を一時データに乗せる'
					},
					{
						'revision': '515bb8135debcaf0a5be792665bdd19c9c96df11',
						'subject': '#70: メインウィンドウのアプリケーション情報をきちんと表示'
					},
					{
						'revision': 'fcc7d499aa835309b1c07a999e3d1688fce6a555',
						'subject': '#58: アップデート確認を定期的に行う'
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '5bf707455c2f7c2bb987fd614e9f89310c79eba9',
						'subject': '#60: 非ログイン状態でおすすめタグを取得すると表明で死ぬ'
					},
					{
						'revision': 'c88b661a52af5cfbb5d67618e724690cf20a14a6',
						'subject': '#2: 外部ライブラリの情報追加'
					},
					{
						'revision': '47630bef7fc67da4d78b637322a78a2833d5c3a1',
						'subject': '#69: 未設定ブックマークで表示されるファインダーがバグってる'
					},
					{
						'revision': '4e3b7157cb086251191c9c60c520cc56a3045d6f',
						'subject': 'ユーザー情報切替でスレッド間のディスパッチャー的なあれ修正'
					},
					{
						'revision': '86157cdeea0d00c4df1d5297ff6adb6e5ac47d2c',
						'subject': '検索履歴をリンクっぽくした'
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': 'd279ad7b362137cdaa3f041cc6b797dc4f404845',
						'subject': 'AssemblyCopyright追記'
					},
					{
						'revision': '82b24852d09d3b1ef32a73f7bdaf140f86525246',
						'subject': '履歴数を減らした 1000 -> 500'
					}
				]
			}
		]
	},
	{
		'date': '2016/08/18',
		'version': '0.08.0',
		'isRc': false,
		'contents': [
			{
				'type': 'features',
				'logs': [
					{
						'revision': '755db59368218be6a80e6dbdb941ae62023186e8',
						'subject': '#7: ファインダーのリスト設定時に全情報を取得する',
						'comments': [
							'今まであれやこれや可能な限り情報取得は後に回してたけど動画時間とかでフィルタリングするにはデータが足りないこともあり歯がゆかったので読み込み時に全情報取得するようにした',
							'実装イメージは処理順序変更だけなんだけどファインダーのリストに設定する前に読み込みが走るので今までより少し時間かかるようになった、しゃーない'
						]
					},
					{
						'revision': 'c7ebc98baea993c82d681696d62a36a8cd56365d',
						'subject': '#7実装によりフィルタリングに「時間」を追加',
						'comments': [
							'内部実装は前々からあったから活用できるようになったというべきか',
						]
					},
					{
						'revision': 'dc13619b4a61872af99038c41df855405731839d',
						'subject': '#43: ダウンロード中の速度表示'
					},
					{
						'revision': '66fb81e4d943d5d97f94d1fbc2f062f9e248e467',
						'subject': '#46: 一定間隔でデータ保存を行う',
						'comments': [
							'30分間隔',
							'今現在が不安定なので将来的には外すかも'
						]
					},
					{
						'revision': '2402b341d582cd9a7aa2c704f36c8f123b798dcd',
						'subject': '#40: アップデート時に設定保存を明示的に行う',
						'comments': [
							'アップデート処理前に保存を行うようにした'
						]
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '5d0b634b81d41d34e3e3480d9e7c99afedc923bc',
						'subject': 'ログメッセージの出力が IndexOutOfRangeException してた',
						'comments': [
							'多分ね！'
						]
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': '24b6be87f4041feac5ac2d0e0d7aa7b4d91fb62b',
						'subject': 'データ保存と終了処理を明示的に分離した'
					}
				]
			}
		]
	},
	{
		'date': '2016/08/17',
		'version': '0.07.0',
		'isRc': false,
		'contents': [
			{
				'type': 'features',
				'logs': [
					{
						'revision': '0ac5d7bed02fec1706561b0382fa7bdb7810d829',
						'subject': '#49: 設定ファイルのバックアップ',
						'comments': [
							'30世代でローテートした',
							'１ファイルに全部集約する思想の元 gzip にした。zipだるい'
						]
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': 'ddf6567c44e6ad37c3bef599f5c3027336c58183',
						'subject': '#8: プレイヤーウィンドウより先にメインウィンドウを閉じた場合にあまりよくない状態になる',
						'comments': [
							'現状では全ウィンドウを閉じることにした',
							'閉じれなくする・選択肢としては確認ダイアログを表示する・非表示にするなど多々あるけどとりあえず今回対応としては全部閉じる'
						]
					},
					{
						'revision': '21bb44abde0ba857a1c1308848a7929210d95562',
						'subject': '#54: マイリストのアイテム数は何表示してんの？',
						'comments': [
							'ユーザー側のマイリスト件数取得がバグってた'
						]
					},
					{
						'revision': '17b3abbd70d767bc8f24ed889efe432e90443871',
						'subject': '#52: 自動アップデートのダウンロード待ち中にUI操作可能にする',
						'comments': [
							'単純にTaskに切り替えたからもっかしダメかもね'
						]
					},
					{
						'revision': '92371328ef3e866db30bacb985c7ea3912894059',
						'subject': '#53: ユーザー切り替えを複数回行った際にユーザータブの現在ログイン者が変になる'
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': 'a1277eb6c9e09efd10acc1bc63cb4ecc477abe68',
						'subject': '#22: 固定値を app.config に移す',
						'comments': [
							'App.config と static class とメンバ初期値におんぶ抱っこでなぁなぁ実装',
							'本実装ではとりあえず目につくものレベルなので今後見つけた際には随時実装していく'
						]
					}
				]
			}
		]
	},
	{
		'date': '2016/08/14',
		'version': '0.06.0',
		'isRc': false,
		'contents': [
			{
				'type': 'features',
				'logs': [
					{
						'revision': '',
						'subject': '#33: ファイルへのログ出力'
					},
					{
						'revision': '',
						'subject': '#25: 動画情報のメモリキャッシュに擬似参照カウンタでもつける',
						'comments': [
							'参照カウンタ機能のみ追加',
							'この情報を元に追々なんかやるべ'
						]
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '',
						'subject': '#44: 自動アップデートのアーカイブ保存ディレクトリが設定ディレクトリに格納されている'
					},
					{
						'revision': '',
						'subject': '#31: 音とコメント有効領域のポップアップ何とかならないすかね',
						'comments': [
							'だっさいけど一元化しておいた',
							'動作不安定より堅実に。。。'
						]
					}
				]
			}
		]
	},
	{
		'date': '2016/08/14',
		'version': '0.05.0',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'subject': '一応アップデートは出来るけど不安定なので諸々調整'
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': 'a1029f1d3827b88076cc1d76158157751286c80d',
						'subject': '#39: 自動アップデート時の更新履歴取得がファイルダウンロードになる',
						'comments': [
							'ついでに待ち時間も長くした'
						]
					}
				]
			}
		]
	},
	{
		'date': '2016/08/14',
		'version': '0.04.0',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'subject': '本バージョンは自動アップデート試験のためだけに無理やりバージョンアップ'
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': 'b5ad3d7e61b366e0a64df50fce1b4c9ae1cdec3d',
						'subject': '#38: ヘルプの各種情報が Pe のままになっている'
					},
					{
						'revision': '258feafd93a366e4667e0244ec6dbd72675eb8f9',
						'subject': '0.03.0 のリビジョン抜け修正'
					}
				]
			}
		]
	},
	{
		'date': '2016/08/13',
		'version': '0.03.0',
		'isRc': false,
		'contents': [
			{
				'type': 'features',
				'logs': [
					{
						'revision': 'c3188ffb982a4a46b6a5da767689631192cbeab0',
						'subject': '#3: 自動アップデート機能',
						'comments': [
							'かなりざっくり対応なので追々調整するんです'
						]
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '66ce6fa3d08e694def4d2916e80b813048d03301',
						'subject': '#35: 投稿者への労りがONで「っ」を含むコメントがフィルタリングに引っ掛かる'
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': '6704c08ea52b360e0ae53a743a01111090dd3428',
						'subject': '#37: 非ハンドリング例外の補足'
					}
				]
			}
		]
	},
	{
		'date': '2016/08/12',
		'version': '0.02.0',
		'isRc': false,
		'contents': [
			{
				'type': 'features',
				'logs': [
					{
						'revision': 'e1f40ae26697f22cb31cf9df0fd36699014a1993',
						'subject': '#15: デフォルトフィルタの整理'
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': 'a749107f7c6ef87c1e7611d5ef0cc0202361ed8f',
						'subject': '#14: 四文字フィルタ破棄'
					},
					{
						'revision': '3e453647bc250c1760871bb78531f5bebd840f14',
						'subject': '#26: 二重起動の抑制'
					},
					{
						'revision': '7e8897275c976cf16377a2f7ee7c107bef8991ff',
						'subject': '#10: フィルタリングタブをNGからフィルタに変更'
					},
					{
						'revision': '65a09fabb28f39017097bb6c047bcbadf80e2602',
						'subject': '#34: 18歳未満のログイン処理が不完全'
					}
				]
			}
		]
	},
	{
		'date': '2016/08/11',
		'version': '0.01.0',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'subject': 'とりあえずここらで master にマージ'
					}
				]
			}
		]
	}
];
