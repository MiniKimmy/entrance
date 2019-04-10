#### 学习并使用opencvsharp进行开发
* 注:下文中出现的 "//"、"#"、"--" 都是注释,只是解释说明
* 请务必先搭建一切环境后再查看该笔记
* [下载官方demo](https://github.com/shimat/opencvsharp/releases)
    * 注:使用什么版本的opencvsharp就下载什么版本的"Sample"
    * 直接下载然后在vs里打开查看

##### 打开OpenCvSharpSamples.sln
* 只需要关注"SampleBase"和"SampleCS"文件夹即可
* 本人认为值得参考的官方demo是以下几个
``` c
MatOperations       // 抠图
FaceDetection       // 人脸检测
VideoCaptureSample  // 视频流拍照
VideoWriterSample   // 录制视频
MatToBitmap         // 图片格式转化
```

##### 几个常用的opencv函数
``` csharp
// 变成灰色图片
Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

// 新建一个窗口(注:在树莓派不支持"Window"类)
using (var window = new Window("window_name", src)){
    // ..scripts

    Cv2.WaitKey();
}

// 新建window改成Cv2的方式在树莓派能支持
Cv2.ImShow("window_name", src);

// 常用的人脸识别xml配置(可网上下载,也可以在官方demo里找到该文件)
haarcascade_frontalface_alt.xml
```