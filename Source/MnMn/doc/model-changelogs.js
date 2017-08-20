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
		'version': '0.83.1',
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
						'class': 'compatibility',
						'subject': '#744: 全体フィルタ設定の設定名称が typo ってるから血を流そう',
						'comments': [
							'動画に対するコメント全体フィルタの使用状態をリセット',
							'XAML 側もいじったからもっかしバインドミスってるかもね'
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
						'subject': '#746: C#7.0 的なコードに一部書き換えたい'
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
		'date': '2017/08/19',
		'version': '0.83.0',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'subject': '公開から一周年だね',
						'comments': [
							'くっそおもんねーわ この実装'
						]
					}
				]
			},
			{
				'type': 'features',
				'logs': [
					{
						'revision': '6a8af9c6ff32a3681f6f55145d815e33d276c24d',
						'subject': '#689: 表示領域に収まりきらない縦タブのタブアイテムが自己主張強すぎる',
						'comments': [
							'縦タブをマウスホイールでスクロール'
						]
					},
					{
						'revision': 'ef7762174dbd42d59cbbd90f8a3ac02d1106aabb',
						'subject': '#739: ファインダーから複数アイテムのGCを実行'
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': 'a3521fe71b46f6342ab7f46ad9bfeacf207a8e3d',
						'subject': '#738: HTML で言うところの "最終<br />アイテム<br />まで" の方がメニューがシュッとする'
					},
					{
						'revision': '7e373b8d873d181ef67ccc43b6427ca124687fb9',
						'subject': '#675: そろそろニコニコのサービスタブにアイコン付けないと目が滑る'
					},
					{
						'revision': '5c1da5287f2c221d37781f63d4e951e1b2f14dc5',
						'class': 'reopen',
						'subject': '#723: クラッシュレポートより: プログラム終了時に NullReferenceException',
						'comments': [
							'わかんねーなぁ、再現手法だれかおせーて'
						]
					},
					{
						'revision': '15d8d5f86dbe75fbcac0ba3334feb2b6ec8629d0',
						'subject': '#576: getthumbinfo が使えない動画情報は HTML から取得する',
						'comments': [
							'getthumbinfo からデータを取得した際に so ならセッション無し視聴ページのコメント数とかを取得するようにした',
							'取得したデータは getthumbinfo に再設定するので他ツールとかへの連携にも使えると思う',
							'データ取得時なので本バージョン未満で生成した getthumbinfo キャッシュがある場合はその限りではない'
						]
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': '9553e5d59ad73e1674e58b343577bf30d6d00dc4',
						'subject': '#742: T4 生成物はタイムスタンプ付けておきたい'
					}
				]
			}
		]
	},
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
	}
];
