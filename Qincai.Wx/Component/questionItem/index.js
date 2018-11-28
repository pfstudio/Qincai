const moment = require('../../utils/moment-with-locales.js')
Component({
  /**
   * 组件的属性列表
   */
  properties: {
    question: {
      type: Object,
      value: null
    },
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
    detail(){
      wx.navigateTo({
        url: '../detail/detail?questionId=' + this.data.question.id,
      })
    },
  },
  ready:function(){
    moment.locale("zh-cn")
    let que = this.data.question;
    que.questionTime=moment(que.questionTime).fromNow()
    this.setData({
      question:que
    })
  }
})
