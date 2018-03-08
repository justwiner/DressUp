//************************库存查询*************************************//
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
addLoadEvent(ShortageWarningGoPage(1))
addLoadEvent(SaleButtonJudge())
function ShortageWarningGoPage(page) {
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
                ShortageWarningShowTable(pageObj)

                document.getElementById("sale_nowPage").innerHTML = page
                SaleButtonJudge()
            },
        });
    }
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
function ShortageWarningChangePre() {
    var nowPage = Number(document.getElementById("sale_nowPage").innerHTML)
    ShortageWarningGoPage(nowPage - 1)
}
function ShortageWarningChangeNext() {
    var nowPage = Number(document.getElementById("sale_nowPage").innerHTML)
    ShortageWarningGoPage(nowPage + 1)
}
function ShortageWarningGoPageNum() {
    var pageNum = document.getElementById("sale_page").value
    if (pageNum == "") {
        pageNum = "1"
    }
    ShortageWarningGoPage(pageNum)
}
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
function ShortageWarningShowTable(pageObj) {
    if (document.getElementById("saleShortageWarningTable") == null)
        return;
    var innerHtml = "<tr>" +
                        "<th>商品ID</th>" +
                        "<th>商品名</th>" +
                        "<th>商品类别</th>" +
                        "<th>售价</th>" +
                        "<th>商品库存</th>" +
                        "<th>状态</th>" +
                        "<th>操作</th>" +
                    "</tr>"
    if (pageObj != null) {
        for (var i = 0 ; i < pageObj.length ; i++) {
            innerHtml +=
                ("<tr>" +
                    "<td title=\"" + pageObj[i].GoodsId + "\">" + pageObj[i].GoodsId + "</td>" +
                    "<td id=\"" + pageObj[i].GoodsId + "\" onclick=\"SaleGoodsInfo(\'" + pageObj[i].GoodsId + "\')\" title=\"" + pageObj[i].GoodsName + "\"><a href=\"#\">" + pageObj[i].GoodsName + "</a></td>" +
                    "<td>" + pageObj[i].GoodsTypeName + "</td>" +
                    "<td>" + pageObj[i].Price + "</td>" +
                    "<td>" + pageObj[i].Stock + "</td>" +
                    "<td>" + pageObj[i].GoodsStatus + "</td>" +
                    "<td onclick=\"SaleInputPurchaseNum(\'" + pageObj[i].GoodsId + "\')\"><a href=\"#\">进货</a></td>" +
                "</tr>")
        }
    }
    document.getElementById("saleShortageWarningTable").innerHTML = innerHtml;
}
var id
function SaleInputPurchaseNum(goodsId) {
    id = goodsId;
    document.getElementById("ShortageWarningInputBox").style.display = "block";
}

function SalePurChase() {
    var purchaseNum =Number(document.getElementById("ShortageWarningPurchaseNum").value)
    $.ajax({
        url: "/Sale/CreatWarehousingOrder",
        data: { "purchaseNum": purchaseNum, "goodsId": id },
        dataType: "JSON",
        type: "POST",
        error: function (xhr) {
            alert('Error: ' + xhr.statusText);
        },
        success: function (data) {
            if (data == true)
                alert("提交成功，等待仓库管理人员处理。")
            else
                alert("提交失败请重试。");
        },
    })
}
function ShourageWarningFindJudge(name1, name2) {
    FindByType_Status(ByWhich(name1), ByWhich(name2));
}
//**************通过类别——状态查找***************//
function FindByType_Status(typeId, statusId) {
    var type = Number(typeId)
    var status = Number(statusId)
    $.ajax({
        url: "/Sale/ShourageWarningFind",
        type: "GET",
        data: { "typeId": type, "statusId": status },
        dataType: "json",
        error: function (xhr) {
            alert('Error: ' + xhr.statusText);
        },
        success: function (data) {
            var obj = JSON.parse(data)
            ShortageWarningShowTable(obj)
            document.getElementById("sale_nowPage").innerHTML = 1
            SaleButtonJudge()
        }
    })
} 
function ShourageWarningVagueSearch() {
    var goodsName = document.getElementById("goodsName").value;
    $.ajax({
        url: "/Sale/ShourageWarningVagueSearch",
        type: "GET",
        data: { "goodsName": goodsName },
        dataType: "json",
        error: function (xhr) {
            alert('Error: ' + xhr.statusText);
        },
        success: function (data) {
            var obj = JSON.parse(data)
            ShortageWarningShowTable(obj)
            document.getElementById("sale_nowPage").innerHTML = 1
            SaleButtonJudge()
        }
    })
}
function FindMoreStock() {
    window.location.href = "/Sale/ShortageWarning"
}