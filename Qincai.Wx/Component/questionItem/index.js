
Component({
  /**
   * 组件的属性列表
   */
  properties: {
    question: {
      type: Object,
      value: null
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
    _detail(){
      this.triggerEvent("click")
    },
  }
})
