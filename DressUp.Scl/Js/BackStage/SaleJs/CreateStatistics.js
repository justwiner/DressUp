var chart;
$(function () {
    chart = new Highcharts.Chart({
        chart: {
            renderTo: 'salesStatistics-box', //图表放置的容器，DIV 
            defaultSeriesType: 'line', //图表类型line(折线图), 
            zoomType: 'x'   //x轴方向可以缩放 
        },
        credits: {
            enabled: false   //右下角不显示LOGO 
        },
        title: {
            text: '月销量统计图' //图表标题 
        },
        subtitle: {
            text: '本月'  //副标题 
        },
        xAxis: {  //x轴 
            categories: ['1-5日', '6-10日', '11-15日', '16-20日', '21-25日', '26-30日'], //x轴标签名称 
            gridLineWidth: 1, //设置网格宽度为1 
            lineWidth: 2,  //基线宽度 
            labels: { y: 26 }  //x轴标签位置：距X轴下方26像素 
        },
        yAxis: {  //y轴 
            title: { text: '数量(万件)' }, //标题 
            lineWidth: 2 //基线宽度 
        },
        plotOptions: { //设置数据点 
            line: {
                dataLabels: {
                    enabled: true  //在数据点上显示对应的数据值 
                },
                enableMouseTracking: true //取消鼠标滑向触发提示框 
            }
        },
        legend: {  //图例 
            layout: 'horizontal',  //图例显示的样式：水平（horizontal）/垂直（vertical） 
            backgroundColor: '#ffc', //图例背景色 
            align: 'left',  //图例水平对齐方式 
            verticalAlign: 'top',  //图例垂直对齐方式 
            x: 100,  //相对X位移 
            y: 70,   //相对Y位移 
            floating: true, //设置可浮动 
            shadow: true  //设置阴影 
        },
        exporting: {
            enabled: true  //设置导出按钮不可用 
        },
        series: [{  //数据列 
            name: '12月',
            data: [9.1, 8.7,25.8, 14.0, 12.8, 11.4]
        },
        {
            name: '11月',
            data: [13.1, 12.7, 33.8, 25.0, 17.8, 12.4]
        }
        ]
    });
});