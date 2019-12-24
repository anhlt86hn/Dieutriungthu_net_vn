$(document).ready(function () {
    function scrollMouse(classFirst, nameClassAdd) {
        $(classFirst).addClass('hidden');
        $(classFirst).each(function () {
            var spaceTop = $(this).offset().top;
            var chieucao = $(this).height();
            var spaceBottom = $(document).height() - $(this).offset().top - chieucao;
            var scrollBottom = $(document).height() + $(window).height() - $(window).scrollTop();
            var scrollSpaceTop = $(window).scrollTop();
            if ((spaceBottom < scrollBottom - $(window).height()) && (spaceTop < scrollSpaceTop + $(window).height())) {
                $(this).addClass(nameClassAdd);
            }
        });
    }
    var width = $(window).width();
    if (width > 767) {
        $(window).scroll(function () {
            scrollMouse('.wrap h2', 'slideLeft');

            scrollMouse('.khacbiet img', 'slideRight');

            scrollMouse('.khactrong p:nth-child(1)', 'slideLeft');
            scrollMouse('.khactrong p:nth-child(2)', 'slideLeft2');

            scrollMouse('.bnrdung img:nth-child(1)', 'slideLeft');
            scrollMouse('.bnrdung img:nth-child(2)', 'slideLeft2');
            scrollMouse('.bnrdung img:nth-child(3)', 'slideLeft3');
            scrollMouse('.bnrdung img:nth-child(4)', 'slideRight4');

            scrollMouse('.phuongcham p', 'slideRight');
            scrollMouse('.phuongcham img', 'slideLeft');

            scrollMouse('.quanlyphai > p', 'slideRight4');

            scrollMouse('.xaydung .main > img', 'slideRight');
            scrollMouse('.xayphai p:nth-child(1)', 'slideLeft');

            scrollMouse('.sukhacbiet .main > div', 'fadeIn');

            scrollMouse('.uutienphai li:nth-child(1)', 'fadeIn');
            scrollMouse('.uutienphai li:nth-child(2)', 'fadeIn2');
            scrollMouse('.uutienphai li:nth-child(3)', 'fadeIn3');
            scrollMouse('.uutienphai li:nth-child(4)', 'fadeIn4');

            scrollMouse('.bacsigioi li:nth-child(1)', 'slideExpandUp2');
            scrollMouse('.bacsigioi li:nth-child(2)', 'slideExpandUp3');
            scrollMouse('.bacsigioi li:nth-child(3)', 'slideExpandUp4');
            scrollMouse('.bacsigioi li:nth-child(4)', 'slideExpandUp5');
            scrollMouse('.bacsigioi li:nth-child(5)', 'slideExpandUp6');

            scrollMouse('.quanlytrai p:nth-child(1)', 'slideExpandUp2');
            scrollMouse('.quanlytrai p:nth-child(2)', 'slideExpandUp3');
            scrollMouse('.quanlytrai p:nth-child(3)', 'slideExpandUp4');
            scrollMouse('.quanlytrai p:nth-child(4)', 'slideExpandUp5');
            scrollMouse('.quanlytrai p:nth-child(5)', 'slideExpandUp6');
            scrollMouse('.quanlytrai p:nth-child(6)', 'slideExpandUp7');
            scrollMouse('.quanlytrai p:nth-child(7)', 'slideExpandUp8');
            scrollMouse('.quanlytrai p:nth-child(8)', 'slideExpandUp9');

            scrollMouse('.cuxu li:nth-child(1)', 'expandUp');
            scrollMouse('.cuxu li:nth-child(2)', 'expandUp2');
            scrollMouse('.cuxu li:nth-child(3)', 'expandUp3');
            scrollMouse('.cuxu li:nth-child(4)', 'expandUp4');
            scrollMouse('.cuxu li:nth-child(5)', 'expandUp5');
            scrollMouse('.cuxu li:nth-child(6)', 'expandUp6');
            scrollMouse('.cuxu li:nth-child(7)', 'expandUp7');
            scrollMouse('.cuxu li:nth-child(8)', 'expandUp8');

            scrollMouse('.bacsigioi li:nth-child(6)', 'expandOpen');
            scrollMouse('.bacsigioi li:nth-child(7)', 'expandOpen');
        });
    }
});