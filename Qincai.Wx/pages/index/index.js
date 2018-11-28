//index.js
//获取应用实例
const app = getApp()
import api from '../../utils/api/index.js'
const moment = require('../../utils/moment-with-locales.js')
Page({
  data: {
    id:'',
    loading: false,
    questions:[]
  },
  //事件处理函数
  onLoad: function () {
    
    let that = this;
    let user = wx.getStorageSync('user')
    if (user==""){
      wx.redirectTo({
        url: '../login/login',
      })
    }
    else{
      wx.checkSession({
        success:function(res){
          console.log(res)
        },
        fail:function(e){
          wx.redirectTo({
            url: '../login/login',
          })
        }
      })
    }
  },
  onShow:function(){
    let that = this
    api.question.list(1,10)
    .then(function(res){
      that.setData({
        questions:res.data.result
      })
    })
  },
  ask: function(){
    wx.navigateTo({
      url: '../ask/ask',
    })
  }
})
