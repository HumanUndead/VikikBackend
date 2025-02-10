$(document).ready(function(){

  var $main = '#pksizeguide',
      $overlay = '#pksizeguide-overlay',
      $parent = '.hookDisplayGuide';

  $('body').on('click', '#pksizeguide .cross, #pksizeguide-overlay', function(e){
    e.preventDefault();
    $(this).closest($parent).find($overlay).fadeOut('fast');
    $(this).closest($parent).find($main).fadeOut('fast');
  });

  $('body').on('click', '#pksizeguide-show', function(e){
    e.preventDefault();
    $(this).closest($parent).find($overlay).fadeIn('fast');
    $(this).closest($parent).find($main).fadeIn('fast');
  });

});
