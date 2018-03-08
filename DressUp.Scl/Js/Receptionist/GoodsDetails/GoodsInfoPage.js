var end
$(function () {
    end = $("#endPoint").offset();
});
function AddShopCartsGoods() {
    GoodsFly()
    var goodsId = document.getElementById("goodsId").innerHTML
    var goodsNum = Number(document.getElementById("goodsNum").value)
    $.ajax({
        url: "/User/AddShopCartsGoods",
        dataType: "json",
        type: "post",
        data: { "GoodsId": goodsId, "goodsNum": goodsNum },
        success: function (data) {
            if (data == "null") {
                BuyerLoginBox()
                return false;
            }
            else if (data == false) {
                alert("(⊙o⊙)…添加不进去了")
            }
            else if (data == true) {
                document.getElementById("sidebar_shopCartsNum").innerHTML++
                document.getElementById("head_Login_button_myShopCar").innerHTML++
            }
        },
        beforeSend: function () {
            document.getElementById("addGoods").disabled = true
            document.getElementById("addGoods").innerHTML = "<i class=\"icon-spinner icon-spin\"></i>"
        },
        complete: function () {
            document.getElementById("addGoods").disabled = false
            document.getElementById("addGoods").innerHTML = "加入购物车"
        },
    });
}
function GoodsFly() {
    var img = $(".main_img").attr("src"); //获取当前点击图片链接
    var flyer = $('<img class="flyer-img flyer" src="' + img + '">'); //抛物体对象
    flyer.fly({
        start: {
            left: event.pageX - $(window).scrollLeft(),//抛物体起点横坐标   
            top: event.pageY - $(window).scrollTop()//抛物体起点纵坐标   
        },
        end: {
            left: end.left + 10,//抛物体终点横坐标   
            top: end.top + 10, //抛物体终点纵坐标
            width: 30, //结束时宽度 
            height: 30 //结束时高度 
        },
        onEnd: function () {
            flyer.remove(); //销毁抛物体
        }
    });
}
function addNum() {
    var nowNum = Number(document.getElementById("goodsNum").value);
    document.getElementById("goodsNum").value = (nowNum + 1);
}
function decreaseNum() {
    var nowNum = Number(document.getElementById("goodsNum").value);
    if (nowNum == 1) {
        document.getElementById("goodsNum").value = nowNum;
    }
    else {
        document.getElementById("goodsNum").value = (nowNum - 1);
    }
}
function GoodsDetailBuyerIfLogin(ifLogin) {
    if (document.getElementById("buyerRegister").style.display != "none") {
        BuyerLoginBox()
        return false;
    }
    else {
        return true;
    }
}
