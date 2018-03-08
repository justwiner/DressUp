window.onload = function () {
    $.ajax({
        url: "/User/IfBuyerLogin",
        dataType: "json",
        type: "GET",
        data: {},
        success: function (data) {
            if (data == false) {
                NoLogin()
            }
            else {
                var obj = JSON.parse(data);
                HadLogin(obj);
            }
        }
    })
}
function BuyerLoginBox() {
    document.getElementById("shadowLayer").style.display = 'block'
    document.getElementById("loginBox").style.display = 'block'
}
function HideLoginBox() {
    document.getElementById("buyerAccount").value = ""
    document.getElementById("buyerPassword").value = ""
    document.getElementById("loginError").style.display = 'none'
    document.getElementById("shadowLayer").style.display = 'none'
    document.getElementById("loginBox").style.display = 'none'
}
function BuyerLogin() {
    var account = document.getElementById("buyerAccount").value
    if (account == "") {
        alert("请输入正确的信息")
        return;
    }
    var password = document.getElementById("buyerPassword").value
    if (password == "") {
        alert("请输入正确的信息")
        return;
    }
    $.ajax({
        url: "/User/BuyerLogin",
        dataType: "json",
        type: "GET",
        data: {"account" : account,"password": password},
        success: function (data) {
            if (data == false) {
                document.getElementById("loginError").style.display = "block";
                return;
            }
            var obj = JSON.parse(data)
            HadLogin(obj)
            HideLoginBox();
        },
        beforeSend: function () {
            document.getElementById("loginButton").innerHTML = "<i class=\"icon-spinner icon-spin\"></i>"
        },
        complete: function () {
            document.getElementById("loginButton").innerHTML = "登录"
        },
    })
}
function BuyerLogOut() {
    $.ajax({
        url: "/User/BuyerLogOut",
        dataType: "json",
        type: "GET",
        data: {},
        success: function (data) {
            if (data) {
                window.location.href = "/ShowGoods/Index"
            }
            else {
                alert("(⊙o⊙)…出错了！")
            }
        }
    })
}
function NoLogin() {
    document.getElementById("buyerName").innerHTML = "【登录】"
    document.getElementById("buyerName").setAttribute("onclick", "BuyerLoginBox()")
    document.getElementById("buyerRegister").style.display = 'inline-block';
    document.getElementById("head_Login_button_myShopCar").innerHTML = "0"
    document.getElementById("sidebar_shopCartsNum").innerHTML = "0";
    document.getElementById("buyerOutLogin").style.display = "none";
}
function HadLogin(obj) {
    document.getElementById("buyerRegister").style.display = 'none';
    document.getElementById("buyerName").innerHTML = "您好，" + obj.BuyerName;
    document.getElementById("buyerName").setAttribute("onclick", "BuyerInfoPage()")
    document.getElementById("head_Login_button_myShopCar").innerHTML = obj.ShoppingCartsCount;
    document.getElementById("sidebar_shopCartsNum").innerHTML = obj.ShoppingCartsCount;
    document.getElementById("buyerOutLogin").style.display = "inline-block";
}
function BuyerInfoPage() {
    window.location.href = "/ShowGoods/BuyerInfoPage"
}
function BuyerIfLogin() {
    $.ajax({
        url: "/User/IfBuyerLogin",
        dataType: "json",
        type: "GET",
        success: function (data) {
            if (data == false) {
                return false;
            }
            else {
                return true;
            }
        }
    })
}