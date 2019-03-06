namespace EntranceGuard
{
    /// <summary>
    /// 静态工具类.
    /// </summary>
    class Utility
    {
        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="functionID">功能号枚举</param>
        /// <param name="entrancePORT">controller的端口号</param>
        /// <returns>返回是否发送成功:-1失败、0成功发送但匹配失败、1成功发送且匹配成功</returns>
        public static int SendCommandMsg(AppConst.Funtion functionID)
        {
            /// 创建报文命令
            MessageFactory factory = new MessageFactory();
            var msg = factory.CreateMessage(functionID);
            var cmd = msg.Message;

            /// 门锁主控制
            WG3000_COMM.Core.wgMjController entranceCtrl = new WG3000_COMM.Core.wgMjController();
            entranceCtrl.IP = msg.ControllerIP;
            entranceCtrl.PORT = msg.ControllerPort;

            int ret = 0;
            byte[] recv = new byte[msg.MessageSize]; // 接收报文

            /// 发送报文
            if (entranceCtrl.ShortPacketSend(cmd, ref recv) < 0)
            {
                // 发送失败
                FacedeTool.Debug(Controller.Instance,"send cmd failed");
                ret = -1;
            }
            else
            {
                // 发送成功, 匹配接收报文信息.
                if ((recv[0] == msg.MessageType) && (recv[1] == msg.FunctionID))
                {
                    ret = 1;
                }
            }

            entranceCtrl.Dispose();
            return ret;
        }
    }
}
