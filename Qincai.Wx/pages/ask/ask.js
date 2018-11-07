
Page({

  /**
   * 页面的初始数据
   */
  data: {
    id:"",
    title:"",
    content:"",
    loading:false
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that = this;
    wx.getStorage({
      key: 'id',
      success: function(res) {
        that.setData({
          id:res.data
        })
      },
    })
  },
  titleChange:function(value){
    this.setData({
      title:value.detail
    })
  },
  contentChange: function (value) {
    this.setData({
      content: value.detail
    })
  },
  submit:function(){
    this.setData({
      loading:true
    })
    let that = this
    wx.request({
      url: 'http://212.129.134.100:5000/api/Question/Create',
      method:'POST',
      header: {
        'content-Type': 'application/json',
        'userId':that.data.id,
      },
      data:{
        title:that.data.title,
        content:that.data.content
      },
      success:function(res){
        wx.showModal({
          title: '发布成功',
          showCancel: false,
          success: function (res) {
            if (res.confirm) {
              wx.switchTab({
                url: '../index/index',
              })
            }
          }
        })
      }
    })
  }
})