/// <summary>
/// 常量类
/// </summary>
namespace EntranceGuard
{
    class AppConst
    {
        public const int messageSize = 64;			    // 报文长度,64字节byte
        public const int dataSize = 56;                 // 数据长度,56字节
        public const int type = 0x17;		            // 报文类型
        public const int controllerPort = 60000;        // 控制器端口号port（缺省端口）

        //public const long SpecialFlag = 0x55AAAA55;     // 特殊标识 防止误操作
    }
}
