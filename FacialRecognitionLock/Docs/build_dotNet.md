#### 树莓派搭建.net环境
* 注:下文中出现的 "//"、"#"、"--" 都是注释,只是解释说明

##### 请按照和我一样的环境
* 请先完成![树莓派环境配置](build_rpi3B.md)

##### 下载mono
* 打开控制台,输入以下命令
``` c
sudo apt-get install mono-complete
mono -V    // 有信息则表示安装mono成功
```

##### 测试csharp
* 打开控制台,输入以下命令
``` csharp
csharp

// 出现csharp的前缀之后
Console.WriteLine(DateTime.Now)

// 执行结果是返回当前时间. 退出csharp使用"exit"命令
```

##### 测试mono
* 打开控制台,输入以下命令
``` csharp
cd Desktop             // 切到桌面目录
nano HelloWorld.cs     // 创建"HelloWorld"的cs文件

//nano编辑下，输入以下代码
namespace Hello{
    using System;
    class Program{
        static void Main(){
            Console.WriteLine("Hello HelloWorld!");
        }
    }
}

// 按ctrl+o保存;保存之后按ctrl+x退出.
// 重新回到控制台,输入以下命令
mcs HelloWorld.cs     // 编译该.cs脚本,然后桌面会生成.exe的文件
mono HelloWorld.exe   // 使用mono执行.exe文件,看到出现"Hello HelloWorld!"则测试成功.
```