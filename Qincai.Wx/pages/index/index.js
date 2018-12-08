//index.js
//获取应用实例
const app = getApp()
let api = app.globalData.api
const moment = require('../../utils/moment-with-locales.js')
moment.locale("zh-cn")
const orderBys = ["QuestionTime","LastTime"]
Page({
  data: {
    id:'',
    loading: false,
    questions:[],
    tabs:["最新发布","最新回答"]
  },
  //事件处理函数
  onLoad: function () {
  },
  onShow:function(){
    let that = this
    api.ListQuestions()
    .then(function(res){
      res.result.map(function (item) {
        item.questionTime = moment(item.questionTime).fromNow()
        return item
      })
      that.setData({
        questions:res.result
      })
    })
  },
  ask: function(){
    wx.navigateTo({
      url: '../ask/ask',
    })
  },
  tabChange:function(value){
    let that = this
    api.ListQuestions({
      orderBy: orderBys[value.detail]
    })
      .then(function (res) {
        res.result.map(function (item) {
          item.questionTime = moment(item.questionTime).fromNow()
          return item
        })
        that.setData({
          questions: res.result
        })
      })
  },
  search:function(){
    wx.navigateTo({
      url: '../search/search',
    })
  }
})
