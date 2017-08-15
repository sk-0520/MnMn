﻿var changelogs = [
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
		'date': '2017/08/15',
		'version': '0.82.0',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'class': 'warning',
						'subject': '本バージョンから実行環境は .Net Framework 4.7 が必要になります(#706)',
						'comments': [
							'.Net Framework 4.7: https://www.microsoft.com/ja-JP/download/details.aspx?id=55170',
							'Windows 7 を使用している場合は一部注意が必要かもです(#651)'
						]
					},
					{
						'revision': '',
						'subject': '他の機能追加・修正に影響を与えないため本バージョンアップはこんだけです'
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': '',
						'subject': '#706: 動作基盤を .Net Framework 4.7 にアップグレードする'
					}
				]
			}
		]
	},
	{
		'date': '2017/08/14',
		'version': '0.81.1',
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
							'次回リリース予定の 0.82.0 で 4.7 を対象とします',
							'ダウンロードページ: https://www.microsoft.com/ja-JP/download/details.aspx?id=55170',
							'Windows 7 を使用している場合は一部注意が必要かもです(#651)'
						]
					},
					{
						'revision': '',
						'subject': '0.81.0 でやたらクラッシュレポートが飛んできたため #737 で 0.81.1 として緊急リリース'
					}
				]
			},
			{
				'type': 'features',
				'logs': [
					{
						'revision': '3238b103cf26ad7c0193ac62f1a9fcfe53286661',
						'subject': '#730: 更新履歴でアホみたいなことしてみたい',
						'comments': [
							'インフラだけ整えた'
						]
					},
					{
						'revision': 'a489a28aa29d0b0327958af3f3a0391931a450de',
						'subject': '#622: アップデート処理を開発側で強制できる下準備だけしておく'
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '273844a628ed062af4715fbfcabaae5a504659bd',
						'subject': '#733: データダウンロード中のダウンロードアイテムの文字見切れてるね わびさびだね'
					},
					{
						'revision': '',
						'subject': '#723: クラッシュレポートより: プログラム終了時に NullReferenceException',
						'comments': [
							'これ: System.Windows.DeferredAppResourceReference.GetValue',
							'再現出来てないから発生しないことを祈っとく'
						]
					},
					{
						'revision': 'c3116408922a5b37ecf05bcbb6077818c5a4ea84',
						'subject': '#736: コマンドライン・WCFでの指示はもう投げっぱなしでいいと思う',
						'comments': [
							'待機時間なしはそれはそれで再試験したくないし 3 秒待機から 0.5 秒待機に変更'
						]
					},
					{
						'revision': 'ca72c1ea1e52cda870609ab1495c993e07b99807',
						'subject': '#732: ずいぶん前から知ってて起票しなかったけどフルスクリーン・最大化から通常ウィンドウに戻したらウィンドウの境界線なくなるよね'
					},
					{
						'revision': '9fd647210cfcfb50e666986bd8e590bd786ebca2',
						'subject': "#737: クラッシュレポートより: System.InvalidCastException: 型 'MS.Internal.NamedObject' のオブジェクトを型 'System.ComponentModel.ICollectionView' にキャストできません",
						'comments': [
							'ふひゅー'
						]
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': '5ceb21bb1c2d14f2881ced78ed86f50803597860',
						'subject': '#729: 我が家のあの共通ロジックを MnMn に統合してあれしたい'
					},
					{
						'revision': 'e22eb59bfa1e6535885f14dcbf3c540f52e3726c',
						'subject': '#735: 開発用にファインダーからキャッシュディレクトリ開けるようにしておきたいね'
					}
				]
			}
		]
	},
	{
		'date': '2017/08/08',
		'version': '0.80.0',
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
							'2017/08/16 以降になったら 4.7 を対象にします',
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
						'revision': 'f906cda80dc5264601fd9384ea55ed293b4d68e1',
						'subject': '#720: 連続再生をプレイリスト内の項目数だけに限定した挙動を追加する',
						'comments': [
							'ちょっくら色々あって順序入れ替えたり削除した場合の試験してないからデバッグよろしくね'
						]
					},
					{
						'revision': '14db48fc117ce74aae74468fb0fb0b0be7b683ea',
						'subject': '#602: 各種コントロールにショートカットキー(昔でいうキーボードアクセラレータ)を付与する',
						'comments': [
							'軽いだろと思ってたけど予想以上に作業量多かったから諦めた',
							'今後は起票せずゆっくりぼちぼち進めていくことにした'
						]
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '22507cbd6b7343652c85f0cae67e5b6e16434619',
						'subject': '#722: ラボが死んでてボクも死んでる'
					},
					{
						'revision': '2887d6d6587a62a16a4990bb3c58bca016da4435',
						'subject': '#725: GC 未実施ログ出力で正常にスキップしたものが警告表示されてる',
						'comments': [
							'あまりにも不可解なクラッシュレポートが飛んできてるけど多分これのせいだと思う',
							'-> ArgumentOutOfRangeException: インデックスが範囲を超えています',
							'他のレポートも同じようなタイミングだったからこれだじゃないかなぁという思いでリリース'
						]
					},
					{
						'revision': '192062ff5842e14c9d135f86ecb10038e6dd280c',
						'subject': '#721: 関連動画の読み込みはタブ選択まで遅延させる'
					},
					{
						'revision': '0c6ba743262e5884f3faa1ad2a05f722b7271e45',
						'subject': '#724: クラッシュレポートより: System.Runtime.InteropServices.InvalidComObjectException: 基になる RCW から分割された COM オブジェクトを使うことはできません',
						'comments': [
							'再現不可のため隠蔽'
						]
					},
					{
						'revision': 'ac920f9aae834005e0609c4ed542668ca1591a48',
						'subject': '#727: 動画再生方法が「外部プログラムで開く」でプログラム・パスを設定せずに動画を開こうとすると開けないんだけど内部的にエラー連鎖しててダメだろこれ'
					},
					{
						'revision': 'b83c8a27dac1cfcf96e8bf1461b46d23c24abfc6',
						'subject': '#728: クラッシュレポートより: (内部的な)コメント描画領域が設定されていない状態でプレイヤーの再生位置が変わると NullReferenceException',
						'comments': [
							'課題件名の時点で矛盾だらけなんだけどログはそう言ってる'
						]
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': 'cd19a9b23e9dfeeff6f3158fdf714c09c0283b8b',
						'subject': '若干気になったのでプレイヤーの破棄だけじゃなくプレイヤー終了時にもイベント解除した'
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
	}
];
