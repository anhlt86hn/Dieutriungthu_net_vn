$(document).ready(function() {
    
    
    $va= window.location.href;


$('.main-menu > li > a').each(function(){
    $this = $(this);
    if($this.attr('href')==$va){
        $this.parent().addClass('contact');
    }
});
    
 // Megamenu
 $('.main-menu > li').on('mouseover', function(){
  var wW = $(window).width();
  if( wW >= 992 ){ 
    var $this = $(this);
    var wH = $(window).height();
    var offset = Number($(this).offset().top) - Number($(window).scrollTop());
    var H = $('.main-menu').height();
    var equal = Number(wH) - (Number(offset) + Number(H));
    if( $(this).find('.mega-menu').height() >= equal ){ 
      $(this).find('.mega-menu').css('height', equal);
    }
    return;
  }
  return;
 });
 
 
 //(window.location.href );

 $('.main-menu > li').on('mouseleave', function(){
  $('.mega-menu').css('height', 'auto');
 });

     // Owl carousel
  var owl = $("#exclusive-services");
 
  owl.owlCarousel({
      items : 5, 
      itemsDesktop : [1000,5], 
      itemsDesktopSmall : [900,4], 
      itemsTablet: [600,2], 
      itemsMobile : false,
      pagination: false,
      autoPlay: true
  });
 
  // Custom Navigation Events
  $(".exclusive-services .next").click(function(){
    $(this).parents('.module').find(owl).trigger('owl.next');
  })
  $(".exclusive-services .prev").click(function(){
    $(this).parents('.module').find(owl).trigger('owl.prev');
  })
  
  
  // Owl carousel
  var owlz = $("#owl-featured-services");
 
  owlz.owlCarousel({
      items : 4, 
      itemsDesktop : [1000,4], 
      itemsDesktopSmall : [900,3], 
      itemsTablet: [600,2], 
      autoplay:true,
  });
  $("#owl-featured-services").trigger('owl.play',6000);
  
 
  // Custom Navigation Events
  $("#owl-featured-services .next").click(function(){
    $(this).parents('.module').find(owlz).trigger('owl.next');
  })
  $("#owl-featured-services .prev").click(function(){
    $(this).parents('.module').find(owlz).trigger('owl.prev');
  })
  
  $("#banner").owlCarousel({
      autoplay:true,
      singleItem:true
  });
  $("#banner").trigger('owl.play',6000);
  
  // Custom Navigation Events
  $(".banner .next").click(function(){
    $(this).parents('.banner').find("#banner").trigger('owl.next');
  })
  $(".banner .prev").click(function(){
    $(this).parents('.banner').find("#banner").trigger('owl.prev');
  })

  // Bx slider
  $('.bxslider').bxSlider({
    mode: 'fade', auto: true, autoStart: true,
    captions: true
  });

  // Map
  if( $('#map').length){
    map = new GMaps({
      div: '#map',
      lat: -12.043333,
      lng: -77.028333
    });
  }

  if( $('#map-1').length){
    map = new GMaps({
      div: '#map-1',
      lat: -12.043333,
      lng: -77.028333
    });
  }

  if( $('#map-2').length){
    map = new GMaps({
      div: '#map',
      lat: -12.043333,
      lng: -77.028333
    });
  }
  
  // Accordion list megamenu
  // var accordion = $('.list-box > li');
  // var ul = accordion.find('ul');
  // accordion.on('click', function(){
  //   var $this = $(this);
  //   var check = $(this).find('>ul');
  //   if( $(check).is(":visible") ){
  //      ul.slideUp();
  //      accordion.removeClass('accordion-active');
  //   }else{
  //     ul.slideUp();
  //     accordion.removeClass('accordion-active');
  //     check.slideDown();
  //     $this.addClass('accordion-active');
  //   }
  // });

  // Fit video
  $('.container').each(function(){
      $(this).fitVids();
  });

  // Slider pro
  if($( '#hot-news' ).length ){
    $( '#hot-news' ).sliderPro({
      width: 790,
      height: 350,
      orientation: 'vertical',
      loop: true,
      arrows: false,
      buttons: false,
      thumbnailsPosition: 'right',
      thumbnailPointer: true,
      thumbnailWidth: 180,
      thumbnailHeight: 120,
      breakpoints: {
        1260: {
          thumbnailWidth: 170,
          thumbnailHeight: 92,
        },
        1010: {
          orientation: 'horizontal',
          thumbnailsPosition: 'bottom',
          thumbnailWidth: 160,
          thumbnailHeight: 100
        },
        992: {
          orientation: 'horizontal',
          thumbnailsPosition: 'bottom',
          thumbnailWidth: 138,
          thumbnailHeight: 100
        }
        // 768: {
        //   orientation: 'horizontal',
        //   thumbnailsPosition: 'bottom',
        //   thumbnailWidth: 142,
        //   thumbnailHeight: 120,
        // },
        // 500: {
        //   orientation: 'horizontal',
        //   thumbnailsPosition: 'bottom',
        //   thumbnailWidth: 100,
        //   thumbnailHeight: 100
        // }
      }
    });
  }
    
    

  if( $(".fancybox-1").length ){
    $(".fancybox-1").fancybox({
      maxWidth  : 800,
      maxHeight : 600,
      fitToView : false,
      width   : '90%',
      height    : '90%',
      autoSize  : false,
      closeClick  : false,
      openEffect  : 'none',
      closeEffect : 'none'
    });
  }
  if( $(".fancybox-2").length ){
    $(".fancybox-2").fancybox({
      openEffect  : 'none',
      closeEffect : 'none'
    });
  }

});


$(window).load(function() {
  $('.bottom-header').sticky({ topSpacing: 0 });     

  // Masonry
  setTimeout(function(){
    $('.masonry-layout').each(function(){
      var $this = $(this);
      $this.masonry({
        columnWidth: '.masonry-item',
        itemSelector: '.masonry-item'
      });
    });
  },5000);

  $(window).scroll(function(){
    var check = $('.sticky-wrapper').hasClass('is-sticky');
    var header = $('#main-header');
    if( check == true ){
      header.addClass('change');
    }else{
      header.removeClass('change');
    }
  });  
});

