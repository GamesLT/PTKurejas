function ptkurejas_quiz(photo_data) {

	var el_prefix = 'ptkurejas_el_' + Math.random().toString(36).substr(7) + '_';
	var iomgs = new Array();
	var n_to_load = 0;
	var count = 0;
	var options_list = '';
	var cquestion = 0;
	var answers_all_list = new Array();
	var opacity_timer = window.setInterval(opacityMakeBigger, 10);
	var opacity = 0;
	var marquee_top = 0;

	function id(str) {
		return el_prefix + str;
	}

	function displayTimer(interval, message, func, func_chk) {
		if (!(!func_chk) && func_chk()) {
			func();
			return;
		}
		var k = message + "";
		k = k.replace("%d", interval);
		updateView(k);
		interval--;
		if (interval > -2) {
			setTimeout(function() {displayTimer(interval, message, func);}, 1000);
		} else {
			func();
		}
	}

	function showError(text) {
		var obj = document.getElementById('photo_quiz')
		obj.innerHTML = '<b>' + unescape('Klaida%3A') + '</b> ' + text;
		obj.style.borderWidth = '0px';
		obj.style.borderStyle = 'none';
		obj.style.backgroundColor = 'transparent';
		obj.style.color = 'auto';
		obj.style.height = 'auto';
	}

	function updateView(content) {
		var obj = document.getElementById(id('photo_quiz'));
		obj.innerHTML = content;
	}

	function addEvent(el, event, myFunction) {
		if (el.addEventListener) {
			el.addEventListener (event,myFunction,false);
		} else if (el.attachEvent) {
			el.attachEvent ("on" + event,myFunction);		
		} else {
			eval("el.on" + event + " = myFunction;");
		}
	}

	function showMain() {
		var gal = 'ai';
		if ((count > 10 && count <= 20) || (count % 10 == 0)  ) {
			gal = unescape('%26%23371%3B');
		} else if ( count % 10 == 1 ) {
			gal = 'as';
		} 
		var txt = '<div style="text-align: left;"><b>Apie</b><br /><br /><ul>';
		txt    += '<li>' + unescape('%u0160io%20testo%20metu%2C%20Jums%20bus%20rodomi%20paveiksl%26%23279%3Bliai%20i%u0161%20%26%23303%3Bvairi%26%23371%3B%20%u017Eaidim%26%23371%3B.') +  '</li>';
		txt    += '<li>' + unescape('Jums%20reik%26%23279%3Bs%20nuspr%26%23281%3Bsti%2C%20i%u0161%20kokio%20%u017Eaidimo%20kiekvienas%20paveiksl%26%23279%3Blis.') +  '</li>';
		txt    += '<li>' + unescape('Atsakius%20klausim%26%23261%3B%20jo%20atsakymo%20koreguoti%20nebebus%20galima%20-%20tod%26%23279%3Bl%20atid%u017Eiai%20pasirinkite%20atsakymus%21') +  '</li>';
		txt    += '<li>' + unescape('%u0160io%20testo%20rezultatai%20nebus%20i%u0161saugoti%20jokio%20duomen%26%23371%3B%20baz%26%23279%3Bje.') +  '</li>';
		txt    += '<li>' + unescape('Perkrovus%20puslap%26%23303%3B%20testas%20prasid%26%23279%3Bs%20nuo%20prad%u017Ei%26%23371%3B.') +  '</li>';
		txt    += '<li>' + unescape('Teste%20bus%20i%u0161%20viso') +  ' '+ count  +' klausim' + gal +  '.</li>';
		txt    += '</ul></div><p style="text-align: center;">';
		txt    += '<button id="'+id('begin_button')+'" accesskey="p">' + unescape('Prad%26%23279%3Bti') +  '</button>';
		txt    += '</p>';
		updateView(txt);
		document.getElementById(id('begin_button')).onclick = begin;
	}

	function dataExists() {
		if (!photo_data) {
			return false;
		}
		return true;
	}

	function begin() {
		cquestion = 0;
		answers_all_list = new Array();
		showNext();
	}

	function nextLoadStep() {
		if (!photo_data) {
			return showError( unescape('nepavyko%20atsi%26%23371%3Bsti%20%u0161iam%20testui%20reikaling%26%23371%3B%20duomen%26%23371%3B%20i%u0161%20serverio.') );		
		}

		var list = new Array();
		var llist = new Array();
		for (i=0;i<photo_data.answers.length;i++) {
			list[i] = photo_data.answers[i];
			llist[photo_data.answers[i]] = i;
		}
		list.sort();

		options_list += '<select id="'+id('answer')+'">';
		for (i=0;i<list.length;i++) {
			options_list += '<option value="' + llist[list[i]] + '">';
			options_list += list[i];
			options_list += '</option>';	
		}
		options_list += '</select>';

		loadImages();
	}

	function opacityMakeBigger() {
		var obj = document.getElementById(id('oda'));
		if (!obj) return;
		if (!obj.style.opacity) return;
		if (opacity > 100) return;
		obj.style.opacity = (opacity+=2) / 100;
	}

	function showNext() {
		cquestion++;
		if (cquestion > count) {			
			return finish();
		}
		var txt = '<div style="text-align: left;"><b>Klausimas ' + cquestion + unescape('%20i%u0161%20') + count + ' </b><div style="width: 440px; height: 330px; border-style: solid; border-width: 1px; position: relative; left: 4px; background-color: black;">';
		txt    += '<div id="'+id('oda')+'" style="background-image: url(' + iomgs[cquestion-1].src + '); width: 440px; height: 330px; background-repeat: no-repeat; background-color: black; background-position: center center;  opacity: 0.0;"></div></div>';
		txt    += unescape('I%u0161%20kurio%20%u017Eaidimo%20matomas%20vaizdas%20paveiksl%26%23279%3Blyje%3F') +' <br />';
		txt	   += options_list;
		txt	   += '<button id="'+id('record_answer')+'" accesskey="enter">Atsakyti</button>';
		txt    += '</div>';
		updateView(txt);
		opacity = 0;
		document.getElementById(id('record_answer')).onclick = record_answer;
	}

	function record_answer() {
		var answer_list = document.getElementById(id('answer'));
		var sel = answer_list.options[answer_list.selectedIndex];
		var i = 0;
		for (var x in photo_data.images)
		{
			if (++i == cquestion)
			{
				break;
			}
		}
		answers_all_list.push({
			result_data: [iomgs[cquestion-1].src, photo_data.answers[x], sel.text],
				is_good: sel.value == x,
				index: i
		});
		showNext();
	}

	function onLoadImage() {
		n_to_load--;
		updateView(unescape('Kraunasi%20paveiksl%26%23279%3Bliai%20%28liko%20atsi%26%23371%3Bsti%20') + n_to_load + ')...');
		if (n_to_load < 1)	{
			showMain();
		}
	}

	function loadAnotherImage(src) {
		var img = new Image();
		addEvent(img, 'load', onLoadImage);
		img.src = src;
		iomgs.push(img);
	}

	function updateCount() {
		count = 0;
		for(var x in photo_data.images) {
			if (!photo_data.images[x]) {
				continue;
			}
			count++;
		}
	}

	function loadImages() {
		updateCount();
		n_to_load = count;
		updateView(unescape('Kraunasi%20paveiksl%26%23279%3Bliai%20%28liko%20atsi%26%23371%3Bsti%20') + n_to_load + ')...');
		for(var x in photo_data.images) {
			if (!photo_data.images[x]) {
				continue;
			}
			loadAnotherImage(photo_data.images[x]);
		}
	}

	function scroll() {
		var obj = document.getElementById(id('marquee'));
		if (!obj) return;
		if (obj.style.position != 'relative')	{
			obj.style.position = 'relative';
		} 
		if ((-marquee_top) > obj.offsetHeight) {
			marquee_top = 330;
		} else {
			marquee_top--;
		}
		obj.style.top = marquee_top;
	}

	function ucfirst(str) {
		return str.substr(0, 1).toUpperCase() + str.substr(1);
	}

	function cssNameToDomName(css_name) {
		var parts = css_name.toLowerCase().split('-');
		var rez = parts[0];
		for(var i = 1; i < parts.length; i++) {
			rez += ucfirst(parts[0]);
		}
		return rez;
	}

	function createTag(tagName, params, styles) {
		var el = document.createElement(tagName);
		if (typeof params == 'object')
		{
			for(var attr in params) {
				el[attr] = params[attr];
			}
		}
		if (typeof styles == 'object')
		{
			for(var css_name in styles) {
				var dom_name = cssNameToDomName(css_name);
				var xdom_name = ucfirst(dom_name);
				el.style['ms' + xdom_name] = styles[css_name];
				el.style['o' + xdom_name] = styles[css_name];
				el.style['webkit' + xdom_name] = styles[css_name];
				el.style['moz' + xdom_name] = styles[css_name];
				el.style[dom_name] = styles[css_name];
			}
		}
		return el;
	}

	function gen_row(img_src, alt, your_answer, is_good) {
			var answer_code = alt;
			if (!is_good)
			{
				answer_code = '<del>' + your_answer + '</del><br /><ins>' + alt + '</ins>';
			}
			answer_code += '<div style="text-align: right; margin-top: 1em;padding: 0;margin-bottom:0;">';
			if (is_good)
			{
				answer_code	+= "<h5 style=\"color: green; margin-bottom:0; padding: 0;\">Teisinga</h5>";
			} else {
				answer_code	+= "<h5 style=\"color: red; margin-bottom:0; padding: 0;\">Neteisinga</h5>";
			}
			answer_code += '</div>';
			var row = createTag('tr');
			var c1 = createTag('td', {}, {width: "146px"});
			var c2 = createTag('td', {innerHTML: answer_code}, {'vertical-align': 'top'});
			var img = createTag('img', {src: img_src, alt: alt, title: alt, width: "146"});
			c1.appendChild(img);
			row.appendChild(c1);
			row.appendChild(c2);
			return row;
	}

	function finish() {
		var bad_length = 0;
		var table = createTag('table', { border: 0 }, {width: "100%"});
		for(var x in answers_all_list) {
			var answer = answers_all_list[x];
			table.appendChild(
				gen_row(
					answer.result_data[0],
					answer.result_data[1],
					answer.result_data[2],
					answer.is_good
				)
			);
			if (!answer.is_good)
			{
				bad_length++;
			}
		}
		var tbl = table.outerHTML;
		var results_block = '<div style="overflow-x: hidden; overflow-y: scroll; width: 448px; height: 330px; position: relative; left: 0px; border-width: 1px; border-style: solid; border-color: black; background-color: black;"><div id="'+id('oda')+'" style="background-color: #F2F2F2; opacity: 1;"><div id="'+id('marquee')+'">';
		results_block += tbl;
		results_block += '</div></div></div>';
		var txt = '<div style="text-align: left;"><b>Rezultatai:</b> ';
		if (bad_length == 0) {
			txt += 'Esi tikras(-a) guru!!!';
		} else if (bad_length/count < 0.14) {
			txt += unescape('Esi%20tikras%28-a%29%20eksperas%28-%26%23279%3B%29%21%21');
		} else if (bad_length/count < 0.28) {
			txt += unescape('Esi%20beveik%20ekspertas%28-%26%23279%3B%29%21');
		} else if (bad_length/count < 0.47) {
			txt += unescape('Esi%20vidutiniokas%28-%26%23279%3B%29.');
		} else if (bad_length/count < 0.71) {
			txt += unescape('%u0160ioje%20srityje%20esi%20jau%20ka%u017Ekiek%20pa%u017Eeng%26%23281%3Bs%28-usi%29.');
		} else if (bad_length/count < 1) {
			txt += unescape('%u0160ioje%20srityje%20esi%20kol%20kas%20naujokas%28-%26%23279%3B%29.');
		} else {
			txt += unescape('Tu%20esi%20tikras%20Ne%u017Einiukas%21');
		}
		txt    += '<br /><br />' + results_block + '<p></div>';
		updateView(txt);
		opacity = 0;
	}

	document.write('<div style="border-style: solid; border-width: 1px; text-align: center; width: 450px; vertical-align: middle; padding: 5px; background-color: white;" id="'+id('photo_quiz')+'">Kraunasi...</div>');

	displayTimer(150, 'Duomenys kraunasi...', nextLoadStep, dataExists);

	return photo_data;
}