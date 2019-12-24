function scrollMouse(classFirst, nameClassAdd) {
    $(classFirst).addClass('hidden');
    $(classFirst).each(function () {
        var spaceTop = $(this).offset().top;
        var chieucao = $(this).height(); 
        var spaceBottom = $(document).height() - $(this).offset().top - chieucao;
        var scrollBottom = $(document).height() + $(window).height() - $(window).scrollTop();
        var scrollSpaceTop = $(window).scrollTop();
        if ((spaceBottom < scrollBottom - $(window).height()) && (spaceTop < scrollSpaceTop + $(window).height()))
        {
            $(this).addClass(nameClassAdd);
        }
    });
}
var width = $(window).width();
if (width > 767) {
    $(window).scroll(function () {
        scrollMouse('#menu-mainkey ul li:nth-child(1)', 'zoomIn1');
        scrollMouse('#menu-mainkey ul li:nth-child(2)', 'zoomIn2');
        scrollMouse('#menu-mainkey ul li:nth-child(3)', 'zoomIn3');
        scrollMouse('#menu-mainkey ul li:nth-child(4)', 'zoomIn4');
        scrollMouse('#menu-mainkey ul li:nth-child(5)', 'zoomIn5');
        scrollMouse('#menu-mainkey ul li:nth-child(6)', 'zoomIn6');
        scrollMouse('#menu-mainkey ul li:nth-child(7)', 'zoomIn7');
        scrollMouse('#menu-mainkey ul li:nth-child(8)', 'zoomIn8');
        scrollMouse('#menu-mainkey ul li:nth-child(9)', 'zoomIn9');
        scrollMouse('#menu-mainkey ul li:nth-child(10)', 'zoomIn10');
        scrollMouse('#menu-mainkey ul li:nth-child(11)', 'zoomIn11');
        scrollMouse('#menu-mainkey ul li:nth-child(12)', 'zoomIn12');
        scrollMouse('#desc .pic', 'zoomIn5'); 
        scrollMouse('#nguyennhan .cols img', 'zoomInRight2');
        scrollMouse('.layodd img', 'hieuung2');
        scrollMouse('.layeven img', 'run-right2');
        scrollMouse('.cont1 img', 'zoomIn2');
        scrollMouse('.truonghop .col:nth-child(1)', 'run-right1');
        scrollMouse('.truonghop .col:nth-child(2)', 'run-right2');
        scrollMouse('.truonghop .col:nth-child(3)', 'run-right3');
        scrollMouse('.truonghop .col:nth-child(4)', 'run-right4');
        scrollMouse('.row-odd .bacsi', 'run-right1');
        scrollMouse('.row-odd .desc', 'run-right2');
        scrollMouse('.row-even .bacsi', 'hieuung1');
        scrollMouse('.row-even .desc', 'hieuung2');
        scrollMouse('footer', 'display-img');
    });
}