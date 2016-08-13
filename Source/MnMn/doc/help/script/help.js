// 言語設定は一応考えるけど今のところ ja-JP で動けばそれでいい。テストもしてない。

var defaultLanguageKey = 'ja-jp';

var helpLanguage = {
	'ja-jp': {
		title: ' : MnMn ヘルプ',
		outLink: '外部リンク',
		hint: {
			icon: '📝',
			text: 'ヒント',
		},
		warning: {
			icon: '⚠',
			text: '注意'
		},
		bug: {
			icon: '🐞',
			text: 'バグ'
		},
		ref: {
			icon: '🔗',
			text: '参照'
		}
	}
};

var menuList = [
	{
		name: 'general',
		localize: true,
		level: 0,
		title: {
			'ja-jp': 'はじめに'
		}
	},
	{
		name: 'smile',
		localize: true,
		level: 0,
		title: {
			'ja-jp': 'ニコニコ動画'
		}
	},
	{
		name: 'changelog',
		localize: false,
		level: 0,
		title: {
			'ja-jp': '更新履歴'
		}
	}

	//
];
//----------------------------------------------------------------------


// http://stackoverflow.com/questions/19491336/get-url-parameter-jquery?answertab=votes#tab-top
function getParameter(key) {
	var pageUri = decodeURIComponent(window.location.search.substring(1));
	var params = pageUri.split('&');

	for (var i = 0; i < params.length; i++) {
		var paramNames = params[i].split('=');

		if (paramNames[0] === key) {
			return paramNames[1] === undefined ? true : paramNames[1];
		}
	}
}

function getLanguageCode() {
	var lang = getParameter('lang');
	if (!lang || typeof lang !== 'string') {
		return defaultLanguageKey;
	}

	return lang;
}

function getPageName() {
	var pageUri = location.pathname.substring(1)
	var index = pageUri.lastIndexOf('/');
	var fileName = pageUri.substring(index + 1);
	return fileName.split('.')[0];
}

function getPageTitle(lang, menuItem) {
	var title = menuItem.title[lang];
	if (title) {
		return title;
	}

	return menuItem.title[defaultLanguageKey];
}

function createMenu(lang, pageName) {
	var $menu = $('#main-menu');

	var param = 'lang=' + lang;
	var $ul = $('<ul>');
	var $top = null;
	for (var i = 0 ; i < menuList.length; i++) {
		var menuItem = menuList[i];

		var $li = $('<li>');

		$li.addClass('item');
		$li.addClass('level-' + menuItem.level);

		var title = getPageTitle(lang, menuItem);
		var target = menuItem.name + (menuItem.localize ? '.' + lang : '') + '.html?' + param;
		if (menuItem.name == pageName) {
			$top = $li;
			$li.text(title);
			$('h1').text(title);
			$('title').text(title + helpLanguage[lang].title);
			$li.addClass('level-active');
		} else if (menuItem.name.charAt(0) == '*') {
			$li.text(title);
		} else {
			var $link = $('<a>');
			$link.text(title);
			$link.attr('href', target);
			$li.append($link);
		}

		$ul.append($li);
	}
	$menu.append($ul);

	if ($top) {
		if ($menu.height() < $top.offset().top) {
			var y = $top.offset().top - $menu.offset().top;
			$menu.scrollTop(y);
		}
	}
}

function createLink(lang) {
	var param = 'lang=' + lang;

	var $content = $('#content');
	$content.find('.page').each(function () {
		var $page = $(this);
		// TODO: 内部リンク
		var pageName = $page.text();
		for (var i = 0; i < menuList.length; i++) {
			var menuItem = menuList[i];
			if (menuItem.name == pageName) {
				var title = getPageTitle(lang, menuItem);
				var target = menuItem.name + '.' + lang + '.html?' + param;
				var $link = $('<a>');
				$link.text(title);
				$link.attr('href', target);
				$page.empty().append($link);
				break;
			}
		}
	});
	$content.find('.issue').each(function () {
		var $issue = $(this);
		var $link = $('<a>');
		var number = $issue.text();
		var target = issueLink + number;
		$link.text(number);
		$link.attr('href', target);
		$issue.empty().append($link);
	});
	$content.find('a').each(function () {
		var $link = $(this);
		var uri = $link.attr('href');
		if (uri.match(/https?:\/\//)) {
			var help = helpLanguage[lang];
			var $out = $('<span>')
				.text(help.outLink)
				.addClass('out-link')
			;
			$link.append($out);
		}
	});
}

function createComment(lang) {
	var $content = $('#content');
	var help = helpLanguage[lang];
	$content.find('.hint, .warning, .bug, .ref').each(function () {
		var $element = $(this);
		var className = $element.attr('class');
		var comment = help[className];
		var $title = $('<em>').addClass('title');
		var $icon = $('<span>').addClass('icon').text(comment.icon);
		var $text = $('<span>').addClass('text').text(comment.text);

		$title.append($icon);
		$title.append($text);
		$element.prepend($title);
	});
}

function setPadding() {
	var $content = $('#content');
	$content.append($('<div>').addClass('padding'));
}

$(function () {
	var lang = getLanguageCode();
	var pageName = getPageName();
	if (pageName == 'help') {
		return;
	}

	createMenu(lang, pageName);
	createLink(lang);
	createComment(lang);
	setPadding();
});

