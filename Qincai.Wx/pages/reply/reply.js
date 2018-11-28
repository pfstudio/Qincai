// pages/reply/reply.js
const moment = require('../../utils/moment.js')
import Dialog from '../../dist/dialog/dialog'
import api from '../../utils/api/index.js'
Page({
  /**
   * 页面的初始数据
   */
  data: {
    count:0,
    questionId:"",
    question:{},
    answerImages:[],
    refAnswerId:null,
    quote: null,
    answer:"",
    loading:false
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that = this
    let user = wx.getStorageSync('user')
    console.log(options)
    this.setData({
      questionId:options.questionId,
      quote:options.quote,
      refAnswerId: options.refAnswerId
    })
    api.question.getById(that.data.questionId)
    .then(function(res){
      that.setData({
        question:{
          title:res.data.title,
          content:res.data.content.text,
          images:res.data.content.images,
          questioner:res.data.questioner.name,
          questionTime: moment(res.data.questionTime).format('YYYY-MM-DD HH: mm: ss')
        }
      })
    })
    console.log(this.data.images)
  },
  submit:function(){
    let that = this
    this.setData({
      loading:true
    })
    console.log(that.data.answer)
    Promise.all(this.data.answerImages.map(image => api.image.uploadImage(image)))
      .then(urls => {
        api.question.reply(that.data.questionId, that.data.answer, that.data.refAnswerId, urls)
          .then(function (res) {
            wx.navigateBack({
              success: function (res) {
                that.setData({
                  loading: false
                })
              }
            })
          })
      })
  },
  answerChange:function(value){
    this.setData({
      answer:value.detail
    })
    console.log(this.data.answer)
  },
  addImages:function(){
    wx.chooseImage({
      count: 3 - this.data.count,
      sizeType: 'compressed',
      success: res => {
        let images=this.data.answerImages.concat(res.tempFilePaths)
        this.setData({
          answerImages: images,
          count:images.length
        })
      }
    })
  },
  changeImage:function(value){
    wx.chooseImage({
      count: 1,
      sizeType: 'compressed',
      success: res => {
        let images = this.data.answerImages
        images[value.target.id] = res.tempFilePaths[0]
        this.setData({
          answerImages: images,
          count: images.length
        })
      }
    })
  },
})