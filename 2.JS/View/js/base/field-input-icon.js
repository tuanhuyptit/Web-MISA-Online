$(".field-input-icon").on("input", (function() {
    let me = $(this),
        X = me.parent().find(".input-clear-icon");

    X.attr("style", "visibility: visible;");
}))

$(".input-clear-icon").click(function() {
    let me = $(this),
        input = me.parent().find(".field-input-icon");

    me.attr("style", "visibility: hidden;");
    input.val("");
})