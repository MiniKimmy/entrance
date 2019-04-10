#### 树莓派环境配置
* 注:下文中出现的 "//"、"#"、"--" 都是注释,只是解释说明.

##### 请按照和我一样的环境
* windows系统电脑
* 读卡器(若手提电脑有插卡的口可不需要读卡器)
* tiff卡(大小 16G)

##### 下载镜像
* **以下是下载链接,直接下载,保存在win10电脑里**
* [下载ubuntu-mate-16.04.2-desktop-armhf-raspberry-pi.img](https://ubuntu-mate.org/raspberry-pi/ubuntu-mate-16.04.2-desktop-armhf-raspberry-pi.img.xz)
* [下载win32diskimager工具](https://sourceforge.net/projects/win32diskimager/files/latest/download):用于烧写镜像.注意:在写完系统后，可能会提示格式化，选择“取消”就可以了.
* [下载DiskGenius分区助手,提取码:skdh](链接：https://pan.baidu.com/s/17ceoNHpm12yPP6tgYj10Uw):用于格式化读卡器.把读卡器直接格式化FAT32(只分1个盘).

##### 进入RPI3B操作系统
* 使用显示器、HDMI-VGA转化线、鼠标、usb键盘、使用充电头+线供电树莓派、插上tiff卡
* 初次进入需要设置用户信息:建议所有的用户名name都用"pi",所有的用户密码password都用"raspberry"

###### 开启ssh
* [参考](https://blog.csdn.net/wuleiming2009/article/details/78918950)
* 打开控制台(快捷键ctrl+alt+t),输入以下命令
``` c
sudo raspi-config    // 选择第3项"internet options" ->选择第二项ssh->开启yes
ps -e | grep ssh     // 看到sshd则表示开启成功
sudo passwd root     // 先更改密码(最好使用"raspberry"作为密码,以免忘记)
sudo vim /etc/ssh/sshd_config
// 找到 “PermitRootLogin without-password” 或者 “PermitRootLogin prohibit-password” 修改成 “PermitRootLogin yes”
sudo service ssh restart // 重启SSH服务
```

###### 开启xrdp远程桌面
* [参考](https://blog.csdn.net/qq_25556149/article/details/82216190)
* 用于windows在win+R使用"mstsc",输入RPI3B的ip地址来远程桌面
* 打开控制台(ctrl+alt+t),输入以下命令
``` c
sudo apt-get update
sudo apt-get upgrade
sudo apt-get install tightvncserver xrdp
```

###### 关闭蓝牙自动启动
* [参考](https://blog.csdn.net/qq_25556149/article/details/82216190)
* 亲测好像不行.但是还是照做了.
* 打开控制台（ctrl+alt+t）,输入以下命令
``` c
sudo apt install gedit
sudo gedit /etc/rc.local
添加：rfkill block bluetooth  // 找到最后一行"exit 0" 的上面，使用菜单栏的“Save”，保存
sudo reboot                   // 然后重启树莓派看看是否还有蓝牙
```

###### 设置wifi(ubuntu-mate不需要控制台来设置)
* [参考](https://www.jianshu.com/p/9795cd0d7f60)
* 通过右上角图标“Connect to hidden network.”,--> Add
* 不需要用填“BSSID”、“Cloned MAC address”
* 选择"Client"、"automatic"、"WPA & WPA2 Personal"
* ssid是wifi名字，其余自定义。

###### 设置wifi(使用控制台设置的方式)
* 最好先下载vim、gedit编辑,后期有用
``` c
sudo apt-get install vim
sudo apt install gedit
```
* 打开控制台,输入以下命令
``` c
sudo vim /etc/wpa_supplicant/wpa_supplicant.conf

vim编辑里添加wifi配置信息：
network={
    ssid="aaaaaa"   // ssid:Wifi的名字
    psk="123456"    // psk:wifi密码
    key_mgmt=WPA-PSK
    scan_ssid=1     // scan_ssid:连接隐藏WiFi时需要指定该值为1
    priority=1      // priority:连接优先级，数字越大优先级越高（不可以是负数）
}
```

###### 显示中文(可选,但建议别用中文,避免不必要的bug)
* 在“System”->"Personal" -> “Language Support”
* 点install安装中文
* 然后拖拽“中文”到顶部最上面。
* 然后Apply一下
* 控制台输入:sudo reboot ,重启即可.

