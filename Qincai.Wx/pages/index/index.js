//index.js
//获取应用实例
const app = getApp()

Page({
  data: {
    id:'',
    loading: false,
    questions:[]
  },
  //事件处理函数
  onLoad: function () {   
  },
  onShow:function(){
    let that = this
    wx.getStorage({
      key: 'id',
      success: function (res) {
        that.setData({
          id: res.data
        })
        wx.request({
          url: 'http://212.129.134.100:5000/api/Question',
          success: function (res) {
            that.setData({
              questions: res.data.result
            })
          }
        })
      },
      fail: function (e) {
        wx.navigateTo({
          url: '../login/login',
        })
      }
    })
  },
  logout:function(){
    wx.clearStorage();
    wx.navigateTo({
      url: '../login/login',
    })
  },
  tabChange:function(value){
    if (value.detail == 1){
      wx.navigateTo({
        url: '../ask/ask',
      })
    }
  },
  detail: function (detail) {
    console.log(detail)
    wx.navigateTo({
      url: '../detail/detail?questionId=' + detail.currentTarget.id,
    })
  }
})
