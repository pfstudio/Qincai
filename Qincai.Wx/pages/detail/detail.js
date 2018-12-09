// pages/detail/detail.js
const moment = require('../../utils/moment-with-locales.js')
const app = getApp()
const api = app.globalData.api

Page({

  /**
   * 页面的初始数据
   */
  data: {
    questionId:"",//当前问题的id，由进入页面的options获得
    question:{},//问题的具体内容，根据question组件需求进行填充
    answers:[]//回答列表，通过api请求获得
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let page = this
    this.setData({
      questionId: options.questionId,
    })
    //获取该问题
    api.GetQuestionById({
      id: page.data.questionId
    })
    .then(function(res){
      //根据qustion组件封装参数
      page.setData({
        question:{
          title: res.title,//标题
          content: res.content.text,//文本
          images:res.content.images,//图片组
          questioner:res.questioner.name,//提问者
          questionTime: moment(res.questionTime).format('YYYY-MM-DD HH: mm: ss')//提问时间，带格式转化
        }
      })
    })
  },
  onShow:function(){
    let page = this
    //获取该问题的回答
    api.ListAnswersByQuestionId({
      id: page.data.questionId
    })
    .then(function (res) {
      var answerList = res.result.map(function (item) {
        item.answerTime = moment(item.answerTime).format('YYYY-MM-DD HH: mm: ss')//问题列表中的将时间进行格式转化
        return item
      })
      page.setData({
        answers: answerList
      })
    })
  },
  //进入回答页面
  reply:function(res){
    let page = this
    wx.navigateTo({
      url: '../reply/reply?questionId='+page.data.questionId,
    })
  },
})
