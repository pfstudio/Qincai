const app = getApp()
let api = app.globalData.api
import { uploadImage } from '../../utils/api-helper.js'

Page({

  /**
   * 页面的初始数据
   */
  data: {
    count:0,//当前图片数
    title: "",//问题标题
    content: "",//问题内容
    loading: false,//提交标记
    images: []//图片
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
  },
  //标题输入框监听事件，根据输入框的输入更改页面对应的数据
  titleChange: function (value) {
    this.setData({
      title: value.detail
    })
  },
  //内容输入框监听事件，根据输入框的输入更改页面对应的数据
  contentChange: function (value) {
    this.setData({
      content: value.detail
    })
  },
  //增加图片事件，在点击加入图片时调用
  addImages: function () {
    wx.chooseImage({
      count: 3 - this.data.count,//可添加图片数为3-当前图片数
      sizeType: 'compressed',//进行压缩
      success: res => {
        let images = this.data.images.concat(res.tempFilePaths)//将获得的图片加入到当前图片数组中
        this.setData({
          images: images,
          count: images.length//更改当前图片数
        })
      }
    })
  },
  //更改被选中的图片
  changeImage: function (value) {
    wx.chooseImage({
      count: 1,
      sizeType: 'compressed',
      success: res => {
        let images = this.data.images
        images[value.target.id] = res.tempFilePaths[0]//value.target.id为更改图片的索引,更改对应索引的图片
        this.setData({
          images: images,
          count: images.length
        })
      }
    })
  },
  //点击提交按钮
  submit: function () {
    this.setData({
      loading: true
    })
    let page = this
    Promise.all(this.data.images.map(image => uploadImage(image, api)))//上传图片
    .then(urls => {
      //提交问题，参数请见api，需用dto包装
      api.CreateQuestion({
        dto: {
        title: page.data.title,
        text: page.data.content,
        images: urls
        }
      })
      .then(function (res) {
        wx.navigateBack({//页面跳回至detail
          success: function (res) {
            page.setData({
              loading: false//提交标记重置
            })
          }
        })
      })
    })
  }
})