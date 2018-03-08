var num = 0
$(function () {
    $(".prev").addClass("fa fa-angle-left button_left");
    $(".next").addClass("fa fa-angle-right button_right");
    $(".rslidesBox").mouseover(function () {
        if (num == 0) {
            $(".prev").css("display", "block")
            $(".prev").animate({ left: "-=20%" }, 500);
            $(".next").css("display", "block")
            $(".next").animate({ right: "-=20%" }, 500);
            num++;
        }
    })
    $(".prev").hover(function () {
        $(".prev").css("display", "block")
        $(".next").css("display", "block")
    })
    $(".next").hover(function () {
        $(".prev").css("display", "block")
        $(".next").css("display", "block")
    })
    $(".rslidesBox").mouseleave(function () {
        $(".prev").animate({ left: "+=20%" }, 500);
        $(".next").animate({ right: "+=20%" }, 500);
        $(".prev").css("display", "none")
        $(".next").css("display", "none")
        num = 0;
    })
});
$(".rslides").responsiveSlides({
    auto: true,             // Boolean: 设置是否自动播放, true or false
    speed: 500,            // Integer: 动画持续时间，单位毫秒
    timeout: 4000,          // Integer: 图片之间切换的时间，单位毫秒
    pager: false,           // Boolean: 是否显示页码, true or false
    nav: true,             // Boolean: 是否显示左右导航箭头（即上翻下翻）, true or false
    random: false,          // Boolean: 随机幻灯片顺序, true or false
    pause: true,           // Boolean: 鼠标悬停到幻灯上则暂停, true or false
    pauseControls: true,    // Boolean: 悬停在控制板上则暂停, true or false
    prevText: "",   // String: 往前翻按钮的显示文本
    nextText: "",       // String: 往后翻按钮的显示文本
    maxwidth: "",           // Integer: 幻灯的最大宽度
    navContainer: "",       // Selector: Where controls should be appended to, default is after the 'ul'
    manualControls: "",     // Selector: 声明自定义分页导航
    namespace: "rslides",   // String: 修改默认的容器名称
    before: function () {},   // Function: 回调之前的参数
    after: function () {}     // Function: 回调之后的参数
});