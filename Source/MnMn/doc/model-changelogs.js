var changelogs = [
	/*
						'class': 'compatibility' 'notice' 'nuget' 'myget' 'warning'
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
		'version': '0.24.1',
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
						'subject': '#197: App.config に対するユーザファイルの上書き',
						'comments': [
							'MnMn.exe.config と同じディレクトリに MnMn.exe.user.config を配置すると既存の App.config 内容を上書き(マージ)します', ,
							'詳細は https://msdn.microsoft.com/ja-jp/library/aa903313 を参照してください',
							'(.NET アプリで一番使われてそうな構成セクションなのにメンテナンスされてないってすげーなMS)'
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
				'type': 'fixes',
				'logs': [
					{
						'revision': '',
						'subject': '#172: 非ログイン時に非活性なタブをマウスホイールで選択できる'
					},
					{
						'revision': '',
						'subject': '内臓ブラウザの履歴がないときに履歴破棄は行わない'
					},
					{
						'revision': '',
						'subject': '#200: 更新履歴のaタグ変でっせ'
					},
					{
						'revision': '',
						'subject': '#201: タブのピン止めアイコンが画像のまま'
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': '',
						'class': 'open',
						'subject': '#147: 1.5GBの投稿動画',
						'comments': [
							'既存実装から外れるのでややこい'
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
			}
		]
	},
	{
		'date': '2016/10/10',
		'version': '0.24.0',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'subject': '今回リリースで頑張る予定だった UI の実装は超頑張った結果、中途半端実装になった',
						'comments': [
							'Visual Studio が XAML 開くとあっぷあっぷして正直しんどい',
							'継続課題の #193 で引き続き頑張る'
						]
					}
				]
			},
			{
				'type': 'features',
				'logs': [
					{
						'revision': 'aa8bd18e5afa33cab4b0e2ad0d6ed391fd5f572e',
						'subject': '#48: UIを統一する',
						'comments': [
							'テーマは mahapps.metro(http://mahapps.com/) のものをそのまま使用しているだけ',
							'内容に UX も含めたかったけど作業量が多くなるから #193 で継続的に対応していく',
							'何がデフォルトでいいのか分からなかったので初期値はランダム、MnMn -> 設定 -> テーマ で適当に変えてちょ'
						]
					},
					{
						'revision': '74044ee5fd541cddf17749da6ef848891f97d9b0',
						'subject': '#18: 動画ブックマークのノード選択を容易にする',
						'comments': [
							'#48 対応時にスタイルが用意されてたのでそのまま適用'
						]
					},
					{
						'revision': '',
						'subject': '#133: テキストボックスに今風な機能を持たせる',
						'comments': [
							'#48 対応時にあれこれ付け足してみた',
							'機能的に問題なさそうなので今後追加するだろうけど本件はこれで閉じる'
						]
					},
					{
						'revision': '5cc1198f3cf673d95ab3fa9b8f899e4d030ba046',
						'subject': '#186: 新カテゴリ「実況プレイ動画」追加',
						'comments': [
							'忘れないように課題に書いてたんだけど完全に忘れてた',
							'ranking.xml に項目追加するだけのお仕事'
						]
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '09d43886767f1af0c61e13913fc4664c02de2cfc',
						'subject': '#192: プレーヤー領域の公式サイズ(4:3)が動いてない',
						'comments': [
							'App.configは修正した、他は知しらね'
						]
					},
					{
						'revision': 'ca712ef5e80689ba069177cd6c04be1e5236c12c',
						'subject': '#191: 内部ブラウザの履歴破棄時に例外発生'
					},
					{
						'revision': 'f8a7e0a229318b2bb2d6fe224d8c3b7b42f69d55',
						'subject': '#189: リリース情報のDOM狂ってんぜぇ'
					},
					{
						'revision': '2339d4e96716be9dc73fbda57ec22692186a44e4',
						'subject': '#23: コマンドプレビューのコマンド内容が適当過ぎ'
					},
					{
						'revision': '176e9729f1372ca95fb68ae27244c15a0e14f27e',
						'subject': '#180: フルスクリーン時にキーボードショートカット効いてない'
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': '',
						'subject': '#68: UnicodeやWebdingsで置き換えられる画像は置き換える',
						'comments': [
							'#48 対応時にめっちゃ修正した',
							'本体アイコンとニコニコのアイコン以外はラスターからベクター(or 文字)に置き換えられたと思う',
							'大丈夫だと思うけど今回リリースに関しては内部リソースを残しておいた'
						]
					},
					{
						'revision': '3c76d490f4cb36e5075ebb7d216241de4f1bc4c3',
						'class': 'nuget',
						'subject': '#194: GeckoFXのバージョンアップ'
					}
				]
			}
		]
	}, {
		'date': '2016/10/02',
		'version': '0.23.0',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'subject': '次バージョンは「#48: UIを統一する」をさわりたいので致命的な不具合がない限りちょっとだけ機能実装は見送る',
						'comments': [
							'UIは機能と分かれるべきなんだけど実装の未熟さもあるから中々分離できないのがあれなとこっすね～'
						]
					},
					{
						'revision': '',
						'subject': 'あー、今回はニコ生なんも実装してないよ'
					}
				]
			},
			{
				'type': 'features',
				'logs': [
					{
						'revision': 'f9cb52786a980f4b49338ffb5468093d8ececfc8',
						'subject': '#101: ユーザー検索やユーザーブックマーク的なやつの実装',
						'comments': [
							'ユーザー検索はサービス側がそもそも提供してないので今回分の実装からは外した'
						]
					},
					{
						'revision': 'a2a102924eb177126a966b5a2dcd23cb651697b9',
						'subject': '#179: 同性愛者への弾圧',
						'comments': [
							'おんなじようなコメント多いっすわぁ'
						]
					},
					{
						'revision': 'b36756626e868ff8d8c58a7b5ac58eb66fa68794',
						'subject': '#178: 動画サイズを公式サイズに合わせる'
					},
					{
						'revision': '436b9d53f4fded5fb36edf6ca1a52cf417a02cab',
						'subject': '#185: メニューからもフルスクリーンにする'
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '1c87fe1bc649646dd2db86ff55f45b7dac50fd89',
						'subject': '#183: 非リリース版でのウィンドウタイトル補正が頭おかしい'
					},
					{
						'revision': '9aa40befbad86aea393467ebf3f48ced3adde6d7',
						'subject': '#184: ファインダーのフィルタリングON/OFFがデフォルトでOFFになってる'
					},
					{
						'revision': '6d6a4ad14748dd505740629f000a4d141a455dcf',
						'subject': '#176: /etc の中にいらない子がいます',
						'comments': [
							'既存ファイルはアップデート時に自動削除されます',
							'MnMn だとアップデート時のC#コンパイル実行は初めてなんよね、ちょっと緊張'
						]
					},
					{
						'revision': '1c87fe1bc649646dd2db86ff55f45b7dac50fd89',
						'subject': '#177: 一行表示したコメントの改行とスペースが逆な気がする'
					}
				]
			}
		]
	},
	{
		'date': '2016/09/29',
		'version': '0.22.0',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'subject': 'フォーラムやメールで意見をもらってる事象についての対応を #85 にて実施したので優先リリース'
					},
					{
						'revision': '',
						'subject': 'それだけだと味気ないしこれ以上コミット空くとマージに支障が出るので #161 を試験実装としてリリース',
						'comments': [
							'あくまで試験実装なので課題は open のままです',
							'このブランチのマージは色々なクラスの継承関係いじってるからかなり大工事なんだけど気にしないことにした'
						]
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '',
						'subject': '#85: メモリ不足',
						'comments': [
							'フォーラムでの対応アプリケーション構成ファイルを取込'
						]
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': '',
						'subject': '試験実装: #161: ニコ生の閲覧',
						'comments': [
							'live:official, live:category タブが(現実装における)ニコ生用の入口となります',
							'なんだかんだ試行錯誤してはいるんですが、申し訳ないけどニコ生初めて使ってる段階なので意見等頂けると助かります',
							'-> 何が正しくて何がユーザーとして満足できるのかさっぱりわかりません!',
							'Flashや',
							'ああFlashや',
							'Flashや',
						]
					}
				]
			}
		]
	},
	{
		'date': '2016/09/25',
		'version': '0.21.2',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'subject': '#160対応がうんこすぎて#175で再対応したので緊急リリース',
						'comments': [
							'緊急リリースのため 0.21.0 と 0.21.1 は統合'
						]
					}
				]
			},
			{
				'type': 'features',
				'logs': [
					{
						'revision': 'fac7683b1dc9196cad5d3f014c74ac4db4ddd098',
						'subject': '#165: フルスクリーン中に非アクティブになってもフルスクリーンを解除しないようにする',
						'comments': [
							'解除するのが規定値です',
							'必要なら設定で変更してください'
						]
					},
					{
						'revision': '016285effb336814501575c3fb8dc47fd415b1b5',
						'subject': '#128: 検索履歴のタグ・キーワード検索を前回検索と逆のもののみ表示させる',
						'comments': [
							'項目クリックで同一検索する現状でタグ検索した項目を再度明示的にタグ検索する馬鹿がどこにいるのかと'
						]
					},
					{
						'revision': '44c29bdb5d730dcd6e3e9ab2b556aa5a91348bcd',
						'subject': '#166: プレイヤー内の領域サイズを設定に落す'
					},
					{
						'revision': '8218bd46703fa566d6a14e6b7ae57c82c6e11afd',
						'subject': '#162: 検索タブのロック機能',
						'comments': [
							'タブヘッダ部分のコンテキストメニューで制御できます'
						]
					},
					{
						'revision': '976db6bb397ecd1c5cc3cf328f7a72c0a2665249',
						'subject': '#170: マイリストブックマーク名の変更'
					},
					{
						'revision': 'bd542253184b1cba50c6879befae1f31cf57d18b',
						'subject': '#174: ブックマークのノード名変更を#170に合わせてリアルタイム更新とする'
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': 'f8fd493ab08348a7bafc2494762d9b5a30985973',
						'subject': '#164: マウス操作でフルスクリーンにするとフォーカスがフルスクリーンボタンに当たったままになってる'
					},
					{
						'revision': '48de0206e6857167069d0163c35480eb67bb7fd3',
						'subject': '#168: 検索処理でページ遷移時に共有される項目とそうでない項目がある',
						'comments': [
							'ニコ生の実装インフラ整備してて気づいたから将来的にマージでミスること思うと憂鬱'
						]
					},
					{
						'revision': '54e16724b34ab3f427a56d3b84a835df7cd46940',
						'subject': '#169: 動画再生時に再生中プレイヤーで別動画を開くとプレイヤーは前面に出てくるがアクティブにならない'
					},
					{
						'revision': '77f63708f70eaf31c69210edeee8507d8b4fcc23',
						'subject': '#160: 動画ページ情報がキャッシュされてる状態で再度プレイヤーを開くとそれが反映されない',
						'comments': [
							'この事象が伝搬して自動再生とかバグりまくってたと思うんですよ'
						]
					},
					{
						'revision': '9ac824a75978c4d59d44443fe9ece316c8d95827',
						'subject': '#171: マイリストのRSSから名称取得がゴミ混ざる'
					},
					{
						'revision': 'da945f12978677e830e4a22ca41fa881d0b5d02c',
						'subject': '#175: [再] 動画ページ情報がキャッシュされてる状態で再度プレイヤーを開くとそれが反映されない'
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': 'e804daf9d181e3d76f30c302dc9cd3e780037f4e',
						'subject': '#149: 取得データをモデルに落とし込む際に生データも対象にする',
						'comments': [
							'かなり限定的'
						]
					}
				]
			}
		]
	},
	{
		'date': '2016/09/19',
		'version': '0.20.0',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'class': 'warning',
						'subject': '#137の影響により配布アーカイブのサイズがデブチンになりました',
						'comments': [
							'50MN → 90MBくらいです。何がどうなってんだろうね',
							'0.19.0 からのアップデートは最大45分DL待機します',
							'0.18.0 以下からのアップデートは最大30分DL待機します',
						]
					}
				]
			},
			{
				'type': 'features',
				'logs': [
					{
						'revision': 'b72a51264d86c408e66234787b5fe899c3eeb941',
						'subject': '#155: 前回アップデートチェックした時間を表示する'
					},
					{
						'revision': 'c527e56c38d402e667aa63361a8532b6b3c841b0',
						'subject': '#137: WebBrowser死なねーかなぁ',
						'comments': [
							'死にました ;-)',
							'ブラウザのエンジンを WPF 標準の Trident から GeckoFx に載せ替え',
							'別課題 #120 が本実装により破棄となります',
							'GeckoFx 試したくて「公式サイト」を追加しましたがセッション共有程度でそれ以外の制御してません',
							'設定は本体設定で行えますが、基本的に変えることありませんし、それにあれだ、あれ、な？',
							'Trident でのレジストリ掃除処理は #159 で数世代の間継続されます',
							'GeckoFx の設定とプラグインの歪な関係が影響して MnMn に「Flash Player 確認」を追加しました。行儀悪い人が使うんでしょうね！'
						]
					},
					{
						'revision': '627938e23efe57904f01c1f573c6546aa7b57c8e',
						'subject': '#118: キーバインドの設定',
						'comments': [
							'画面クリックとかスペースキーとかマウスホイールとか地味なやつを実装',
							'内容はヘルプでも確認してください',
							'シーク時に動画が読み込めてない場合の問題は #20 で追々対応しますん'
						]
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '021bc64fa90e8ca69647e02a7f257d04327fd643',
						'subject': '#156: 単一プレイヤーで再生する際に新しい動画を再生するのであればウィンドウを前面に移動させる'
					},
					{
						'revision': '2a4cfa7682667394f3ef26b14ccaa60e4c7680ee',
						'subject': '#158: ランキングや検索のタブヘッダ部のコントロールの活性・非活性はリロードボタンに連動させる',
						'comments': [
							'長いことほったらかしてたけど本修正の挙動がまともな姿'
						]
					},
					{
						'revision': 'b0fdb992f7e40a9d49d23becf1fc7056fd2ab9f8',
						'subject': '一部環境でファインダーリスト部のホイールスクロールがタブヘッダのスクロールになる問題への暫定対応',
						'comments': [
							'MnMn.exe.config の以下パラメータを false に変更してください',
							'finder_tab-tab_header-using-mouse-wheel',
							'注意: このファイルはアップデート等で上書きされます'
						]
					}
				]
			}
		]
	},
	{
		'date': '2016/09/14',
		'version': '0.19.0',
		'isRc': false,
		'contents': [
			{
				'type': 'features',
				'logs': [
					{
						'revision': '932bb3aa446eab65ed511a3115187cc332ef088d',
						'subject': '#138: プレイヤーウィンドウの開き方をユーザー制御に任せる',
						'comments': [
							'初期値は「新しいウィンドウで動画再生を行う」です。設定で好きに変えるかコンテキストメニューで適当に選択して下さい',
							'悩んだんだ、初期値ほんと悩んだんだ。1ウィンドウで開けと多数意見もらってるけど既存動作と異なるから操作の互換性上変えなかったんです、今しか変えられるタイミングないんだけどなんとなく変えなかったのです',
							'深い理由はないですが困っていて欲してる人はわざわざ自分で設定するからどうでもいいわ、っていうのが本心だけど建前は既存動作と維持のため。で落としどころにしましょうか'
						]
					},
					{
						'revision': '83a1c18bf13055adb93de8ed5bcbe1ea61f7cb07',
						'subject': '#116: ユーザーによるキャッシュ破棄'
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': 'a27fe43f8e0ee2817f38e53123eacd5681c4ab7e',
						'subject': '1.18.0 の更新履歴 #139 がエスケープシーケンス死んでた'
					},
					{
						'revision': '0b44d629cbdef69995abb13b489e56c1d7054f35',
						'subject': '#150: 動画読込中にプレイヤーサイズをメニューから変更するとキュッてなる'
					},
					{
						'revision': '3737563c5161459d6227e4718c138ac7e76c7453',
						'subject': '#145: マイリスト連続再生では並び替えてもマイリストの番号順に再生されるので、並び替えた順番で再生されるようにして欲しい'
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': '503ea1c60339ef23695e1afd4275261653a2f4c8',
						'subject': '#151: β版はバージョン番号を分かりやすくする'
					},
					{
						'revision': '81568531f96d52a771db9ac387355ca293fc7168',
						'subject': '#153: WebBrowser置き換え準備',
						'comments': [
							'アップデート用アーカイブダウンロード待ち時間を増やそうと思ったけど今時点で以外と待ってたので少し増やした',
							'30分 -> 45分'
						]
					},
					{
						'revision': 'f939d4e5a7852dbf0d7b23b5760755998fb121b6',
						'class': 'nuget',
						'subject': 'OxyPlot: 1.0.0-unstable2100 -> 1.0.0'
					}
				]
			}
		]
	},
	{
		'date': '2016/09/10',
		'version': '0.18.0',
		'isRc': false,
		'contents': [
			{
				'type': 'features',
				'logs': [
					{
						'revision': '1ba34884a05f5813e5a3a6da44af5133d01ef6a5',
						'subject': '#51: 外部再生機能の追加'
					},
					{
						'revision': 'e30d376a7ab45174ed797d93b54f0dd1cd7db0a3',
						'subject': '#21: 自動再生のタイミングを設定可能にする'
					},
					{
						'revision': '9c10cefa0c7891ef6b76c5a2f99ea2cf04e0dac1',
						'subject': '#24: 自動再生制御を確認できるUIが必要',
						'comments': [
							'自動再生で待ち状態のイメージに再生ボタンを砂時計にした',
							'ロジックもUIも何もかもが気に食わんけど多分動くから寛大な心で commit と close'
						]
					},
					{
						'revision': 'f2a1a337ed087aeed260ead0dc374ecbcadaa181',
						'subject': '#12: プレイヤーサイズを動画サイズにより調整',
						'comments': [
							'制限としてディスプレイの解像度を超過することは来ません',
							'起きうる問題としてマルチディスプレイで横並べだと横方向はかなり長くまで伸ばせるけど縦方向は伸ばせないから微妙なサイズになる',
							'恐らくレベルだと WPF の制限じゃないかと',
							'恐らくに多分だけどこねくり回せば何とかできると思うけど必要があればその時やる'
						]
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': 'cb88d1123d06a3d3d1c53e97a41f0fc9f32f5423',
						'subject': '#135: 本体設定タブをMnMnに入れる',
						'comments': [
							'こんなもん誰も頻繁にさわらん'
						]
					},
					{
						'revision': 'd8a8f2fdd41e5419086e76b4d33da9044472b56b',
						'subject': '#114: プレイリストメニューの次とか前とかのコントロール部分を上に持っていくべきか',
						'comments': [
							'試してみて思ったけどどう考えても上部配置が標準だわ',
							'> プレイリストが多いとスクロールが発生するから下にあるとなんだかなーと'
						]
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': '81427f0826140150e73b64ecdd084b9e001e019b',
						'subject': 'ゴミ処理後のログ出力処理を統一'
					},
					{
						'revision': 'ab296b85b767c3ff4ad9c422c9aaee15e2383829',
						'subject': '#139: β版実行を分かりやすくする',
						'comments': [
							'β版をいい感じに走らせるには <MnMn>\\bat\\beta.bat を実行してください',
							'多分いい感じにβ版が動きます'
						]
					}
				]
			}
		]
	},
	{
		'date': '2016/09/07',
		'version': '0.17.0',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'subject': 'ワタクシにメールをくださった誰かに',
						'comments': [
							'メールくれること自体は構いません',
							'内容がなんであれ判断はこちらの勝手なので要望でも要求でもなんでもいいんですが、使い捨てメール使われますと返信できないのでそこのとこ いい感じにお願いします'
						]
					}
				]
			},
			{
				'type': 'features',
				'logs': [
					{
						'revision': 'f83b8a851dd2d8b608aa3b1bb7e45acfd5232bb7',
						'subject': '#108: コメント数が 0 ならコメントグラフは表示しない'
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '44257edcc10bcd05a449a27e905d3c3d6a24798e',
						'subject': '#98: マイリストから動画を削除した時表示上まだ残ってる',
						'comments': [
							'律儀にキャッシュを読み込んでた'
						]
					},
					{
						'revision': 'e0f6045842a5176f989b8abcb59ed2c88532f65b',
						'subject': '#79: 検索やマイリスト遷移時でメインウィンドウ内自動遷移がうまくいかない'
					},
					{
						'revision': 'cb44891a365fc4ea1aa07d63df56f74306b09211',
						'subject': '#64: 動画サイズによってコメントがでかかったり表示領域が不思議',
						'comments': [
							'さわった感じプレイヤー(Meta.Vlc)のUIにおける実サイズと比率保持したうえでの動画描画サイズを算出した残骸っぽかった',
							'通常の動画だと動画の描画サイズに一致するけど縦長の動画の場合はコメント描画用領域の実サイズに合わせる形で横方向へ補正かかるようになりました',
							'コメント描画用領域の実サイズをどうしてもいじりたい人は App.config の以下項目を編集してくらはい',
							'service-smile-smilevideo-player-comment-width',
							'service-smile-smilevideo-player-comment-height'
						]
					},
					{
						'revision': 'dda9edc66451ba438b86967e4b1095ee89d4eb64',
						'subject': '#103: プレイヤーウィンドウのアイコン部分ダブルクリックでウィンドウを閉じられるようにする',
						'comments': [
							'Windows の UX だと左クリックでも出せないとダメなんだろうけど InvokeCommandAction だと気楽に実現できないので ダブルクリックと右クリックだけ対応',
							'物理座標と Windows API だから高DPI関係ないよね、ないよね、ないよね'
						]
					},
					{
						'revision': '9cf10fae2523dee812c158661c2f5dea58efed13',
						'subject': '#6: ランキングに年齢制限を反映する',
						'comments': [
							'20-30分で実装・テスト終わると思ってたけどえらい時間かかった',
							'(どっちにしろ再生できないけど)青少年に清い灰色な青春を'
						]
					}
				]
			}
		]
	},
	{
		'date': '2016/09/05',
		'version': '0.16.0',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '740c2d51baf8184f22b6cc8993021a7e1f621b37',
						'class': 'compatibility',
						'subject': '#105の実装に伴い 0.16.0 未満のコメント設定(フォントやサイズ等)が失われます'
					},
					{
						'revision': 'eae33f23d6730d7bffad26a7e3fc42ebabd3c5c9',
						'class': 'warning',
						'subject': '#106の実装に伴いログ出力内容が変わりました。通信内容を出力するようにしたため認証で使用する資格情報がログ出力されます',
						'comments': [
							'障害報告等でログを送っていただく際にはパスワードなどが含まれていないか確認してください',
							'標準では View 以外に出力は行いませんが明示的にファイル出力・クリップボードコピーする場合は注意してください',
							'View 側のログリスト内容も SecureString を使用していないため知らない人(やプログラム)にメモリを直接読まれないようにしてください',
							'でもまぁメモリ内容を読まれる恐れがある環境では遅かれ早かれ情報抜かれるので気にしなくてもいいと思います',
							'こんなこと書くとセキュリティが心配な人から変なメール飛んでくるので先に伝えておきます。',
							'MnMn ではパスワード保存に DPAPI を使用して端末のユーザーに紐付けた暗号化を行っているので Microsoft(っていうか Windows と .NET Framework)依存で安全です',
							'https://bitbucket.org/sk_0520/mnmn/src/master/Source/MnMn/Logic/Converter/PasswordConverter.cs',
							'死ぬときは MS と一緒に死にましょう'
						]
					}
				]
			},
			{
				'type': 'features',
				'logs': [
					{
						'revision': '0460b30cdd690ac10d766e3070bc5f31114d7aac',
						'subject': '#112: 動画個別設定に最終再生日時を保持させる'
					},
					{
						'revision': '433483be34e14cfef7d6a0b1b2826b20b40f015e',
						'subject': '#11: マイリストの並び替え'
					},
					{
						'revision': '8b98e4768bf5636fffbd0d99bedda9d07c894493',
						'subject': '#28: ユーザー制御で「あとで見る」に追加する'
					},
					{
						'revision': '871bfca4dd2cf0f7f8adc304054fc7d75dc1fcf2',
						'subject': '#125: タブ操作におけるUXを改善する',
						'comments': [
							'タブ上でマウスホイール操作を行うことでタブ移動します(左右スクロール可能なタブのみ)',
							'タブアイテム上でマウスホイールクリックで該当タブを閉じます(×ボタンありタブのみ)'
						]
					},
					{
						'revision': '7781a8a1376d7e3fa593cc2610ae7ee4fb549302',
						'subject': '#132: ヘルプファイルをWebブラウザで直接開く',
						'comments': [
							'別件(#120)でローカルのヘルプを直接のさ',
							'開いたところで何も書いてないけどね！'
						]
					},
					{
						'revision': '66f9a2ddedbb81bde3547f0aae9fa2048ef70e5a',
						'subject': '#96: ユーザーの任意操作でログの継続出力を実行する'
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '16e9c166eea3e0945e5c50d8c9af66c4c2f9fa31',
						'subject': '#122: せきゅ®ひてぃ保護'
					},
					{
						'revision': '740c2d51baf8184f22b6cc8993021a7e1f621b37',
						'subject': '#105: プレイヤー情報はプレイヤーウィンドウ破棄時に保存させる',
						'comments': [
							'コメント設定周りを一新したため過去の設定部分を破棄',
							'バージョンが 1.0.0 以下だと気楽に互換切れて開発側に優しい！',
							'もしかしたらどっかおかしいかもね'
						]
					},
					{
						'revision': 'e0a3002e3a659cb15ce3cfbd741b289a46759c1c',
						'subject': '#129: ファイルGCのサイズ単位が byte'
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': 'c5d4f861c9349c165ca8f0c7938c91b7a39abb9c',
						'subject': '0.15.0 の更新履歴で #42 の後続課題が #120 でなかった部分を修正',
						'comments': [
							'単独更新履歴の方は手で修正'
						]
					},
					{
						'revision': '75e81587d6a5b617c2c685f7efcc2e996b74ee17',
						'subject': '#130: 起動時のGC処理を抑制(ほぼデバッグ用)',
						'comments': [
							'App.config: background-garbage-collection-is-enabled-startup'
						]
					},
					{
						'revision': 'eae33f23d6730d7bffad26a7e3fc42ebabd3c5c9',
						'subject': '#106: ログ出力内容を見直す'
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
