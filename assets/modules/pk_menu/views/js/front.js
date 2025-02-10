function getHtmlHide(nIpad,numLiItem)
{var htmlLiHide="";if($("#more_menu").length==0)
for(var i=(nIpad+1);i<=numLiItem;i++)
htmlLiHide+='<li>'+$('#_desktop_top_menu_pk ul.menu-content li.level-1:nth-child('+i+')').html()+'</li>';return htmlLiHide;}
function addMoreResponsive(nIpadHorizontal,nIpadVertical,htmlLiH,htmlLiV,htmlMenu)
{if(nIpadHorizontal>0&&nIpadVertical>0)
{if($("#more_menu").length>0)
$("#_desktop_top_menu_pk .container").html(htmlMenu);if($(window).width()>750&&$(window).width()<=992)
{if($("#more_menu").length==0)
{$('<li id="more_menu" class="level-1 more-menu"><a href="#"><span class="icon-plus"></span>'+text_more+'</a><ul class="menu-dropdown cat-drop-menu pk-sub-auto">'+htmlLiV+'</ul></li>').insertAfter('#_desktop_top_menu_pk ul.menu-content li.level-1:nth-child('+nIpadVertical+')');}
if($("#more_menu").length>0)
for(var j=(nIpadVertical+2);j<=(numLiItem+1);j++)
{$("#_desktop_top_menu_pk ul.menu-content li:nth-child("+j+").level-1").remove();var delItem=nIpadVertical+2;$("#_desktop_top_menu_pk ul.menu-content li:nth-child("+delItem+").level-1").remove();}}
else if($(window).width()>992&&$(window).width()<=1199)
{if($("#more_menu").length==0)
$('<li id="more_menu" class="level-1 more-menu"><a href="#"><span class="icon-plus"></span>'+text_more+'</a><ul class="menu-dropdown cat-drop-menu pk-sub-auto">'+htmlLiH+'</ul></li>').insertAfter('#_desktop_top_menu_pk ul.menu-content li.level-1:nth-child('+nIpadHorizontal+')');if($("#more_menu").length>0)
for(var j=(nIpadHorizontal+2);j<=(numLiItem+1);j++)
{$("#_desktop_top_menu_pk ul.menu-content li:nth-child("+j+").level-1").remove();var delItem=nIpadHorizontal+2;$("#_desktop_top_menu_pk ul.menu-content li:nth-child("+delItem+").level-1").remove();}}}}
$(document).ready(function()
{$('.icon_menu').click(function(){$(this).next().toggle();});$container_width=$('#header').width();$.each($('#top-menu .menu-dropdown'),function(index,element){if(!$(element).hasClass('pk-sub-right')){$elem_width=$(element).width();$elem_offset=$(element).offset();totalWidth=parseInt($elem_offset.left)+parseInt($elem_width);if(totalWidth>$container_width){$(this).addClass('pk-sub-right');}}});if($(window).width()<768){$('#_desktop_top_menu_pk ul.menu-content').css('display','none');}
$(window).resize(function(){if($(window).width()<768){$('#_desktop_top_menu_pk ul.menu-content').css('display','none');}
else{$('#_desktop_top_menu_pk ul.menu-content').css('display','block');}});var w=$(window).width();if(w>768){var timer;var dd_cont='.pk-sub-menu';$('#top-menu .parent').hover(function(){clearTimeout(timer);$(dd_cont).not($(this).parents().not(".dd_el_hover")).stop().slideUp(200,'easeOutExpo');$(this).children(dd_cont).stop().slideDown(500,'easeOutExpo');$(this).addClass('dd_el_hover');},function(){var $self=$(this).children(dd_cont);timer=setTimeout(function(){$self.stop().slideUp(500,'easeOutExpo');$(this).removeClass('dd_el_hover');},500);});}});