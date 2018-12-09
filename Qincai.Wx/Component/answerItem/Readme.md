# 单个回答组件

## 传入参数

answer:{
  "id": "string",
  "answerer": {
    "id": "string",
    "name": "string",
    "avatarUrl": "string"
  },
  "content": {
    "text": "string",
    "images": [
      "string"
    ]
  },
  "answerTime": "2018-12-09T08:26:22.078Z",
  "refAnswer": {
    "id": "string",
    "answerer": {
      "id": "string",
      "name": "string",
      "avatarUrl": "string"
    },
    "content": {
      "text": "string",
      "images": [
        "string"
      ]
    }
  }
}

用于显示该问题的具体信息，格式同api中的result

"questionId":"string"

该回答所回答的问题的id，用于引用该回答时的页面跳转

## 组件方法

追答方法：点击追答页面跳转至回复页面被将对应回答引用。

追问方法
