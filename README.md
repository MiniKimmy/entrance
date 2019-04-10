#### 毕业设计--基于树莓派的人脸识别门锁
* From 2019年3月4日20:22:32 to 2019年4月7日11:47:53
* 本项目是两大部分人脸识别模块、门锁模块
    * 门锁模块可以根据具体的门锁产品给的接口来自己实现
    * 人脸识别模块是使用百度人脸识别sdk
    * 这2个模块可以在![AppConst.cs](FacialRecognitionLock/Docs/AppConst.md)配置后,单独分开测试.
* **不打任何产品广告,纯属娱乐学习.**

##### log历程
* 2019年3月6日18:30:49 远程开门测试完毕.门锁sdk基于.net framework,因此需要搭建.net的开发环境.
* 2019年3月10日01:09:01 win10Iot无法正常调用外部.dll文件，终止在win10iot上开发此项目.
* 2019年3月11日23:33:34 RPI3B搭建.net环境成功.使用mono方式运行.exe程序.
* 2019年3月2i日17:28:12 RPI3B的镜像Yahboom-Raspibian由于太大,占用了8G空间无法正常安装opencvsharp.需要换系统.
* 2019年3月28日00:31:27 RPI3B的官方镜像2017-03-Raspibian-jessie，由于软件源太少经常缺各种包无法正常安装opencvsharp.继续换系统
* 2019年3月29日06:58:29 改用ubuntu mate 16.04作为RPI3B系统.解决软件源不足或下载源速度慢问题.
* 2019年3月30日00:37:03 基于utuntu mate 16.04系统RPI3B搭建opencvsharp-3.4.1成功.而opencvsharp-4.0.0搭建失败,不过旧版的3.4.1不影响正常使用摄像头.
* 2019年4月1日20:34:43 vs测试使用.net 4.6.1 framework是一切正常,但树莓派apt下载的mono来运行会报错"missing method exception Array.empty".该method是.net 4.6 framework才有的,而亲测发现树莓派mono里的.net仅支持.net 4.5 framework.通过修改vs项目Property为4.5,然后手动下载的nuget包要卸载再重新安装,在树莓派运行就不会报上面的错.
* 2019年4月2日09:00:05 在树莓派使用.net的"Window"类时无法显示窗口,没报错而且没反应,原因不明,需要使用Cv2.ImShow方法显示窗口.
* 2019年4月2日16:22:55 树莓派和电脑连同一个wifi,使用win10的适配器搭网桥给门锁控制器提供有线网络,在vs里测试能搜索到门锁的IP,但在树莓派环境里却搜索不到门锁的IP.问题找出是这个门锁IP是164.254开头是私有IP(仅电脑能访问),而正常的局域网应该是192.168开头,而使用路由器给门锁控制器提供有线网络却是一切正常.但这种局限于附近要有路由器才能给门锁控制器通信.
* 2019年4月3日15:03:02 由于使用win10远程桌面mstsc连接树莓派来启动应用程序,usb摄像头工作视频图像会比较卡.改用连接显示器来查看RPI3B的桌面画面时改善许多.
* 2019年4月3日18:05:58 使用async/await实现多线程解决winform的闪烁与摄像头捕捉帧的卡顿问题.
* 2019年4月3日22:15:5 RPI3B使用充电宝会供电不足,在RPI3B开启耗内存的程序时,RPI3B桌面右上角会有一个"闪电"的标志,估计是显示器就供电不足,显示器屏幕黑屏几秒又恢复显示,测试发现若不连接显示器,键盘鼠标是不会盲屏工作,推测显示器的供电不足导致树莓派自动崩溃重启。这种重启不是正常的重启，需要手动重启才能恢复过来。改用充电头+充电线来供电会比较稳定.
* 2019年4月4日0:15:44 使用usb摄像头连接RPI3B启动应用程序,经常是一开始画面流畅,但有时拿起摄像头来环绕四周查看捕捉情况时,这个时候程序就自动关掉,RPI3B控制台报opencvsharp的错"No such device"，然后线程也报错"System.Threading.ThreadAbortException".需要把摄像头固定好一个地方,不要拿起摄像头来使用,因为拿起来摄像头时连接它的线会太松容易接触不良.
* 2019年4月4日9:22:29 使用windows系统自带的适配器网桥功能对门锁主控制器提供有线网络.即门锁主控制器的网口与手提相连而无需连路由器.
* 2019年4月4日14:21:07 将winform应用程序转化成控制台应用程序开发当前项目.即可直接避开winfrom的闪烁问题,还减少运行时内存使用.
* 2019年4月6日08:40:35 控制台应用程序开发时出现"托管调试助手 "ContextSwitchDeadlock":“CLR 无法从 COM 上下文 0x1638c68 转换为 COM 上下文 0x1638bb0,这种状态已持续60 秒..."的错误。把程序的摄像头开启代码放成在Main()函数里作为主线程运行.而不要使用"Console.ReadKey()"来阻塞主线程让子线程的摄像头运行即可解决。
* 2019年4月7日09:43:42 查看vs输出经常有"System.Net.Sockets.SocketException"是正常报错,正在连接对方时被中断或线程中断，但是它过一会就会连接上就不会再报错.主要原因是本项目需要连网控制,所以会连网申请时候会出现该错误.
* 2019年4月7日11:47:53 "Task.Delay"在.NET 4.0并未出现,需要.NET 4.5才有。若使用的百度sdk不是.NET 4.5的话就出现不必要的bug,而如果想在.NET 4.0实现"Task.Delay"的效果则需要自行参考网上例子.本项目开发的第一版结束.
* 2019年4月10日14:28:55 相关笔记整理并上传.

##### 购买硬件清单
* RPI3B(树莓派3B型,只买了一个裸板也ok,没买它的封装盒子,建议也别用3B+)
* usb键盘、鼠标
* uvc摄像头(分辨率要求自己决定,建议不要用树莓派官方摄像头,使用usb摄像头)
* 读卡器(如果手提电脑有读卡的口可以不用买)
* microSD卡、tiff卡(我的是tiff卡 16G)
* 显示器、VGA线、HDMI转VGA线(因为RPI3B只有HDMI口没有VGA口)
* 网线（这个是由于我的门锁只能用有线网络.树莓派3B不需要网线）
* 门锁（最好是提供c#语言二次开发的门锁,本项目用的是c#语言）
* 门锁相关的螺丝、电线材料、螺丝批(一字、十字)、剪刀、固定装置(自己造轮子也可以)

##### 搭建环境
* ![树莓派环境配置](FacialRecognitionLock/Docs/build_rpi3B.md)
* ![树莓派搭建.net环境](FacialRecognitionLock/Docs/build_dotNet.md)
* ![树莓派搭建ssh文件传输环境](FacialRecognitionLock/Docs/build_sshShell.md)
* ![树莓派搭建opencvsharp环境](FacialRecognitionLock/Docs/build_openCVSharp.md)

##### 准备开发
* ![使用vs开发、发布到树莓派环境运行](FacialRecognitionLock/Docs/build_app.md)
* ![学习并使用opencvsharp进行开发](FacialRecognitionLock/Docs/study_opencvsharp.md)
* ![学习并使用百度人脸识别sdk](FacialRecognitionLock/Docs/study_baiduFACESDK.md)
