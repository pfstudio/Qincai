const moment = require('../../utils/moment.js')
const app = getApp()
const api = app.globalData.api
import { uploadImage } from '../../utils/api-helper.js'

Page({
  /**
   * 页面的初始数据
   */
  data: {
    count:0,
    questionId:"",
    question:{},
    refAnswerId:null,
    quote: null,
    answer:"",
    loading:false,
    answerImages: []
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that = this
    let user = wx.getStorageSync('user')
    this.setData({
      questionId:options.questionId,
      quote:options.quote,
      refAnswerId: options.refAnswerId
    })
    api.GetQuestionById({
      id: that.data.questionId
    })
    .then(function(res){
      that.setData({
        question:{
          title:res.title,
          content:res.content.text,
          images:res.content.images,
          questioner:res.questioner.name,
          questionTime: moment(res.questionTime).format('YYYY-MM-DD HH: mm: ss')
        }
      })
    })
  },
  submit:function(){
    let that = this
    this.setData({
      loading:true
    })
    // console.log(that.data.answer)
    Promise.all(this.data.answerImages.map(image => uploadImage(image, api)))
      .then(urls => {
        api.ReplyQuestion({
          id: that.data.questionId,
          dto: {
            text: that.data.answer,
            images: urls,
            refAnswerId: that.data.refAnswerId
          }
        })
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
  // TODO：获取文本框数据是否放在点击事件里面更好
  answerChange:function(value){
    this.setData({
      answer:value.detail
    })
    //console.log(this.data.answer)
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