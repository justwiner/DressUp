function GoodsInfoShowOrNot() {
    if (document.getElementById("goodsInfo_promotionPrice_") == null)
        return;
    if (document.getElementById("goodsInfo_IfHot").innerHTML == "true") {
        document.getElementById("goodsInfo_promotionPrice_").style.display = 'block'
    }
    else {
        document.getElementById("goodsInfo_promotionPrice_").style.display = 'none'
    }
    if (document.getElementById("goodsInfo_ShelvesNum").innerHTML == "" || document.getElementById("goodsInfo_ShelvesNum").innerHTML == "0") {
        document.getElementById("goodsInfo_shelvesNum_").style.display = 'none'
    }
    else {
        document.getElementById("goodsInfo_shelvesNum_").style.display = 'block'
    }
}
function GoodsInfoChangeSelect(selectValue,id) {
    if(selectValue == "是")
        document.getElementById(id).style.display = 'block'
    else
        document.getElementById(id).style.display = 'none'
}
function GoodsInfoGetNum(num) {
    if (num == "" || num == null) {
        num = "0";
    }
    return num
}
function GoodsInfo_submit() {
    var goodsName = document.getElementById("goodsInfo_name").value;
    var goodsDetail = document.getElementById("goodsInfo_detail").value;
    var goodsPrice = GoodsInfoGetNum(document.getElementById("goodsInfo_price").value);
    var goodsPromotionPrice = GoodsInfoGetNum(document.getElementById("goodsInfo_promotionPrice").value);
    var goodsIfPromotion = document.getElementById("goodsInfo_changeBox_ifPromotion").value
    var goodsId = document.getElementById("goodsInfo_goodsId").innerHTML
    var goodsIfOnShelf = document.getElementById("goodsInfo_changeBox_ifOnShelf").value
    var goodsShelfNum = GoodsInfoGetNum(document.getElementById("goodsInfo_shelvesNum").value);
    
    $.ajax({
        url: "/Sale/ChangeGoodsInfo",
        data: {
            "goodsId": goodsId,
            "goodsName": goodsName,
            "goodsDetail": goodsDetail,
            "goodsPrice": goodsPrice,
            "goodsPromotionPrice": goodsPromotionPrice,
            "goodsIfPromotion": goodsIfPromotion,
            "goodsIfOnShelf": goodsIfOnShelf,
            "goodsShelfNum": goodsShelfNum,
        },
        dataType: "json",
        type: "GET",
        error: function (xhr) {
            alert('Error: ' + xhr.statusText);
        },
        success: function (data) {
            var obj = JSON.parse(data)
            document.getElementById("goodsInfo_GoodsName").innerHTML = obj.GoodsName
            document.getElementById("goodsInfo_Price").innerHTML = obj.Price
            document.getElementById("goodsInfo_PromotionPrice").innerHTML = obj.PromotionPrice
            document.getElementById("goodsInfo_GoodsDetail").innerHTML = obj.GoodsDetail
            document.getElementById("goodsInfo_goodsSimpleGraph").src = obj.GoodsSimpleGraph
            document.getElementById("goodsInfo_IfHot").innerHTML = obj.IfNewGoods

            document.getElementById("goodsInfo_OnShelfTime").innerHTML = obj.OnShelfTime
            document.getElementById("goodsInfo_OffShelfTime").innerHTML = obj.OffShelfTime
            document.getElementById("goodsInfo_ShelvesNum").innerHTML = obj.ShelvesNum
            document.getElementById("goodsInfo_GoodsStatus").innerHTML = obj.GoodsStatus
            document.getElementById("goodsInfo_GoodsStock").innerHTML = obj.Stock
        }
    });
}
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
addLoadEvent(GoodsInfoShowOrNot())

function $$(obj) {
    return document.getElementById(obj);
}
function upload(f,obj) {
    var str = "";
    for (var i = 0; i < f.length; i++) {
        var reader = new FileReader();
        reader.readAsDataURL(f[i]);
        reader.onload = function (e) {
            str += "<img src='" + e.target.result + "' class=\"goodsChangeImgsBox\"/>";
            $$(obj).innerHTML = str;
        }
    }
}
