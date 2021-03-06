/**
最新のバージョンを抜き出す。
*/
var adSaveCreateOverWrite = 2;
var adWriteChar = 0;

var appName = 'MnMn';

var loadPath = "Source\\MnMn\\doc\\model-changelogs.js";
var imageBaseDirPath = "Source\\MnMn\\doc\\help\\image\\changelog";
var changelogCorePath = "Source\\MnMn\\etc\\script\\changelog-core.js";
var changelogMainPath = "Source\\MnMn\\etc\\script\\changelog.js";
var styleCommonPath = "Source\\MnMn\\etc\\style\\common.css";
var styleChangelogPath = "Source\\MnMn\\etc\\style\\changelog.css";
var saveRcPath = "Changelog\\update-rc.html";
var saveReleasePath = "Changelog\\update-release.html";
var downloadPageUri = 'https://bitbucket.org/sk_0520/mnmn/downloads/';
var jqueryPath = "Source\\MnMn\\etc\\script\\jquery.js"
var changelogHookScriptPath = ""; // hookChangelog(); を持つ js ファイルパス。

function isEnabledChangelogHook() {
	return changelogHookScriptPath != null && changelogHookScriptPath.length;
}

function createStream()
{
	var stream = WScript.CreateObject('ADODB.Stream');
	stream.Mode = 3;
	stream.Type = 2;
	stream.Charset = 'UTF-8';
	stream.Open();

	return stream;
}

function loadFile(path)
{
	var stream = createStream();
	stream.LoadFromFile(path);
	return stream.ReadText;
}

function writeLine(stream, s)
{
	stream.WriteText(s);
	stream.WriteText("\r\n");
}

function writeHead(stream)
{
	var scriptStream = createStream();
	scriptStream.LoadFromFile(changelogMainPath);
	var scriptText = scriptStream.ReadText;
	
	var styleCommonStream = createStream();
	styleCommonStream.LoadFromFile(styleCommonPath);
	var styleCommonText = styleCommonStream.ReadText;
	
	var styleChangelogStream = createStream();
	styleChangelogStream.LoadFromFile(styleChangelogPath);
	var styleChangelogText = styleChangelogStream.ReadText;
	
	writeLine(stream, "<!DOCTYPE html>\r\n");
	writeLine(stream, '<html>');
	writeLine(stream, '<head>');
	writeLine(stream, '<meta charset="utf-8">');
	writeLine(stream, '<title>' + appName + ' Update: 最新バージョン更新情報</title>');
	writeLine(stream, '<script>');
	writeLine(stream, scriptText);
	
	if(isEnabledChangelogHook()) {
		writeLine(stream, 'window.onload = function() { makeChangelogLink(); hookChangelog(); }');
	} else {
		writeLine(stream, 'window.onload = function() { makeChangelogLink(); }');
	}
	if(isEnabledChangelogHook()) {
		writeLine(stream, loadFile(jqueryPath));
		writeLine(stream, '//更新履歴のみの専用処理');
		writeLine(stream, loadFile(changelogHookScriptPath));
	}
	
	writeLine(stream, '</script>');
	writeLine(stream, '<style>');
	writeLine(stream, styleCommonText);
	writeLine(stream, '</style>');
	writeLine(stream, '<style>');
	writeLine(stream, styleChangelogText);

	writeLine(stream, '</style>');
	writeLine(stream, '</head>');
	writeLine(stream, '<body>');
}

function writeFoot(stream)
{
	writeLine(stream, '<p>');
	writeLine(stream, 'アップデートに失敗する場合や手動で適用する場合は <a href="' + downloadPageUri + '" target="_blank">ダウンロードページ</a> を参照してください。<br />');
	writeLine(stream, '0.74.0 未満からアップデート行う場合は手動もしくは「旧アップデート処理を使用」にチェックを入れてください。');
	writeLine(stream, '</p>');
	
	writeLine(stream, '</body>');
	writeLine(stream, '</html>');
}

// 処理開始

eval(loadFile(loadPath));
eval(loadFile(changelogCorePath));
eval(loadFile(changelogMainPath));

var updateVersions = [
	{
		isRc: true,
		path: saveRcPath
	},
	{
		isRc: false,
		path: saveReleasePath
	}
];


//個別履歴出力
for(var i = 0; i < updateVersions.length; i++) {
	var update = updateVersions[i];

	var stream = createStream();
	writeHead(stream);

	var targetLog = null;
	for(var j = 0; j < changelogs.length; j++) {
		if(changelogs[j].isRc) {
			if(update.isRc) {
				targetLog = changelogs[j];
				break;
			}
		} else if(!update.isRc) {
			targetLog = changelogs[j];
			break;
		}
	}
	if(!targetLog) {
		continue;
	}

	var log = makeChangeLogBlock(targetLog, true, imageBaseDirPath);
	writeLine(stream, log.title.toCode());
	writeLine(stream, log.contents.toCode());
	
	writeFoot(stream);

	stream.SaveToFile(update.path, adSaveCreateOverWrite);
	stream.Close();
}



