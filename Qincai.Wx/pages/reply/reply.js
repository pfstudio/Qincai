// pages/reply/reply.js
const moment = require('../../utils/moment.js')
import Dialog from '../../dist/dialog/dialog'
Page({

  /**
   * 页面的初始数据
   */
  data: {
    id:"",
    userId:"",
    title:"",
    content:"",
    questioner:"",
    questionTime:"",
    answer:"",
    loading:false
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that = this
    this.setData({
      id:options.id,
      userId: wx.getStorageSync('id')
    })
    wx.request({
      url: 'http://212.129.134.100:5000/api/Question/'+that.data.id,
      success:function(res){
        console.log(res)
        that.setData({
          title:res.data.title,
          content:res.data.content.text,
          questioner:res.data.questioner.name,
          questionTime: moment(res.data.questionTime).format('YYYY-MM-DD HH: mm: ss')
        })
      }
    })

  },
  submit:function(){
    this.setData({
      loading:true
    })
    let that = this
    wx.request({
      url: 'http://212.129.134.100:5000/api/Question/'+that.data.id+'/Reply',
      method:'POST',
      header:{
        userId:that.data.userId
      },
      data:{
        content:that.data.answer
      },
      success:function(res){
        wx.showModal({
          title: '提交成功',
          showCancel:false,
          success:function(res){
            if (res.confirm){
              wx.switchTab({
                url: '../index/index',
              })
            }
          }
        })
      }
    })
  },
  answerChange:function(value){
    this.setData({
      answer:value.detail
    })
  }
})