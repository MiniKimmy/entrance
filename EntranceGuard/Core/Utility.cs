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
        /// <param name="functionID">功能号</param>
        /// <param name="entrancePORT">controller的端口号</param>
        public static void SendCommandMsg(AppConst.Funtion functionID)
        {
            /// 创建报文
            MessageFactory factory = new MessageFactory();
            var msg = factory.CreateMessage(functionID);
            if (msg == null){
                FacadeTool.Debug(Controller.Instance, "no such funtionID");
                return;
            }

            var cmd = msg.CmdMsg;                        // byte[]请求报文
            using (WG3000_COMM.Core.wgMjController entranceCtrl = new WG3000_COMM.Core.wgMjController())
            {
                byte[] recv = new byte[msg.MessageSize]; // byte[]接收报文
                entranceCtrl.IP = msg.ControllerIP;      // 配置门锁IP地址
                entranceCtrl.PORT = msg.ControllerPort;  // 配置门锁Port端口

                if (entranceCtrl.ShortPacketSend(cmd, ref recv) < 0){
                    // 发送失败.
                    FacadeTool.Debug(Controller.Instance, "send cmd failed"); 
                }
                else{
                    // 发送成功, 匹配接收报文信息.
                    if ((recv[0] == msg.MessageType) && (recv[1] == msg.FunctionID)){
                        msg.HandleReceiveMsg(recv);       // 处理接收的报文
                    }
                }
            }
        }

        /// <summary>
        /// 检查控制器配置是否正常.
        /// </summary>
        /// <returns></returns>
        public static bool CheckController()
        {
            if (string.IsNullOrEmpty(Controller.Instance.ControllerSN)) {
                System.Windows.Forms.MessageBox.Show("please check the controller has connected network.");
                return false;
            }

            else if (string.IsNullOrEmpty(Controller.Instance.Input_ServerPort)){
                System.Windows.Forms.MessageBox.Show("please complete the Server Port.");
                return false;
            }

            else if (string.IsNullOrEmpty(Controller.Instance.Input_ServerIP))
            {
                System.Windows.Forms.MessageBox.Show("please complete the Server IP.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 自动查找本地服务器IP地址.
        /// </summary>
        /// <returns>IP地址</returns>
        public static string AutoSearchServerIP()
        {
            string hostName = System.Net.Dns.GetHostName();
            bool st = false;
            string ret = string.Empty;

            // 获取主机的IP地址列表 获取主机的IP地址(只允许Ipv4通过)
            foreach (System.Net.IPAddress ipAddress in System.Net.Dns.GetHostEntry(hostName).AddressList)
            {
                if (ipAddress.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork) continue;
                if (ipAddress.IsIPv6LinkLocal) continue;
                if (ipAddress.ToString() == "127.0.0.1") continue;

                if (st){
                    System.Windows.Forms.MessageBox.Show("电脑存在多个IP, 建议只使用一个IP操作, 若无线与网线口同时在用时, 请关键无线口");
                    ret = string.Empty;
                    return ret;
                }
                
                st = true; // 连接成功
                ret = ipAddress.ToString();
            }

            if (!st) System.Windows.Forms.MessageBox.Show("network is not available.");
            return ret;
        }

        /// <summary>
        /// 自动查找控制器SN序列号和控制器IP地址.
        /// </summary>
        /// <returns>str[0]=sn,str[1]=ip,if error=null</returns>
        public static string[] AutoSerachControllerSNandIP()
        {
            using (WG3000_COMM.Core.wgMjController controllers = new WG3000_COMM.Core.wgMjController())
            {
                System.Collections.ArrayList arrControllers = new System.Collections.ArrayList();
                controllers.SearchControlers(ref arrControllers);
                if (arrControllers != null) {
                    if (arrControllers.Count <= 0) {
                        System.Windows.Forms.MessageBox.Show("Not found any controllerSN and controllerIP");
                        return null;
                    }

                    string[] config = arrControllers[0].ToString().Split(',');
                    FacadeTool.Debug(Controller.Instance, "controller connected success");
                    return config;
                }
            }
            return null;
        }

    }
}
