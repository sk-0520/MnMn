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
		'version': '0.73.1',
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
						'subject': '#672: チャンネルの動画ファインダーのエクスパンダ―が開発中ヘッダの文言のまま'
					},
					{
						'revision': '',
						'subject': '#661: もっさい件数表示を今風にする'
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
						'subject': '0.69.1 の更新履歴がなんだかダブってた'
					},
					{
						'revision': '',
						'class': 'reopen',
						'subject': '#667: 開発のメールアドレス変える',
						'comments': [
							'エスケープしてなかった'
						]
					},
					{
						'revision': '',
						'subject': '#671: チャンネルのファインダーが読み込み済みでも特定操作で再読み込みを実施してしまう'
					},
					{
						'revision': '',
						'subject': '#676: HTML5 版で取得するデータの Jsonで謎い部分をとりあえず string から object にしとく',
						'comments': [
							'その項目が何かしらの (Jsonとしての)object だったら object でとるようにした',
							'データ保全っていうよりパース失敗対応',
							'パッと見たとこだけしか対応してないから発見次第対応していきたい',
							'見れない動画とか教えてもらえるとたいていこれ関係だろうから教えてくれるとありがたい'
						]
					},
					{
						'revision': '',
						'subject': '#669: チャンネルのファインダーではフィルタを無効にすべし'
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
		'date': '2017/07/17',
		'version': '0.73.0',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'class': 'warning',
						'subject': '大工事でいろんなとこに手を入れたからデバッグよろしく！'
					},
					{
						'revision': '',
						'subject': '本バージョンで #665 正式対応が完了したはずなので HTML5 版を正とします',
						'comments': [
							'Flash 版 -> HTML5 版の違いによりいくつか内部的にも変わっている部分があります',
							'視聴ページの情報から必要情報を取得するためここが取れない場合死にます(多分これは前からなので別にいいと思う)',
							'コメントのデータ形式が XML -> Json になってるので前バージョンでキャッシュされたコメントは使えません',
							'動画形式が SWF の場合は Flash 版として処理します(とはいっても ffmpeg なんちゃって変換)',
							'この SWF 互換(ソース的には *_Issue665NA)が綱渡りみたいな実装になってるのでこれ以上なんかあるとゲロ吐き散らします',
							'あんまし言いたかないけどテストが足りないので色々問題ありそうなので見つけたら早めに教えてね'
						]
					}
				]
			},
			{
				'type': 'features',
				'logs': [
					{
						'revision': 'e752c014ab0fc9be7589cbc67864e4743ee611a1',
						'subject': '#644: 外部プログラム設定などで使用する置き換え可能文字列を自動入力できるようにする'
					},
					{
						'revision': '5c898073bcb5cd78600221a5842ab1cbcf9c660e',
						'subject': '#662: プレイリストの次動画自動遷移をショートカットキーで対応可能にする',
						'comments': [
							'F9 キー',
							'Windows Media Player で該当するキーが分からんかった'
						]
					},
					{
						'revision': '58feae681e6f26bc10eeeda196fd5ddddc34585a',
						'subject': '#81: チャンネルページをユーザーページみたいに表示する',
						'comments': [
							'WPF の TabControl 難くね？',
							'内蔵ブラウザが色々おかしな挙動するけど気にするなし'
						]
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': 'cf11d7e8de006e651f20d0d6773e5d420ff366a2',
						'subject': '#660: ユーザータブ一覧メニュー表示の項目表示が出来てない'
					},
					{
						'revision': '6719745f4ba81fce97d07fad08976e9a7f326d87',
						'subject': '#665: 2017-07-13 サービス実装変更',
						'comments': [
							'いやはや短い時間で結構頑張ったね',
							'さしてまともに見れない SWF 互換を残したからこれが問題でなんかこう死ぬ可能性あるね',
							'あとコメント投稿周りも変わってるから怖いけど興味ないんだなこれが',
							'細かい修正が必要なら追って別課題で対応する'
						]
					},
					{
						'revision': '0493bb2d3406c22813ac1c085a75950e525efb8d',
						'subject': '#668: ニコニコのタブ順序調整',
						'comments': [
							'独断と偏見で並び順変えた'
						]
					},
					{
						'revision': '8e4fb2d5eb0642aabe4fb9564a1abc831605246d',
						'subject': '#664: タグ取得のRSSでページ指定って 1 基底じゃないのでしょうかですか',
						'comments': [
							'うむ。ミスってた',
							'運用上今まででも問題はないけど気分的になんだか変な感じだった'
						]
					},
					{
						'revision': '554d6a66675c4a7710561fcb26655f8454eff22c',
						'subject': '#663: フィルタ内タブのタブ一覧メニューで表示要素の瞬間移動'
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': 'cf60dc26c6916d3806c446d11caccd44f286cc98',
						'subject': '#667: 開発のメールアドレス変える',
						'comments': [
							'gmail のエイリアスで +mnmn を付けた',
							'これでラベルが強制できるのだ',
							'もうエイリアスついてないメール無視することにする、探すのがしんどい'
						]
					}
				]
			}
		]
	},
	{
		'date': '2017/07/14',
		'version': '0.72.1',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'subject': '💩💩💩💩💩',
						'comments': [
							'ちょっとこれやばい',
							'次バージョンの開発版は一旦止めて新しい方に完全対応せにゃならん',
							'デバッグも何もしてないから細かいところは勘弁'
						]
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': 'a9b99fda0c175a7cd7ef371cf3d7e34ab88ea9ad',
						'class': 'open',
						'subject': '#665: 2017-07-13 サービス実装変更',
						'comments': [
							'クッキーでやられると簡易アップデートじゃないからふぇぇ',
							'暫定対処過ぎてふぇぇ',
							'ふぇぇ'
						]
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
	}
];
