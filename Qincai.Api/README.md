# Qincai.Api

计划任务: 见Project

## ImageApi的使用

- 请求Token(每个文件一个Token)

  - 使用`Post`方法

    在表单中添加待上传文件的后缀名

    使用范例
    ``` js
    wx.request({
      url: '填入业务服务器的路径',
      method: 'POST',
      data: {
        //文件的后缀
        'fileExt': fileExt
      },
      success: function (res) {
        if(res.statusCode!=200) {
          return;
        }
      }
    })
    ```

  - 返回的结构

    - Token : string 待上传的文件的Token

    - Scope : string 使用Scope控制文件上传的路径

    - UploadDomain : string 图片服务器的域名

- 上传图片

  - 使用`POST`方法  (或`PUT` (未验证))

  表单中带上`Scope`和`Token`

  范例
  ```js
  wx.uploadFile({
      url: '请求Token时拿到的上传域名',
      filePath: '文件本地路径',
      name: 'file',
      formData: {
        'token': 'Token',
        'key': '请求Token时拿到的Scope'
      },
      success: function (res) {
        //...
      }
    })
  ```