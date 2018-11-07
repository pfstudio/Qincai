// pages/detail/detail.js
const moment = require('../../utils/moment.js')
Page({

  /**
   * 页面的初始数据
   */
  data: {
    questionId:"",
    questionContent:"",
    questionTime:"",
    questioner:"",
    answers:[]
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    console.log(options)
    let that = this
    this.setData({
      questionId: options.questionId,
    })
    wx.request({
      url: 'http://212.129.134.100:5000/api/Question/' + that.data.questionId,
      success: function (res) {
        console.log(res)
        that.setData({
          questionTitle: res.data.title,
          questionContent: res.data.content.text,
          questioner: res.data.questioner.name,
          questionTime: moment(res.data.questionTime).format('YYYY-MM-DD HH: mm: ss')
        })
      }
    })
    wx.request({
      url: 'http://212.129.134.100:5000/api/Question/'+that.data.questionId+'/Answers',
      success:function(res){
        console.log(res)
        that.setData({
          answers:res.data.result
        })
      }
    })
  },
})
