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
		'version': '0.74.1',
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
						'subject': '#670: チャンエルブックマークの検証・調整があまあま',
						'comments': [
							'もうワケわからん',
							'設計せずに TabControl と WindowsFormsHost 合わせるとダメだね'
						]
					},
					{
						'revision': '',
						'subject': '#673: 元々頭おかしい内蔵ブラウザがチャンネルタブを閉じた際に頭狂う',
						'comments': [
							'#670 と同時'
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
						'subject': '#504: DMC形式の処理を独立',
						'comments': [
							'＼(^_^)／ばんざーい'
						]
					},
					{
						'revision': '',
						'class': 'nuget',
						'subject': '#681: HtmlAgilityPack 1.5.0 -> 1.5.1'
					},
					{
						'revision': '',
						'class': 'nuget',
						'subject': '#680: Geckofx 45.0.31 -> 45.0.32'
					},
					{
						'revision': '',
						'class': 'nuget',
						'subject': '#679: Extended WPF Toolkit 3.0.0 -> 3.1.0'
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
	}
];
