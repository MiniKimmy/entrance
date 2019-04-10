#### AppConst.cs 项目控制台脚本
* 注:下文中出现的 "//"、"#"、"--" 都是注释,只是解释说明
* 初次运行项目需要自行配置好相关信息.
**需要配置的信息如下:**
``` csharp
在百度人脸识别网页控制台里查看你的应用信息，并填写到该脚本里.
public const string APP_ID = "你的appid";       // 暂时无用.(可以不填)
public static string API_KEY = "你的ak";        // AK
public static string SECRET_KEY = "你的sk";     // SK

public static string localServerIP = "192.168.1.108"; // RPI3B的IP地址,可以在树莓派控制台"ifconfig"查看.若"isOpenLocalServer"是false则项目不会使用该字段.

public static bool isDebugMode = true;       // true:在vs的"输出"能看到debug信息
public static bool isOpenLock = false;       // true:使用门锁模块
public static bool isOpenCamera = true;      // true:使用人脸识别模块
public static bool isOpenLocalServer = false;// true:使用本地测试(自己手动输入ip给"localServerIP")
public static bool isOpenLocalLock = false;  // true:使用本地测试(自己手动输入ip给"LockController"类里的"ip"字段变量)

// 以下信息最好不修改,若深入了解本项目代码后,可以自行修改,可"选中字段变量"->"查看所有引用(快捷键shift+F12)"看看下面字段变量的用途.
public const string serverPort = "60000";   // RPI3B的缺省端口
public const int lockPort = 60000;          // 门锁模块缺省端口号
public static double repeatActionDelta = 5; // 重复执行方法延迟
public static double lockOpenningDelay = 5; // 门锁模块开门延迟
```