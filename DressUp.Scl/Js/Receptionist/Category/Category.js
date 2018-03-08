var nowPage = 1
$(document).ready(function () {
    $(window).scroll(function () {
        var scrollTop = $(this).scrollTop();
        var scrollHeight = $(document).height();
        var windowHeight = $(this).height();
        if (scrollTop + windowHeight == scrollHeight) {
            nowPage = nowPage + 1;
            LoadMoreGoods();
        } 
    })
});
function LoadMoreGoods() {
    $.ajax({
        url: "/ShowGoods/LoadMore",
        dataType: "json",
        type: "GET",
        data: { "pageNum": nowPage },
        beforeSend: function () {
            document.getElementById("loadIcon").style.display = 'block'
            document.getElementById("allGoodsShow").style.display = 'none'
        },
        success: function (data) {
            if (data != false) {
                document.getElementById("goodsShowBox").innerHTML += data
            }
            else {
                document.getElementById("allGoodsShow").style.display = 'block'
            }
        },
        complete: function(){
            document.getElementById("loadIcon").style.display = 'none'
        }
    })
}