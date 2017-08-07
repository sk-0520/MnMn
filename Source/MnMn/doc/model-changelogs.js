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
		'version': '0.79.1',
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
						'subject': '#720: 連続再生をプレイリスト内の項目数だけに限定した挙動を追加する',
						'comments': [
							'ちょっくら色々あって順序入れ替えたり削除した場合の試験してないからデバッグよろしくね'
						]
					},
					{
						'revision': '',
						'subject': '#602: 各種コントロールにショートカットキー(昔でいうキーボードアクセラレータ)を付与する',
						'comments': [
							'軽いだろと思ってたけど予想以上に作業量多かったから諦めた',
							'今後は起票せずゆっくりぼちぼち進めていくことにした'
						]
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
						'subject': '#722: ラボが死んでてボクも死んでる'
					},
					{
						'revision': '',
						'subject': '#725: GC 未実施ログ出力で正常にスキップしたものが警告表示されてる'
					},
					{
						'revision': '',
						'subject': '#721: 関連動画の読み込みはタブ選択まで遅延させる'
					},
					{
						'revision': '',
						'subject': '#724: クラッシュレポートより: System.Runtime.InteropServices.InvalidComObjectException: 基になる RCW から分割された COM オブジェクトを使うことはできません',
						'comments': [
							'再現不可のため隠蔽'
						]
					},
					{
						'revision': '',
						'subject': '#727: 動画再生方法が「外部プログラムで開く」でプログラム・パスを設定せずに動画を開こうとすると開けないんだけど内部的にエラー連鎖しててダメだろこれ'
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
						'subject': '若干気になったのでプレイヤーの破棄だけじゃなくプレイヤー終了時にもイベント解除した'
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
	}
];
