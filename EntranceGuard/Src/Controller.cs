using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntranceGuard
{
    using System;
    using System.Windows.Forms;
    using System.Net;

    public partial class Controller : Form
    {
        /// <summary>
        /// 初始化View层
        /// </summary>
        public Controller()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 启动.exe程序时的初始化
        /// </summary>
        private void Controller_Load(object sender, EventArgs e)
        {
            string hostName = Dns.GetHostName();
            bool st = false;

            // 获取主机的IP地址列表 获取主机的IP地址
            foreach (IPAddress ipaddr in Dns.GetHostEntry(hostName).AddressList)
            {
                // 只允许Ipv4通过
                if (ipaddr.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    continue;
                }

                if (ipaddr.IsIPv6LinkLocal)
                {
                    continue;
                }

                if (ipaddr.ToString() == "127.0.0.1")
                {
                    continue;
                }

                if (st)
                {
                    MessageBox.Show("电脑存在多个IP, 建议前期开发时只使用一个IP操作.  [假如无线与网线口同时在用时, 请关键无线口]");
                    break;
                }

                // 连接成功
                st = true;

                FacedeTool.Debug(this, ipaddr.ToString());
                //txtWatchServerIP.Text = ipaddr.ToString();
            }

            if (!st)
            {
                MessageBox.Show("网络不通! 请接好网线..");
            }

        }

        /// <summary>
        /// 自动搜索控制器SN和IP
        /// </summary>
        private void SearchControllerSN_Click(object sender, EventArgs e)
        {
            FacedeTool.Debug(this, "searching");
        }
    }
}
