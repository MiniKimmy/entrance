/// <summary>
/// Controller的C层
/// </summary>
namespace EntranceGuard
{
    using System;
    using System.Windows.Forms;
    using System.Collections.Generic;

    public partial class Controller : Form
    {
        private static Controller instance = null;
        public static Controller Instance{ get { return instance; } }

        /// <summary>
        /// 初始化View层.加载winform
        /// </summary>
        public Controller()
        {
            instance = this;

            InitializeComponent();
            FacedeTool.Debug(this, "init ok..");
        }

        /// <summary>
        /// 启动.exe程序时的初始化.
        /// </summary>
        private void Controller_Load(object sender, EventArgs e)
        {
            string hostName = System.Net.Dns.GetHostName();
            bool st = false;

            // 获取主机的IP地址列表 获取主机的IP地址
            foreach (System.Net.IPAddress ipAddress in System.Net.Dns.GetHostEntry(hostName).AddressList)
            {
                // 只允许Ipv4通过
                if (ipAddress.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    continue;
                }

                if (ipAddress.IsIPv6LinkLocal) {
                    continue;
                }

                if (ipAddress.ToString() == "127.0.0.1") {
                    continue;
                }

                if (st)
                {
                    MessageBox.Show("电脑存在多个IP, 建议前期开发时只使用一个IP操作.  [假如无线与网线口同时在用时, 请关键无线口]");
                    break;
                }

                // 连接成功
                st = true;

                // 自动设置好主机IP和PORT
                FacedeTool.Debug(this, ipAddress.ToString());
                this.input_ServerIP.Text = ipAddress.ToString();
                this.input_ServerPort.Text = AppConst.controllerPort.ToString() ;
            }

            if (!st) {
                MessageBox.Show("无法连接网络");
            }
        }

        /// <summary>
        /// 自动搜索控制器SN和IP.
        /// </summary>
        private void SearchControllerSN_Click(object sender, EventArgs e)
        {
            FacedeTool.Debug(this, "SearchControllerSN");

            /// 变成沙漏鼠标(--处于等待状态)
            this.Cursor = Cursors.WaitCursor;

            System.Collections.ArrayList arrControllers = new System.Collections.ArrayList();

            /// 搜索控制器
            using (WG3000_COMM.Core.wgMjController controllers = new WG3000_COMM.Core.wgMjController())
            {
                controllers.SearchControlers(ref arrControllers);
            }

            /// 变成箭头鼠标(--搜索完毕，恢复鼠标正常状态)
            this.Cursor = Cursors.Default;

            if (arrControllers != null)
            {
                if (arrControllers.Count <= 0)
                {
                    MessageBox.Show("Not found any controllerSN and IP");
                    FacedeTool.Debug(this, "failed to connected");
                    return;
                }

                string[] config = arrControllers[0].ToString().Split(',');
                this.text_ControllerSN.Text = config[0];
                this.text_ControllerIP.Text = config[1];

                FacedeTool.Debug(this, "ControllerSN={0},ControllerIP={1}\nServerIP={2}\n", this.text_ControllerSN.Text, this.text_ControllerIP.Text,"search success");
            }
        }

        /// <summary>
        /// 远程开门:0x40
        /// </summary>
        private void TestOpen_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.text_ControllerSN.Text)){
                MessageBox.Show("请查看控制器连接网络是否正常");
                return;
            }

            int ret = 0;
            ret = Utility.SendCommandMsg(AppConst.Funtion.REMOTEOPEN);

            if (ret > 0)
            {
                FacedeTool.Debug(this, "remote-open success..");
            }

            else
            {
                FacedeTool.Debug(this, "remote-open failed..");
            }
        }
    }
}
