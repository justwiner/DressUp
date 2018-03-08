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
addLoadEvent(GoPage(1, 'Jurisdiction', 'GoPage'))
addLoadEvent(ButtonJudge())
function ShowMyTable(pageObj) {
    if (document.getElementById("table") == null)
        return;
    var innerHtml = "<tr>" +
                        "<th>角色ID</th>" +
                        "<th>角色名</th>" +
                        "<th>角色码</th>" +
                        "<th>是否默认</th>" +
                        "<th>创建时间</th>" +
                        "<th>操作</th>" +
                    "</tr>"
    if (pageObj.length != 0) {
        for (var i = 0 ; i < pageObj.length ; i++) {
            innerHtml +=
                ("<tr>" +
                    "<td title=\"" + pageObj[i].RoleId + "\">" + pageObj[i].RoleId + "</td>" +
                    "<td>" + pageObj[i].Name + "</td>" +
                    "<td>" + pageObj[i].Code + "</td>" +
                    "<td>" + pageObj[i].IsDefault + "</td>" +
                    "<td>" + pageObj[i].CreationTime + "</td>" +
                    "<td class=\"modify\" data-remodal-target=\"modal\" onclick=\"GetRolePermissions(\'" + pageObj[i].RoleId + "\')\">修改</td>" +
                    "</tr>"
                );
        }
    }
    document.getElementById("table").innerHTML = innerHtml;
}
var zNodes
var setting
var tree
var roleID
function GetRolePermissions(roleId) {
    roleID = roleId
    $.ajax({
        url: "/Jurisdiction/GetRolePermissions",
        data: {"roleId":roleId},
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
            $.fn.zTree.init($("#treeDemo"), setting, zNodes);
        }
    })
}
function IfCanModifyRoles() {
    $.ajax({
        url: "/Jurisdiction/IfCanModifyRoles",
        data: { "roleId": roleID },
        dataType: "json",
        type: "GET",
        success: function (data) {
            if (data) {
                alert("此角色不允许修改它的信息")
                return;
            }
            else {
                ModifyRoleMenu()
            }
        }
    })
}
function ModifyRoleMenu() {
    var treeObj = $.fn.zTree.getZTreeObj("treeDemo")
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
        url: "/Jurisdiction/ModifyRoles",
        data: { "menuIds": menuIds, "roleID": roleID },
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
function AddRoleCheckInput() {
    var regx = /^[\u4E00-\u9FA5A-Za-z0-9]+$/
    var roleName = document.getElementById("roleName").value;
    if (regx.test(roleName)) {
        $.ajax({
            url: "/Jurisdiction/AddRole",
            data: { "roleName": roleName},
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data) {
                    var nowPage = document.getElementById("nowPage").innerHTML;
                    GoPage(nowPage, 'Jurisdiction', 'GoPage')
                }
                else {
                    alert("(⊙o⊙)…出错了！")
                }
            }
        })
    }
    else {
        alert("输入格式有误！")
    }
    document.getElementById("roleName").value = "";
}