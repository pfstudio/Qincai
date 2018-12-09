// Component/answerItem/index.js
Component({
  /**
   * 组件的属性列表
   */
  properties: {
    answer:{
      type:Object,
      value:null
    },
    questionId:{
      type:String,
      value:null
    }
  },

  /**
   * 组件的初始数据
   */
  data: {

  },

  /**
   * 组件的方法列表
   */
  methods: {
    plus: function (res) {
      let that = this
      console.log(res)
      wx.navigateTo({
        url: '../reply/reply?questionId=' + that.data.questionId + '&quote=' + res.target.dataset.quote + '&refAnswerId=' + res.target.id,
      })
    },
    showImage: function (value) {
      wx.previewImage({
        current:value.target.dataset.url,
        urls: this.data.answer.content.images,
      })
    }
  }
})
