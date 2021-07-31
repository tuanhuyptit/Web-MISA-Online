// $(".toggle").click(function() {
//     $(".menu-item .menu-item-text").hide();
//     // $(".menu").addClass("narrow");
//     // $(".content").addClass("expand");
//     $(".toggle-menu").addClass("rot-180");
//     $(".menu").attr("style", "width: 52px; transition: width 1s;");
//     $(".content").attr("style", "width: calc(100% - 52px); left: 52px; transition: left 1s;");
// });

$(".toggle-menu").click(function() {
    if ($(this).hasClass("rot-180")) {
        $(".menu-item .menu-item-text").show();
        // $(".menu").removeClass("narrow");
        // $(".content").removeClass("expand");
        $(".toggle-menu").removeClass("rot-180");
        $(".menu").attr("style", "width: 225px; transition: width 1s;");
        $(".content").attr("style", "width: calc(100% - 225px); left: 224px; transition: left 1s;");
    } else {
        $(".menu-item .menu-item-text").hide();
        // $(".menu").addClass("narrow");
        // $(".content").addClass("expand");
        $(".toggle-menu").addClass("rot-180");
        $(".menu").attr("style", "width: 52px; transition: width 1s;");
        $(".content").attr("style", "width: calc(100% - 52px); left: 52px; transition: left 1s;");
    }

});