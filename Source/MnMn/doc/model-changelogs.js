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
		'date': '2017/08/05',
		'version': '0.79.1',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'subject': '0.78.1 ? なにそれ'
					},
					{
						'revision': '',
						'subject': '0.79.0 ? 知らないバージョンだね'
					},
					{
						'revision': '',
						'class': 'notice',
						'subject': '必要環境の .Net Framework を 4.6 から 4.7 に変更します(#706)',
						'comments': [
							'ver 0.80.0 以降で 2017/08/16 以降になったら 4.7 を対象にします',
							'ダウンロードページ: https://www.microsoft.com/ja-JP/download/details.aspx?id=55170',
							'Windows 7 を使用している場合は一部注意が必要かもです(#651)'
						]
					}
				]
			},
			{
				'type': 'features',
				'logs': [
					{
						'revision': '5ec481c8c3f83355d59a1fd390a866b84da034c7',
						'subject': '#697: アップデート時に MnMn インストールディレクトリ以下にあるプログラムが稼働していれば終了できるようにする'
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': 'b0d710f00996ce99693b6f28712c45d7a2a970c3',
						'subject': '#715: バグの温床になりそうだけど一致ではなく消極的な動画サイズチェックを行う'
					},
					{
						'revision': '3a6b57ce642debb4b348a02367479c72c69e624f',
						'subject': '#710: クラッシュレポートより: 動画プレイヤーを開いた瞬間に閉じると NullReferenceException'
					},
					{
						'revision': '8c2359f4d99403709a4c479b07e9717c3b0f0049',
						'subject': '#716: マウスホイールで切り替え可能なタブが全て選択できない場合にフリーズする'
					},
					{
						'revision': '97db8458788cedb9149522112fc61094c72d21b0',
						'subject': '#717: #715 をもうちっと消極的にする',
						'comments': [
							'file.size == totalsize -> totalsize <= file.size',
							'このソフトいい感じにクソだろ'
						]
					},
					{
						'revision': 'a87d7b594f436650710cd80270ae55df2cf3e130',
						'subject': '#718: 外部プログラム・カスタムコピーの書式置き換えが動いていない'
					},
					{
						'revision': '3e67dafcab4ab287c019bca0660d256c4f1e1f56',
						'subject': '#719: あっれー、キャッシュ状態本格的にバグってない？'
					}
				]
			}
		]
	},
	{
		'date': '2017/08/05',
		'version': '0.78.1',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'class': 'notice',
						'subject': '必要環境の .Net Framework を 4.6 から 4.7 に変更します(#706)',
						'comments': [
							'ver 0.80.0 以降で 2017/08/16 以降になったら 4.7 を対象にします',
							'ダウンロードページ: https://www.microsoft.com/ja-JP/download/details.aspx?id=55170',
							'Windows 7 を使用している場合は一部注意が必要かもです(#651)'
						]
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '',
						'subject': '#714: #708 で動画に対する設定も GC 対象にしたけどそれはそれでキャッシュ状態のフラグが狂うっていうかバグってる',
						'comments': [
							'んふふふ'
						]
					}
				]
			}
		]
	},
	{
		'date': '2017/08/03',
		'version': '0.78.0',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'class': 'notice',
						'subject': '必要環境の .Net Framework を 4.6 から 4.7 に変更します(#706)',
						'comments': [
							'ver 0.80.0 以降で 2017/08/16 以降になったら 4.7 を対象にします',
							'ダウンロードページ: https://www.microsoft.com/ja-JP/download/details.aspx?id=55170',
							'Windows 7 を使用している場合は一部注意が必要かもです(#651)'
						]
					},
					{
						'revision': '',
						'subject': '機能実装の前に修正えんやこらさっさー'
					}
				]
			},
			{
				'type': 'features',
				'logs': [
					{
						'revision': '9afaf8c66aead706c6da0176a26976c8155246fb',
						'subject': '#713: クラッシュレポート自動送信させたくね？ させたいよね！',
						'comments': [
							'クラッシュレポートってすっごい大事なんよ',
							'とりあえず嫌がらせのように初期値は真にしといた',
							'自動送信の場合は送信前に 7 秒待つ',
							'個人情報保護とか小難しい意見をメールでもらうけどそういう人に対しては実装見てないんだろなぁとほのぼのしながら文面眺めてからゴミ箱に入れてるからそんな感じ',
							'そもそもクラッシュレポートの送信内容はおめーの PC のストレージに保存されてるからそれ見て判断しろよと言いたいね'
						]
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '05c5f77ad6945b5eb5be10f28f1e851778962620',
						'subject': '#707: 非 Tooltip の Popup 系コントロールを他ウィンドウより前面に表示させるべきじゃない'
					},
					{
						'revision': '222a468c56d2adee0425f19e32fad1fa76abfc49',
						'subject': '#705: クラッシュレポートより: System.ArgumentOutOfRangeException: インデックスが範囲を超えています',
						'comments': [
							'再現手順の提供ありがとー'
						]
					},
					{
						'revision': 'd14c7bfd67fc0fbe13a5fd87c51cb2a46c64db94',
						'subject': '#640: #619 で捨てたピン留めのデータを完全破棄する',
						'comments': [
							'設定ファイルからデータ消えます',
							'ばいばい (^_^)/~~~'
						]
					},
					{
						'revision': '485268ece8b7c552761bd4b351240328d6882062',
						'subject': '#708: 動画キャッシュのGCにおいて破棄すらしていないファイルがある'
					},
					{
						'revision': '980127cabb5837f1412a39ec1cde7695269ba9a2',
						'subject': '#698: クラッシュレポートより: System.InvalidOperationException: ディスパッチャーの処理の中断中は、この操作を実行できません',
						'comments': [
							'追加情報ありがとさん'
						]
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': '01550588d2d65cc00c564f36738851a81e54a2b6',
						'subject': '#604: シーケンシャル移動のプレイリストの次・前アイテムの選定に現在アイテムインデックスを再調査する',
						'comments': [
							'#705 対応でうまくいったと思うので処理だけ共通化してこの課題は無効とした'
						]
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
	}
];
