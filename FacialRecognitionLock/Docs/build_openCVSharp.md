### 树莓派搭建opencvsharp环境

#### 请按照和我一样的环境
* 请先完成![树莓派环境配置](build_rpi3B.md)
* 请先完成![树莓派搭建.net环境](build_dotNet.md)
* 上述2步完成之后，再回来查看该笔记内容.
* 搭建完opencvsharp环境之后,再查看[使用vs开发、发布到树莓派环境运行](build_dotNet.md)

#### 前言
* 如果想一次性搭建成功，切记，一条一条命令输入,不要心急,不能出错，安装一次要3~4h！！
    * 请尽量使用ctrl+c/ctrl+v来输入命令,如果控制台复制有字数限制可以分2次复制粘贴！
    * 每一条命令最好写两遍
    * 第二遍会出现 xxx  is already the newest version (xx.xx.xx）则表示成功
* opencv、opencv_contrib、opencvsharp以及opencvsharp的.nuget包，这4个的版本要全部对应一样！！

#### (再次声明)本项目使用的opencvsharp环境
* 试过树莓派的官方系统但是依赖包没ubuntu的资源好.
* ubuntu-mate-16.04.2-desktop-armhf-raspberry-pi.img
* opencv-3.4.1.zip
* opencvsharp-3.4.1.20181108.zip
* opencv_contrib-3.4.1.zip
* mono 3.2.8 (支持.net framework 4.5, 不支持.net framework 4.6.1)
* 该笔记2019年4月,亲测opencv-4.0.0的版本太高不能安装到Ubuntu MATE 16.04,而3.4.1是可以的.而其他opencv历史版本未试过.建议先尝试走一遍3.4.1版本再自行尝试更新的版本
* 请最后先和我上面的环境保持一致之后，再进行opencvsharp搭建

* * *

#### 在RPI3B搭建opencvsharp环境
* 可以使用RPI3B+显示器+鼠标键盘的方式去操作控制台；也可以mstsc作RPI3B控制台(推荐)
* 所有的"//"、"#"、"--" 都是注释内容，只是解释说明。
* 接下里的命令"下载依赖包、下载opencv、下载opencv_contrib、下载opencvsharp"可以直接copy，中途需要切root管理员还是切回用户的都照我命令输入即可
* 最终所有东西都安装到 "/usr/local/opencv/"文件夹里
* 先在“/usr/local/”里创建"opencv"的文件夹
``` c
cd /usr/local/
su root    // 切换管理员
mkdir opencv
su pi      // 切回普通用户
cd ~       // 回到"~"目录下
```
##### 下载依赖包
* 中途遇到的y/n都选"y"
* 一条一条命令输入,每条命令输2次,出现“xxx  is already the newest version 则表示成功”
``` c
sudo apt-get install -y build-essential
sudo apt-get install -y cmake
sudo apt-get install -y libgtkglext1-dev
sudo apt-get install -y libvtk6-dev
sudo apt-get install -y zlib1g-dev
sudo apt-get install -y libjpeg-dev
sudo apt-get install -y libwebp-dev
sudo apt-get install -y libpng-dev
sudo apt-get install -y libtiff5-dev
sudo apt-get install -y libopenexr-dev
sudo apt-get install -y libgdal-dev
sudo add-apt-repository "deb http://security.ubuntu.com/ubuntu xenial-security main"
sudo apt update
sudo apt-get install -y libjasper1
sudo apt-get install -y libjasper-dev
sudo apt-get install -y libdc1394-22-dev
sudo apt-get install -y libavcodec-dev
sudo apt-get install -y libavformat-dev
sudo apt-get install -y libswscale-dev
sudo apt-get install -y libtheora-dev
sudo apt-get install -y libvorbis-dev
sudo apt-get install -y libxvidcore-dev
sudo apt-get install -y libx264-dev
sudo apt-get install -y yasm
sudo apt-get install -y libopencore-amrnb-dev
sudo apt-get install -y libopencore-amrwb-dev
sudo apt-get install -y libv4l-dev
sudo apt-get install -y libxine2-dev
sudo apt-get install -y libtbb-dev
sudo apt-get install -y libeigen3-dev
sudo apt-get install -y python-dev
sudo apt-get install -y python-tk
sudo apt-get install -y python-numpy
sudo apt-get install -y python3-dev
sudo apt-get install -y python3-tk
sudo apt-get install -y python3-numpy
sudo apt-get install -y libhdf5-dev
sudo apt-get install -y libtiff5-dev
# sudo apt-get install -y libtiffxx0c2 # 这个依赖包可以无(因为Not Found)
``` c
##### 下载opencv [OPENCV_VERSION=3.4.1]
* 打开控制台,默认下载到"~"的目录
* 如果不下载3.4.1版本,则可以套用以下句型,把‘${OPENCV_VERSION}’替换成版本‘4.0.0’
```
wget  ${OPENCV_VERSION}.zip && unzip ${OPENCV_VERSION}.zip && rm ${OPENCV_VERSION}.zip
su root  // 切换管理员
mv opencv-${OPENCV_VERSION} /usr/local/opencv/OpenCV
su pi    // “pi” 是用户名,切换回普通用户.
cd ~     // 以普通用户回到"~"的目录下
```
* 以下命令是下载opencv-3.4.1版本
``` c
wget https://github.com/opencv/opencv/archive/3.4.1.zip && unzip 3.4.1.zip && rm 3.4.1.zip
su root // 切换管理员
mv opencv-3.4.1/ /usr/local/opencv/OpenCV
su pi    // “pi” 是用户名,切换回普通用户.
cd ~     // 以普通用户回到"~"的目录下
```
##### 下载opencv_contrib [OPENCV_VERSION=3.4.1]
* 打开控制台,默认先下载到"~"的目录，然后mv到"/usr/local/opencv/"里,
* 如果不下载3.4.1版本,则可以套用以下句型,把‘${OPENCV_VERSION}’替换成版本‘4.0.0’
``` c
wget https://github.com/opencv/opencv_contrib/archive/${OPENCV_VERSION}.zip && unzip ${OPENCV_VERSION}.zip && rm ${OPENCV_VERSION}.zip
su root   // 切换成管理员
mv opencv_contrib-${OPENCV_VERSION} /usr/local/opencv/OpenCV_contrib
su pi
su ~
```
* 以下命令是下载opencv_contrib-3.4.1版本
``` c
wget https://github.com/opencv/opencv_contrib/archive/3.4.1.zip && unzip 4.0.0.zip && rm 3.4.1.zip
su root    // 切换成管理员
mv opencv_contrib-3.4.1 /usr/local/opencv/OpenCV_contrib
su pi      // “pi” 是用户名,切换回普通用户.
su ~       // 以普通用户回到"~"的目录下
```
##### 编译opencv
* 注意cmake 最后面是2个点‘..’
* cmake 那段命令有点长，一次性copy可能不允许,可以分几次copy
``` c
cd /usr/local/opencv/OpenCV
su root
mkdir build
cd build
sudo cmake -D OPENCV_EXTRA_MODULES_PATH=/usr/local/opencv/OpenCV_contrib/modules -D WITH_OPENMP=ON CMAKE_BUILD_TYPE=RELEASE -D BUILD_EXAMPLES=OFF -D BUILD_DOCS=OFF -D BUILD_PERF_TESTS=OFF -D BUILD_TESTS=OFF -D BUILD_opencv_java=OFF -D BUILD_opencv_python=OFF ..
sudo make -j4
sudo make install && ldconfig
```
##### 下载opencvsharp [OPENCVSHARP_VERSION=3.4.1.20180320]
* 先下载安装好opencv再下载暗转opencvsharp,如果opencv未安装成功则不要安装opencvsharp
``` c
cd /usr/local/opencv/
su root
git clone https://github.com/shimat/opencvsharp.git
cd opencvsharp
```
* 如果不下载3.4.1版本,可以套下面的命令，将'${OPENCVSHARP_VERSION}'替换成'4.0.0.20181130'
* 至于这个‘3.4.1.20180320’是到opencvsharp的github去[查看这里](https://github.com/shimat/opencvsharp)
``` c
git fetch --all --tags --prune && git checkout tags/${OPENCVSHARP_VERSION}
```
* 以下是下载opencvsharp-3.4.1.20180320版本
``` c
git fetch --all --tags --prune && git checkout tags/3.4.1.20180320
su pi
cd ~
cd /usr/local/opencv/opencvsharp
su root
mkdir make
cd make
sudo cmake -D CMAKE_INSTALL_PREFIX=/usr/local/opencv/opencvsharp/make /usr/local/opencv/opencvsharp/src
sudo make
sudo make install
```
##### 最后一步
* 请把上面的编译工作都完成之后,再进行最后一步操作
    * opencvsharp编译生成的/usr/local/opencv/opencvsharp/make/lib下的所有文件
    * opencv编译生成的/usr/local/opencv/OpenCV/build/lib的所有文件
    * 都copy到/usr/lib的文件夹下
* 使用远程桌面或者显示器可能不能手动copy，则可参考一下命令
    * 以下是先把这2个的lib先copy到树莓派Desktop再copy到"/usr/lib"文件夹下
    * 当然,如果你觉得直接copy到/usr/lib/文件夹也ok
    * 参考命令：cp <文件A路径> <要移动到的目标文件夹路径>
    * 参考命令: cp  -r <文件夹A路径> <要移动到的目标文件夹路径>
``` c
cd Desktop
mkdir opencvlib   // 先在Desktop里创建一个叫"opencvlib"文件夹
cd opencvlib
mkdir opencv
mkdir opencvsharp // 分别创建2个文件夹装"lib"
cd ~

cd /usr/local/opencv/opencvsharp/make/
su root
cp -r lib /home/pi/Desktop/opencvlib/opencvsharp
su pi
su ~

cd /usr/local/opencv/OpenCV/build/
su root
cp -r lib /home/pi/Desktop/opencvlib/opencv
su pi
su ~

cd Desktop
su root
cp -r opencvlib /usr/lib/
su pi
su ~
```

