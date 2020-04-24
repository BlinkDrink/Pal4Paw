$(document).ready(function () {
    $(".scrollspy").scrollSpy({
        scrollOffset: 64,
        throttle: 10
    });
    $("#container-div").removeClass("container");
    $('.parallax').parallax();

    // Window parameters
    var $window = $(window);
    var window_height = $window.height();

    // Nav height
    var nav_height = $(".nav-wrapper").height();

    // Set the logo page to fit the window screen
    $("#logo-page").css("height", window_height - nav_height);

    // The heading text of the logo page
    var page_logo_heading_height = $('#page-logo-heading').height();
    var page_logo_top = window_height - page_logo_heading_height;
    $('#page-logo-heading').css('height', window_height - nav_height)

    var difference_top_bot = page_logo_top;

    $('#arrow-button').css('top', window_height);
    $("#downwards-arrow").on('click', function (event) {
        if (this.hash !== "") {
            event.preventDefault();
            var hash = this.hash;
            $('html, body').animate({
                scrollTop: $(hash).offset().top
            },
                1500, //time in miliseconds to scroll
                function () {
                    window.location.hash = hash;
                });
        }
    });
});