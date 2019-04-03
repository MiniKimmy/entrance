/// <summary>
/// 常量类
/// </summary>
namespace EntranceGuard
{
    public class AppConst
    {
        // 控制器缺省端口
        public const int controllerPort = 60000;

        // 功能号
        public enum Funtion : byte
        {
            NONE         =  0x00,     /// 无
            STATUS        = 0x20,     /// 查询状态
            REMOTEOPEN   =  0x40,     /// 远程开门
        }

        public static bool isOpenLock = true;        // 是否使用控制器
        public static bool isOpenCamera = true;       // 是否使用摄像头
        public static bool isOpenLocalServer = true;  // 是否使用本地ip服务器
        public static bool isOpenLocalLock = false;   // 是否使用本地ip门锁控制器
        public static double repeatActionDelta = 5.0; // 重复执行延迟
        public static int lockOpenningDelay = 6;      // 开门延时

        // 人脸识别Api
        public static string APP_ID = "15700137";
        public static string API_KEY = "xNEfNoAZCr78tX2Wv4WfeRDy";
        public static string SECRET_KEY = "V0vM0hhw0n4hcVVGFvGYMSBVzHr5klDx";

        // opencvsharp人脸检测
        public static string HaarCascadeAlt = @"Data/haarcascade_frontalface_alt.xml";
    }
}
