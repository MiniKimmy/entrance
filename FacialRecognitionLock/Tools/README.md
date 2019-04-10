### Tools文件夹里是第三方dll工具
* AipSdk.dll 是百度人脸识别C# SDK V3版本
* AipSdk.pdb 是调试的文件,可不需要
* Newtonsoft.Json.dll 是Json数据结构的工具
* n3kAdrtC.dll 是门锁二次开发包的工具

#### 关于n3kAdrtC.dll
* 直接使用WG3000_COMM.Core.wgMjController aaa = new WG3000_COMM.Core.wgMjController();
    * 不要using(WG3000_COMM.Core.wgMjController aaa = new WG3000_COMM.Core.wgMjController())
    * 也不要Dispose它
    * 不然老是报错
* 由于这个是门锁模块的外部dll,阅读代码时不用深入了解.

#### 关于Newtonsoft.Json.dll
* 使用可参考![这里](FacialRecognitionLock/camera/OpencvSharpHelper.cs)

#### 关于AipSdk.dll
* 使用可参考[这里](https://cloud.baidu.com/doc/FACE/Face-Csharp-SDK.html#.E4.BA.BA.E8.84.B8.E6.90.9C.E7.B4.A2)

