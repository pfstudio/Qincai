import { url, getAuthorize } from './common.js'
export { uploadImage }

function uploadImage(imageUrl) {
  return new Promise((resolve, reject) =>
    getUploadToken().then(imageToken => {
      wx.uploadFile({
        url: imageToken.uploadDomain,
        filePath: imageUrl,
        name: 'file',
        formData: {
          'token': imageToken.token,
          'key': imageToken.key
        },
        success: res => {
          if (res.statusCode != 200) {
            console.log(res);
            return;
          }
          resolve(JSON.parse(res.data).key)
        }
      })
    })
  )

}

function getUploadToken() {
  return new Promise(function (resolve, reject) {
    getAuthorize().then(function (token) {
      console.log(token)
      wx.request({
        url: url + '/api/Image/UploadToken',
        method: 'GET',
        header: {
          Authorization: 'Bearer ' + token.data.token
        },
        success: res => resolve(res.data),
        fail: res => reject(res)
      })
    })
  })
}