function FindNewMessage(role) {
    if (role == "销售") {
        window.location.href = "/Sale/NewOrder"
    }
    if (role == "仓库") {
        window.location.href = "/StoreManagement/StoreNewProcessMessage"
    }
}
window.onload = function () {
    $.ajax({
        url: "/HomePage/GetNewMessageNum",
        type: "GET",
        dataType: "json",
        success: function (data) {
            document.getElementById("newMessageNum").innerHTML = data
        },
    })
}