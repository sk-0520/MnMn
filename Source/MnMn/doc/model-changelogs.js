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
		'version': '0.31.2',
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
	{
		'date': '2016/11/20',
		'version': '0.31.0',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'subject': 'あ、安定性が向上、したんじゃないかなぁ？'
					},
					{
						'revision': '',
						'subject': '0.31.0 は 0.31.1 に統合',
						'comments': [
							'リリース直後不具合対応は疲れるのです'
						]
					}

				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '1be7e4691c31a95909fae3d6e3e311e3d6987ec3',
						'subject': '#248: キャッシュ整理時にキャッシュディレクトリが存在せずメモリ内に動画情報を保持してると落ちる',
						'comments': [
							'GC処理で新形式のファイル整理を行うためにディレクトリ内ファイルを列挙するけどディレクトリがなければ連鎖的に死んでいた'
						]
					},
					{
						'revision': 'ba288f75a3bf673fd0f7d9d5a0f58cb53c293b4a',
						'subject': '#246: タブ切り替え時に落ちる',
						'comments': [
							'わっからん！',
							'一応なんやかんや実装を変えたけど多分無意味だろうから同じようなのが再発すれば別課題として対応する',
							"再現できないとやっぱつらい。でも再現しやすいようにシングルスレッドで組むと UX 最悪だから＼( 'ω')／ウオオオオアアーーーッッ！！"
						]
					},
					{
						'revision': '90db01a3cea3d43b4621837939899d485f0fbcef',
						'subject': '#252: 動画生成が正しく行われない',
						'comments': [
							'リリース後 3 秒でリリース撤回した 0.31.0 は #250 の影響で動画切り替え時に音声のみ再生される摩訶不思議プレイヤーになったので Meta.Vlc を前バージョンに差し戻し',
							'Vlc 側のドキュメント読んだりソース眺めたりコードいじったりしたけどぜっんぜんうまくいかなかったのね'
						]
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': '17601f282160127ee510e4359f3ca5b19d2d9ab5',
						'subject': '#251: マイリストのプレースホルダがtoolkitを使用したまま'
					}
				]
			}
		]
	}, {
		'date': '2016/11/13',
		'version': '0.30.0',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'class': 'compatibility',
						'subject': '#239対応により動画のブックマークデータが破棄されます',
						'comments': [
							'どうしても引き継ぎたい場合は setting.json を手動で何とかする必要があります',
							'https://groups.google.com/d/topic/mnmn-forum/-5SHfM0_tW0/discussion'
						]
					}
				]
			},
			{
				'type': 'features',
				'logs': [
					{
						'revision': 'dfd0f64694184571f6adb6abf97c7e95349e5bfb',
						'subject': '#167: マイリスト・ブックマークの領域調整を設定に落す',
						'comments': [
							'#239 対応によりブックマークのデータ拡張性が増したので対応',
							'課題件名にはないけどユーザーも領域設定できるようにした'
						]
					},
					{
						'revision': '978b492f4d67a7ec848e6f4d5e68fc32acb217e3',
						'subject': '#241: とりあえずブックマーク的なことを行う'
					},
					{
						'revision': '43fc7cb898f992bf0826e04143b00e3481d86dc4',
						'subject': '#242: ファインダー表示要素のD&Dをサポートする',
						'comments': [
							'ブックマークのアイテム移動ができるようになったよ',
							'マイリストはAPIがあるっぽいから今回分からは見送り'
						]
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': 'd1c06d30a6495e42492ad2f8820b1db0122c145f',
						'subject': '#239: ブックマークのデータ構造を変更',
						'comments': [
							'やべぇ……2コミットでそれも少ししかキーボードさわってないのに出来てもた、マウス操作の方が多かった',
							'実装中の不具合が少なければ少ないほどその実装の危うさを感じることはないね'
						]
					},
					{
						'revision': 'f6406367744703f8873c2b92fbd6c5814ffe7d0f',
						'subject': '#237: 全ての動画に対するフィルタリングチェック状態がなんか変だね'
					},
					{
						'revision': '2076e3d7b58f5349a8f2230786244a2a3244804c',
						'subject': '#131: GC処理のタスクが精神衛生上きつい'
					},
					{
						'revision': 'c6e22d6277f3536094a9034b0085e5cd1fe0bf63',
						'subject': '#234: マウスカーソルを動かさずにフルスクリーンにするとカーソルが表示されたままになる'
					},
					{
						'revision': 'e49d245ceafd068ff709b9c8556e1a3bb4759f9b',
						'subject': '#225: すんごい今更だけど検索時の動画につく表示番号が全て1始まりってどうなの'
					}
				]
			}
		]
	},
	{
		'date': '2016/11/05',
		'version': '0.29.0',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'class': 'compatibility',
						'subject': 'このままいけば次回リリース(0.30.0)でブックマークが初期化されますですわよ',
						'comments': [
							'課題: #239 ブックマークのデータ構造を変更',
							'窓口: https://groups.google.com/d/topic/mnmn-forum/-5SHfM0_tW0/discussion'
						]
					},
					{
						'revision': '',
						'subject': '簡易アンケートフォームとして「目安箱」を実装しました',
						'comments': [
							'ログインやメールが不要なので気軽に書けます',
							'ただし気軽に書けるのでその内容は無責任で投げやりだと思いますので開発側としてはそう頻繁に確認しません',
							'意見の交換とそれを外部に周知できるフォーラムが、ソース公開しているフリーソフトウェアとしては健全だと思うのですよ'
						]
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
						'subject': '#233: コメントグラフを Popup から内部コンテンツに移す',
						'comments': [
							'ついでに線の幅をシュッとさせた'
						]
					},
					{
						'revision': '5f45d7f200f062135bc745ed7a5ae43fb7cb8c7e',
						'subject': '#227: あとで見るにユーザー操作で追加する場合はチェック済み期間を無視する',
						'comments': [
							'ファインダーからあと見るを選択した場合は既に見たか否かに関係なくあとで見るに追加されます'
						]
					},
					{
						'revision': '89d75d4089fa4abdde39f75ebc4f876950615cf4',
						'subject': '#224: 投稿者コメントのコメント有効領域追従機能を有効領域設定部のコンテキストメニューに移す',
						'comments': [
							'一等地からの追放'
						]
					},
					{
						'revision': 'f8e12e9bc35c2b0abfb44f39c2c7aa1abcfa1a3d',
						'subject': 'プレイヤーのナビゲータ部分をマウスオーバー時のみ表示するように変更した',
						'comments': [
							'タッチデバイス？ 聞こえんな～'
						]
					},
					{
						'revision': '8a62c18be22dee7375458cbbac35b1d8b4c088c2',
						'subject': 'アンケート機能を一応つけた',
						'comments': [
							'MnMn -> 目安箱',
							'意見や共有情報が匿名の一方通行で閉じてしまうのであんまりやりたくなかったけどとりあえず追加した',
							'基本的に私はあんまり回答を見ないと思うけど意見収集という機能だけに注視すれば害より益に傾くかなと思った'
						]
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '',
						'subject': '#111: コメントグラフの初回表示でかくつく',
						'comments': [
							'たぶん #233 で直ったんじゃないすかね',
							'しらんけど'
						]
					},
					{
						'revision': '0ebee38a1616cc56769373fb2c6732c329457df8',
						'subject': '#236: フォーラムタブ(のgecko)で新規ウィンドウ生成のハンドリングが出来てない'
					},
					{
						'revision': 'a273b3d0c842ddcb9e17876d425c7ebd707b63b0',
						'subject': '#238: 定型フィルタをちょこっと整理したい',
						'comments': [
							'不具合があったとかじゃないけどフィルタ内容いじったし「修正」かな',
							'定型に「あのゲーム機」を追加',
							'あのゲーム機だよ、ほらあれだよ'
						]
					},
					{
						'revision': '1370d603af41ab9982e8ff20614bf728caa98aa4',
						'subject': '#235: コメントフィルタリングで非NGには表示不許可項目を表示すべきでない'
					}
				]
			}
		]
	},
	{
		'date': '2016/11/03',
		'version': '0.28.0',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'class': 'warning',
						'subject': '内部的なお話だけど #228 で設定データモデルを改名したのでもっかしたらコメントフィルタ設定吹っ飛ぶかも',
						'comments': [
							'開発環境では大丈夫だったから大丈夫なんじゃないかな',
							'でも保証できないから一応書いとく'
						]
					},
					{
						'revision': '',
						'subject': '世間的なソフトウェアのリリース周期から見るとそうでもないけど、なんか久しぶりのリリースって気がするね！',
						'comments': [
							'飲み会だったり酒飲んだりダークソウル3のDLCだったり宴会だったりで忙しいのですよ！'
						]
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
						'revision': '4c13c07c2be50878e7a6dca3ed9cca21f877334b',
						'subject': '#215: 投稿者コメントはフィルタリング対象から外すべき',
						'comments': [
							'初期状態では外すようにした',
							'更新履歴としてこれが機能なのか修正なのかよくわからんかったからとりあえず機能にした'
						]
					},
					{
						'revision': '0514d998e8b8b99f179b3ddd118f4268d1f11ea1',
						'subject': '#182: コメントフィルタがどのフィルタ項目で許容されなくなったっか表示する'
					},
					{
						'revision': '307d85d77b526ba86dca1675211ca022fe9a94fd',
						'subject': '#217: フィルタリングされたコメントをコメント一覧で定型文に置き換える'
					},
					{
						'revision': 'c5d1dc0832f44e2446fe907f8804b3cf2e7548bb',
						'subject': '#62: フィルタリングアイテムの有効無効を切り替えられるようにする',
						'comments': [
							'無理やり工事したので内部的に設計と実装が喧嘩してる'
						]
					},
					{
						'revision': '6eb1393748b509c07a546347c6e18c419715d636',
						'subject': '#126: ファインダーフィルタを簡易操作で行う'
					},
					{
						'revision': '6b6d8cecfd3cab5f8490e7ba6500abb12e66a2a6',
						'subject': '#127: コメントフィルタを簡易操作で行う',
						'comments': [
							'コメント一覧からコメント詳細で各種フィルタ選択を行えます',
							'全ての動画にフィルタを適用するにはShiftキーを押しながらボタンやメニューを選択してください',
							'WPF 長くやってるつもりだけど未だに毎回解消する方法を模索してる -> System.Windows.Data Error: 4 : Cannot find source for binding with reference'
						]
					},
					{
						'revision': '9e4bcfd58171474dab2bc224c815ca7eac5a98b8',
						'subject': '#143: 外部再生プログラムの選択ダイアログを少し親切にする'
					},
					{
						'revision': 'd422f880dd29ca34d94059fd28107624068f4e5c',
						'subject': '#232: 本体タブにフォーラム追加',
						'comments': [
							'君は正しく、そしてフォーラムに書き込みたまえよ',
							'良いか悪いかはともかく未制御の GeckoFx だと Cookie 落さないんだね'
						]
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '4e6362ee5da6ba067db44dd15f25f7d94d82aea1',
						'subject': '#226: ファインダーフィルタを使用しない項目でもフィルタUIが表示できる'
					},
					{
						'revision': '71e2b0ed03d9c0c4210ef259c7a3b5851ae10a0e',
						'subject': '#229: 連続再生時が非ランダムだとプレイリスト周回毎に一周分の動画情報データがメモリにのしかかる',
						'comments': [
							'プレイリスト一周分の動画数 * クラス参照サイズ(たぶん x86 で 4 byte?) + (メタ情報) でプレイヤーウィンドウ破棄時にはクリアされるけどなんとなく気持ち悪いよね',
							'要は直ちに影響はないからこそ影響ないうちに直しておこうって話'
						]
					},
					{
						'revision': '6b6d8cecfd3cab5f8490e7ba6500abb12e66a2a6',
						'subject': '#230: 外部プログラム未設定時に外部プログラムで動画を開くと落ちる。落ちるて……',
						'comments': [
							'ネットワークやサービス、サードパーティ製ライブラリの都合ならともかくこのソフトウェア内部の都合で落ちるのはしんどいよね',
							'つーか使用者の分母数も分からんけど報告もらってないし誰も使ってないんだろうなこれ'
						]
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': 'eb0302b9be76f1856a0cd7df71c4909862d4c66e',
						'subject': '#228: コメントフィルタの命名が頭おかしい',
						'comments': [
							'SmileVideoCommentFilteringItemEditViewMode -> SmileVideoCommentFilteringItemSettingModel',
							'ファイル名はまともだったからクラス名の SmileVideoCommentFilteringItemEditViewMode はリファクタで事故ったな'
						]
					}
				]
			}
		]
	},
	{
		'date': '2016/10/23',
		'version': '0.27.0',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'subject': '新形式動画のキャッシュに意地悪するとキャッシュ出来てるけどキャッシュ出来ていない状態になってそれでも再読み込み時にはキャッシュが使われるけどキャッシュされなくてキャッシュされている不具合の修正が主です',
						'comments': [
							'考えるな、感じろ'
						]
					}
				]
			},
			{
				'type': 'features',
				'logs': [
					{
						'revision': '0d0ede080dd0e7c1af8d3d89d4b95e80629ac31b',
						'subject': '#74: 本体設定をそろそろちゃんとする',
						'comments': [
							'そもそも本体設定がほとんどなかった'
						]
					},
					{
						'revision': 'a099398c9c8a14274eabc6be8dcc1644bec89cba',
						'subject': '#71: ランダム再生をきちんとランダム再生する'
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': 'fda727368b2975be7c5e8750b398b77fd6fbd85e',
						'subject': '#219: typoとかコメント整備'
					},
					{
						'revision': '98f1717c4e2d62674a5f26a3c14537390e72a592',
						'subject': '#223: アップデート情報のHTML生成プログラムがPeからパクってきたまま'
					},
					{
						'revision': '9db6efd6d1748273e11838a26756eba3bdd0c34c',
						'subject': '#220: 新形式ダウンロードにおける動画情報が設定に保持されないことがある',
						'comments': [
							'実運用に支障はないけどプログラムの内部的にエラーになるため修正'
						]
					},
					{
						'revision': '0a57bd2d8d55978f1bf4f81e67646bff856a60ed',
						'subject': 'プレイヤー領域の 4:3 が  4:4 になっていた'
					},
					{
						'revision': 'c12eda8ac338628ad4a43ddde39a8d40c21ce858',
						'subject': '#199: アクセントカラーに対する Path の描画スタイルが汎用的でない'
					}
				]
			}
		]
	},
	{
		'date': '2016/10/18',
		'version': '0.26.0',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '',
						'subject': '#147 対応により新形式の動画を取得するのでキャッシュ容量にご注意を',
						'comments': [
							'たぶんだけど公式側でちょくちょく仕様変更入ると思うので調子悪い時は設定からチェックを外してください',
							'一応新形式が落せないものは旧型式の処理で対応するから大丈夫だとは思いますが頭の片隅にでも置いといてください',
							'まぁ仕様変更あったらプログラム吹き飛ぶだけじゃないかな。死にはしないよ、問題ないさ'
						]
					},
					{
						'revision': '',
						'subject': '新形式にちゃんと対応したと見せかけて内部的には #181 の方が大工事だったりした'
					}
				]
			},
			{
				'type': 'features',
				'logs': [
					{
						'revision': 'abaf71ba8cbdc01de91530c05ff36b4851c95aa5',
						'subject': '#209: App.configのユーザファイルをひな形として準備しておく'
					},
					{
						'revision': '5c746c9f07957e553f791b9f72ca0b10358a1b82',
						'subject': '#206: 新規タブの位置を標準的なUXにする',
						'comments': [
							'新規タブは右側に追加されます'
						]
					},
					{
						'revision': '8c742cfe2e4ca513720998570d2b42602037673a',
						'subject': '#146: 動画の背景色変更',
						'comments': [
							'自動の場合はサムネイルから算出する'
						]
					},
					{
						'revision': 'cbcb6454806566e7a8f149edd3995546b887409e',
						'subject': '#147: 1.5GBの投稿動画',
						'comments': [
							'一応対応できたっぽい',
							'とりあえず初期設定では新形式を有効にしておいた',
							'新形式で落しているときはステータスバーのニコニコアイコンが黄色くなってるはず',
							'なんか遅い。未読込地点へのシーク移動と組み合わせれば効果的なんだろうけどキャッシュ前提のこのソフトでとびとびのデータ書き込んだらえらいことなるから何も考えないことにした',
							'ニコニコの設定に「ダウンロード」を追加(キャッシュのダウンロード設定であってダウンローダーじゃないよ)',
							'動画ソース・音声ソースでどの品質を用いるか重み付で設定するイメージ。仕様も分からん・ソース数も分からん、となるとこの方式しかなかったんよ',
							'原理的には実際のソース群を昇順ソートしてソース数から設定した重みに適合するソースを先頭にして残りソース数の1/2が適合ソースの元位置を超えるかどうかで反転させてる',
							'何書いてんのかわからんね、詳しくはコードでも見てちょ',
							'https://bitbucket.org/sk_0520/mnmn/src/3b14010e0235426b2b46cfbe49b9e50b8ecfe008/Source/MnMn/Logic/Utility/Service/Smile/Video/SmileVideoDmcObjectUtility.cs?fileviewer=file-view-default#SmileVideoDmcObjectUtility.cs-99',
							'状況によるけどキャッシュファイルがわんさかできたりサイズも大きかったりするのでその辺り注意してね',
							'あとファインダーでいうところのキャッシュ有無表示はどれか一つのソースでもキャッシュがあれば「あり」と判断するので悪いソースでキャッシュ作ってても良いソースが使えるのならダウンロードすることになる'
						]
					},
					{
						'revision': '3c08fcb3bffd2221cb758581194e1b95acdef2e1',
						'subject': '#212: コメント有効領域に対する自明的な説明'
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '595ebeff81a36c9b6894b7c49be641503b31bf8b',
						'subject': '#208: ファインダーのチェックボタン消えた！',
						'comments': [
							'ファインダーの Grid 要素の 0 column が上書きされてたよ！'
						]
					},
					{
						'revision': 'd82132d5bcd3d6158fb39f87a504a13d0b0109ae',
						'subject': '#205: フルスクリーン状態のプレイヤーサイズが変更できる'
					},
					{
						'revision': 'b955b95f81e62c3214e1cb80f22a6f2627164080',
						'subject': '#202: リンクを示す表示要素がテーマとケンカするし一目でリンクと分からないものもある',
						'comments': [
							'妥協と妥協と妥協の産物',
							'思想的なものでもなく互換性でもなくただの実装上都合での妥協ってひさびさだからﾑｷｰってなるわ'
						]
					},
					{
						'revision': '08c69528f48c7307fe3228b427755bedb553995d',
						'subject': '#207: 動画によってコメントの描画領域が補正できていない'
					},
					{
						'revision': '3f90019947a0cbbd9da5969d10eafe6871bd9d1a',
						'subject': '#181: 同一プレイヤーでの動画遷移がうまくいかない',
						'comments': [
							'ダウンロード処理周りにかなり手を入れた',
							'いっぱいマウスクリックしたので大丈夫だと思いたい',
							'右見ても左見ても非同期処理しかなくて脳内同期エミュレートできなったので取りこぼしいっぱいあるはず'
						]
					},
					{
						'revision': '0b7f37436da8a1b8fc87434608a6bd5cd5372d27',
						'subject': '#213: テーマの適用タイミングを設定画面に書いとく'
					}
				]
			}
		]
	},
	{
		'date': '2016/10/15',
		'version': '0.25.0',
		'isRc': false,
		'contents': [
			{
				'type': 'note',
				'logs': [
					{
						'revision': '7640c21936d196bc0009008a90107456ecbed41e',
						'class': 'open',
						'subject': '#147: 1.5GBの投稿動画',
						'comments': [
							'最近の問い合わせガン無視してがむしゃら実装により、新形式の投稿動画に暫定対応しました',
							'有効にするにはニコニコ動画 -> 設定 -> 動画再生方法 から 新形式を使用 にチェックしてください',
							'暫定ということもありチェック状態は保存されません',
							'既存の動画取得方法と異なりネット上に情報が転がってなかったので解析してみたけど本当に正しい方法なのかなーんも分かりません',
							'なので新形式で配信されてて何か動作が変な動画があれば教えてください、デバッグしてるワタクシはアナタよりこのソフトを連続使用してないので探せません'
						]
					}
				]
			},
			{
				'type': 'features',
				'logs': [
					{
						'revision': 'bfd6f21dec363a910c3efc4347cfb72470d3e499',
						'subject': '#197: App.config に対するユーザファイルの上書き',
						'comments': [
							'MnMn.exe.config と同じディレクトリに MnMn.exe.user.config を配置すると既存の App.config 内容を上書き(マージ)します', ,
							'詳細は https://msdn.microsoft.com/ja-jp/library/aa903313 を参照してください',
							'(.NET アプリで一番使われてそうな構成セクションなのにメンテナンスされてないってすげーなMS)'
						]
					},
					{
						'revision': 'a2276fc10c1c330894bbfb8ec3f45081a45a366a',
						'subject': '#36: 使用許諾の文言をまともにする'
					}
				]
			},
			{
				'type': 'fixes',
				'logs': [
					{
						'revision': '64439bf5db206e109df41d25601f1421cfe52b11',
						'subject': '#172: 非ログイン時に非活性なタブをマウスホイールで選択できる'
					},
					{
						'revision': '61984fb40b0d5929900141f5d295ed77cc2cfe71',
						'subject': '内臓ブラウザの履歴がないときに履歴破棄は行わない'
					},
					{
						'revision': '085f0773431044080b327ddddc6b04a3a24e87d1',
						'subject': '#200: 更新履歴のaタグ変でっせ'
					},
					{
						'revision': '83b833f3b0c34a63de5783d9a07f41afed7c64e7',
						'subject': '#201: タブのピン止めアイコンが画像のまま'
					},
					{
						'revision': '7133c103971d8575332725d98d449a7455516288',
						'subject': '#198: フルスクリーン時にウィンドウの境界線が残ってる'
					}
				]
			},
			{
				'type': 'developer',
				'logs': [
					{
						'revision': '7640c21936d196bc0009008a90107456ecbed41e',
						'class': 'open',
						'subject': '#147: 1.5GBの投稿動画',
						'comments': [
							'既存実装から外れるのでややこい',
							'今のとこポーリング期間とかは完全固定値',
							'次の次くらいのリリースには close したい'
						]
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
