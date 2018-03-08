function LoginkeyUp(e) {
    var currKey = 0, e = e || event;
    currKey = e.keyCode || e.which || e.charCode;
    if(currKey == 13)
        LogJudge()
}
document.onkeyup = LoginkeyUp;

function LogJudge() {
    document.getElementById("logButton").disabled = true;
    userAccount = document.getElementById("account").value;
    userPassword = document.getElementById("password").value;
    $.ajax({
        url: "/HomePage/LogJudge",
        type: 'POST',
        data: { "userAccount": userAccount, "userPassword": userPassword },
        dataType: 'json',
        error: function (xhr) {
            alert('Error: ' + xhr.statusText);
        },
        success: function (result) {
            if (result == false) {
                document.getElementById("logErro").style.display = 'block';
            }
            else
            {
                window.location.href = "/HomePage/BackHome";
            }
        },
    });
    document.getElementById("logButton").disabled = false;
}