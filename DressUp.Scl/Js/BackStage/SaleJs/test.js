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
addLoadEvent(GoPage("Sale",1))
addLoadEvent(ButtonJudge("Sale"))

function ShowTable(pageObj) {
    if (document.getElementById("saleGoodsTable") == null)
        return;
    var innerHtml = "<tr>" +
                        "<th>商品ID</th>" +
                        "<th>商品名</th>" +
                        "<th>商品类别</th>" +
                        "<th>进价</th>" +
                        "<th>售价</th>" +
                        "<th>是否促销</th>" +
                        "<th>促销价</th>" +
                        "<th>商品库存</th>" +
                        "<th>上架时间</th>" +
                        "<th>下架时间</th>" +
                        "<th>状态</th>" +
                        "<th>销量</th>" +
                    "</tr>"
    if (pageObj.length != 0) {
        for (var i = 0 ; i < pageObj.length ; i++) {
            innerHtml +=
                ("<tr>" +
                    "<td title=\"" + pageObj[i].GoodsId + "\">" + pageObj[i].GoodsId + "</td>" +
                    "<td id=\"" + pageObj[i].GoodsId + "\" onclick=\"SaleGoodsInfo(\'" + pageObj[i].GoodsId + "\')\" title=\"" + pageObj[i].GoodsName + "\"><a href=\"#\">" + pageObj[i].GoodsName + "</a></td>" +
                    "<td>" + pageObj[i].GoodsType + "</td>" +
                    "<td>" + pageObj[i].GoodsPurchasePrice + "</td>" +
                    "<td>" + pageObj[i].GoodsPrice + "</td>" +
                    "<td>" + pageObj[i].IfPromotion + "</td>" +
                    "<td>" + pageObj[i].GoodsPromotionPrice + "</td>" +
                    "<td>" + pageObj[i].GoodsStock + "</td>" +
                    "<td>" + pageObj[i].GoodsOnShelfTime + "</td>" +
                    "<td>" + pageObj[i].GoodsOffShelfTime + "</td>" +
                    "<td>" + pageObj[i].GoodsStatus + "</td>" +
                    "<td>" + pageObj[i].GoodsSalesNum + "</td>" +
                "</tr>")
        }
    }
    document.getElementById("saleGoodsTable").innerHTML = innerHtml;
}

function SaleVagueSearch() {
    var goodsName = document.getElementById("goodsName").value;
    $.ajax({
        url: "/Sale/VagueSearch",
        type: "GET",
        data: { "goodsName": goodsName },
        dataType: "json",
        error: function (xhr) {
            alert('Error: ' + xhr.statusText);
        },
        success: function (data) {
            var pageObj = JSON.parse(data)
            SaleShowTable(pageObj)

            if (document.getElementById("sale_nowPage") == null) {
                return;
            }
            document.getElementById("sale_nowPage").innerHTML = 1
            SaleButtonJudge()
        }
    })
}