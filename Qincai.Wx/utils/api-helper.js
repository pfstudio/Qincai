export { uploadImage }

function uploadImage(imageUrl, api) {
  return new Promise((resolve, reject) =>
    api.GetUploadToken().then(imageToken => {
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