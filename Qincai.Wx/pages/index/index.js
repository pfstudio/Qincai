//index.js
//获取应用实例
const app = getApp()
let api = app.globalData.api
const moment = require('../../utils/moment-with-locales.js')
moment.locale("zh-cn")
const orderBys = ["QuestionTime","LastTime"]
Page({
  data: {
    loading: false,//下拉刷新加载中标识
    questions:[],//问题列表
    tabs:["最新发布","最新回答"],//顶部导航栏文本
    active:0,//记录当前选中的顶部导航栏
    totalPage: 1,//总页面数，判断是否需要进行下拉刷新
    crtPage: 1,//当前页面
  },
  onLoad: function () {
  },
  
  onShow:function(){
    let page = this
    //获取问题列表
    api.ListQuestions()
    .then(function(res){
      res.result.map(function (item) {
        //将返回结果的时间转化为距今为止的时间差
        item.questionTime = moment(item.questionTime).fromNow()
        return item
      })
      //将获得的数据传给页面
      page.setData({
        questions:res.result,
        totalPage:res.totalPage
      })
    })
  },
  //提问按钮点击事件，跳转到问题页面
  ask: function(){
    wx.navigateTo({
      url: '../ask/ask',
    })
  },
  //导航栏选择项变换事件，从value.detail中获得被选中的导航栏项的索引值
  tabChange:function(value){
    let page = this
    //根据导航栏项获取排序后的结果
    api.ListQuestions({
      orderBy: orderBys[value.detail]
    })
      .then(function (res) {
        res.result.map(function (item) {
          item.questionTime = moment(item.questionTime).fromNow() //时间转换
          return item
        })
        page.setData({
          crtPage:1,//将当前页数重置为1
          questions: res.result
        })
      })
  },
  //点击放大镜跳转至搜索页面
  search:function(){
    wx.navigateTo({
      url: '../search/search',
    })
  },
  //触底加载
  onReachBottom: function () {
    let page = this
    //设置触底加载提示
    this.setData({
      loading:true,
    })
    //如果当前页数未到达总页数
    if (this.data.crtPage<this.data.totalPage){
        this.setData({
          //页数+1
          crtPage: this.data.crtPage+1
        })
        //请求新数据
        api.ListQuestions({
          page: this.data.crtPage,
          orderBy: orderBys[this.data.active]
        })
        .then(res =>{
          //新数据时间转换
          res.result.map(function (item) {
            item.questionTime = moment(item.questionTime).fromNow()
            return item
          })
          //新旧问题列表拼接
          let questionList = page.data.questions.concat(res.result)
          page.setData({
            //更新最大页面
            totalPage: res.totalPage,
            questions: questionList
          })
        })
    }
  },
  //页面滚动事件
  onPageScroll: function (e) {
    //当页面下来至一定程度后，将图标改变，并将提示文本变为刷新
    if (e.scrollTop > 100) {
      wx.setTabBarItem({
        index:0,
        text:"刷新"
      });
    }
    //到底部时恢复
    else{
      wx.setTabBarItem({
        index: 0,
        text: "首页"
      });
    }
  },
  //tab点击事件,主要用于刷新
  onTabItemTap:function(item) {
    let page = this
    //判断点击的tab是否为首页
    if (item.index == 0){
      //刷新数据，this.data.crtPage即当前整个页面的量
      api.ListQuestions({
        orderBy: orderBys[this.data.active]
      })
      .then(function (res) {
        res.result.map(function (item) {
          item.questionTime = moment(item.questionTime).fromNow()
          return item
        })
        page.setData({
          crtPage:1,
          totalPage: res.totalPage,
          questions: res.result
        })
      })
    }
  }
})
