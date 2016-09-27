
const PlayerStateNone = 0;
const PlayerStateLoading = 1;
const PlayerStateLoaded = 2;
const PlayerStateTuning = 3;
const PlayerStateSuccess = 4;
const PlayerStateFailure = 5;


$(function () {
	var $frame = $('#cttn_mnmn_service_smile_live_frame');

	$frame.on('load', function () {
		//alert($frame.attr('src'));
		if (!$frame.attr('src')) {
			return;
		}
		var i = $frame[0];
		if (i.contentDocument) {
			alert(i.contentWindow.document.getElementsByTagName('body')[0].innerHTML);
		} else {
			alert('取得できない');
		}
		var event = document.createEvent('MessageEvent');
		var origin = window.location.protocol + '//' + window.location.host;
		event.initMessageEvent('onload', true, true, 'some data', origin, 1234, window, null);
	});
});

function loadedFrame($contents) {
	alert("2" + $contents)

	var $body = $contents.find('body');
	alert($body.html());
	var $flvplayer = $contents.find('#flvplayer');
	$body.append($flvplayer);
}
