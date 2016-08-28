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
		'version': '0.12.1',
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
						'subject': '#82: フルスクリーン表示',
						'comments': [
							'本実装に伴いプレイヤーのウィンドウ UI が Windows 標準のものと変わります',
							'UI 変更とはいうものの UI 実装はまだきちんとする気がないのですごく適当です',
							'フルスクリーンの解除は再度フルスクリーンボタンを押下する以外には、ESCキー押下・プレイヤー部分のダブルクリックになります',
							'(バグみたいなもんだけど仕様として)ただしコメントをダブルクリックした場合なにも起こりません'
						]
					},
					{
						'revision': '',
						'subject': '#87: 開発中内容を一覧表示する'
					},
					{
						'revision': '',
						'subject': '#83: ログ出力を人に優しくする'
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
						'subject': '#80: マイリストのタブ遷移時にファインダーのリスト内容が変わらない'
					},
					{
						'revision': '',
						'subject': 'アカウント切り替え時にマイリストの状態をぐいっとした'
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
						'subject': 'ログ出力をコマンドラインから使用'
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
