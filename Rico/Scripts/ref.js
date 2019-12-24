//Hide Top
window.addEventListener("load", function () {
    // Set a timeout...
    setTimeout(function () {
        // Hide the address bar!
        window.scrollTo(0, 1);
    }, 0);
});
// Side Bar
$(document).ready(function () {
    var is_expand = false;
    $('.wrapper').css('overflow-x', 'hidden');
    $('#sib_btn').click(function () {
        if (is_expand == false) {
            $('.left_menu').animate({ left: '0px' }, 100);
            $('.toptop').animate({ left: '240px' }, 100);
            $('body,html').animate({scrollTop:0},0);			
            $('.carea').animate({ left: $('.left_menu').width() + 'px' }, 100);
            $('.carea').css('overflow', 'hidden');
            is_expand = true;

            // Expand Header
            $('.header').animate({ left: $('.sidebar').width() + 'px' }, 100);
            $('.header').css('position', 'fixed');
            $('.header_menu').css('margin-top', $('.header').height() + 'px');
        }
        else {
            $('.left_menu').animate({ left: '-' + $('.left_menu').width() + 'px' }, 100);
            $('.carea').animate({ left: '0' }, 100);
            is_expand = false;
            $('body').height($('.carea').height() + 'px');
            $('body').css('position', 'relative');
            $('.carea').height('auto');
            $('.toptop').animate({ left: '0px' }, 100);
        }
        return false;
    });
	 

	 $('#popup').click(function() {
        var name = $("#hoten").val();
        var email = $("#mail").val();
        var sdt = $("#sdt").val();
        var captcha = $("#captcha1").val();
		if (name == '' || email == '' || captcha == '' || sdt == '')
        {
            alert("Fill All Fields");
        }

        else {
	//validating CAPTCHA with user input text
            var dataString = 'captcha=' + captcha;
            $.ajax({
                type: "POST",
                url: "verify.php",
                data: dataString,
                success: function(html) {
                    alert(html);
                }
            });
		}
    });

	$('#popup2').click(function() {
        var name2 = $("#hoten2").val();
        var email2 = $("#mail2").val();
        var sdt2 = $("#sdt2").val();
        var captcha2 = $("#captcha12").val();
		if (name2 == '' || email2 == '' || captcha2 == '' || sdt2 == '')
        {
            alert("Fill All Fields");
        }

        else {
	//validating CAPTCHA with user input text
            var dataString = 'captcha=' + captcha2;
            $.ajax({
                type: "POST",
                url: "verify.php",
                data: dataString,
                success: function(html) {
                    alert(html);
                }
            });
		}
    });

	$('.btn-register').click(function() {
        var name3 = $("#hoten3").val();
        var benh3 = $("#benh3").val();
        var sdt3 = $("#sdt3").val();
		if (name3 == '' || benh3 == '' || sdt3 == '')
        {
            alert("Fill All Fields");
        }

        else {
            $.ajax({
                type: "POST",
                url: "send.php",
                success: function(html) {
                    alert(html);
                }
            });
		}
    });
	
/* show hide menu mobile */
    $('ul.subcat').hide();

    $('li.showhi').click(function(event) {
        event.stopPropagation();
        $('> ul', this).toggle();

    });	

});


//function to display Popup
function div_show(){ 
document.getElementById('showhide').style.display = "block";
}

//function to hide Popup
function div_hide(){ 
document.getElementById('showhide').style.display = "none";
}