import {url} from './common.js'
export { create, list, getById, answerList, reply}

function create(title,content,userId){
  return new Promise(function(resolve, reject) {
    wx.request({
      url: url + '/api/Question/Create',
      method: 'POST',
      data: {
        title: title,
        content: content
      },
      header: {
        userId: userId
      },
      success: res => resolve(res),
      fail: res => reject(res)
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

function reply(questionId,content,userId){
  return new Promise(function (resolve, reject) {
    wx.request({
      url: url + '/api/Question/' + questionId + '/Reply',
      method:'POST',
      data:{
        content:content
      },
      header:{
        userId:userId,
      },
      success: res => resolve(res),
      fail: res => reject(res)
    })
  })
}