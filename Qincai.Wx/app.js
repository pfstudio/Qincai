import Qincai from './utils/api.js'

App({
  globalData: {
    api: null
  },
  onLaunch: function () {
    let api = new Qincai()
    api.setAuthenticate(function () {
      return new Promise((resolve, reject) => {
        let sessionId = wx.getStorageSync('sessionId')
        api.WxAuthorize({ dto: { sessionId: sessionId } })
          .then(token => resolve(token))
      })
    })
    this.globalData.api = api;
  },
})