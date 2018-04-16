$(function () {
    //点击全选框
    $("#all").click(function () {
        //复选框全选的值 = boolean
        var chAll = document.getElementById("all").checked;
        //取出数组
        var ch = document.getElementsByName("status");
        for (var i = 0; i < ch.length; i++) {
            ch[i].checked = chAll;
        }
    });
    //如果所有的多选框都勾选了，则全选框选中，反之，不选中
    $(".status").click(function () {
        var count = 0;//选中的数量
        var sum = 0;//总数量
        //全选框
        var chAll = document.getElementById("all");
        //多选框数组
        var ch = document.getElementsByName("status");
        for (var i = 0; i < ch.length; i++) {
            sum++;
            if (ch[i].checked == true) {
                count++;
            }
        }
        //如果总数量与选中的数量一致且sum不等于0，则全选框选中
        if ((count == sum) && sum != 0) {
            chAll.checked = true;
        } else {
            chAll.checked = false;
        }
    });
    $(".btn").click(function () {
        var checkBoxs = $(".status:checked");
        if (checkBoxs.length == 0) {
            $(".message").html("请选择用户");
            return false;
        }
        return true;
    });
});