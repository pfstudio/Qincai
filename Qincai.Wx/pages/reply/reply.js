// pages/reply/reply.js
const moment = require('../../utils/moment.js')
import Dialog from '../../dist/dialog/dialog'
import api from '../../utils/api/index.js'
Page({

  /**
   * 页面的初始数据
   */
  data: {
    questionId:"",
    userId:"",
    title:"",
    content:"",
    quote: null,
    questioner:"",
    questionTime:"",
    answer:"",
    loading:false
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that = this
    this.setData({
      questionId:options.questionId,
      quote:options.quote,
      userId: wx.getStorageSync('userId')
    })
    api.question.getById(that.data.questionId)
    .then(function(res){
      that.setData({
        title: res.data.title,
        content: res.data.content.text,
        questioner: res.data.questioner.name,
        questionTime: moment(res.data.questionTime).format('YYYY-MM-DD HH: mm: ss')
      })
    })
  },
  submit:function(){
    this.setData({
      loading:true
    })
    let that = this
    console.log(that.data.questionId, that.data.answer, that.data.userId)
    api.question.reply(that.data.questionId,that.data.answer,that.data.userId)
    .then(function(res){
      wx.showToast({
        title: '提交成功',
        duration: 2000,
        complete:function(res){
          wx.switchTab({
            url: '../index/index',
          })
        }
      })
    })
  },
  answerChange:function(value){
    this.setData({
      answer:value.detail
    })
  }
})