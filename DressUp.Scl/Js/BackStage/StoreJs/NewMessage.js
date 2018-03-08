//************************未处理消息*************************************//
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
addLoadEvent(NewMessageGoPage(1))
addLoadEvent(NewMessageButtonJudge())
function NewMessageGetTotalPageNum() {
    $.ajax({
        url: "/StoreManagement/NewMessageGetTotalPageNum",
        type: 'GET',
        data: {},
        dataType: "json",
        error: function (xhr) {
            alert('Error: ' + xhr.statusText);
        },
        success: function (data) {
            document.getElementById("newMessage_totalPage").innerHTML = data
        },
    });
}
function NewMessageShowTable(pageObj) {
    var innerHtml = "<tr><th>单号</th><th>类型</th><th>商品ID</th><th>商品名</th><th>数量</th><th>操作</th>"
    if (pageObj != null) {
        for (var i = 0 ; i < pageObj.length ; i++) {
            innerHtml +=
                ("<tr>" +
                    "<td title=\"" + pageObj[i].Id + "\">" + pageObj[i].Id + "</td>" +
                    "<td>" + pageObj[i].Type + "</td>" +
                    "<td title=\"" + pageObj[i].GoodsId + "\">" + pageObj[i].GoodsId + "</td>" +
                    "<td>" + pageObj[i].GoodsName + "</td>" +
                    "<td>" + pageObj[i].Num + "</td>" +
                    "<td><a href=\"#\" onclick=\"NewMessageInfoBox(\'" + pageObj[i].Id + "\')\">详情</a></td>" +
                "</tr>")
        }
    }
    document.getElementById("newMessageTable").innerHTML = innerHtml;
}
var messageId
function NewMessageInfoBox(id) {
    messageId = id;
    document.getElementById("NewMessageInfoBox").style.display = "block"
    $.ajax({
        url: "/StoreManagement/NewMessageInfo",
        data: { "messageId": messageId },
        dataType: "json",
        type:"GET",
        success: function (data) {
            var obj = JSON.parse(data)
            if (obj.MessageType == "出库") {
                document.getElementById("ShipmentInfoBox").style.display = "block"
                document.getElementById("PurchaseInfoBox").style.display = "none"
                document.getElementById("Purchase").style.display = "none"
                document.getElementById("Shipment").style.display = "inline-block"

                document.getElementById("Shipment_messageId").innerHTML = obj.MessageId
                document.getElementById("Shipment_messageType").innerHTML = obj.MessageType
                document.getElementById("Shipment_GoodsId").innerHTML = obj.GoodsId
                document.getElementById("Shipment_GoodsName").innerHTML = obj.GoodsName
                document.getElementById("Shipment_GoodsType").innerHTML = obj.GoodsType
                document.getElementById("Shipment_ContactInfo").innerHTML = obj.ContactInfo
                document.getElementById("Shipment_GoodsPurchase").innerHTML = obj.GoodsPurchasePrice
                document.getElementById("Shipment_GoodsNum").innerHTML = obj.GoodsNum
                document.getElementById("Shipment_Consignee").innerHTML = obj.Consignee
                document.getElementById("Shipment_ReceiptAddress").innerHTML = obj.ReceiptAddress
                document.getElementById("Shipment_OrderId").innerHTML = obj.OrderId
            }
            else {
                document.getElementById("PurchaseInfoBox").style.display = "block"
                document.getElementById("ShipmentInfoBox").style.display = "none"
                document.getElementById("Shipment").style.display = "none"
                document.getElementById("Purchase").style.display = "inline-block"

                document.getElementById("Purchase_messageId").innerHTML = obj.MessageId
                document.getElementById("Purchase_messageType").innerHTML = obj.MessageType
                document.getElementById("Purchase_GoodsId").innerHTML = obj.GoodsId
                document.getElementById("Purchase_GoodsName").innerHTML = obj.GoodsName
                document.getElementById("Purchase_GoodsType").innerHTML = obj.GoodsType
                document.getElementById("Purchase_GoodsPurchase").innerHTML = obj.GoodsPurchasePrice
                document.getElementById("Purchase_GoodsNum").innerHTML = obj.Num
            }
        }
    })
}
function ShipmentSubmit() {
    var messageId = document.getElementById("Shipment_messageId").innerHTML
    var goodsId = document.getElementById("Shipment_GoodsId").innerHTML
    var goodsName = document.getElementById("Shipment_GoodsName").innerHTML
    var contactInfo = document.getElementById("Shipment_ContactInfo").innerHTML
    var goodsNum = document.getElementById("Shipment_GoodsNum").innerHTML
    var consignee = document.getElementById("Shipment_Consignee").innerHTML
    var receiptAddress = document.getElementById("Shipment_ReceiptAddress").innerHTML
    var orderId = document.getElementById("Shipment_OrderId").innerHTML
    $.ajax({
        url: "/StoreManagement/GoodsShipment",
        data: { "orderId": orderId, "messageId": messageId, "goodsId": goodsId, "goodsName": goodsName, "contactInfo": contactInfo, "goodsNum": goodsNum, "consignee": consignee, "receiptAddress": receiptAddress },
        dataType: "json",
        type: "POST",
        success: function (data) {
            if (data == true) {
                alert("商品出库成功！")
            }
            else {
                alert("商品出库过程遇到错误，请重试。")
            }
            NewMessageGoPage(1)
        }
    })
}
function PurchaseSubmit() {
    var messageId = document.getElementById("Purchase_messageId").innerHTML
    var goodsId = document.getElementById("Purchase_GoodsId").innerHTML
    var goodsName = document.getElementById("Purchase_GoodsName").innerHTML
    var goodsType = document.getElementById("Purchase_GoodsType").innerHTML
    var purchasePrice = Number(document.getElementById("Purchase_GoodsPurchase").innerHTML)
    var goodsNum = Number(document.getElementById("Purchase_GoodsNum").innerHTML)
    $.ajax({
        url: "/StoreManagement/GoodsPurchase",
        data: { "messageId": messageId, "goodsId": goodsId, "goodsName": goodsName, "goodsType": goodsType, "goodsNum": goodsNum, "purchasePrice": purchasePrice },
        dataType: "json",
        type: "POST",
        success: function (data) {
            if (data == true) {
                alert("商品入库成功！")
            }
            else {
                alert("商品入库过程遇到错误，请重试。")
            }
            NewMessageGoPage(1)
        }
    })
}
function NewMessageButtonJudge() {
    if (document.getElementById("newMessage_nowPage") == null)
        return;
    var nowPage = parseInt(document.getElementById("newMessage_nowPage").innerHTML)
    var totalPage = parseInt(document.getElementById("newMessage_totalPage").innerHTML)
    if (nowPage == 1) {
        document.getElementById("newMessage_butPre").disabled = true;
    }
    else {
        document.getElementById("newMessage_butPre").disabled = false;
    }
    if (nowPage == totalPage) {
        document.getElementById("newMessage_butNext").disabled = true;
    }
    else {
        document.getElementById("newMessage_butNext").disabled = false;
    }
}
//**************分页方法***************//
function NewMessageJudgePage(page) {
    if (document.getElementById("newMessage_totalPage") == null)
        return;
    var totalPage = Number(document.getElementById("newMessage_totalPage").innerHTML);
    if (page > totalPage || page <= 0) {
        return false;
    }
    else {
        return true;
    }
}
function NewMessageGoPage(page) {
    if (NewMessageJudgePage(page)) {
        $.ajax({
            url: "/StoreManagement/NewMessageGoPage",
            type: 'GET',
            data: { "goPage": page },
            dataType: "json",
            error: function (xhr) {
                alert('Error: ' + xhr.statusText);
            },
            success: function (data) {
                var pageObj = JSON.parse(data)
                NewMessageShowTable(pageObj)
                document.getElementById("newMessage_nowPage").innerHTML = page
                NewMessageButtonJudge()
            },
        });
    }
}
function NewMessageChangePre() {
    var nowPage = Number(document.getElementById("newMessage_nowPage").innerHTML)
    NewMessageGoPage(nowPage - 1)
}
function NewMessageChangeNext() {
    var nowPage = Number(document.getElementById("newMessage_nowPage").innerHTML)
    NewMessageGoPage(nowPage + 1)
}
function NewMessageGoPageNum() {
    var pageNum = document.getElementById("newMessage_page").value
    if (pageNum == "") {
        pageNum = "1"
    }
    NewMessageGoPage(pageNum)
}
//**************通过类别***************//
function FindByMessageType(obj) {
    var type = Number(obj.getAttribute("id"))
    $.ajax({
        url: "/StoreManagement/FindByMessageType",
        type: "GET",
        data: { "typeId": type},
        dataType: "json",
        error: function (xhr) {
            alert('Error: ' + xhr.statusText);
        },
        success: function (data) {
            var obj = JSON.parse(data)
            NewMessageShowTable(obj)
            document.getElementById("newMessage_nowPage").innerHTML = 1
            NewMessageButtonJudge()
        }
    })
}