namespace EntranceGuard
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

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
                FacadeTool.Debug("no such funtionID");
                return;
            }

            var cmd = msg.CmdMsg;                        // byte[]请求报文
            WG3000_COMM.Core.wgMjController entranceCtrl = new WG3000_COMM.Core.wgMjController();
            
            byte[] recv = new byte[msg.MessageSize]; // byte[]接收报文
            entranceCtrl.IP = msg.ControllerIP;      // 配置门锁IP地址
            entranceCtrl.PORT = msg.ControllerPort;  // 配置门锁Port端口

            if (entranceCtrl.ShortPacketSend(cmd, ref recv) < 0)
            {
                FacadeTool.Debug("send cmd failed"); // 发送失败.
            }
            else
            {
                // 发送成功, 匹配接收报文信息.
                if ((recv[0] == msg.MessageType) && (recv[1] == msg.FunctionID))
                {
                    msg.HandleReceiveMsg(recv);      // 处理接收的报文
                }
            }


            //using (WG3000_COMM.Core.wgMjController entranceCtrl = new WG3000_COMM.Core.wgMjController())
            //{
            //    byte[] recv = new byte[msg.MessageSize]; // byte[]接收报文
            //    entranceCtrl.IP = msg.ControllerIP;      // 配置门锁IP地址
            //    entranceCtrl.PORT = msg.ControllerPort;  // 配置门锁Port端口

            //    if (entranceCtrl.ShortPacketSend(cmd, ref recv) < 0){
            //        FacadeTool.Debug("send cmd failed"); // 发送失败.
            //    }
            //    else{
            //        // 发送成功, 匹配接收报文信息.
            //        if ((recv[0] == msg.MessageType) && (recv[1] == msg.FunctionID)){
            //            msg.HandleReceiveMsg(recv);      // 处理接收的报文
            //        }
            //    }
            //}
            
        }

        /// <summary>
        /// 检查控制器配置是否正常.
        /// </summary>
        /// <returns></returns>
        public static bool CheckEntranceInit()
        {
            if ((string.IsNullOrEmpty(Controller.Instance.ControllerSN) && AppConst.isOpenLock) ||
                string.IsNullOrEmpty(Controller.Instance.ServerIP)) 
                    return false;

            return true;
        }

        /*
        /// <summary>
        /// 自动查找本地服务器IP地址.
        /// </summary>
        /// <returns>IP地址</returns>
        public static string AutoSearchServerIP()
        {
            string hostName = System.Net.Dns.GetHostName();
            bool st = false;
            string ret = null;

            // 获取主机的IP地址列表 获取主机的IP地址(只允许Ipv4通过)
            foreach (System.Net.IPAddress ipAddress in System.Net.Dns.GetHostEntry(hostName).AddressList)
            {
                if (ipAddress.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork) continue;
                if (ipAddress.IsIPv6LinkLocal) continue;
                if (ipAddress.ToString() == "127.0.0.1") continue;

                
                //if (st){
                //    System.Diagnostics.Debug.WriteLine("电脑存在多个IP, 建议只使用一个IP操作, 若无线与网线口同时在用时, 请关键无线口");
                //    ret = null ;
                //    return ret;
                //}
                

                st = true; // 连接成功
                ret = ipAddress.ToString();
            }

            if (!st) FacadeTool.Debug("network is not available.");
            return ret;
        }
        
        */

        /// <summary>
        /// 自动查找控制器SN序列号和控制器IP地址.
        /// </summary>
        /// <returns>str[0]=sn,str[1]=ip,if error=null</returns>
        public static string[] AutoSerachControllerSNandIP()
        {
            System.Collections.ArrayList arrControllers = new System.Collections.ArrayList();

            WG3000_COMM.Core.wgMjController controllers = new WG3000_COMM.Core.wgMjController();
            controllers.SearchControlers(ref arrControllers);

            /*
            using (WG3000_COMM.Core.wgMjController controllers = new WG3000_COMM.Core.wgMjController())
            {
                controllers.SearchControlers(ref arrControllers);
            }
            */
            if (arrControllers == null || arrControllers.Count <= 0) {
                    FacadeTool.Debug("Not found any controllerSN and controllerIP");
                    return null;
            }

            string[] config = arrControllers[0].ToString().Split(',');
            FacadeTool.Debug("controller connected success");
            return config;
        }

        /// <summary>
        /// 间隔时间重复执行方法
        /// </summary>
        /// <param name="action">间隔执行的方法</param>
        /// <param name="time">间隔时间</param>
        /// <param name="check">执行停止判断</param>
        /// <param name="callback">回调方法</param>
        public static async void RepeatAction(Action action, double time, Func<bool> check, Action callback = null)
        {
            while (true)
            {
                using (Task t = new Task(action))
                {
                    t.Start();
                    await Task.Delay(TimeSpan.FromSeconds(time));
                    FacadeTool.Debug("repeat not ok");
                }

                if (check())
                {
                    FacadeTool.Debug("check ok");
                    if (callback != null) callback();
                    break;
                }
            }
        }

        /// <summary>
        /// 获取当前时间.(默认:hh:mm:ss格式)
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentTime()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }


        /// <summary>
        /// 延迟执行方法
        /// </summary>
        /// <param name="action">方法</param>
        /// <param name="time">延迟时间s(秒)</param>
        public static void Invoke(Action action, double time)
        {
            System.Timers.Timer timer = new System.Timers.Timer((int)(time*1000));
            timer.AutoReset = false;
            timer.Elapsed += new System.Timers.ElapsedEventHandler((x, y) => { action(); });
            timer.Start();
            FacadeTool.Debug("wait " + time);
        }


        /// <summary>
        /// 文本内容写入txt(异步调用)
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        private static Task WriteAllText(string path, string content)
        {
            var t = new Task(() =>{
                System.IO.File.WriteAllText(path, content);
            });
            t.Start();
            return t;
        }

        /// <summary>
        /// 文件图片字节数组转化为BAS64格式
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static string GetBASE64(byte[] image)
        {
            return Convert.ToBase64String(image);
        }

        /// <summary>
        /// 文件图片字节数组转化为BAS64格式
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Task<string> GetBASE64_2(byte[] image)
        {
            var t = new Task<string>(() => Convert.ToBase64String(image));
            t.Start();
            return t;
        }
    }
}
