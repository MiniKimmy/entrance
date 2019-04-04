#### 毕业设计
From 2019年3月4日20:22:32 to 2019年4月4日21:26:35

##### 基于树莓派的人脸识别门锁
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
* 2019年4月4日14:21:07 将winform应用程序转化成控制台应用程序开发当前项目.即可直接避开winfrom的闪烁问题,优化运行时内存使用.本项目开发的第一版结束.
