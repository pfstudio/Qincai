// pages/detail/detail.js
const moment = require('../../utils/moment-with-locales.js')
const app = getApp()
const api = app.globalData.api

Page({

  /**
   * 页面的初始数据
   */
  data: {
    questionId:"",
    question:{},
    answers:[]
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that = this
    this.setData({
      questionId: options.questionId,
    })
    console.log(api)
    api.GetQuestionById({
      id: that.data.questionId
    })
    .then(function(res){
      that.setData({
        question:{
          title: res.title,
          content: res.content.text,
          images:res.content.images,
          questioner:res.questioner.name,
          questionTime: moment(res.questionTime).format('YYYY-MM-DD HH: mm: ss')
        }
      })
    })
  },
  onShow:function(){
    let that = this
    api.ListAnswersByQuestionId({
      id: that.data.questionId
    })
    .then(function (res) {
      var answerList = res.result.map(function (item) {
        item.answerTime = moment(item.answerTime).format('YYYY-MM-DD HH: mm: ss')
        return item
      })
      that.setData({
        answers: answerList
      })
    })
  },
  reply:function(res){
    let that = this
    wx.navigateTo({
      url: '../reply/reply?questionId='+that.data.questionId,
    })
  },
})
