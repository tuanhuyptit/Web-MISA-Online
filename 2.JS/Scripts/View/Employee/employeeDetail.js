/**
 * Hàm format tiền lương được nhập
 * Dvanh 19/7/2021
 */
$("#txtSalary").on("input", function() {
    let me = $(this),
        val = me.val();
    val = val.replaceAll(".", "");
    me.val(CommonFn.formatMoney(val));
});


/**
 * Hàm validate email
 * Dvanh 22/7/2021
 */
function validateEmail() {
    let isValid = true;

    let value = $("#txtEmail").val(),
        at = value.indexOf("@"),
        dot = value.lastIndexOf("."),
        space = value.indexOf(" ");
    if ((at != -1) && //có ký tự @
        (at != 0) && //ký tự @ không nằm ở vị trí đầu
        (dot != -1) && //có ký tự .
        (dot > at + 1) && (dot < value.length - 1) //phải có ký tự nằm giữa @ và . cuối cùng
        &&
        (space == -1)) //không có khoẳng trắng 
    {
        isValid = true;
    } else {
        alert("Email không đúng định dạng");
        isValid = false;
    }

    return isValid;
}