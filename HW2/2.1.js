
option = {
    backgroundColor: '#0f374f',
    tooltip: {},
    legend: {
        data: ['all'],
        textStyle: {color: '#ddd'}
    },
    xAxis: [{
        data: ['艾斯奥特曼', '奥特之父', '泰罗奥特曼', '雷欧奥特曼'],
        axisTick: {show: false},
        axisLine: {show: false},
        axisLabel: {
            margin: 20,
            textStyle: {
                color: '#ddd',
                fontSize: 14
            }
        }
    }],
    yAxis: {
        splitLine: {show: false},
        axisTick: {show: false},
        axisLine: {show: false},
        axisLabel: {show: false}
    },
    markLine: {
        z: -1
    },
    animationEasing: 'elasticOut',
    series: [{
        type: 'pictorialBar',
        name: 'all',
        hoverAnimation: true,
        label: {
            normal: {
                show: true,
                position: 'top',
                formatter: '{c} m',
                textStyle: {
                    fontSize: 16,
                    color: '#e54035'
                }
            }
        },
        data: [{
            value: 40,
            symbol: 'image://https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1553684610499&di=413bbafa4a3f3037bc7fbf4b591f169b&imgtype=0&src=http%3A%2F%2Fb-ssl.duitang.com%2Fuploads%2Fitem%2F201704%2F02%2F20170402170750_RuacV.thumb.224_0.jpeg',
            symbolSize: ['130%', '20%'],
            symbolSize: ['130%', '105%'],
            symbolPosition: 'end',
            z: 10
        }, {
            value: 45,
            symbol: 'image://https://ss1.bdstatic.com/70cFvXSh_Q1YnxGkpoWK1HF6hhy/it/u=2350312078,3975088225&fm=26&gp=0.jpg',
            symbolSize: ['130%', '105%'],
            symbolPosition: 'end',
            z: 10
        }, {
            value: 53,
            symbol: 'image://https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1553684896784&di=20ebafac10512ec05efc5a46876cead1&imgtype=0&src=http%3A%2F%2Fb-ssl.duitang.com%2Fuploads%2Fitem%2F201703%2F02%2F20170302193703_aCvxi.thumb.700_0.jpeg',
            symbolSize: ['130%', '105%'],
            symbolPosition: 'end',
            z: 10
        }, {
            value: 52,
            symbol: 'image://https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1553684948499&di=8637e606e6b086d8415efbbd427ab4d5&imgtype=0&src=http%3A%2F%2Fb-ssl.duitang.com%2Fuploads%2Fitem%2F201703%2F02%2F20170302190214_mkZQE.thumb.700_0.jpeg',
            symbolSize: ['130%', '105%'],
            symbolPosition: 'end'
        }],
        markLine: {
            symbol: ['none', 'none'],
            label: {
                normal: {show: false}
            },
            lineStyle: {
                normal: {
                    color: '#e54035',
                    width: 2
                }
            },
            data: [{
                yAxis: 53
            }]
        }
    }, {
        name: 'all',
        type: 'pictorialBar',
        barGap: '-100%',
        symbol: 'circle',
        itemStyle: {
            normal: {
                color: '#185491'
            }
        },
        silent: true,
        symbolOffset: [0, '50%'],
        z: -10,
        data: [{
            value: 1,
            symbolSize: ['150%', 50]
        }, {
            value: 1,
            symbolSize: ['200%', 50]
        }, {
            value: 1,
            symbolSize: ['200%', 50]
        }, {
            value: 1,
            symbolSize: ['200%', 50]
        }]
    }]
};