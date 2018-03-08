function EnterThisMenu(obj) {
    ClearMenu()
    obj.innerHTML += "<i class=\"icon-angle-right\"></i>"
    SetNavigation(obj.id)
}
function ClearMenu() {
    var objs = document.getElementsByName("menuName")
    for (var i = 0; i < objs.length; i++) {
        objs[i].innerHTML = objs[i].id
    }
}
function SetNavigation(nowPosition) {
    document.getElementById("nowPosition").innerHTML = nowPosition
}
$.fn.ready(function () {
    $('.default').dropkick();
});
function Colse() {
    layer.closeAll()
}
function SeeOrderDetail(orderId) {
    $.ajax({
        url: "/User/GetOrderById",
        dataType: "Json",
        type: "GET",
        data: { "orderId": orderId },
        success: function (data) {
            layer.open({
                title: '订单详情',
                shift: 4,
                shade: 0.3,
                area: ['50%', 'auto'],
                type: 1,
                content: data,
                shadeClose: true,
            });
        }
    })
}
//*********************主页************************************
function BuyerInfoHomePage() {
    $.ajax({
        url: "/User/GetUserInfoToString",
        dataType: "Json",
        type: "GET",
        success: function (data) {
            document.getElementById("buyerInfoMenuBoxRight").innerHTML = data
        },
        beforeSend: function () {
            document.getElementById("buyerInfoMenuBoxRight").innerHTML = "<i class=\"icon-spinner icon-spin buyerInfoLoading\"></i>"
        }
    })
}
//*********************收货信息********************************
function BuyerInfoReceivingAddress() {
    $.ajax({
        url: "/User/ReceivingAddress",
        dataType: "Json",
        type: "GET",
        success: function (data) {
            document.getElementById("buyerInfoMenuBoxRight").innerHTML = data
            SetReceivingAddressContent()
        },
        beforeSend: function () {
            document.getElementById("buyerInfoMenuBoxRight").innerHTML = "<i class=\"icon-spinner icon-spin buyerInfoLoading\"></i>"
        }
    })
}
function SetReceivingAddressContent() {
    $.ajax({
        url: "/User/SetReceivingAddress",
        dataType: "Json",
        type: "GET",
        success: function (data) {
            $("#receivingAddressPage").Page({
                totalPages: data,//分页总数
                liNums: 5,//分页的数字按钮数(建议取奇数)
                activeClass: 'activP', //active 类样式定义
                callBack: function (page) {
                    GoReceivingAddressPageByNum(page)
                }
            });
        }
    })
}
function GoReceivingAddressPageByNum(page) {
    $.ajax({
        url: "/User/ReceivingAddressPageGoNum",
        dataType: "Json",
        type: "GET",
        data: { "page": page },
        success: function (data) {
            document.getElementById("receivingAddressContent").innerHTML = data
        },
        beforeSend: function () {
            document.getElementById("receivingAddressContent").innerHTML = "<i class=\"icon-spinner icon-spin\"></i>"
        }
    })
}
function AddReceivingAddress() {
    ClearAddAddressInput()
    layer.open({
        title: '添加收货地址',
        shift: 4,
        shade: 0.3,
        area: ['35%', 'auto'],
        type: 1,
        content: $("#addReceivingBox"),
        shadeClose: true,
    });
}
function ClearAddAddressInput() {
    document.getElementById("newConsignee").value = ""
    document.getElementById("newReceiptAddress").value = ""
    document.getElementById("newContactInfo").value = ""
}
function SaveNewAddress() {
    var newConsignee = document.getElementById("newConsignee").value
    var newReceiptAddress = document.getElementById("newReceiptAddress").value
    var newContactInfo = document.getElementById("newContactInfo").value
    $.ajax({
        url: "/User/SaveReceivingInfo",
        dataType: "Json",
        type: "GET",
        data: { "newConsignee": newConsignee, "newReceiptAddress": newReceiptAddress, "newContactInfo": newContactInfo },
        success: function (data) {
            if (data) {
                zeroModal.success({
                    content: '添加成功!',
                    okFn: function () {
                        GoReceivingAddressPageByNum(1)
                        SetReceivingAddressContent()
                    }
                });
            }
            else {
                zeroModal.error('服务器出了点小问题(⊙o⊙)…!');
            }
        },
        beforeSend: function () {
            document.getElementById("saveAddressBut").innerHTML = "<i class=\"icon-spinner icon-spin buyerInfoLoading\"></i>"
        },
        complete: function () {
            document.getElementById("saveAddressBut").innerHTML = "添加"
        }
    })
}
function ShowEditReceivingInfo(receivingInfoId) {
    $.ajax({
        url: "/User/GetReceivingInfoById",
        dataType: "Json",
        type: "GET",
        data: { "receivingInfoId": receivingInfoId },
        success: function (data) {
            layer.open({
                title: '修改收货地址信息',
                shift: 4,
                shade: 0.3,
                area: ['50%', 'auto'],
                type: 1,
                content: data,
                shadeClose: true,
            });
        }
    })
}
function SaveAddress(receivingInfoId) {
    
    var consignee = document.getElementById("Consignee").value
    var receivingAddress = document.getElementById("ReceivingAddress").value
    var contactInfo = document.getElementById("ContactInfo").value
    var isDefault = document.getElementById("onoffswitch1").checked
    var receivingObj = {
        UserId: '00000000-0000-0000-0000-000000000000',
        Consignee: consignee,
        ReceiptAddress: receivingAddress,
        ContactInfo: contactInfo,
        IsDefault: isDefault,
        ReceivingInfoId: receivingInfoId
    }
    var receivingObjJson = JSON.stringify(receivingObj)
    $.ajax({
        url: "/User/EditReceivingInfo",
        dataType: "Json",
        type: "GET",
        data: { "receivingObjJson": receivingObjJson},
        success: function (data) {
            if (data) {
                zeroModal.success({
                    content: '修改成功!',
                    okFn: function () {
                        GoReceivingAddressPageByNum(1)
                        SetReceivingAddressContent()
                    }
                });
            }
            else {
                zeroModal.error('服务器出了点小问题(⊙o⊙)…!');
            }
        },
        beforeSend: function () {
            document.getElementById("editReceivingInfo").setAttribute("className", "icon-spinner icon-spin edit")
        },
        complete: function () {
            document.getElementById("editReceivingInfo").setAttribute("className", "icon-edit pointer edit")
        }
    })
}
function DeleteReceivingInfo(receivingInfoId) {
    $.ajax({
        url: "/User/DeleteReceivingInfo",
        dataType: "Json",
        type: "GET",
        data: { "receivingInfoId": receivingInfoId},
        success: function (data) {
            if (data) {
                zeroModal.success({
                    content: '删除成功!',
                    okFn: function () {
                        GoReceivingAddressPageByNum(1)
                        SetReceivingAddressContent()
                    }
                });
            }
            else {
                zeroModal.error('服务器出了点小问题(⊙o⊙)…!');
            }
        },
        beforeSend: function () {
            document.getElementById("deleteReceivingInfo").setAttribute("className", "icon-spinner icon-spin remove")
        },
        complete: function () {
            document.getElementById("deleteReceivingInfo").setAttribute("className", "icon-remove pointer remove")
        }
    })
}
//*********************订单页面*********************************
function BuyerInfoOrders(orderStatusId) {
    $.ajax({
        url: "/User/GetAllOrderPage",
        dataType: "Json",
        type: "GET",
        data: { "orderStatusId": orderStatusId },
        success: function (data) {
            document.getElementById("buyerInfoMenuBoxRight").innerHTML = data
            SetOrderPageContent(orderStatusId)
        },
        beforeSend: function () {
            document.getElementById("buyerInfoMenuBoxRight").innerHTML = "<i class=\"icon-spinner icon-spin buyerInfoLoading\"></i>"
        }
    })
}
function SetOrderPageContent(orderStatusId){
    $.ajax({
        url:"/User/SetOrdersPage",
        dataType:"Json",
        type: "GET",
        data:{"orderStatusId":orderStatusId},
        success:function(data){
            $("#orderPage").Page({
                totalPages: data,//分页总数
                liNums: 5,//分页的数字按钮数(建议取奇数)
                activeClass: 'activP', //active 类样式定义
                callBack: function (page) {
                    GoOrderPageByNum(page)
                }
            });
        }
    })
}
function GoOrderPageByNum(page) {
    $.ajax({
        url: "/User/OrdersPageGoNum",
        dataType: "Json",
        type: "GET",
        data: { "page": page },
        success: function (data) {
            document.getElementById("orderInfoBox").innerHTML = data
        },
        beforeSend: function () {
            document.getElementById("orderInfoBox").innerHTML = "<i class=\"icon-spinner icon-spin\"></i>"
        }
    })
}
function GoOrderPageByStatus(orderStatusId) {
    $.ajax({
        url: "/User/GetOrdersByStatusId",
        dataType: "Json",
        type: "GET",
        data: { "orderStatusId": orderStatusId },
        success: function (data) {
            document.getElementById("orderInfoBox").innerHTML = data
            SetOrderPageContent(orderStatusId)
        },
        beforeSend: function () {
            document.getElementById("orderInfoBox").innerHTML = "<i class=\"icon-spinner icon-spin\"></i>"
        }
    })
}
function SelectByStatusId() {
    $.ajax({
        url: "/User/GetOrdersByStatusId",
        dataType: "Json",
        type: "GET",
        data: { "orderStatusId": $("#ordersStatus").val() },
        success: function (data) {
            document.getElementById("orderInfoBox").innerHTML = data
            SetOrderPageContent($("#ordersStatus").val())
        },
        beforeSend: function () {
            document.getElementById("orderInfoBox").innerHTML = "<i class=\"icon-spinner icon-spin\"></i>"
        }
    })
}
function ConfirmReceipt(orderId) {
    $.ajax({
        url: "/User/ConfirmReceipt",
        dataType: "Json",
        type: "GET",
        data: { "orderId": orderId },
        success: function (data) {
            if (data) {
                zeroModal.success({
                    content: '收货成功,感谢您的配合!',
                    okFn: GoOrderPageByNum(1)
                });
            }
            else {
                zeroModal.error('服务器出了点小问题(⊙o⊙)…!');
            }
        },
        beforeSend: function () {
            document.getElementById(orderId + "+C").innerHTML = "<i class=\"icon-spinner icon-spin\"></i>"
        },
        complete: function () {
            document.getElementById(orderId + "+C").innerHTML = "确认收货"
        }
    })
}
function ApplyRefund(orderId) {
    $.ajax({
        url: "/User/ApplyRefund",
        dataType: "Json",
        type: "GET",
        data: { "orderId": orderId },
        success: function (data) {
            if (data) {
                zeroModal.success({
                    content: '已提交退款申请，请静待系统回复。',
                    okFn: GoOrderPageByNum(1)
                });
            }
            else {
                zeroModal.error('服务器出了点小问题(⊙o⊙)…!');
            }
        },
        beforeSend: function () {
            document.getElementById(orderId + "+A").innerHTML = "<i class=\"icon-spinner icon-spin\"></i>"
        },
        complete: function () {
            document.getElementById(orderId + "+A").innerHTML = "申请退款"
        }
    })
}
//********************个人信息**********************************
function BuyerInfoPage() {
    $.ajax({
        url: "/User/UserInfoPage",
        dataType: "Json",
        type: "GET",
        success: function (data) {
            document.getElementById("buyerInfoMenuBoxRight").innerHTML = data
        },
        beforeSend: function () {
            document.getElementById("buyerInfoMenuBoxRight").innerHTML = "<i class=\"icon-spinner icon-spin buyerInfoLoading\"></i>"
        }
    })
}
function ChangeBuyerInfo() {
    var buyerName = document.getElementById("BuyerName").value
    var buyerContactInfo = document.getElementById("BuyerContactInfo").value
    $.ajax({
        url: "/User/ChangeBuyerInfo",
        dataType: "Json",
        type: "GET",
        data: { "buyerName": buyerName, "buyerContactInfo": buyerContactInfo },
        success: function (data) {
            HadLogin(JSON.parse(data))
            zeroModal.success({
                content: '信息已成功修改!'
            });
        },
        beforeSend: function () {
            document.getElementById("saveChange").innerHTML = "<i class=\"icon-spinner icon-spin\"></i>"
            document.getElementById("saveChange").disabled = true
        },
        complete: function () {
            document.getElementById("saveChange").innerHTML = "保存修改"
            document.getElementById("saveChange").disabled = false
        }
    })
}
//********************修改密码**********************************
var newPassword
var reNewPassword
function BuyerInfoChangePassword() {
    $.ajax({
        url: "/User/ChangePasswordPage",
        dataType: "Json",
        type: "GET",
        success: function (data) {
            document.getElementById("buyerInfoMenuBoxRight").innerHTML = data
        },
        beforeSend: function () {
            document.getElementById("buyerInfoMenuBoxRight").innerHTML = "<i class=\"icon-spinner icon-spin buyerInfoLoading\"></i>"
        }
    })
}
function CheckPassword() {
    var str = document.getElementById("newPassword").value;
    var regexp = new RegExp('^(?![\d]+$)(?![a-zA-Z]+$)(?![^\da-zA-Z]+$).{6,20}$')
    if (regexp.test(str)) {
        document.getElementById("erroTipPassWord").style.display = "none"
        newPassword = true
        CheckButton()
    }
    else {
        document.getElementById("erroTipPassWord").style.display = "inline-block"
        newPassword = false
        CheckButton()
    }
}
function CheckRePassword() {
    var password = document.getElementById("newPassword").value;
    var rePassword = document.getElementById("reNewPassword").value;
    if (password != rePassword) {
        document.getElementById("erroTipRePassWord").style.display = "inline-block"
        reNewPassword = false
        CheckButton()
    }
    else {
        document.getElementById("erroTipRePassWord").style.display = "none"
        reNewPassword = true
        CheckButton()
    }
}
function CheckButton() {
    if (newPassword == true && reNewPassword == true) {
        document.getElementById("saveChange").disabled = false;
    }
    else {
        document.getElementById("saveChange").disabled = true;
    }
}
function ChangePassword() {
    var oldPassword = document.getElementById("oldPassword").value;
    var newPassword = document.getElementById("newPassword").value;
    $.ajax({
        url: "/User/ChangePassword",
        dataType: "Json",
        type: "GET",
        data: { "newPassword": newPassword, "oldPassword": oldPassword },
        success: function (data) {
            if (data) {
                zeroModal.success({
                    content: '密码已成功修改!'
                });
                window.location.href = "/ShowGoods/Index"
            }
            else {
                zeroModal.error('修改失败,可能是原密码有问题!');
            }
        },
        beforeSend: function () {
            document.getElementById("saveChange").innerHTML = "<i class=\"icon-spinner icon-spin\"></i>"
            document.getElementById("saveChange").disabled = true
        },
        complete: function () {
            document.getElementById("saveChange").innerHTML = "保存修改"
            document.getElementById("saveChange").disabled = false
        }
    })
}