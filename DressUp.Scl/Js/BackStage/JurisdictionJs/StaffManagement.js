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
addLoadEvent(GoPage(1, 'Jurisdiction', 'GoUsersPage'))
addLoadEvent(ButtonJudge())
function ShowMyTable(pageObj) {
    if (document.getElementById("table") == null)
        return;
    var innerHtml = "<tr>" +
                        "<th>用户ID</th>" +
                        "<th>用户名</th>" +
                        //"<th>角色</th>" +
                        //"<th>权限</th>" +
                        "<th>用户账号</th>" +
                        "<th>创建时间</th>" +
                        "<th>联系方式</th>" +
                        "<th>操作</th>" +
                    "</tr>"
    if (pageObj.length != 0) {
        for (var i = 0 ; i < pageObj.length ; i++) {
            innerHtml +=
                ("<tr>" +
                    "<td title=\"" + pageObj[i].UserId + "\">" + pageObj[i].UserId + "</td>" +
                    "<td>" + pageObj[i].Name + "</td>" +
                    //"<td class=\"focus\" data-remodal-target=\"checkRoles\">查看角色</td>" +
                    //"<td class=\"focus\" data-remodal-target=\"checkSpecialPermissions\">查看特殊权限</td>" +
                    "<td>" + pageObj[i].Account + "</td>" +
                    "<td>" + pageObj[i].CreationTime + "</td>" +
                    "<td>" + pageObj[i].ContactInfo + "</td>" +
                    "<td class=\"focus\" data-remodal-target=\"modifyPermissions\" onclick=\"GetRolePermissions(\'" + pageObj[i].UserId + "\')\">修改权限</td>" +
                "</tr>"
                );
        }
    }
    document.getElementById("table").innerHTML = innerHtml;
}
function AddUser() {
    var nowPage = document.getElementById("nowPage").innerHTML
    var userName = document.getElementById("userName").value;
    var userAccount = document.getElementById("userAccount").value;
    var userPassword = document.getElementById("userPassword").value;
    var userContactInfo = document.getElementById("userContactInfo").value;
    if (userName == "") {
        alert("请输入。")
        return;
    }
        $.ajax({
            url: "/Jurisdiction/AddUser",
            data: { 'userName': userName, "userAccount": userAccount, "userPassword": userPassword, "userContactInfo": userContactInfo },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data) {
                    alert("添加成功.")
                    GoPage(nowPage, 'Jurisdiction', 'GoUsersPage')
                }
                else {
                    alert("(⊙o⊙)…添加失败了！")
                }
            }
        })
}
var zNodes
var setting
var tree
var UserID
function GetRolePermissions(userId) {
    UserID = userId
    $.ajax({
        url: "/Jurisdiction/GetUserPermissions",
        data: { "userId": userId },
        dataType: "json",
        type: "GET",
        success: function (data) {
            setting = {
                check: {
                    enable: true
                },
                data: {
                    simpleData: {
                        enable: true
                    }
                }
            };
            zNodes = JSON.parse(data)
            $.fn.zTree.init($("#tree"), setting, zNodes);
        }
    })
}
function ModifyUserPermissons() {
    var treeObj = $.fn.zTree.getZTreeObj("tree")
    if (treeObj.getCheckedNodes(true) == null) {
        return;
    }
    nodes = treeObj.getCheckedNodes(true)
    var menuIds = ""
    for (var i = 0; i < nodes.length; i++) {
        menuIds += (nodes[i].id + ",")
    }
    menuIds = menuIds.substring(0, menuIds.length - 1);
    $.ajax({
        url: "/Jurisdiction/ModifyUsersPermission",
        data: { "menuIds": menuIds, "userId": UserID },
        dataType: "json",
        type: "POST",
        success: function (data) {
            if (data) {
                alert("修改成功！")
            }
            else {
                alert("(⊙o⊙)…出错了！")
            }
        }
    })
}