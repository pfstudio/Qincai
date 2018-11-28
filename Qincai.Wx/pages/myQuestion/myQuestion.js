import api from '../../utils/api/index.js'
// pages/myQuestion/myQuestion.js
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
    api.question.me(1,10)
    .then(function(res){
      console.log(res)
      that.setData({
        myquestions:res.data.result
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