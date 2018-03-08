function GetTotalPageNum(controllerName,actionName) {
    $.ajax({
        url: "/" + controllerName + "/" + actionName + "",
        type: 'GET',
        data: {},
        dataType: "json",
        error: function (xhr) {
            alert('Error: ' + xhr.statusText);
        },
        success: function (data) {
            document.getElementById("totalPage").innerHTML = data
        },
    });
}
function ButtonJudge() {
    if (document.getElementById("nowPage") == null)
        return;
    var nowPage = parseInt(document.getElementById("nowPage").innerHTML)
    var totalPage = parseInt(document.getElementById("totalPage").innerHTML)
    if (nowPage == 1) {
        document.getElementById("butPre").disabled = true;
    }
    else {
        document.getElementById("butPre").disabled = false;
    }
    if (nowPage == totalPage) {
        document.getElementById("butNext").disabled = true;
    }
    else {
        document.getElementById("butNext").disabled = false;
    }
}
//**************分页方法***************//
function GoPage(page, controllerName, actionName) {
        $.ajax({
            url: "/" + controllerName + "/" + actionName + "",
            type: 'GET',
            data: { "goPage": page },
            dataType: "json",
            error: function (xhr) {
                alert('Error: ' + xhr.statusText);
            },
            success: function (data) {
                var pageObj = JSON.parse(data)
                ShowMyTable(pageObj)
                document.getElementById("nowPage").innerHTML = page
                ButtonJudge()
            },
        });
}
function JudgePage(page) {
    if (document.getElementById("totalPage") == null)
        return;
    var totalPage = Number(document.getElementById("totalPage").innerHTML);
    if (page > totalPage || page <= 0) {
        return false;
    }
    else {
        return true;
    }
}
function ChangePre(controllerName, actionName) {
    var nowPage = Number(document.getElementById("nowPage").innerHTML)
    GoPage(nowPage - 1, controllerName, actionName)
}
function ChangeNext(controllerName, actionName) {
    var nowPage = Number(document.getElementById("nowPage").innerHTML)
    GoPage(nowPage + 1, controllerName, actionName)
}
function GoPageNum(controllerName, actionName) {
    var pageNum = document.getElementById("page").value
    if (pageNum == "") {
        pageNum = "1"
    }
    GoPage(pageNum, controllerName, actionName)
}