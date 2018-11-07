Page({

  /**
   * 页面的初始数据
   */
  data: {
    questions: []
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onShow: function () {
    let that = this;
    wx.request({
      url: 'http://212.129.134.100:5000/api/Question',
      success: function (res) {
        that.setData({
          questions: res.data.result
        })
        console.log(that.data.questions)
      }
    })
    
  },
  reply:function(detail){
    console.log(detail)
    wx.navigateTo({
      url: '../reply/reply?id='+detail.currentTarget.id,
    })
  }



})