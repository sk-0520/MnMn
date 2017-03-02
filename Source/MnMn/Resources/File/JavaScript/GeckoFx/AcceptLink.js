(function() {
	var link = document.getElementsByTagName('a');

	for (var i = 0; i < link.length; i++) {
		var a = link[i];
		a.setAttribute('target', '_blank');
	}
})()

