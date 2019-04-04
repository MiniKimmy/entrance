namespace FacialRecognitionLock
{
    public class AppConst
    {
        public static bool isDebugMode = false;                                          // 是否使用Demo测试模式
        public static bool isOpenLock = true;                                            // 是否使用控制器
        public static bool isOpenCamera = true;                                          // 是否使用摄像头
        public static bool isOpenLocalServer = false;                                    // 是否使用本地ip服务器
        public static bool isOpenLocalLock = false;                                      // 是否使用本地ip门锁控制器

        public static string localControllerIP = "192.168.1.120";                        // 控制器lock的IP地址(本地测试)
        public static string localServerIP = "192.168.1.108";                            // RPI3B的IP地址(本地测试)
        public const string serverPort = "60000";                                        // 本地服务器默认缺省端口
        public const int lockPort = 60000;                                               // lock主控制器默认缺省端口

        public static double repeatActionDelta = 5;                                      // 重复执行方法延迟
        public static double lockOpenningDelay = 5;                                      // 开门延迟

        /// Baidu sdk配置 (通过ak、sk换取AI调用web接口)
        public const string APP_ID = "15700137";                                         // 暂时无用.
        public static string API_KEY = "xNEfNoAZCr78tX2Wv4WfeRDy";                       // AK
        public static string SECRET_KEY = "V0vM0hhw0n4hcVVGFvGYMSBVzHr5klDx";            // SK
        public static string HaarCascadeAlt = @"data/haarcascade_frontalface_alt.xml";   // HaarCascadeAlt算法人脸检测xml配置
    }
}
