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
addLoadEvent(goPage(1))
addLoadEvent(buttonJudge())
function GetTotalPageNum() {
    $.ajax({
        url: "/StoreManagement/GetTotalPageNum",
        type: 'GET',
        data: { },
        dataType: "json",
        error: function (xhr) {
            alert('Error: ' + xhr.statusText);
        },
        success: function (data) {
            document.getElementById("store_totalPage").innerHTML = data
        },
    });
}
function ShowTable(pageObj) {
    if (document.getElementById("storeGoodsTable") == null) {
        return;
    }
    var innerHtml = "<tr><th>商品ID</th><th>商品图</th><th>商品名</th><th>商品类别</th><th>商品库存</th><th>售价</th><th>进价</th><th>状态</th></tr>"
    if (pageObj != null) {
        for (var i = 0 ; i < pageObj.length ; i++) {
            innerHtml +=
                ("<tr>" +
                    "<td title=\"" + pageObj[i].GoodsId + "\">" + pageObj[i].GoodsId + "</td>" +
                    "<td><img src=\"" + pageObj[i].GoodsSimpleGraph + "\" class=\"store_goodsImg\" /></td>" +
                    "<td title=\"" + pageObj[i].GoodsName + "\">" + pageObj[i].GoodsName + "</td>" +
                    "<td>" + pageObj[i].GoodsTypeName + "</td>" +
                    "<td>" + pageObj[i].Stock + "</td>" +
                    "<td>" + pageObj[i].Price + "</td>" +
                    "<td>" + pageObj[i].PromotionPrice + "</td>" +
                    "<td>" + pageObj[i].GoodsStatus + "</td>" +
                "</tr>")
        }
    }
    document.getElementById("storeGoodsTable").innerHTML = innerHtml;
}
function buttonJudge() {
    if (document.getElementById("store_nowPage") == null)
        return;
    var nowPage = parseInt(document.getElementById("store_nowPage").innerHTML)
    var totalPage = parseInt(document.getElementById("store_totalPage").innerHTML)
    if (nowPage == 1) {
        document.getElementById("store_butPre").disabled = true;
    }
    else {
        document.getElementById("store_butPre").disabled = false;
    }
    if (nowPage == totalPage) {
        document.getElementById("store_butNext").disabled = true;
    }
    else {
        document.getElementById("store_butNext").disabled = false;
    }
}
//**************分页方法***************//
function JudgePage(page) {
    if (document.getElementById("store_totalPage") == null)
        return;
    var totalPage = Number(document.getElementById("store_totalPage").innerHTML);
    if (page > totalPage || page <= 0) {
        return false;
    }
    else {
        return true;
    }
}
function goPage(page) {
    if (JudgePage(page)) {
        $.ajax({
            url: "/StoreManagement/GoPage",
            type: 'GET',
            data: { "goPage": page },
            dataType: "json",
            error: function (xhr) {
                alert('Error: ' + xhr.statusText);
            },
            success: function (data) {
                var pageObj = JSON.parse(data)
                ShowTable(pageObj)
                document.getElementById("store_nowPage").innerHTML = page
                buttonJudge()
            },
        });
    }
}
function changePre() {
    var nowPage = Number(document.getElementById("store_nowPage").innerHTML)
    goPage(nowPage - 1)
}
function changeNext() {
    var nowPage = Number(document.getElementById("store_nowPage").innerHTML)
    goPage(nowPage + 1)
}
function goPageNum() {
    var pageNum = document.getElementById("page").value
    if (pageNum == "") {
        pageNum = "1"
    }
    goPage(pageNum)
}

//************************快速查找方法*************************//
function ChangeColor(allLabel, obj) {
    var all = document.getElementsByName(allLabel)
    for (var i = 0 ; i < all.length ; i++) {
        all[i].style.color = "black"
    }
    obj.style.color = "red"
}
function ByWhich(name) {
    var obj = document.getElementsByName(name)
    for (var i = 0 ; i < obj.length ; i++)
    {
        if (obj[i].style.color == "red") {
            return obj[i].getAttribute("id");
        }
    }
    return "400"
}
function StoreFindJudge(name1, name2, name3) {
    StoreFindByType_Stock_Status(ByWhich(name1), ByWhich(name2), ByWhich(name3));
}
//**************通过类别——库存——状态查找***************//
function StoreFindByType_Stock_Status(typeId, stockId, statusId) {
    var type = Number(typeId)
    var stock = Number(stockId)
    var status = Number(statusId)
    $.ajax({
        url: "/StoreManagement/Find",
        type: "GET",
        data: { "typeId": type, "stockId": stock, "statusId": status },
        dataType: "json",
        error: function (xhr) {
            alert('Error: ' + xhr.statusText);
        },
        success: function (data) {
            var obj = JSON.parse(data)
            ShowTable(obj)
            document.getElementById("store_nowPage").innerHTML = 1
            buttonJudge()
        }
    })
}
//**************模糊查询*****************//
function StoreVagueSearch() {
    var goodsName = document.getElementById("goodsName").value;
    $.ajax({
        url: "/StoreManagement/VagueSearch",
        type: "GET",
        data: { "goodsName": goodsName},
        dataType: "json",
        error: function (xhr) {
            alert('Error: ' + xhr.statusText);
        },
        success: function (data) {
            var obj = JSON.parse(data)
            ShowTable(obj)
            document.getElementById("store_nowPage").innerHTML = 1
            buttonJudge()
        }
    })
}
//************************商品入库*************************************//
function newStorage() {
    var goodsName = document.getElementById("storage_goodsName").value;
    if (goodsName == "") {
        alert("请输入商品名。")
        return;
    }
    var goodsType = document.getElementById("storage_goodsType").value;
    var goodsNum = document.getElementById("storage_goodsNum").value;
    if (goodsNum == "") {
        alert("请输入商品数量。")
        return;
    }
    else {
        goodsNum = Number(goodsNum);
        if (goodsNum <= 0) {
            alert("输入的商品数量有误，请重新输入。")
            return;
        }
    }
    var purchasePrice = document.getElementById("storage_purchasePrice").value;
    if (purchasePrice == "") {
        alert("请输入商品进价。")
        return;
    }
    else {
        purchasePrice = Number(purchasePrice);
        if (purchasePrice <= 0) {
            alert("输入的商品进价有误，请重新输入。")
            return;
        }
    }
    $.ajax({
        url: "/StoreManagement/NewStorage",
        type: "GET",
        data: { "goodsName": goodsName, "goodsType": goodsType, "goodsNum": goodsNum, "purchasePrice": purchasePrice },
        dataType: "json",
        error: function (xhr) {
            alert('Error: ' + xhr.statusText);
        },
        success: function (data) {
            //var obj = JSON.parse(data)
            //ShowTable(obj)
            //document.getElementById("store_nowPage").innerHTML = 1
            //buttonJudge()
            if (data == true) {
                alert("添加成功。")
            }
            else {
                alert("添加失败，请重试。")
            }
            document.getElementById("storage_goodsName").value = "";
            document.getElementById("storage_goodsType").value = "";
            document.getElementById("storage_goodsNum").value = "";
            document.getElementById("storage_purchasePrice").value = ""
        }
    })
}