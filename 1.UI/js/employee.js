/**
 * Event hiện pop up 
 * Dvanh 16/7/2021
 */
$(".content-header .button").click(function() {
    $(".popup").css('visibility', 'visible');
    $("#focus").focus();
    $(this).attr("disable", 'true');
});

/**
 * Event ẩn pop up 
 * Dvanh 16/7/2021
 */
$(".head-close, .button.cancel").click(function() {
    $(".popup").css('visibility', 'hidden');
    $(".X").attr("style", "visibility: hidden;")
})

/**
 * reload lại trang
 * Dvanh 16/7/2021
 */
$(".refresh").click(function() {
    location.reload();
})

/**
 * Load ảnh từ máy lên form thêm mới
 * Dvanh 16/7/2021
 */
$('.image').click(function() {
    $('#myFile').trigger('click');
})

$('#myFile').click(function(e) {
    $('#myFile').change(function(e) {
        var img = URL.createObjectURL(e.target.files[0]);
        $('.image').css("background-image", `url(${img})`);
    })
})