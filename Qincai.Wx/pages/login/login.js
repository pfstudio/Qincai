const app = getApp()
import api from '../../utils/api/index.js'
Page({

  /**
   * 页面的初始数据
   */
  data: {
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let sessionId = wx.getStorageSync('sessionId')
    wx.checkSession({
      success: function (res) {
        if(sessionId) {
          wx.switchTab({
            url: '../index/index',
          })
        }
      },
      fail: e => console.log(e)
    })
  },
  wxLogin: function (user) {
    api.wx.wxLogin().then(function(res){
      if (res.statusCode == 200){
        wx.setStorageSync('sessionId', res.data.sessionId)
        if (res.data.user !== null) {
          wx.setStorageSync('user', res.data.user)
          wx.switchTab({
            url: '../index/index',
          })
        }
        else{
          let sessionId = wx.getStorageSync('sessionId')
          api.wx.wxRegister(user,sessionId).then(function(res){
            console.log(res)
            wx.setStorageSync('user', res.data)
            wx.switchTab({
              url: '../index/index',
            })
          })
        }
      }
    })
  }
})