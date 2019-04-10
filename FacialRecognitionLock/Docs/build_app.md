### 开发项目

#### 请按照和我一样的环境
* 请先完成![树莓派搭建opencvsharp环境](build_openCVSharp.md)
* [下载Comunity版本的vs2017](https://visualstudio.microsoft.com/thank-you-downloading-visual-studio/?sku=Community&rel=15)
* vs2017(安装.NET桌面开发；安装框架.NET Framework 4.5目标包【项目里不要使用.net .NET 4.6因为apt下载的mono不支持】)
* 下载opencvsharp-AnyCPU的对应3.4.1-20180319版本的.nuget包:[nuget官网](https://www.nuget.org/)
* 若使用其他版本的opencv则自行下载其他的版本.

<img src="https://raw.githubusercontent.com/MiniKimmy/entrance
/FacialRecognitionLock/FacialRecognitionLock/Docs/resources/buildNuget/nugetDownLoad.jpg?raw=true" alt="can't find .png" width="800px">

#### 使用vs开发项目,配置nuget
* 新建winform 或者 控制台应用程序都可以
* 进入项目之后,按ctrl+:,出现"解决方案资源管理器"
* 双击"Properties" ->"应用程序"->"目标框架"选择".NET Framework 4.5"
* 在"解决方案资源管理器"->右键"引用"->"管理.Nuget程序包..."

<img src="https://raw.githubusercontent.com/MiniKimmy/entrance
/FacialRecognitionLock/FacialRecognitionLock/Docs/resources/buildNuget/nugetInstall.jpg?raw=true" alt="can't find .png" width="800px">

#### 发布到树莓派环境
* 请先完成![树莓派搭建ssh文件传输环境](build_sshShell.md)
* 使用显示器+键盘+鼠标的方式运行RPI3B
* 打开控制台(ctrl+alt+t)
* cd到ssh文件上传的.exe文件夹目录，使用mono方式运行程序
```
mono xxxx.exe
```

