import { url, getAuthorize} from './common.js'
export { create, list, getById, answerList, reply}

function create(title,content){
  return new Promise(function(resolve, reject) {
    getAuthorize().then(function(token){
      console.log(token)
      wx.request({
        url: url + '/api/Question/Create',
        method: 'POST',
        data: {
          title: title,
          content: content
        },
        header: {
          Authorization:'Bearer '+token.data.token
        },
        success: res => resolve(res),
        fail: res => reject(res)
      })
    })
  })
}

function list(page,size){
  return new Promise(function(resolve,reject){
    wx.request({
      url: url+'/api/Question?page=' + page + '&size=' + size,
      success: res => resolve(res),
      fail: res => reject(res)
    })
  })
}

function getById(questionId){
  return new Promise(function (resolve, reject) {
    wx.request({
      url: url + '/api/Question/'+questionId,
      success: res => resolve(res),
      fail: res => reject(res)
    })
  })
}

function answerList(questionId,page,size){
  return new Promise(function (resolve, reject) {
    wx.request({
      url: url + '/api/Question/' + questionId+'/Answers',
      success: res => resolve(res),
      fail: res => reject(res)
    })
  })
}

function reply(questionId, content, refAnswerId){
  return new Promise(function (resolve, reject) {
    getAuthorize().then(function (token) {
      wx.request({
        url: url + '/api/Question/' + questionId + '/Reply',
        method:'POST',
        data:{
          content:content,
          refAnswerId: refAnswerId
        },
        header:{
          Authorization: 'Bearer ' + token.data.token
        },
        success: res => resolve(res),
        fail: res => reject(res)
      })
    })
  })
}