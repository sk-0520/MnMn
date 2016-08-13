/**
最新のバージョンを抜き出す。
*/
var adSaveCreateOverWrite = 2;
var adWriteChar = 0;

var loadPath = "Source\\MnMn\\doc\\model-changelogs.js";
var changelogCorePath = "Source\\MnMn\\etc\\script\\changelog-core.js";
var changelogMainPath = "Source\\MnMn\\etc\\script\\changelog.js";
var styleCommonPath = "Source\\MnMn\\etc\\style\\common.css";
var styleChangelogPath = "Source\\MnMn\\etc\\style\\changelog.css";
var saveRcPath = "Changelog\\update-rc.html";
var saveReleasePath = "Changelog\\update-release.html";

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
	writeLine(stream, '<title>Pe Update: 最新バージョン更新情報</title>');
	writeLine(stream, '<script>');
	writeLine(stream, scriptText);
	writeLine(stream, 'window.onload = function() { makeChangelogLink(); }');
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

	var log = makeChangeLogBlock(targetLog);
	writeLine(stream, log.title.toCode());
	writeLine(stream, log.contents.toCode());
	
	writeFoot(stream);

	stream.SaveToFile(update.path, adSaveCreateOverWrite);
	stream.Close();
}



