function change() {
    if ($("#searchType").val() == "title" || $("#searchType").val() == "ContentDescription" || $("#searchType").val() == "author") {
        $("#keyText").show();
        $("#keyCategory").hide();
        $("#keyPublisher").hide();
    } else if ($("#searchType").val() == "CategoryId") {
        $("#keyText").hide();
        $("#keyCategory").show();
        $("#keyPublisher").hide();
    } else if ($("#searchType").val() == "PublisherId") {
        $("#keyText").hide();
        $("#keyCategory").hide();
        $("#keyPublisher").show();
    }
}
$(function () {
    change();
    $("#searchType").change(function () {
        change();
    });
});