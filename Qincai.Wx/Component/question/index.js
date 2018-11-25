// Component/question/index.js
Component({
  /**
   * 组件的属性列表
   */
  properties: {
    question:{
      type:Object,
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
    showImage:function(value) {
      console.log(value)
      let image = [value.target.dataset.url]
      wx.previewImage({
        urls: image,
      })
    }
  }
})
