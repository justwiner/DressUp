function addLoadEvent(func) {
    "use strict";
    var oldonload = window.onload;
    if (typeof window.onload !== "function") {
        window.onload = func;
    } else {
        window.onload = function () {
            oldonload();
            func();
        };
    }
}
$(document).ready(
    function () {
        $(".mainMenu").hover(function () {
            $(this).children("ul").slideToggle(100);
        })
    }
)
$(document).ready(function () {
    var topMain = $(".head_Navigation_GoodsType").height() + $(".head_Login").height() + 70//是头部的高度加头部与nav导航之间的距离
    var nav = $(".head_Navigation_GoodsType");
    $(window).scroll(function () {
        if ($(window).scrollTop() > topMain) {//如果滚动条顶部的距离大于topMain则就nav导航就添加类.nav_scroll，否则就移除
            nav.addClass("nav_scroll");
        } else {
            nav.removeClass("nav_scroll");
        }
    });
})
function SearchGoods() {
    var keywords = document.getElementById("searchText").value
    $.ajax({
        url: "/ShowGoods/SetGoodsPageListbySearch",
        dataType: "json",
        type: "POST",
        data: { "keywords": keywords },
        success: function (data) {
            if (data) {
                window.location.href = "/ShowGoods/Category"
            }
        }
    })
}