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
    
  },
  wxLogin: function (user) {
    wx.login({
      success: res => {
        wx.request({
          url: app.globalData.url + '/api/WxOpen/Login',
          method: 'POST',
          data: {
            code: res.code
          },
          success: function(res){
            console.log(res)
            if (res.data.status) {
              wx.setStorageSync('sessionId', res.data.sessionId)
              if (res.data.user !== null) {
                console.log(res)
                console.log('has user')
                wx.setStorageSync('user', res.data.user)
                wx.switchTab({
                  url: '../index/index',
                })
              } else {
                let sessionId = wx.getStorageSync('sessionId')
                console.log(sessionId);
                console.log(user);
                wx.request({
                  url: app.globalData.url+'/api/WxOpen/Register',
                  method: 'POST',
                  data: {
                    sessionId: sessionId,
                    encryptedData: user.detail.encryptedData,
                    iv: user.detail.iv
                  },
                  success: function(res) {
                    wx.setStorageSync('user', user)
                    wx.switchTab({
                      url: '../index/index',
                    })
                  }
                })                  
              }
            }
          }
        })
      }
    })
  },
})