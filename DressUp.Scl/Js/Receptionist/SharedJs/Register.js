var passwordResult
var rePasswordResult
var codeResult
var nameResult
var nicknameResult

var inp = document.getElementById('inputCode');
var code = document.getElementById('code');

var inp2 = document.getElementById('inputCode2');
var code2 = document.getElementById('code2');

var c = new KinerCode({
    len: 4,//需要产生的验证码长度
    //        chars: ["1+2","3+15","6*8","8/4","22-15"],//问题模式:指定产生验证码的词典，若不给或数组长度为0则试用默认字典
    chars: [
        1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
    ],//经典模式:指定产生验证码的词典，若不给或数组长度为0则试用默认字典
    question: false,//若给定词典为算数题，则此项必须选择true,程序将自动计算出结果进行校验【若选择此项，则可不配置len属性】,若选择经典模式，必须选择false
    copy: false,//是否允许复制产生的验证码
    bgColor: "",//背景颜色[与背景图任选其一设置]
    bgImg: "bg.jpg",//若选择背景图片，则背景颜色失效
    randomBg: false,//若选true则采用随机背景颜色，此时设置的bgImg和bgColor将失效
    inputArea: inp,//输入验证码的input对象绑定【 HTMLInputElement 】
    codeArea: code,//验证码放置的区域【HTMLDivElement 】
    click2refresh: true,//是否点击验证码刷新验证码
    false2refresh: false,//在填错验证码后是否刷新验证码
    validateEven: "blur",//触发验证的方法名，如click，blur等
    validateFn: function (result, code) {//验证回调函数
        if (result) {
            document.getElementById("erroTipCode").style.display = "none"
            codeResult = true
            CheckButton()
        } else {
            document.getElementById("erroTipCode").style.display = "block"
            codeResult = false
            CheckButton()
        }
    }
});
function CheckPassword() {
    var password = document.getElementById("password").value;
    var rePassword = document.getElementById("rePassword").value;
    var regexp = new RegExp('^(?![\d]+$)(?![a-zA-Z]+$)(?![^\da-zA-Z]+$).{6,20}$')
    if (regexp.test(password)) {
        if (rePassword != "" && password == rePassword) {
            document.getElementById("erroTipPassWord").style.display = "none"
            passwordResult = true
            CheckButton()
        }
        else if (rePassword == "") {
            document.getElementById("erroTipPassWord").style.display = "none"
            passwordResult = true
            CheckButton()
        }
        else {
            document.getElementById("erroTipRePassWord").style.display = "block"
            rePasswordResult = false
            CheckButton()
        }
    }
    else {
        document.getElementById("erroTipPassWord").style.display = "block"
        passwordResult = false
        CheckButton()
    }
}
function CheckRePassword() {
    var password = document.getElementById("password").value;
    var rePassword = document.getElementById("rePassword").value;
    if (password == rePassword && password != null) {
        document.getElementById("erroTipRePassWord").style.display = "none"
        rePasswordResult = true
        CheckButton()
    }
    else {
        document.getElementById("erroTipRePassWord").style.display = "block"
        rePasswordResult = false
        CheckButton()
    }
}
function CheckButton() {
    if (passwordResult == true && rePasswordResult == true && codeResult == true && nameResult == true && nicknameResult == true) {
        document.getElementById("submitBut").disabled = false
        document.getElementById("submitBut").style.backgroundColor = "#e22"
    }
    else {
        document.getElementById("submitBut").disabled = true
        document.getElementById("submitBut").style.backgroundColor = "gray"
    }
}
function CheckNickname() {
    var nickname = document.getElementById("buyerNickname").value
    if (nickname == "") {
        nicknameResult = false
        CheckButton()
    }
    else {
        nicknameResult = true
        CheckButton()
    }
}
function NameIfExist() {
    var name = document.getElementById("buyerName").value
    if (name == "") {
        document.getElementById("erroTipName").style.display = "block"
        nameResult = false
        CheckButton()
        return;
    }
    else {
        $.ajax({
            url: "/User/NameIfExist",
            dataType: "json",
            type: "GET",
            data: {"name":name},
            success: function (data) {
                if (data) {
                    document.getElementById("erroTipName").style.display = "block"
                    document.getElementById("allowTipName").style.display = "none"
                    nameResult = false
                    CheckButton()
                }
                else {
                    document.getElementById("erroTipName").style.display = "none"
                    document.getElementById("allowTipName").style.display = "block"
                    nameResult = true
                    CheckButton()
                }
            },
            beforeSend: function () {
                document.getElementById("nameIfExistTip").innerHTML = '<i class=\"icon-spinner icon-spin\"></i>';
                document.getElementById("nameIfExistTip").setAttribute("onclick", "");
                document.getElementById("nameIfExistTip").style.margin = '0 0 0 25%'

            },
            complete: function () {
                document.getElementById("nameIfExistTip").innerHTML = '检测此用户名';
                document.getElementById("nameIfExistTip").setAttribute("onclick", "NameIfExist()");
                document.getElementById("nameIfExistTip").style.margin = '0 0 0 12%'
            }
        })
    }
}
function ReflushButton() {
    document.getElementById("allowTipName").style.display = "none"
    nameResult = false;
    CheckButton()
}
function RegisterNewBuer() {
    var nickname = document.getElementById("buyerNickname").value
    var account = document.getElementById("buyerName").value
    var password = document.getElementById("password").value;
    $.ajax({
        url: "/User/RegisterNewBuer",
        dataType: "json",
        type: "POST",
        data: {"nickname":nickname,"account":account,"password":password},
        success: function (data) {
            if (data) {
                zeroModal.success({
                    content: '注册成功!',
                    okFn: function () {
                        window.location.href = "/ShowGoods/Index"
                    }
                });
            }
            else {
                zeroModal.error('注册失败!');
            }
        },
        beforeSend: function () {
            document.getElementById("submitBut").innerHTML = '<i class=\"icon-spinner icon-spin\"></i>';

        },
        complete: function () {
            document.getElementById("submitBut").innerHTML = '立即注册';
        }
    })
}
function GotoHomePage() {
    window.location.href = "/ShowGoods/Index"
}