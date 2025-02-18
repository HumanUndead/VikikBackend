var wishlistProductsIds=[];$(document).ready(function(){wishlistRefreshStatus();$(document).on('change','select[name=wishlists]',function(){WishlistChangeDefault('ws_wishlist_block_list',$(this).val());});$(document).on('change','.wishlist_group_actions',function(){if($(this).val()=='1'){var id_wishlist=$("#table_wishlist").data("id-wishlist")
$('.wishlist_group_chb:checked').each(function(){var tt_id_product=$(this).data("id-product");var tt_id_pa=$(this).data("id-pa");var tt_qty=$('#quantity_'+tt_id_product+'_'+tt_id_pa).val();var tt_prior=$('#priority_'+tt_id_product+'_'+tt_id_pa).val();WishlistProductManage('wlp_bought','delete',id_wishlist,tt_id_product,tt_id_pa,tt_qty,tt_prior);});$(this).val('0');}});$('.wishlist').each(function(){current=$(this);$(this).children('.wishlist_button_list').popover({html:true,content:function(){return current.children('.popover-content').html();}});});});function WishlistCart(id,action,id_product,id_product_attribute,id_wishlist)
{var t_quantity=1
if($('#quantity_wanted').length>0){t_quantity=$('#quantity_wanted').val();}
if(prestashop.customer.is_logged===false){$('body').pkPopup({state:"info",text:wishlist.login_first});return false;}
$("#wishlist_button_block > a svg").addClass('in_progress');$.ajax({type:'GET',url:prestashop.urls.base_url+'modules/pk_wishlist/cart.php?rand='+new Date().getTime(),headers:{"cache-control":"no-cache"},async:true,cache:false,data:'action='+action+'&id_product='+id_product+'&quantity='+t_quantity+'&token='+wishlist.static_token+'&id_product_attribute='+id_product_attribute+'&id_wishlist='+id_wishlist,success:function(data)
{if(action=='add')
{if(prestashop.customer.is_logged==true){wishlistProductsIdsAdd(id_product);wishlistRefreshStatus();$('body').pkPopup({state:"success",text:wishlist.added_to_wishlist_msg});var old_count=parseInt($(".wishlist_count").html())+1;$(".wishlist_count").html(old_count);$("#login_wish").find(".alert_note").slideDown().delay('1300').slideUp();if(id=='ws_wishlist_block_list'){}}
else
{$('body').pkPopup({state:"success",text:loggin_required});}}
if(action=='delete'){wishlistProductsIdsRemove(id_product);wishlistRefreshStatus();var old_count=parseInt($(".wishlist_count").html())-1;$(".wishlist_count").html(old_count);}
if($('#'+id).length!=0)
{$('#'+id).slideUp('normal');document.getElementById(id).innerHTML=data;$('#'+id).slideDown('normal');}
$("#wishlist_button_block > a").addClass('icon_checked').find('svg').removeClass('in_progress');}});}
function WishlistChangeDefault(id,id_wishlist)
{$.ajax({type:'GET',url:prestashop.urls.base_url+'modules/pk_wishlist/cart.php?rand='+new Date().getTime(),headers:{"cache-control":"no-cache"},async:true,data:'id_wishlist='+id_wishlist+'&token='+wishlist.static_token,cache:false,success:function(data)
{$('#'+id).slideUp('normal');document.getElementById(id).innerHTML=data;$('#'+id).slideDown('normal');}});}
function WishlistBuyProduct(token,id_product,id_product_attribute,id_quantity,button,ajax)
{if(ajax)
ajaxCart.add(id_product,id_product_attribute,false,button,1,[token,id_quantity]);else
{$('#'+id_quantity).val(0);WishlistAddProductCart(token,id_product,id_product_attribute,id_quantity)
document.forms['addtocart'+'_'+id_product+'_'+id_product_attribute].method='POST';document.forms['addtocart'+'_'+id_product+'_'+id_product_attribute].action=baseUri+'?controller=cart';document.forms['addtocart'+'_'+id_product+'_'+id_product_attribute].elements['token'].value=wishlist.static_token;document.forms['addtocart'+'_'+id_product+'_'+id_product_attribute].submit();}
return(true);}
function WishlistAddProductCart(token,id_product,id_product_attribute,id_quantity)
{if($('#'+id_quantity).val()<=0)
return(false);$.ajax({type:'GET',url:prestashop.urls.base_url+'modules/pk_wishlist/buywishlistproduct.php?rand='+new Date().getTime(),headers:{"cache-control":"no-cache"},data:'token='+token+'&static_token='+wishlist.static_token+'&id_product='+id_product+'&id_product_attribute='+id_product_attribute,async:true,cache:false,success:function(data)
{if(data)
{$('body').pkPopup({state:"success",text:data});}
else
$('#'+id_quantity).val($('#'+id_quantity).val()-1);}});return(true);}
function WishlistManage(id,id_wishlist)
{$.ajax({type:'GET',async:true,url:wishlist.advansedwishlist_controller_url+'?rand='+new Date().getTime(),headers:{"cache-control":"no-cache"},data:'id_wishlist='+id_wishlist+'&refresh='+false,cache:false,success:function(data)
{$('#'+id).hide();document.getElementById(id).innerHTML=data;$('#'+id).fadeIn('slow');$('.wishlist_change_button').each(function(index){$(this).change(function(){wishlistProductChange($('option:selected',this).attr('data-id-product'),$('option:selected',this).attr('data-id-product-attribute'),$('option:selected',this).attr('data-id-old-wishlist'),$('option:selected',this).attr('data-id-new-wishlist'));});});}});}
function WishlistProductManage(id,action,id_wishlist,id_product,id_product_attribute,quantity,priority)
{$.ajax({type:'GET',async:true,url:wishlist.advansedwishlist_controller_url+'?rand='+new Date().getTime(),headers:{"cache-control":"no-cache"},data:'action='+action+'&id_wishlist='+id_wishlist+'&id_product='+id_product+'&id_product_attribute='+id_product_attribute+'&quantity='+quantity+'&priority='+priority+'&refresh='+true,cache:false,success:function(data)
{if(action=='delete'){$('#wlp_'+id_product+'_'+id_product_attribute).fadeOut('fast');var wishlist_count=$(".wishlist_count").html();wishlist_count=wishlist_count-1;$(".wishlist_count").html(wishlist_count);}else if(action=='update'){$('#wlp_'+id_product+'_'+id_product_attribute).fadeOut('fast');$('#wlp_'+id_product+'_'+id_product_attribute).fadeIn('fast');}
nb_products=0;$("[id^='quantity']").each(function(index,element){nb_products+=parseInt(element.value);});$("#wishlist_"+id_wishlist).children('td').eq(1).html(nb_products);}});}
function WishlistDelete(id,id_wishlist,msg)
{var res=confirm(msg);if(res==false)
return(false);if(typeof wishlist.mywishlist_url=='undefined')
return(false);$.ajax({type:'GET',async:true,dataType:"json",url:wishlist.mywishlist_url,headers:{"cache-control":"no-cache"},cache:false,data:{rand:new Date().getTime(),deleted:1,myajax:1,id_wishlist:id_wishlist,action:'deletelist'},success:function(data)
{var mywishlist_siblings_count=$('#'+id).siblings().length;$('#'+id).fadeOut('slow').remove();$("#block-order-detail").html('');if(mywishlist_siblings_count==0)
$("#block-history").remove();if(data.id_default)
{var td_default=$("#wishlist_"+data.id_default+" > .wishlist_default");$("#wishlist_"+data.id_default+" > .wishlist_default > a").remove();td_default.append('<p class="is_wish_list_default"><i class="icon icon-check-square"></i></p>');}}});}
function WishlistDefault(id,id_wishlist)
{if(typeof wishlist.mywishlist_url=='undefined')
return(false);$.ajax({type:'GET',async:true,url:wishlist.mywishlist_url,headers:{"cache-control":"no-cache"},cache:false,data:{rand:new Date().getTime(),'default':1,id_wishlist:id_wishlist,myajax:1,action:'setdefault'},success:function(data)
{var old_default_id=$(".is_wish_list_default").parents("tr").attr("id");var td_check=$(".is_wish_list_default").parent();$(".is_wish_list_default").remove();td_check.append('<a href="#" onclick="javascript:event.preventDefault();(WishlistDefault(\''+old_default_id+'\', \''+old_default_id.replace("wishlist_","")+'\'));"><svg class="svgic"><use xlink:href="#si-done"></use></a>');var td_default=$("#"+id+" > .default-section");$("#"+id+" > .default-section > a").remove();td_default.append('<span class="db is_wish_list_default"><svg class="svgic"><use xlink:href="#si-done"></use></span>');}});}
function WishlistVisibility(bought_class,id_button)
{if($('#hide'+id_button).is(':hidden'))
{$('.'+bought_class).slideDown('fast');$('#show'+id_button).hide();$('#hide'+id_button).css('display','block');}
else
{$('.'+bought_class).slideUp('fast');$('#hide'+id_button).hide();$('#show'+id_button).css('display','block');}}
function WishlistSend(id,id_wishlist,id_email)
{$.post(prestashop.urls.base_url+'modules/pk_wishlist/sendwishlist.php',{token:wishlist.static_token,id_wishlist:id_wishlist,email1:$('#'+id_email+'1').val(),email2:$('#'+id_email+'2').val(),email3:$('#'+id_email+'3').val(),email4:$('#'+id_email+'4').val(),email5:$('#'+id_email+'5').val(),email6:$('#'+id_email+'6').val(),email7:$('#'+id_email+'7').val(),email8:$('#'+id_email+'8').val(),email9:$('#'+id_email+'9').val(),email10:$('#'+id_email+'10').val()},function(data)
{if(data)
{if(!!$.prototype.fancybox)
$.fancybox.open([{type:'inline',autoScale:true,minHeight:30,content:'<p class="fancybox-error">'+data+'</p>'}],{padding:0});else
alert(data);}
else
WishlistVisibility(id,'hideSendWishlist');});}
function wishlistProductsIdsAdd(id)
{if($.inArray(parseInt(id),wishlistProductsIds)==-1)
wishlistProductsIds.push(parseInt(id))}
function wishlistProductsIdsRemove(id)
{wishlistProductsIds.splice($.inArray(parseInt(id),wishlistProductsIds),1)}
function wishlistRefreshStatus()
{$('.addToWishlist').each(function(){if($.inArray(parseInt($(this).prop('rel')),wishlistProductsIds)!=-1)
$(this).addClass('checked');else
$(this).removeClass('checked');});}
function wishlistProductChange(id_product,id_product_attribute,id_old_wishlist,id_new_wishlist)
{if(typeof wishlist.mywishlist_url=='undefined')
return(false);var quantity=$('#quantity_'+id_product+'_'+id_product_attribute).val();$.ajax({type:'GET',url:wishlist.mywishlist_url,headers:{"cache-control":"no-cache"},async:true,cache:false,dataType:"json",data:{id_product:id_product,id_product_attribute:id_product_attribute,quantity:quantity,priority:$('#priority_'+id_product+'_'+id_product_attribute).val(),id_old_wishlist:id_old_wishlist,id_new_wishlist:id_new_wishlist,myajax:1,action:'productchangewishlist'},success:function(data)
{if(data.success==true){$('#wlp_'+id_product+'_'+id_product_attribute).fadeOut('slow');$('#wishlist_'+id_old_wishlist+' td:nth-child(2)').text($('#wishlist_'+id_old_wishlist+' td:nth-child(2)').text()-quantity);$('#wishlist_'+id_new_wishlist+' td:nth-child(2)').text(+$('#wishlist_'+id_new_wishlist+' td:nth-child(2)').text()+ +quantity);}
else
{$('body').pkPopup({state:"success",text:data.error});}}});}
var ajaxWhish={count_wish:function(){var wishlist_count=$(".wishlist_count").html();wishlist_count=parseInt(wishlist_count)+1;$(".wishlist_count").html(wishlist_count);$("#login_wish").find(".alert_note").slideDown().delay('1300').slideUp();}}