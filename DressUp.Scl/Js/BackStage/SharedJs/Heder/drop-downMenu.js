$(function () {
    $(".main-header-user").mouseover(function () {
        $("#User-Menu").slideDown(100);
    })
    $(".main-header-user").mouseleave(function () {
        $("#User-Menu").mouseleave(function () {
            $("#User-Menu").slideUp(100);
        })
    })
})