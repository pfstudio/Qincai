export const url = "http://localhost:5000"
// export const url = "http://212.129.134.100:5000"
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
