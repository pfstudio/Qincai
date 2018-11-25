export const url = "http://wxopen.pfstudio.xyz:5001"
export function getAuthorize() {
  return new Promise(function(resolve,reject){
  let sessionId = wx.getStorageSync('sessionId')
  wx.request({
    url: url + '/api/WxOpen/Authorize',
    method:'POST',
    data:{
      sessionId: sessionId
    },
    success: res => resolve(res),
    fail: res => reject(res)
    })
  })
}
