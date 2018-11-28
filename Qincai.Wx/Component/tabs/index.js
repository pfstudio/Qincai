// Component/tabs.js
Component({
  /**
   * 组件的属性列表
   */
  properties: {
    active:{
      type:Number,
      value:0
    },
    tabs:{
      type:Array,
    },
    tabColor:{
      type:String,
      value:"#2a95ed"
    },
    width:{
      type:Number,
      value:460
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
    tabClick:function(value){
      console.log(value)
      if (parseInt(value.target.id) != this.data.active){
          this.setData({
            active: parseInt(value.target.id)
          })
          this.triggerEvent("change",this.data.active)
      }
    }
  }
})
