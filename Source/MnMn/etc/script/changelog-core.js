// 更新履歴出力・表示で統一使用するためのがんばり

function is(type, obj) {
	var clas = Object.prototype.toString.call(obj).slice(8, -1);
	return obj !== undefined && obj !== null && clas === type;
}

function Element(tagName) {
	if (!tagName) {
		throw "tagName is null";
	}

	this.tagName = tagName;
	this.nodes = [];
	this.attributes = {};

	this.setAttibute = function (name, value) {
		this.attributes[name] = value;
	}
	this.append = function (node) {
		this.nodes.push(node);
	}
	this.toCode = function () {
		var attr = [];
		for (var key in this.attributes) {
			var value = this.attributes[key];
			if (value) {
				attr.push(key + '=' + convertValue(value));
			} else {
				attr.push(key)
			}
		}

		var result = [];
		var attr = attr.join(' ');
		result.push('<' + this.tagName + (attr ? ' ' + attr : '') + '>');

		for (var i = 0; i < this.nodes.length; i++) {
			var node = this.nodes[i];
			if (is('String', node)) {
				result.push(convertElementText(node));
			} else if (is('Object', node)) {
				result.push(node.toCode());
			}
		}

		result.push('</' + this.tagName + '>');

		return result.join('');
	}
}

/**
主処理。
*/
function makeChangeLogBlock(changelog, isEmbeddedImage, imageBaseDirPath) {
	var title = makeChangeLogTitle(changelog);
	var contents = makeChangeLogContents(changelog['contents'], isEmbeddedImage, imageBaseDirPath);

	return {
		'title': title,
		'contents': contents
	};
}

function makeChangeLogTitle(changelog) {
	var version = changelog['version'];
	var date = changelog['date'];
	var title = new Element('h2');
	title.append(version + ', ' + date);

	if (changelog.isRc) {
		title.append(' ');

		var rc = new Element('em');
		rc.append('RC版');

		title.append(rc);
	}

	return title;
}

function makeChangeLogContents(contents, isEmbeddedImage, imageBaseDirPath) {
	var parent = new Element('dl')
	parent.setAttibute('class', 'changelog');
	var hasContent = false;

	for (var i = 0; i < contents.length; i++) {
		var content = makeChangeLogContent(contents[i], isEmbeddedImage, imageBaseDirPath);
		if (content.body != null) {
			hasContent = true;
			parent.append(content.head);
			parent.append(content.body);
		}
	}

	return hasContent ? parent : null;
}

function makeChangeLogContent(content, isEmbeddedImage, imageBaseDirPath) {
	var result = {
		head: null,
		body: null
	};
	var type = content['type'];
	var dt = new Element('dt');
	dt.setAttibute('class', type);
	dt.append(changelogTypeMap[type]);
	result.head = dt;

	if (content['logs']) {
		var hasBody = false;
		var body = new Element('dd');
		var parent = new Element('ul');
		for (var i = 0; i < content['logs'].length; i++) {
			var log = content['logs'][i];
			if (log['subject']) {
				hasBody = true;
				var subject = new Element('li');
				subject.append(convertMessage(log['subject']));
				if (log['class']) {
					subject.setAttibute('class', log['class']);
				}

				if (log['revision']) {
					var rev = new Element('a');
					rev.setAttibute('class', 'rev');
					rev.append(log['revision']);

					subject.append(rev);
				}

				if (log['comments']) {
					var hasComment = false;
					var comments = new Element('ul');
					comments.setAttibute('class', 'comment');
					for (var j = 0; j < log['comments'].length; j++) {
						var commentData = log['comments'][j];
						if (commentData) {
							hasComment = true;
							var comment = new Element('li')
							if (!is('String', commentData)) {
								var text = commentData[0];
								var image = commentData[1];
								var imageElement = CreateImageElement(text, image, isEmbeddedImage, imageBaseDirPath);
								comment.append(imageElement);
							} else {
								comment.append(convertMessage(commentData));
							}

							comments.append(comment);
						}
					}
					if (hasComment) {
						subject.append(comments);
					}
				}
				parent.append(subject);
				hasBody = true;
			}
		}
		if (hasBody) {
			body.append(parent);
			result.body = body;
		}
	}

	return result;
}

function CreateImageElement(text, image, isEmbeddedImage, imageBaseDirPath)
{
	var img = new Element('img');
	img.setAttibute('alt', text);
	if (isEmbeddedImage) {
		var ext = image.split('.').pop().toLowerCase();
		var map = {
			'jpg': 'jpeg',
			'jpeg': 'jpeg',
			'png': 'png'
		};
		var srcHead = 'data:image/' + map[ext] + ';base64,';

		var path = imageBaseDirPath + '\\' + image;

		var stream = new ActiveXObject("ADODB.Stream");
		stream.Type = 1;
		stream.Open();

		stream.LoadFromFile(path);
		
		//var binary = stream.Read(stream.Size);
		var binary = stream.Read(stream.Size);
		var doc = new ActiveXObject("Msxml2.DOMDocument");
		var element = doc.createElement("hex");
		element.dataType = "bin.hex";
		element.nodeTypedValue = binary;
		// 「text」を取得
		binary = element.text;

		img.setAttibute('src', binary);

		stream.Close();


		img.setAttibute('class', 'embedded')
	} else {
		img.setAttibute('src', imageBaseDirPath + '/' + image);
	}

	return img;
}

function convertMessage(s) {
	switch (s.slice('-1')) {
		case '!':
		case '！':
		case '?':
		case '？':
		case '…':
		case '。':
			return s;

		default:
			return s + '。';
	}
}

function convertElementText(s) {
	if (!is('String', s)) {
		return String(s);
	}
	return s.replace(/[&'`"<>]/g, function (match) {
		return {
			'&': '&amp;',
			"'": '&#x27;',
			'`': '&#x60;',
			'"': '&quot;',
			'<': '&lt;',
			'>': '&gt;'
		}[match]
	});
}

/**
http://stackoverflow.com/questions/7753448/how-do-i-escape-quotes-in-html-attribute-values
*/
function convertValue(s, preserveCR) {
	preserveCR = preserveCR ? '&#13;' : '\n';
	var value = ('' + s) /* Forces the conversion to string. */
		.replace(/&/g, '&amp;') /* This MUST be the 1st replacement. */
		.replace(/'/g, '&apos;') /* The 4 other predefined entities, required. */
		.replace(/"/g, '&quot;')
		.replace(/</g, '&lt;')
		.replace(/>/g, '&gt;')
		/*
		You may add other replacements here for HTML only
		(but it's not necessary).
		Or for XML, only if the named entities are defined in its DTD.
		*/
		.replace(/\r\n/g, preserveCR) /* Must be before the next replacement. */
		.replace(/[\r\n]/g, preserveCR);
	;

	return "'" + value + "'";
}

