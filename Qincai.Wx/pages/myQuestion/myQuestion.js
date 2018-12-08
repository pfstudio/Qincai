const app = getApp()
const api = app.globalData.api

Page({
  data: {
   id:'',
   myquestions:[],
   loading: false
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function () {
  
  },

  onShow: function () {
    let that = this
    api.ListMyQuestions()
    .then(function(res){
      that.setData({
        myquestions:res.result
      })
    })
  },
  
  detail: function (detail) {
    console.log(detail)
    wx.navigateTo({
      url: '../detail/detail?questionId=' + detail.currentTarget.id,
    })
  },
 
})