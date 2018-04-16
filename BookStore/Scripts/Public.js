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
    //查看/隐藏 所有图书分类(前20个)
    $("#openAllCategoryTree").toggle(function () {
        $("#nav_m").find("li:gt(19)").show();
        $(this).text("收起>>");
    }, function () {
        $("#nav_m").find("li:gt(19)").hide();
        $(this).text("查看所有分类>>");
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
    $("#title").autocomplete(AutoCompleteKeywords("#title"));
    $("#searchTitle").autocomplete(AutoCompleteKeywords("#searchTitle"));
});