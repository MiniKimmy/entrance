/// <summary>
/// 常量类
/// </summary>
namespace EntranceGuard
{
    public class AppConst
    {

        #region app
        public static bool isOpenLock = true;                      // 是否使用控制器
        public static bool isOpenCamera = true;                    // 是否使用摄像头
        public static bool isOpenLocalServer = true;               // 是否使用本地ip服务器
        public static bool isOpenLocalLock = true;                // 是否使用本地ip门锁控制器
        public static double repeatActionDelta = 5.0;              // 重复执行延迟
    

        #endregion

        #region RPI3B
        public static string localServerIP = "192.168.1.108";      // RPI3B的IP地址(本地测试)

        #endregion

        #region lock
        public static string localControllerIP = "192.168.1.120";  // 控制器的IP地址(本地测试)
        public const int controllerPort = 60000;                   // 控制器默认缺省端口
        public static int lockOpenningDelay = 6;                   // 开门延时
        public enum Funtion : byte                                 // 门锁协议功能号
        {
            NONE         =  0x00,     /// 无
            STATUS       =  0x20,    /// 查询状态
            REMOTEOPEN   =  0x40,     /// 远程开门
        }

        #endregion

        #region camera

        /// 人脸识别sdk配置 
        public static string APP_ID = "15700137";
        public static string API_KEY = "xNEfNoAZCr78tX2Wv4WfeRDy";
        public static string SECRET_KEY = "V0vM0hhw0n4hcVVGFvGYMSBVzHr5klDx";

        /// opencvsharp
        public static string HaarCascadeAlt = @"Data/haarcascade_frontalface_alt.xml";

        #endregion
    }
}
