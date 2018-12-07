import { url, getAuthorize} from './common.js'
export { create, list, getById, answerList, reply,me}

function create(title,text,images){
  return new Promise(function(resolve, reject) {
    getAuthorize().then(function(token){
      console.log(token)
      wx.request({
        url: url + '/api/Question/Create',
        method: 'POST',
        data: {
          title: title,
          text: text,
          images: images
        },
        header: {
          Authorization:'Bearer '+token
        },
        success: res => resolve(res),
        fail: res => reject(res)
      })
    })
  })
}

function list(page=10, size=1, search="", orderby="QuestionTime", descending=true){
  return new Promise(function(resolve,reject){
    wx.request({
      url: url+'/api/Question?page=' + page + '&size=' + size + '&search=' + search + '&orderby=' + orderby + '&descending=' + descending,
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

function reply(questionId, content, refAnswerId,images){
  return new Promise(function (resolve, reject) {
    getAuthorize().then(function (token) {
      wx.request({
        url: url + '/api/Question/' + questionId + '/Reply',
        method:'POST',
        data:{
          text:content,
          images:images,
          refAnswerId: refAnswerId
        },
        header:{
          Authorization: 'Bearer ' + token
        },
        success: res => resolve(res),
        fail: res => reject(res)
      })
    })
  })
}

function me (page ,size){
  return new Promise(function(resolve,reject){
    getAuthorize().then(function(token){
      wx.request({
        url: url+'/api/Question/me?page=' + page + '&size=' + size,
        header: {
          Authorization: 'Bearer ' + token
        },
        success: res => resolve(res),
        fail: res => reject(res)
      })
    })
  })
}
