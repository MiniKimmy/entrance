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
            InitializeComponent();
            instance = this;
        }

        /// <summary>
        /// 初次启动.exe程序时的初始化.
        /// </summary>
        private void Controller_Load(object sender, EventArgs e)
        {
            FacadeTool.Debug(this, "Loading...");
            this.input_ServerIP.Text = Utility.AutoSearchServerIP();
            this.input_ServerPort.Text = AppConst.controllerPort.ToString();

            if(this.input_ServerIP.Text != string.Empty){
                FacadeTool.Debug(this, "serverIP:" + this.input_ServerIP.Text + ",serverPORT:" + this.input_ServerPort.Text);
            }

            FacadeTool.Debug(this, "init ok");
        }

        /// <summary>
        /// 自动搜索控制器SN和IP按钮事件处理.
        /// </summary>
        private void SearchControllerSN_Click(object sender, EventArgs e)
        {
            FacadeTool.Debug(this, "CLICK searching SN...");
            this.Cursor = Cursors.WaitCursor;

            Utility.AutoSerachControllerSNandIP();

            this.Cursor = Cursors.Default;
            if (this.controllerSN != null && this.ControllerIP != null){
                FacadeTool.Debug(this, "controllerIP:" + this.controllerSN + ",controllerIP:" + this.ControllerIP);
            }

        }

        /// <summary>
        /// 远程开门按钮事件处理
        /// </summary>
        private void TestOpen_Click(object sender, EventArgs e)
        {
            FacadeTool.Debug(this, "CLICK remote open door...");

            if (!Utility.CheckController()) return;
            Utility.SendCommandMsg(AppConst.Funtion.REMOTEOPEN);
        }
    }
}
