#### 搭建ssh文件传输环境

##### 下载安装
* [下载ssh secure shell client 3.2.9](https://dlc2.pconline.com.cn/filedown_398471_7208846/nZAUN1b9/pconline1477391532068.zip)
* 下载安装之后,运行SshClient.exe

##### 配置ssh文件传输
* 打开树莓派控制台(ctrl+alt+t),输入以下命令
* 注意:下面这3段分开copy，因为可能控制台接收的字符数量有限
    * 第3段KexAl..开头的这段分成可以2半来copy
``` c
sudo apt-get install vim
sudo vim /etc/ssh/sshd_config

// vim编辑下,找到 # See the sshd_config(5) manage for details 的这一行上面添加一下信息
Ciphers aes128-cbc,aes192-cbc,aes256-cbc,aes128-ctr,aes192-ctr,aes256-ctr,3des-cbc,arcfour128,arcfour256,arcfour,blowfish-cbc,cast128-cbc

MACs hmac-md5,hmac-sha1,umac-64@openssh.com,hmac-ripemd160,hmac-sha1-96,hmac-md5-96

KexAlgorithms diffie-hellman-group1-sha1,diffie-hellman-group14-sha1,diffie-hellman-group-exchange-sha1,diffie-hellman-group-exchange-sha256,ecdh-sha2-nistp256,ecdh-sha2-nistp384,ecdh-sha2-nistp521,diffie-hellman-group1-sha1,curve25519-sha256@libssh.org


// 然后保存 :wq, 回到控制台，启动sshd服务
sudo /etc/init.d/ssh restart
```
* 参考如下
<img src="https://raw.githubusercontent.com/MiniKimmy/entrance/master/FacialRecognitionLock/Docs/resources/buildRPI3B/sshClientConfig.png" alt="can't find .png" width="800px">



##### 使用ssh文件传输软件
* 先启动树莓派，再sshClient软件登录树莓派

<img src="https://raw.githubusercontent.com/MiniKimmy/entrance/master/FacialRecognitionLock/Docs/resources/buildRPI3B/sshClientLogin.png?" alt="can't find .png" width="800px">

* 远程传输文件的按钮
<img src="https://raw.githubusercontent.com/MiniKimmy/entrance
/FacialRecognitionLock/FacialRecognitionLock/Docs/resources/buildRPI3B/sshClientUI.png?raw=true" alt="can't find .png" width="800px">

* 使用拖拽的方式互传文件
    * 如果想拖拽到root的文件夹的话，建议先拖拽到"~"文件夹,再通过命令行启动管理员身份"mv"过去

<img src="https://raw.githubusercontent.com/MiniKimmy/entrance/master/FacialRecognitionLock/Docs/resources/buildRPI3B/sshClientIO.png?" alt="can't find .png" width="800px">

