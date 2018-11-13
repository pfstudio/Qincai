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
          url: app.globalData.url + '/api/User/WxLogin',
          method: 'POST',
          data: {
            code: res.code
          },
          success: function(res){
            console.log(res)
            if (res.data.status) {
              wx.setStorage({
                key: 'sessionId',
                data: res.data.sessionId,
              })
              if (res.data.user !== null) {
                console.log('has user')
                wx.setStorageSync('user', res.data.user)
                wx.switchTab({
                  url: '../index/index',
                })
              } else {
                let sessionId = wx.getStorageSync('sessionId')
                console.log("userInfo")
                console.log(user);
                wx.request({
                  url: app.globalData.url+'/api/User/WxRegister',
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