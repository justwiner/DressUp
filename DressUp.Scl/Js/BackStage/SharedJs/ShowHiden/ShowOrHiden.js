function ShowOrHiden(checkbox)
{
    if ($(checkbox).attr("checked") == "checked")
    {
        $("#" + $(checkbox).val()).hide("1000");
        $(checkbox).attr("checked",false)
    }
    else
    {
        $("#" + $(checkbox).val()).show("500");
        $(checkbox).attr("checked", true)
    }
}
function Initialization(str)
{
    var obj = $("p[name='set']")
    for (i in obj) {
        if (i == 'length') {
            return;
        }
        else {
            if ($(obj[i]).attr('id') == str) {
                $(obj[i]).css('display', 'inline-block');
                $(obj[i]).attr('name', 'hadSet')
                s = $(obj[i]).find("input").attr("value")
                $("#" + s)[0].style.display = 'block'
                return;
            }
            else {
                $(obj[i]).css('display', 'none');
                s = $(obj[i]).find("input").attr("value")
                $("#" + s)[0].style.display = 'none'
            }
        }
    }
}
$(function () {
    $.ajax({
        url: "/HomePage/GetUserRoles",
        type: 'POST',
        data: {},
        dataType: 'json',
        error: function (xhr) {
            alert('Error: ' + xhr.statusText);
        },
        success: function (result) {
            if (result.length == 0) {
                var obj = $("p[name='set']")
                for (i in obj) {
                    if (i == 'length') {
                        return;
                    }
                    $(obj[i]).css('display', 'none');
                    s = $(obj[i]).find("input").attr("value")
                    $("#" + s)[0].style.display = 'none'
                }
            }
            else
            {
                for (var i = 0 ; i < result.length ; i++) {
                    Initialization(result[i])
                }
            }
        },
    });
});