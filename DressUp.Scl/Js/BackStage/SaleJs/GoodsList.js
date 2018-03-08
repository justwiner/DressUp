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
//**************分页方法***************//
function SaleJudgePage(page) {
    if (document.getElementById("sale_totalPage") == null)
        return;
    var totalPage = Number(document.getElementById("sale_totalPage").innerHTML);
    if (page > totalPage || page <= 0) {
        return false;
    }
    else {
        return true;
    }
}
function SaleGoPage(page) {
    if (SaleJudgePage(page)) {
        $.ajax({
            url: "/Sale/GoPage",
            type: 'GET',
            data: { "goPage": page },
            dataType: "json",
            error: function (xhr) {
                alert('Error: ' + xhr.statusText);
            },
            success: function (data) {
                var pageObj = JSON.parse(data)
                SaleShowTable(pageObj)
                
                document.getElementById("sale_nowPage").innerHTML = page
                SaleButtonJudge()
            },
        });
    }
}
function SaleChangePre() {
    var nowPage = Number(document.getElementById("sale_nowPage").innerHTML)
    SaleGoPage(nowPage - 1)
}
function SaleChangeNext() {
    var nowPage = Number(document.getElementById("sale_nowPage").innerHTML)
    SaleGoPage(nowPage + 1)
}
function SaleGoPageNum() {
    var pageNum = document.getElementById("sale_page").value
    if (pageNum == "") {
        pageNum = "1"
    }
    SaleGoPage(pageNum)
}

//************************查看商品详情*************************//
function SaleGoodsInfo(goodsId) {
    window.location.href = "/Sale/GoodsInfo?goodsId=" + goodsId;
}
//**************通过类别——库存——状态查找***************//
function SaleFindByType_Stock_Status(typeId, stockId, statusId) {
    var type = Number(typeId)
    var stock = Number(stockId)
    var status = Number(statusId)
    $.ajax({
        url: "/Sale/Find",
        type: "GET",
        data: { "typeId": type, "stockId": stock, "statusId": status },
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
//**************模糊查询*****************//
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
//************************库存查询*************************************//
function SaleFindJudge(name1, name2, name3) {
    SaleFindByType_Stock_Status(ByWhich(name1), ByWhich(name2), ByWhich(name3));
}
function SaleGetTotalPageNum() {
    $.ajax({
        url: "/Sale/GetTotalPageNum",
        type: 'GET',
        data: {},
        dataType: "json",
        error: function (xhr) {
            alert('Error: ' + xhr.statusText);
        },
        success: function (data) {
            document.getElementById("sale_totalPage").innerHTML = data
        },
    });
}
function SaleShowTable(pageObj) {
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
    if (pageObj != null) {
        for (var i = 0 ; i < pageObj.length ; i++) {
            innerHtml +=
                ("<tr>" +
                    "<td title=\"" + pageObj[i].GoodsId + "\">" + pageObj[i].GoodsId + "</td>" +
                    "<td id=\"" + pageObj[i].GoodsId + "\" onclick=\"SaleGoodsInfo(\'" + pageObj[i].GoodsId + "\')\" title=\"" + pageObj[i].GoodsName + "\"><a href=\"#\">" + pageObj[i].GoodsName + "</a></td>" +
                    "<td>" + pageObj[i].GoodsTypeName + "</td>" +
                    "<td>" + pageObj[i].PurchasePrice + "</td>" +
                    "<td>" + pageObj[i].Price + "</td>" +
                    "<td>" + pageObj[i].IfNewGoods + "</td>" +
                    "<td>" + pageObj[i].PromotionPrice + "</td>" +
                    "<td>" + pageObj[i].Stock + "</td>" +
                    "<td>" + pageObj[i].OnShelfTime + "</td>" +
                    "<td>" + pageObj[i].OffShelfTime + "</td>" +
                    "<td>" + pageObj[i].GoodsStatus + "</td>" +
                    "<td>" + pageObj[i].GoodsSaleNum + "</td>" +
                "</tr>")
        }
    }
    document.getElementById("saleGoodsTable").innerHTML = innerHtml;
}
function SaleButtonJudge() {
    if (document.getElementById("sale_nowPage") == null)
        return;
    var nowPage = parseInt(document.getElementById("sale_nowPage").innerHTML)
    var totalPage = parseInt(document.getElementById("sale_totalPage").innerHTML)
    if (nowPage == 1) {
        document.getElementById("sale_butPre").disabled = true;
    }
    else {
        document.getElementById("sale_butPre").disabled = false;
    }
    if (nowPage == totalPage) {
        document.getElementById("sale_butNext").disabled = true;
    }
    else {
        document.getElementById("sale_butNext").disabled = false;
    }
}
addLoadEvent(SaleGoPage(1))
addLoadEvent(SaleButtonJudge())