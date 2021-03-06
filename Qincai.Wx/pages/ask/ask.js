const app = getApp()
let api = app.globalData.api
import { uploadImage } from '../../utils/api-helper.js'

Page({

  /**
   * 页面的初始数据
   */
  data: {
    count:0,
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
  addImages: function () {
    wx.chooseImage({
      count: 3 - this.data.count,
      sizeType: 'compressed',
      success: res => {
        let images = this.data.images.concat(res.tempFilePaths)
        this.setData({
          images: images,
          count: images.length
        })
      }
    })
  },
  changeImage: function (value) {
    wx.chooseImage({
      count: 1,
      sizeType: 'compressed',
      success: res => {
        let images = this.data.images
        images[value.target.id] = res.tempFilePaths[0]
        this.setData({
          images: images,
          count: images.length
        })
      }
    })
  },
  submit: function () {
    this.setData({
      loading: true
    })
    let that = this
    Promise.all(this.data.images.map(image => uploadImage(image, api)))
    .then(urls => {
      api.CreateQuestion({
        dto: {
        title: that.data.title,
        text: that.data.content,
        images: urls
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
  }
})