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
            REMOTEOPEN   =  0x40,     /// 远程开门
        }

    }
}
