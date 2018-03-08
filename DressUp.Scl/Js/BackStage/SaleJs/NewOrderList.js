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
addLoadEvent(NewOrderGoPage(1))
addLoadEvent(AllOrderButtonJudge())
function NewOrderGoPage(page) {
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
                NewOrderShowTable(pageObj)

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
function NewOrderChangePre() {
    var nowPage = Number(document.getElementById("AllOrder_nowPage").innerHTML)
    NewOrderGoPage(nowPage - 1)
}
function NewOrderChangeNext() {
    var nowPage = Number(document.getElementById("AllOrder_nowPage").innerHTML)
    NewOrderGoPage(nowPage + 1)
}
function NewOrderGoPageNum() {
    var pageNum = document.getElementById("AllOrder_page").value
    if (pageNum == "") {
        pageNum = "1"
    }
    NewOrderGoPage(pageNum)
}
function NewOrderShowTable(pageObj) {
    if (document.getElementById("saleNewOrderTable") == null)
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
                        "<th>操作</th>" +
                    "</tr>"
    if (pageObj.length != 0) {
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
                    "<td><a href=\"#\" onclick=\"NewOrderLibrary(\'" + pageObj[i].OrderNum + "\')\">处理</a></td>" +
                "</tr>")
        }
    }
    document.getElementById("saleNewOrderTable").innerHTML = innerHtml;
}
//**************申请出库*****************//
var orderId;
function NewOrderLibrary(id) {
    orderId = id;
    document.getElementById("NewOrderLibrary").style.display = "block"
}
function NewOrderLibrarySubmit() {
    $.ajax({
        url: "/Sale/Library",
        data: { "orderId": orderId },
        dataType: "JSON",
        type: "POST",
        error: function (xhr) {
            alert('Error: ' + xhr.statusText);
        },
        success: function (data) {
            if (data == true) {
                alert("提交成功，等待仓库管理人员处理。")
                var nowPage = Number(document.getElementById("AllOrder_nowPage").innerHTML)
                NewOrderGoPage(nowPage)
                AllOrderButtonJudge()
            }
            else
                alert("提交失败请重试。");
        },
    })
}
//**************通过状态查找***************//
function NewOrderFindByStatus(obj) {
    var status = Number(obj.getAttribute("id"))
    $.ajax({
        url: "/Sale/NewOrderFindByStatus",
        type: "GET",
        data: { "statusId": status },
        dataType: "json",
        error: function (xhr) {
            alert('Error: ' + xhr.statusText);
        },
        success: function (data) {
            var obj = JSON.parse(data)
            NewOrderShowTable(obj)
            document.getElementById("AllOrder_nowPage").innerHTML = 1
            AllOrderButtonJudge()
        }
    })
}
//**************通过时间查找***************//
function NewOrderFindByTime() {
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
        url: "/Sale/NewOrderFindByTime",
        type: "GET",
        data: { "minTime": minDate, "maxTime": maxDate },
        dataType: "json",
        error: function (xhr) {
            alert('Error: ' + xhr.statusText);
        },
        success: function (data) {
            var obj = JSON.parse(data)
            NewOrderShowTable(obj)
            document.getElementById("AllOrder_nowPage").innerHTML = 1
            AllOrderButtonJudge()
        }
    })
}