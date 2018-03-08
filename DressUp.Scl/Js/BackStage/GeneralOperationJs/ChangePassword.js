function ChangePassword() {
    var oldPassword = document.getElementById("oldPassword").value
    var newPassword = document.getElementById("newPassword").value
    $.ajax({
        url: "/GeneralOperation/CheckPassword",
        type: 'POST',
        data: { "oldPassword": oldPassword, "newPassword": newPassword },
        dataType: 'json',
        error: function (xhr) {
            alert('Error: ' + xhr.statusText);
        },
        success: function (result) {
            if (result == false) {
                alert("你输入的密码有误。")
                document.getElementById("oldPassword").value == '';
            }
            else {
                alert("成功修改密码，点击确认重新登录。")
                window.location.href = "/HomePage/BackLogPage";
            }
        },
    });
}
function CheckPassword() {
    var password = document.getElementById("password").value
    if (password.toString().length < 6) {
        document.getElementById("erro_2").style.display = "inline-block"
    }
    else {
        document.getElementById("erro_2").style.display = "none"
    }
    var reg = /^[\u4e00-\u9fa5]+$/;
    if (!reg.test(password)) {
        document.getElementById("erro_1").style.display = "none"
    }
    else {
        document.getElementById("erro_1").style.display = "inline-block"
    }
    checkButton();
}
function CheckBothPassword() {
    var password = document.getElementById("password").value
    var newPassword = document.getElementById("newPassword").value
    if (newPassword == '') {
        document.getElementById("erro_5").style.display = "inline-block"
    }
    else {
        document.getElementById("erro_5").style.display = "none"
    }
    if (password == newPassword) {
        document.getElementById("erro_3").style.display = "none"
    }
    else {
        document.getElementById("erro_3").style.display = "inline-block"
    }
    checkButton();
}
function CheckOldPassword() {
    if (document.getElementById("oldPassword").value != "") {
        document.getElementById("erro_4").style.display = "none"
    }
    else {
        document.getElementById("erro_4").style.display = "inline-block"
    }
}
function checkButton() {
    var but = document.getElementById("changeSubmit")
    if (document.getElementById("erro_3").style.display == "none" &&
        document.getElementById("erro_2").style.display == "none" &&
        document.getElementById("erro_1").style.display == "none" &&
        document.getElementById("oldPassword").value != "" &&
        document.getElementById("newPassword").value != "") {
        but.disabled = false
    }
    else {
        but.disabled = true
    }
}