import api from '../../utils/api/index.js'
const app = getApp()
Page({

  /**
   * 页面的初始数据
   */
  data: {
    title:"",
    content:"",
    loading:false
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
  },
  titleChange:function(value){
    this.setData({
      title:value.detail
    })
  },
  contentChange: function (value) {
    this.setData({
      content: value.detail
    })
  },
  submit:function(){
    this.setData({
      loading:true
    })
    let that = this
    api.question.create(that.data.title,that.data.content)
    .then(function(res){
      wx.switchTab({
        url: '../index/index',
        success:function(res){
          that.setData({
            loading: false
          })
        }
      })
    })

  }
})