import {url} from './common'
export {
  list
}
function list(){
  wx.request({
    url: url+'/api/User',
    success:function(res){
      return res.data
    }
  })
}