Page({

  /**
   * 页面的初始数据
   */
  data: {
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    
  },
  login:function(){
    wx.request({
      url: 'http://212.129.134.100:5000/api/User/Random',
      success:function(res){
        wx.setStorage({
          key: 'id',
          data: res.data.id,
        })
        wx.setStorage({
          key: 'name',
          data: res.data.name,
        })
      }
    })
    wx.navigateBack({
      url:'../index/index'
    })
  },
})