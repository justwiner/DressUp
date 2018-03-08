function ChangeStatus() {
    var status = document.getElementById("useNewAddress").checked;
    if (status) {
        document.getElementById("useNewAddress").checked = false;
        $("#inputNewAddressBox").hide(500);
        ClearInputAddressBox()
    }
    else {
        document.getElementById("useNewAddress").checked = true;
        $("#inputNewAddressBox").show(500);
    }
}
function ChangeCheckBox(obj) {
    var checkBoxs = document.getElementsByName("checkBox")
    obj.checked = true
    for (var i = 0; i < checkBoxs.length; i++) {
        if (checkBoxs[i] != obj) {
            checkBoxs[i].checked = false
        }
    }
}
function ShowOtherAddress() {
    layer.open({
        title: '选择地址',
        shift: 4,
        shade: 0.3,
        area: ['30%', 'auto'],
        type: 1,
        content: $("#buyerAddresses"),
        shadeClose: true,
    });
    $.ajax({
        url: "/User/GetReceivingInfo",
        dataType: "json",
        type: "GET",
        success: function (data) {
            if (data == false) {
                document.getElementById("buyerAddressesBox").innerHTML = "<i class=\"icon-warning-sign\"></i>&nbsp;&nbsp;&nbsp;你还没有收货地址信息，请先添加新地址再进行购买。"
            }
            else {
                document.getElementById("buyerAddressesBox").innerHTML = data
            }
        },
        beforeSend: function () {
            document.getElementById("buyerAddressesBox").innerHTML = "<i class=\"icon-spinner icon-spin\"></i>"
        }
    })
    
}
function ChangeOtherAddress() {
    var checkBoxs = document.getElementsByName("checkBox")
    for (var i = 0; i < checkBoxs.length; i++) {
        if (checkBoxs[i].checked == true) {
            $.ajax({
                url: "/User/GetReceivingInfoByIdInOrderPage",
                dataType: "json",
                type: "POST",
                data: { "receivingInfoId": checkBoxs[i].id},
                success: function (data) {
                    var obj = JSON.parse(data);
                    SetReceivingInfo(obj.ReceivingInfoId, obj.Consignee, obj.ReceiptAddress, obj.ContactInfo)
                },
                beforeSend: function () {
                    document.getElementById("changeReceivingBut").innerHTML = "<i class=\"icon-spinner icon-spin\"></i>"
                },
                complete: function () {
                    document.getElementById("changeReceivingBut").innerHTML = "确定"
                }
            })
        }
    }
}
function Close() {
    layer.closeAll()
}
function CloseInputAddressBox() {
    document.getElementById("useNewAddress").checked = false;
    $("#inputNewAddressBox").hide(500);
    ClearInputAddressBox()
}
function ClearInputAddressBox() {
    document.getElementById("newConsignee").value = ""
    document.getElementById("newReceiptAddress").value = ""
    document.getElementById("newContactInfo").value = ""
}
function CheckSaveButton() {
    var newConsignee = document.getElementById("newConsignee").value
    var newReceiptAddress = document.getElementById("newReceiptAddress").value
    var newContactInfo = document.getElementById("newContactInfo").value
    if (newConsignee != "" && newReceiptAddress != "" && newContactInfo != "") {
        document.getElementById("saveAddressBut").disabled = false;
        document.getElementById("saveAddressBut").style.backgroundColor = "#D34A2D";
        document.getElementById("saveAddressBut").style.borderColor = "#D34A2D"
    }
    else {
        document.getElementById("saveAddressBut").disabled = true;
        document.getElementById("saveAddressBut").style.backgroundColor = "grey";
        document.getElementById("saveAddressBut").style.borderColor = "grey"
    }
}
function SaveNewAddress() {
    var newConsignee = document.getElementById("newConsignee").value
    var newReceiptAddress = document.getElementById("newReceiptAddress").value
    var newContactInfo = document.getElementById("newContactInfo").value
    $.ajax({
        url: "/User/SaveReceivingInfo",
        dataType: "json",
        type: "POST",
        data: { "newConsignee": newConsignee, "newReceiptAddress": newReceiptAddress, "newContactInfo": newContactInfo },
        success: function (data) {
            var obj = JSON.parse(data);
            SetReceivingInfo(obj.ReceivingInfoId, obj.Consignee, obj.ReceiptAddress, obj.ContactInfo)
            zeroModal.success('添加成功，系统将使用此地址作为你该次购物的收货地址。');
        },
        beforeSend: function () {
            document.getElementById("saveAddressBut").innerHTML = "<i class=\"icon-spinner icon-spin\"></i>"
        },
        complete: function () {
            document.getElementById("saveAddressBut").innerHTML = "保存收货人信息"
        }
    })
}
function SetReceivingInfo(receivingInfoId, consignee, receiptAddress, contactInfo) {
    document.getElementById("receivingInfoId").value = receivingInfoId
    document.getElementById("consignee").innerHTML = consignee
    document.getElementById("receiptAddress").innerHTML = receiptAddress
    document.getElementById("contactInfo").innerHTML = contactInfo
}

function Payment() {
    var receivingInfoId = document.getElementById("receivingInfoId").value
    if (receivingInfoId == -1) {
        zeroModal.error('请选择收货信息,默认使用默认地址!');
    }
    else {
        var minimalistOrders = CreatMiniOrders()
        var minimalistOrdersString = JSON.stringify(minimalistOrders)
        $.ajax({
            url: "/User/CreatOrders",
            dataType: "json",
            type: "POST",
            data: { "minimalistOrders": minimalistOrdersString },
            success: function (data) {
                if (data) {
                    zeroModal.success({
                        content: '订单已成功生成，点击确定查看订单!',
                        okFn: function () {
                            window.location.href = "/ShowGoods/BuyerInfoPage"
                        }
                    });
                }
                else {
                    zeroModal.error('(⊙o⊙)…系统出了点小问题，请求失败，请进入个人中心，查看订单是否生成。');
                }
            },
            beforeSend: function () {
                document.getElementById("payment").innerHTML = "<i class=\"icon-spinner icon-spin\"></i>"
            },
            complete: function () {
                document.getElementById("payment").innerHTML = "支付订单>"
            }
        })
    }
}
function CreatMiniOrders() {
    var goodsIds = document.getElementsByName("goodsId");
    var goodsNum = document.getElementsByName("goodsNum");
    var shopCartsId = document.getElementsByName("shopCartsId");
    var minimalistOrders = new Array()
    var receivingInfoId = document.getElementById("receivingInfoId").value;
    for (var i = 0 ; i < goodsIds.length ; i++) {
        minimalistOrders[i] = { GoodsId: goodsIds[i].id, GoodsNum: goodsNum[i].id, ReceivingInfoId: receivingInfoId, ShopCartsId: shopCartsId[i].id };
    }
    return minimalistOrders;
}