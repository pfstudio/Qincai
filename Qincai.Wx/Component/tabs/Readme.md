# 导航栏组件

该组件的为inline-block组件，使用时请注意

## 传入参数

active:0,//当前被选中项的索引
tabs:[],//导航栏的具体内容，传入字符串数组
tabColor:"#2a95ed",//导航栏颜色
width:460//导航栏宽度，单位rpx

以上值为默认值

## 组件方法

组件点击：当点击的项与当前被选中的项不同，可触发绑定事件change

## 绑定事件

可通过 bind:change="method" 的形式绑定change事件

可通过传入方法的参数的.detail获得新点击的tab的索引,如
method:function(value){
  console.log(value.detail)
}