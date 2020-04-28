$(document).ready(function(){
	var width = $('.in-slide').width();
	//position
	$('.slider-div').css({"width":width*3});
	$('.slider-div > li > div').css({"width":width});
	$('.slider-div').css({"left":-width});
	$('.slider-div > li:last-child').prependTo('.slider-div');
	var i =0;
	function next(){
		$('.slider-div').animate({
			"margin-left":-width
		},500,function(){
			$('.slider-div > li:first-child').appendTo('.slider-div');
			$('.slider-div').css({
				"margin-left":0
			});
		});
		if(i<2){i++;}else{i = 0;}

		$('#'+i +'>a').addClass('act');
	}
	function prev(){
		$('.slider-div').animate({
			"margin-left":width
		},500,function(){
			$('.slider-div > li:last-child').prependTo('.slider-div');
			$('.slider-div').css({
				"margin-left":0
			});
		});
		if(i>0){i--;}else{i = 2;}
		$('#'+i +'>a').addClass('act');
	}
	var idS = setInterval(function(){
		next();
		$('#'+i +'>a').removeClass('act');
	},10000);

	
	
	$('.next').click(function(){
		clearInterval(idS);
		$('#'+i +'>a').removeClass('act');
		next();

		idS = setInterval(function(){
			next();
			$('#'+i +'>a').removeClass('act');
		},14000);
	});
	$('.prev').click(function(){
		clearInterval(idS);
		$('#'+i +'>a').removeClass('act');
		prev();
		idS = setInterval(function(){
			
			next();
			$('#'+i +'>a').removeClass('act');
		},14000);
	});
	
	/* changed part */
	
	$('.index-li li').click(function(){
		
		switch(this.id){
			case "0":
				if(i == 2){
					$('.next').click();
				} else if (i == 1){
					$('.prev').click();
				}
			break;
			case "1":
				if(i == 0){
					$('.next').click();
				} else if (i == 2){
					$('.prev').click();
				}
			break;
			case "2":
				if(i == 0){
					$('.prev').click();
				} else if (i == 1){
					$('.next').click();
				}
			break;
			
		}
		
	});
	
	/* changed part */
});

