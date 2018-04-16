//公共的js
$(function () {
    //按钮悬浮变色
    $(".btn").mouseover(function () {
        $(this).css("border","2px solid red");
        $(this).css("color", "red");
    }).mouseout(function () {
        $(this).css("border", "2px solid gray");
        $(this).css("color", "black");
    });
    //自动补全搜索词
    function AutoCompleteKeywords(name) {
        $(name).autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/Home/AutoCompleteKeywords",
                    data: "keyword=" + $(name).val(),
                    type: "post",
                    success: function (result) {
                        return response(result);
                    }
                });
            },
            minLength: 1
        });
    }
    $("#keyword").autocomplete(AutoCompleteKeywords("#keyword"));
});