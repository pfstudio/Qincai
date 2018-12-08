const app = getApp()

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
    let api = app.globalData.api;
    console.log(api)
    wx.login({
      success: res => {
        api.WxLogin({dto: {code: res.code}})
          .then(res => {
            wx.setStorageSync('sessionId', res.sessionId)
            if (res.user !== null) {
              wx.setStorageSync('user', res.user)
              wx.switchTab({
                url: '../index/index',
              })
            } else {
              api.WxRegister({
                dto: {
                  sessionId: res.sessionId,
                  encryptedData: user.detail.encryptedData,
                  iv: user.detail.iv
                }
              })
                .then(res => {
                  wx.setStorageSync('user', res)
                  wx.switchTab({
                    url: '../index/index',
                  })
                })
            }
          })
          .catch(e => {
            console.log(e)
            wx.showToast({
              title: '登录失败'
            })
          })
      }
    })
  }
})