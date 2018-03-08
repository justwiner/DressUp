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
addLoadEvent(GeneralOperationGoPage(1))
addLoadEvent(GeneralOperationButtonJudge())
function GeneralOperationGetTotalPageNum() {
    $.ajax({
        url: "/GeneralOperation/GetTotalPageNum",
        type: 'GET',
        data: {},
        dataType: "json",
        error: function (xhr) {
            alert('Error: ' + xhr.statusText);
        },
        success: function (data) {
            document.getElementById("GeneralOperation_totalPage").innerHTML = data
        },
    });
}
function GeneralOperationShowTable(pageObj) {
    var innerHtml = "<tr><th>员工编号</th><th>用户名</th><th>部门</th><th>时间</th><th>单号</th><th>备注</th></tr>"
    if (pageObj != null) {
        for (var i = 0 ; i < pageObj.length ; i++) {
            innerHtml +=
                ("<tr>" +
                    "<td title=\"" + pageObj[i].UserId + "\">" + pageObj[i].UserId + "</td>" +
                    "<td>" + pageObj[i].UserName + "</td>" +
                    "<td>" + pageObj[i].Mode + "</td>" +
                    "<td>" + pageObj[i].Time + "</td>" +
                    "<td title=\"" + pageObj[i].RecordId + "\">" + pageObj[i].RecordId + "</td>" +
                    "<td>" + pageObj[i].Detail + "</td>" +
                "</tr>")
        }
    }
    document.getElementById("GeneralOperation_WorkLog").innerHTML = innerHtml;
}
function GeneralOperationButtonJudge() {
    if (document.getElementById("GeneralOperation_nowPage") == null)
        return;
    var nowPage = parseInt(document.getElementById("GeneralOperation_nowPage").innerHTML)
    var totalPage = parseInt(document.getElementById("GeneralOperation_totalPage").innerHTML)
    if (nowPage == 1) {
        document.getElementById("GeneralOperation_butPre").disabled = true;
    }
    else {
        document.getElementById("GeneralOperation_butPre").disabled = false;
    }
    if (nowPage == totalPage) {
        document.getElementById("GeneralOperation_butNext").disabled = true;
    }
    else {
        document.getElementById("GeneralOperation_butNext").disabled = false;
    }
}
//**************分页方法***************//
function GeneralOperationJudgePage(page) {
    if (document.getElementById("GeneralOperation_totalPage") == null)
        return;
    var totalPage = Number(document.getElementById("GeneralOperation_totalPage").innerHTML);
    if (page > totalPage || page <= 0) {
        return false;
    }
    else {
        return true;
    }
}
function GeneralOperationGoPage(page) {
    if (GeneralOperationJudgePage(page)) {
        $.ajax({
            url: "/GeneralOperation/GoPage",
            type: 'GET',
            data: { "goPage": page },
            dataType: "json",
            error: function (xhr) {
                alert('Error: ' + xhr.statusText);
            },
            success: function (data) {
                var pageObj = JSON.parse(data)
                GeneralOperationShowTable(pageObj)
                document.getElementById("GeneralOperation_nowPage").innerHTML = page
                GeneralOperationButtonJudge()
            },
        });
    }
}
function GeneralOperationChangePre() {
    var nowPage = Number(document.getElementById("GeneralOperation_nowPage").innerHTML)
    GeneralOperationGoPage(nowPage - 1)
}
function GeneralOperationChangeNext() {
    var nowPage = Number(document.getElementById("GeneralOperation_nowPage").innerHTML)
    GeneralOperationGoPage(nowPage + 1)
}
function GeneralOperationGoPageNum() {
    var pageNum = document.getElementById("GeneralOperation_page").value
    if (pageNum == "") {
        pageNum = "1"
    }
    GeneralOperationGoPage(pageNum)
}


//**************通过时间查找***************//
function GeneralOperationFindByTime(obj) {
    var timeId =Number(obj.getAttribute("id"))
    $.ajax({
        url: "/GeneralOperation/FindByTime",
        type: "GET",
        data: { "timeId": timeId},
        dataType: "json",
        error: function (xhr) {
            alert('Error: ' + xhr.statusText);
        },
        success: function (data) {
            var obj = JSON.parse(data)
            GeneralOperationShowTable(obj)
            document.getElementById("GeneralOperation_nowPage").innerHTML = 1
            GeneralOperationButtonJudge()
        }
    })
}