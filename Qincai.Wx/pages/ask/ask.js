import api from '../../utils/api/index.js'
const app = getApp()
Page({

  /**
   * 页面的初始数据
   */
  data: {
    title: "",
    content: "",
    loading: false,
    images: []
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
  },
  titleChange: function (value) {
    this.setData({
      title: value.detail
    })
  },
  contentChange: function (value) {
    this.setData({
      content: value.detail
    })
  },
  chooseImages: function () {
    wx.chooseImage({
      count: 3,
      sizeType: 'compressed',
      success: res => {
        this.setData({
          images: res.tempFilePaths
        })
      }
    })
  },
  submit: function () {
    this.setData({
      loading: true
    })
    let that = this
    Promise.all(this.data.images.map(image => api.image.uploadImage(image)))
    .then(urls => {
      api.question.create(that.data.title, that.data.content, urls)
        .then(function (res) {
          wx.switchTab({
            url: '../index/index',
            success: function (res) {
              that.setData({
                loading: false
              })
            }
          })
        })
    })
  }
})