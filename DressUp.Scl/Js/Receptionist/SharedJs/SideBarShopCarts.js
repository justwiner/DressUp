function ChangeSimpleShopCarts() {
    $("#sidebarShopCarts").fadeToggle(500);
    if (document.getElementById("sidebarShopCarts").style.display == "block") {
        $.ajax({
            url: "/ShowGoods/GetShoppingCartsList",
            dataType: "json",
            type: "Get",
            success: function (data) {
                document.getElementById("sidebarShopCarts_Center").innerHTML = data;
            },
            beforeSend: function () {
                document.getElementById("sidebarShopCarts_Center").innerHTML = "<i class=\"icon-spinner icon-spin loading\"></i>"
            }
        });
    }
}
function ChooseAll() {
    var totalNum = 0
    var totalMoney = 0.00
    if (document.getElementsByName("shopCartsNum") == null) {
        return;
    }
    var nums = document.getElementsByName("shopCartsNum")
    var checkBoxs = document.getElementsByName("checkBox")
    var money = document.getElementsByName("goodsTotalMoney")
    if (document.getElementById("chooseAll").checked == true) {
        for (var i = 0 ; i < checkBoxs.length ; i++) {
            checkBoxs[i].checked = true
            document.getElementById("_" + checkBoxs[i].id).value = "true"
        }
        for (var i = 0 ; i < nums.length ; i++) {
            totalNum += Number(nums[i].innerHTML)
        }
        for (var i = 0 ; i < money.length ; i++) {
            totalMoney += Number(money[i].innerHTML)
        }
        document.getElementById("chooseNum").innerHTML = totalNum
        document.getElementById("totalMoney").innerHTML = totalMoney.toFixed(2)
    }
    else {
        for (var i = 0 ; i < checkBoxs.length ; i++) {
            checkBoxs[i].checked = false
            document.getElementById("_" + checkBoxs[i].id).value = "false"
        }
        document.getElementById("chooseNum").innerHTML = 0
        document.getElementById("totalMoney").innerHTML = 0.00
    }
}
function ChooseOne(obj,num,price) {
    if (obj.checked == false) {
        document.getElementById("chooseNum").innerHTML = (Number(document.getElementById("chooseNum").innerHTML) - num);
        document.getElementById("totalMoney").innerHTML = (Number(document.getElementById("totalMoney").innerHTML) - price * num).toFixed(2)
        document.getElementById("_" + obj.id).value = "false"

    }
    else {
        document.getElementById("chooseNum").innerHTML = (Number(document.getElementById("chooseNum").innerHTML) + num);
        document.getElementById("totalMoney").innerHTML = (Number(document.getElementById("totalMoney").innerHTML) + price * num).toFixed(2)
        document.getElementById("_" + obj.id).value = "true"
    }
}
function DeleteShopCartsGoods(shoppingCartsId) {
    $.ajax({
        url: "/User/DeleteShopCartsGoods",
        dataType: "json",
        type: "Get",
        data: { "shoppingCartsId": shoppingCartsId },
        success: function (data) {
            document.getElementById("sidebarShopCarts_Center").innerHTML = data;
            document.getElementById("sidebar_shopCartsNum").innerHTML--
            document.getElementById("head_Login_button_myShopCar").innerHTML--
        },
        beforeSend: function () {
            document.getElementById("sidebarShopCarts_Center").innerHTML = "<i class=\"icon-spinner icon-spin loading\"></i>"
        }
    });
}
function CheckBuyerAndShopCarts() {
    if (BuyerIfLogin() == false) {
        BuyerLoginBox()
        return false;
    }
    else {
        var checkBoxs = document.getElementsByName("checkBox")
        for (var i = 0 ; i < checkBoxs.length ; i++) {
            if (checkBoxs[i].checked == true) {
                return true;
            }
        }
        zeroModal.error('请选择要购买的商品!');
        return false;
    }
}
function BuyerInfo() {
    if (document.getElementById("buyerRegister").style.display != "none") {
        BuyerLoginBox()
        return false;
    }
    else {
        window.location.href = "/ShowGoods/BuyerInfoPage"
    }
}