/**
 * Sự kiện hiện/ẩn dropdown
 * Dvanh 16/7/2021
 */
$(".select-arrow").click(function() {
    let me = $(this),
        dropdown = me.parent().parent().find(".dropdown");
    if ($(".select-arrow.rot-180").not(me).hasClass("rot-180")) {
        $(".select-arrow.rot-180").addClass("rot-0");
        $(".select-arrow.rot-180").removeClass("rot-180");
    }
    $(".dropdown").slideUp("slow");
    if (me.hasClass("rot-180")) {
        me.addClass("rot-0");
        me.removeClass("rot-180");
        dropdown.slideUp("slow");
    } else {
        me.removeClass("rot-0");
        me.addClass("rot-180");
        dropdown.slideDown("slow");
    }
})

$(".inp").click(function() {
    let me = $(this),
        arrow = me.parent().find(".select-arrow"),
        dropdown = me.parent().parent().find(".dropdown");
    if ($(".select-arrow.rot-180").not(arrow).hasClass("rot-180")) {
        $(".select-arrow.rot-180").addClass("rot-0");
        $(".select-arrow.rot-180").removeClass("rot-180");
    }
    $(".dropdown").slideUp("slow");
    console.log("arrow")
    if (arrow.hasClass("rot-180")) {
        arrow.addClass("rot-0");
        arrow.removeClass("rot-180");
        dropdown.slideUp("slow");
    } else {
        arrow.removeClass("rot-0");
        arrow.addClass("rot-180");
        dropdown.slideDown("slow");
    }
})

/**
 * Bấm chọn các nội dung khác trong dropdown
 * Dvanh 16/7/2021
 */
$(".dropdown-item").click(function() {
    let me = $(this),
        val = me.find(".dropdown-text").text(),
        text = me.parent().parent().find(".inp").text();

    me.parents('.dropdown').find('.dropdown-item').removeClass("bg-select");
    me.addClass("bg-select");
    me.parent().parent().find(".inp").text(val);
    me.parent().parent().find(".X").attr("style", "visibility: visible;");
    //bấm X thì các .dropdown-item được bỏ chọn và khôi phục text như lúc đầu
    $(".X").click(function() {
        $(this).attr("style", "visibility: hidden;")
        $(this).parent().find(".inp").text(text);
        $(this).parent().find(me).removeClass("bg-select");
    })
})


/**
 * Sự kiện bấm bất kì thứ gì ngoài cái mũi tên thì dropdown ẩn đi 
 * Dvanh 16/7/2021
 */
$(document).on("click", function(e) {
    if (!$(".select-arrow").is(e.target) && !$(".inp").is(e.target)) {
        $(".dropdown").slideUp("slow");
        $(".select-arrow").addClass("rot-0");
        $(".select-arrow").removeClass("rot-180");
    }
})