$('.category-list .list').find('.active').parent().parent().parent().css('display', 'block'); $('.category-list .list').find('.active').parent().parent().parent().prev().addClass('active'); $(document).ready(function () {
    function delayHoverMenuLeft(element) { timer = setTimeout(function () { if ($(element).parent().next().is(':hidden') == true) { $(element).parent().next().stop().slideDown(300); $(element).parent().addClass('active'); } else { $(element).parent().next().stop().slideUp(300); $(element).parent().removeClass('active'); } }, 200); }; var width = $(window).width(); if (width > 1024) { $('.category-list .category-title > h3').hover(function () { delayHoverMenuLeft(this); }, function () { clearTimeout(timer); }); } else { $('.category-list .category-title > h3').click(function () { delayHoverMenuLeft(this); }); } if (width > 1024) {

        $banner = $('.banner_left');
        $window = $(window);
        $topDefault = parseFloat($banner.css('top'), '100px');
        $window.scroll(function () {
            $top = $(this).scrollTop();
            $banner.stop().animate({
                top: $top + $topDefault
            }, 800, 'easeOutCirc');
        });
        $window.scroll(function () { $top = $(this).scrollTop(); $banner.stop().animate({ top: $top + $topDefault }, 800, 'easeOutCirc'); }); cod = false; $('#main-menu > ul > li').hover(function () { if (cod == false) { $('#main-menu > ul > li div').css('display', 'none').css('margin-top', '0'); cod = true; } $(this).find('div').stop().slideDown(200).css('margin-top', '0'); }, function () { $(this).find('div').stop().slideUp(200).css('margin-top', '0'); });
    } else { co = false; $('#main-menu > ul > li').click(function () { if (co == false) { $('#main-menu > ul > li div').css('display', 'none').css('margin-top', '0'); co = true; } $('#main-menu > ul > li div').slideUp(500); if ($(this).find('div').is(':hidden')) { $(this).find('div').stop().slideDown(500).css('margin-top', '0'); } else { $(this).find('div').stop().slideUp(500).css('margin-top', '0'); } }); } cod2 = false; $('#menu-top .main > ul > li').hover(function () { if (cod2 == false) { $('#menu-top .main > ul > li > ul').css('display', 'none').css('margin-left', '0'); cod2 = true; } $(this).find('ul').stop().show(500).css('margin-left', '0'); }, function () { $(this).find('ul').css('display', 'none'); }); is_plag = false; $('.btn_menu').click(function () { if (is_plag == false) { $('.mobile-menu').animate({ 'left': '0' }, 200); $('body,html').animate({ scrollTop: 0 }, 0); $('.lienhe2').animate({ 'left': '220px' }, 200); $(this).parent().animate({ 'left': '220px' }, 200); is_plag = true; } else { $('.lienhe2').animate({ 'left': '0' }, 200); $(this).parent().animate({ 'left': '0' }, 200); $('.mobile-menu').animate({ 'left': '-220px' }, 200); is_plag = false; } }); $('header .search').addClass('nodisplay'); $('.search-icond').addClass('nodisplay').fadeIn(1000).removeClass('nodisplay'); $('.search-icond').click(function () { if ($('header .search').hasClass('nodisplay') == true) { $('header .search').fadeIn(400).removeClass('nodisplay'); $(this).css('display', 'none'); } else { $('header .search').fadeOut(400).addClass('nodisplay'); $('#banner').css('margin-top', '0'); } }); var point = $('#commentForm .point').val(); for (var i = 1; i <= point; ++i) { $('.comment .ratings li:nth-child(' + i + ')').addClass('vote-active'); } $('.comment .ratings li').click(function () { var hovervalue = $(this).attr('itemvalue'); var valuevote = new Array(); $('#commentForm .point').attr('value', hovervalue); $('.comment .ratings li').removeClass('vote-active'); for (var i = 1; i <= 5; ++i) { valuevote[i] = $('.comment .ratings li:nth-child(' + i + ')').attr('itemvalue'); if (valuevote[i] <= hovervalue) { $('.comment .ratings li:nth-child(' + i + ')').addClass('vote-active'); } } }); $(window).scroll(function () { $('header .search').each(function () { var abcd = $(this).offset().right; var aaaa = $(window).scrollTop(); if (aaaa > 10) { $('header .search').addClass('nodisplay'); $('.search-icond').css('display', 'block'); $(this).css('display', 'none'); } }); }); $(".comment").animate({ scrollTop: $(document).height() }, "slow"); $("#pagination-comment a, .button-vote a").click(function () {
        var url = $(this).attr("href"); $.ajax({ type: "POST", url: url, data: "ajax", async: true, beforeSend: function () { $("#box-comment").html(""); }, success: function (kq) { $("#box-comment").html(kq); } })
        return false;
    }); $(".ratingblock li a").click(function () {
        var url = $(this).attr("href"); $.ajax({ type: "POST", url: url, data: "ajax", async: true, beforeSend: function () { $(".unit-rating").html("<div class='loading'></div>"); }, success: function (kq) { $(".blog-cate-vote").html(kq); } })
        return false;
    }); $("a.refresh").click(function () { jQuery.ajax({ type: "POST", url: "/comment/captcha_refresh", success: function (res) { if (res) { jQuery("#wordcapcha").html(res); } } }); }); $(".datepicker").datepicker({ dateFormat: "dd/mm/yy" }); $(".dangkykham, .registration a").click(function () { $("#popupContact").css('margin-left', 0); }); $("#close").click(function () { $("#popupContact").css('margin-left', "-9999px"); }); $('#popup2').click(function () { var captcha2 = $("#captcha12").val(); var form = $("#contact-form"); form.validate(); if (!form.valid()) { return false; } else { $.ajax({ type: "POST", url: "/lien-he/verify.php", data: { captcha: $('#captcha12').val(), hoten: $('#hoten2').val(), mail: $('#mail2').val(), sdt: $('#sdt2').val(), benh: $('#benh2').val(), ngay: $('#ngay2').val() }, beforeSend: function () { $(".thongbao-lienhe").html("<span class='loading'></span>"); }, success: function (html) { $(".thongbao-lienhe").html(""); alert(html); } }); } }); $('.btn-register').click(function () { var form = $("#contact-form-footer"); form.validate(); if (!form.valid()) { return false; } else { $.ajax({ type: "POST", url: "/lien-he/send.php", data: { hoten: $('#hoten3').val(), sdt: $('#sdt3').val(), benh: $('#benh3').val(), date: $('#date').val(), month: $('#month').val(), year: $('#year').val() }, beforeSend: function () { $(".thongbao-lienhe").html("<span class='loading'></span>"); }, success: function (html) { $(".thongbao-lienhe").html(""); alert(html); } }); } }); $(".btn-subm2it").click(function () { var form = $("#commentForm"); form.validate(); if (!form.valid()) { return false; } else { $.ajax({ type: "POST", url: "/comment/add/", success: function (kq) { $("#box-comment").html(kq); } }); } return false; });
});
$(function () {
    $("#phone_input .close").click(function () {
        $("#phone_input").slideToggle(1000);
    });
    $('#phone_input .gui').click(function () {
        var sodienthoai = $('#form-phone .sodienthoai').val();
        if (sodienthoai == '') {
            alert("Vui lòng nhập số điện thoại!");
        } else {
            $.ajax({
                type: "POST",
                url: "/mail/sendPhone",
                data: {
                    sodienthoai: sodienthoai
                },
                beforeSend: function () {
                    $('#form-phone .sodienthoai').val('');
                    $('#preloader').fadeIn(200);
                },
                success: function (html) {
                    $('#preloader').fadeOut(200);
                    alert(html);
                }
            });
        }
    });
    $('#form_left_input .gui').click(function () {
        var sodienthoai = $('#form_left_input .sodienthoai').val();
        if (sodienthoai == '') {
            alert("Vui lòng nhập số điện thoại!");
        } else {
            $.ajax({
                type: "POST",
                url: "/mail/sendPhone",
                data: {
                    sodienthoai: sodienthoai
                },
                beforeSend: function () {
                    $('#form_left_input .sodienthoai').val('');
                    $('#preloader').fadeIn(200);
                },
                success: function (html) {
                    $('#preloader').fadeOut(200);
                    alert(html);
                }
            });
        }
    });
});