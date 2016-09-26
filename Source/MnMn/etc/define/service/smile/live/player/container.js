
const PlayerStateNone = 0;
const PlayerStateLoading = 1;
const PlayerStateLoaded = 2;
const PlayerStateTuning = 3;
const PlayerStateSuccess = 4;
const PlayerStateFailure = 5;


$(function () {
	var $frame = $('#cttn_mnmn_service_smile_live_frame');

	$frame.on('load', function () {
		if (!$frame.attr('src')) {
			return;
		}

		// 何かしら読み込んだ
		var aaa = $frame.contents;
		alert(aaa);
		loadedFrame();
	});
});

function loadedFrame($contents) {
	alert($contents)

	var $body = $contents.find('body');
	alert($body.html());
	var $flvplayer = $contents.find('#flvplayer');
	$body.append($flvplayer);
}
