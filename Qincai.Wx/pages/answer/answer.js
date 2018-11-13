import api from '../../utils/api/index.js'
Page({
  /**
   * 页面的初始数据
   */
  data: {
    questions: []
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onShow: function () {
    let that = this;
    api.question.list(1,10)
    .then(function(res){
      that.setData({
        questions: res.data.result
      })
    })
  },
  reply:function(detail){
    console.log(detail)
    wx.navigateTo({
      url: '../reply/reply?id='+detail.currentTarget.id,
    })
  }
})