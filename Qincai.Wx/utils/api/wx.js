import {url} from './common.js'
export{
  wxLogin,wxRegister
}
function wxLogin(){
  return new Promise(function(resolve,reject){
    wx.login({
      success:function(log){
        wx.request({
          url: url + '/api/WxOpen/Login',
          method: 'POST',
          data: {
            code: log.code
          },
          success: res => resolve(res),
          fail: res => reject(res)
        })
      }
    })
  })
}
function wxRegister(user, sessionId){
  return new Promise(function(resolve,reject){
    wx.request({
      url: url + '/api/WxOpen/Register',
      method:'POST',
      data: {
        sessionId: sessionId,
        encryptedData: user.detail.encryptedData,
        iv: user.detail.iv
      },
      success:res => resolve(res),
      fail:res => reject(res)
    })
  })
}