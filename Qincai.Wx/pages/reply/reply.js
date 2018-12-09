const moment = require('../../utils/moment.js')
const app = getApp()
const api = app.globalData.api
import { uploadImage } from '../../utils/api-helper.js'

Page({
  /**
   * 页面的初始数据
   */
  data: {
    count:0,//当前图片数
    questionId:"",//所回答问题的id
    question:{},//问题的具体内容
    refAnswerId:null,//所引用的回答的Id
    quote: null,//引用的内容
    answer:"",//回答的内容
    loading:false,//加载状态
    answerImages: []//图片组
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that = this
    //根据传入页面的options中的内容，将问题，引用等相关信息写入页面
    this.setData({
      questionId:options.questionId,
      quote:options.quote,
      refAnswerId: options.refAnswerId
    })
    //根据问题Id获得问题的具体内容
    api.GetQuestionById({
      id: that.data.questionId
    })
    .then(function(res){//根据组件question的所需内容的格式，赋值页面的question
      that.setData({
        question:{
          title:res.title,//标题
          content:res.content.text,//内容
          images:res.content.images,//图片
          questioner:res.questioner.name,//提问者
          questionTime: moment(res.questionTime).format('YYYY-MM-DD HH: mm: ss')//时间格式转化
        }
      })
    })
  },
  //提交事件
  submit:function(){
    let that = this
    this.setData({
      loading:true//将加载标记设置为加载中
    })
    // console.log(that.data.answer)
    Promise.all(this.data.answerImages.map(image => uploadImage(image, api))) //先将图片上传，将上传后图片的地址存入urls中
      .then(urls => {
        api.ReplyQuestion({//获得图片地址后开始创造回答
          id: that.data.questionId,//提交问题id
          dto: {
            text: that.data.answer,//提交问题内容
            images: urls,//提交图片地址
            refAnswerId: that.data.refAnswerId//提交引用的回答的Id
          }
        })
        .then(function (res) {
          wx.navigateBack({
            success: function (res) {
              that.setData({
                loading: false//完成，标记设为否
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
  },
  //图片添加
  addImages:function(){
    wx.chooseImage({
      count: 3 - this.data.count,//最大图片上限为3，所以当前可添加图片数=3-count
      sizeType: 'compressed',//压缩
      success: res => {
        let images=this.data.answerImages.concat(res.tempFilePaths)//将新加的图片拼接到进图片数组中
        this.setData({
          answerImages: images,//将图片数组存入页面中
          count:images.length//更新图片数
        })
      }
    })
  },
  //更改图片
  changeImage:function(value){
    wx.chooseImage({
      count: 1,
      sizeType: 'compressed',
      success: res => {
        let images = this.data.answerImages
        images[value.target.id] = res.tempFilePaths[0]//将新图片替换对应位置的图片
        this.setData({
          answerImages: images,//图片存入页面中
          count: images.length
        })
      }
    })
  },
})