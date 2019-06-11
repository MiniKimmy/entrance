#### 学习并使用百度人脸识别sdk进行开发
* 注:下文中出现的 "//"、"#"、"--" 都是注释,只是解释说明
* 请务必先搭建一切环境后再查看该笔记
* [百度开放平台](http://ai.baidu.com/)
* [下载官方人脸识别C# sdk](http://ai.baidu.com/download?sdkId=83)
* [人脸识别C# sdk例子教程](https://cloud.baidu.com/doc/FACE/Face-Detect.html)
    * 选择"人脸识别"->"C# SDK V3文档"查看详细说明.
* [人脸属性推荐参数](https://cloud.baidu.com/doc/FACE/Face-Detect.html#.E8.83.BD.E5.8A.9B.E4.BB.8B.E7.BB.8D)
* [人脸检测错误列表](http://ai.baidu.com/docs#/Face-ErrorCode-V3/top)
    * 在浏览器ctrl+F,搜索错误码即可快速定位
* [登录百度控制台](https://login.bce.baidu.com/)
* [百度人脸识别使用视频教程](http://ai.baidu.com/support/video)
##### 人脸识别sdk介绍
* ak、sk换取AI调用web接口
    * ak = API key     (参数变量名:client_id)
    * sk = Secret Key  (参数变量名:client_secret)
* access_token的有效期为30天，切记需要每30天进行定期更换，或者每次请求都拉取新token【**使用V3版本SDK的不需要理会过期的问题**】
    * [获取access_token示例教程](http://ai.baidu.com/docs#/Auth/top):"新手指南"->"鉴权认证机制"->查看示例代码
    * 使用 C# SDK V3 的开源包并不需要理会"access_token"的问题
