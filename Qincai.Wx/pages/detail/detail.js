// pages/detail/detail.js
const moment = require('../../utils/moment-with-locales.js')
import api from '../../utils/api/index.js'
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
    console.log(this.data.answers)
    console.log(options)
    let that = this
    this.setData({
      questionId: options.questionId,
    })
    api.question.getById(that.data.questionId)
    .then(function(res){
      that.setData({
        question:{
          title: res.data.title,
          content: res.data.content.text,
          images:res.data.content.images,
          questioner:res.data.questioner.name,
          questionTime: moment(res.data.questionTime).format('YYYY-MM-DD HH: mm: ss')
        }
      })
    })
  },
  onShow:function(){
    let that = this
    api.question.answerList(that.data.questionId, 1, 10)
      .then(function (res) {
        console.log(res)
        var answerList = res.data.result.map(function (item) {
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
