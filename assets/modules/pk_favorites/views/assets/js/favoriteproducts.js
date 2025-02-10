$(document).ready(function(){var fButton='favoritesButton',removeButton='remove-fav';$('body').on('click','.'+fButton,function(){if(favorites.favorite_products_id_product!==false){pid=favorites.favorite_products_id_product;}else{pid=$(this).data('pid');}
if($(this).hasClass('addToFav')&&$(this).hasClass('loggedin')){addToFavorites($(this),false);}
if($(this).hasClass('removeFromFav')&&$(this).hasClass('loggedin')){removeFromFavorites($(this),false);}
if($(this).hasClass('loginToAdd')){$('body').pkPopup({state:"info",text:favorites.phrases.haveToLogin});}
return false;});$('body').on('click','.'+removeButton,function(){removeFromFavorites($(this),true);return false;});if($('#favoriteproducts_block_account')[0]){$('#favoriteproducts_block_account .product-miniature').each(function(index,value){$(this).append('<div class="'+removeButton+' btn">'+favorites.phrases.delete+'</div>');});}});function ToggleButtonState(button){button.toggleClass('removeFromFav').toggleClass('addToFav').toggleClass('icon_checked');}
function hideProduct(item){item.closest('.product-miniature').slideUp();}
function addToFavorites($th,icon_button){$.ajax({url:favorites.favorite_products_url_add+'&rand='+new Date().getTime(),type:"POST",headers:{"cache-control":"no-cache"},data:{"id_product":pid},success:function(result){ToggleButtonState($th);$('body').pkPopup({state:"success",text:favorites.phrases.added});$th.find('span').text(favorites.phrases.remove);},error:function(){},beforeSend:function(){beforeSendCallback(icon_button,$th);},complete:function(){completeCallback(icon_button,$th);}});}
function removeFromFavorites($th,icon_button){if(icon_button){pid=$th.parent().data('id-product');}else{pid=favorites.favorite_products_id_product;}
$.ajax({url:favorites.favorite_products_url_remove+'&rand='+new Date().getTime(),type:"POST",headers:{"cache-control":"no-cache"},data:{"id_product":pid},success:function(result){if(icon_button){hideProduct($th);}else{ToggleButtonState($th);}
$('body').pkPopup({state:"info",text:favorites.phrases.removed});$th.find('span').text(favorites.phrases.add);},error:function(){},beforeSend:function(){beforeSendCallback(icon_button,$th);},complete:function(){completeCallback(icon_button,$th);}});}
function completeCallback(icon_button,$th){if(icon_button){$th.removeClass('in_progress');}else{$th.find('svg').removeClass('in_progress');}}
function beforeSendCallback(icon_button,$th){if(icon_button){$th.addClass('in_progress');}else{$th.find('svg').addClass('in_progress');}}