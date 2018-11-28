// pages/me.js

Page({

  /**
   * 页面的初始数据
   */
  data: {
    user:null
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let user = wx.getStorageSync('user')
    console.log(user)
    this.setData({
      user:user
    })
  },
  myQuestion: function(){
    wx.navigateTo({
      url: '../myQuestion/myQuestion',
    })
  },
  userinfo: function(){
    wx.navigateTo({
      url: '../userinfo/userinfo',
    })
  }
})