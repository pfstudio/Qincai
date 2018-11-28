# Qincai.Wx

请添加Vant组件，至路径/dist

## 更新日志

### 2018.11.28 14:30 monitor4

大幅修改了questionItem组件样式 将页面跳转功能包装进了组件中（不再需要传函数参数）增加问题提问距今的时间

增加了tabs组件，即标签页组件（该组件为inline-block组件，用于和其他元素处在同一行中） 
该组件有4个属性 tabs 展示在tab中的字符串数组， width 即该组件在该行中所占宽度(只用写数字就行，单位rpx)
active(初始选中标签，默认为0，输入对应的索引号数字) tabsColor表示被选的颜色
通过bind:change来跟踪选中标签改变事件，所提供参数中的.detail获得变化后的索引,如
change(value){
  console.log(value.detail)
}