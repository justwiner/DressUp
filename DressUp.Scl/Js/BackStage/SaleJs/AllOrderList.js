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
addLoadEvent(AllOrderGoPage(1))
addLoadEvent(AllOrderButtonJudge())
function AllOrderGoPage(page) {
    if (AllOrderJudgePage(page)) {
        $.ajax({
            url: "/Sale/AllOrderGoPage",
            type: 'GET',
            data: { "goPage": page },
            dataType: "json",
            error: function (xhr) {
                alert('Error: ' + xhr.statusText);
            },
            success: function (data) {
                var pageObj = JSON.parse(data)
                AllOrderShowTable(pageObj)

                document.getElementById("AllOrder_nowPage").innerHTML = page
                AllOrderButtonJudge()
            },
        });
    }
}
function AllOrderGetPageNum() {
    $.ajax({
        url: "/Sale/GetAllOrderPageNum",
        type: 'GET',
        data: {},
        dataType: "json",
        error: function (xhr) {
            alert('Error: ' + xhr.statusText);
        },
        success: function (data) {
            document.getElementById("AllOrder_totalPage").innerHTML = data
        },
    });
}
function AllOrderButtonJudge() {
    if (document.getElementById("AllOrder_nowPage") == null)
        return;
    var nowPage = parseInt(document.getElementById("AllOrder_nowPage").innerHTML)
    var totalPage = parseInt(document.getElementById("AllOrder_totalPage").innerHTML)
    if (nowPage == 1) {
        document.getElementById("AllOrder_butPre").disabled = true;
    }
    else {
        document.getElementById("AllOrder_butPre").disabled = false;
    }
    if (nowPage == totalPage) {
        document.getElementById("AllOrder_butNext").disabled = true;
    }
    else {
        document.getElementById("AllOrder_butNext").disabled = false;
    }
}
function AllOrderJudgePage(page) {
    if (document.getElementById("AllOrder_totalPage") == null)
        return;
    var totalPage = Number(document.getElementById("AllOrder_totalPage").innerHTML);
    if (page > totalPage || page <= 0) {
        return false;
    }
    else {
        return true;
    }
}
function AllOrderChangePre() {
    var nowPage = Number(document.getElementById("AllOrder_nowPage").innerHTML)
    AllOrderGoPage(nowPage - 1)
}
function AllOrderChangeNext() {
    var nowPage = Number(document.getElementById("AllOrder_nowPage").innerHTML)
    AllOrderGoPage(nowPage + 1)
}
function AllOrderGoPageNum() {
    var pageNum = document.getElementById("AllOrder_page").value
    if (pageNum == "") {
        pageNum = "1"
    }
    AllOrderGoPage(pageNum)
}
function AllOrderShowTable(pageObj) {
    if (document.getElementById("saleAllOrderTable") == null)
        return;
    var innerHtml = "<tr>" +
                        "<th>订单ID</th>" +
                        "<th>订单时间</th>" +
                        "<th>商品图</th>" +
                        "<th>商品名</th>" +
                        "<th>单价</th>" +
                        "<th>数量</th>" +
                        "<th>收货地址</th>" +
                        "<th>收货人</th>" +
                        "<th>联系方式</th>" +
                        "<th>订单状态</th>" +
                    "</tr>"
    if (pageObj != null) {
        for (var i = 0 ; i < pageObj.length ; i++) {
            innerHtml +=
                ("<tr>" +
                    "<td title=\"" + pageObj[i].OrderNum + "\">" + pageObj[i].OrderNum + "</td>" +
                    "<td>" + pageObj[i].OrderGenerationTime + "</td>" +
                    "<td><img src=\"" + pageObj[i].GoodsImg + "\"\></td>" +
                    "<td>" + pageObj[i].GoodsName + "</td>" +
                    "<td>" + pageObj[i].GoodsPrice + "</td>" +
                    "<td>" + pageObj[i].GoodsNum + "</td>" +
                    "<td>" + pageObj[i].ReceiptAddress + "</td>" +
                    "<td>" + pageObj[i].Consignee + "</td>" +
                    "<td>" + pageObj[i].ContactInfo + "</td>" +
                    "<td>" + pageObj[i].OrderStatus + "</td>" +
                "</tr>")
        }
    }
    document.getElementById("saleAllOrderTable").innerHTML = innerHtml;
}
//**************通过状态查找***************//
function AllOrderFindByStatus(obj) {
    var status = Number(obj.getAttribute("id"))
    $.ajax({
        url: "/Sale/AllOrderFindByStatus",
        type: "GET",
        data: {"statusId": status },
        dataType: "json",
        error: function (xhr) {
            alert('Error: ' + xhr.statusText);
        },
        success: function (data) {
            var obj = JSON.parse(data)
            AllOrderShowTable(obj)
            document.getElementById("AllOrder_nowPage").innerHTML = 1
            AllOrderButtonJudge()
        }
    })
}
//**************通过时间查找***************//
function AllOrderFindByTime() {
    var minDate = document.getElementById("OrderMinTime").value.toString();
    if (minDate == "") {
        alert("未输入日期，请输入。")
        return;
    }
    var maxDate = document.getElementById("OrderMaxTime").value.toString();
    if (maxDate == "") {
        alert("未输入日期，请输入。")
        return;
    }
    $.ajax({
        url: "/Sale/AllOrderFindByTime",
        type: "GET",
        data: { "minTime": minDate,"maxTime":maxDate },
        dataType: "json",
        error: function (xhr) {
            alert('Error: ' + xhr.statusText);
        },
        success: function (data) {
            var obj = JSON.parse(data)
            AllOrderShowTable(obj)
            document.getElementById("AllOrder_nowPage").innerHTML = 1
            AllOrderButtonJudge()
        }
    })
}