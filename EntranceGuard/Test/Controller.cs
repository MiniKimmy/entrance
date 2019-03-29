/// <summary>
/// Controller的C层
/// </summary>
namespace EntranceGuard
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    

    public partial class Controller : Form
    {
        private static Controller instance = null;
        public static Controller Instance{ get { return instance; } }

        private OpencvSharpHelper m_OpencvSharpHelper;

        /// <summary>
        /// 初始化View层.加载winform
        /// </summary>
        public Controller()
        {
            InitializeComponent();
            RemoveFlashing();
        /*
            Style style = new Style();
            style.TitleBackColor = Color.FromArgb(27, 123, 210);
            style.MinBoxBackColor = Color.FromArgb(70, Color.White);
            style.MaxBoxBackColor = Color.FromArgb(70, Color.White);
       */
            instance = this;
            m_OpencvSharpHelper = new OpencvSharpHelper(); 
        }


        #region 减少闪烁

        /// </summary>
        /// 减少闪烁
        /// </summary>
        private void RemoveFlashing()
        {
            Application.AddMessageFilter(new MouseMsgFilter());
            MouseMsgFilter.MouseMove += OnGlobalMouseMove;
            base.FormBorderStyle = FormBorderStyle.Sizable;
            SetStyles();
        }

        /// <summary>
        /// 鼠标消息过滤器
        /// </summary>
        internal class MouseMsgFilter : IMessageFilter
        {
            private const int mousemove = 0x0200;
            public static event MouseEventHandler MouseMove;

            public bool PreFilterMessage(ref Message m)
            {
                if (m.Msg == mousemove)
                {
                    if (MouseMove != null)
                    {
                        int x = Control.MousePosition.X;
                        int y = Control.MousePosition.Y;
                        MouseMove(null, new MouseEventArgs(MouseButtons.None, 0, x, y, 0));
                    }
                }
                return false;
            }
        }

        /// </summary>
        /// 使用双缓冲
        /// </summary>
        private void SetStyles()
        {
            this.DoubleBuffered = true;              //设置本窗体
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint | // 禁止擦除背景.
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.DoubleBuffer, true);   // 双缓冲

            UpdateStyles(); //强制分配样式重新应用到控件上
            base.AutoScaleMode = AutoScaleMode.None;
        }

        /// </summary>
        /// 禁掉清除背景消息
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0014) return;            
            base.WndProc(ref m);
        }
      
        /// <summary>
        /// Convert to client position and pass to Form.MouseMove
        /// </summary>
        private void OnGlobalMouseMove(object sender, MouseEventArgs e)
        {
            if (IsDisposed) return;

            var clientCursorPos = PointToClient(e.Location);
            var newE = new MouseEventArgs(MouseButtons.None, 0, clientCursorPos.X, clientCursorPos.Y, 0);
            OnMouseMove(newE);
        }
        #endregion

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

            // 启动摄像头功能.

            FacadeTool.Debug(this, "init ok");
        }


        /// <summary>
        /// 自动搜索控制器SN和IP按钮事件处理.
        /// </summary>
        private void SearchControllerSN_Click(object sender, EventArgs e)
        {
            FacadeTool.Debug(this, "CLICK searching SN...");
            this.Cursor = Cursors.WaitCursor;

            var config = Utility.AutoSerachControllerSNandIP();

            this.Cursor = Cursors.Default;
            if (config == null){
                FacadeTool.Debug(this, "controllerIP:" + this.controllerSN + ",controllerIP:" + this.ControllerIP);
                return;
            }

            this.controllerSN = config[0];
            this.controllerIP = config[1];
        }

        /// <summary>
        /// 远程开门按钮事件处理.
        /// </summary>
        private void TestOpen_Click(object sender, EventArgs e)
        {
            FacadeTool.Debug(this, "CLICK remote open door...");

            if (!Utility.CheckController()) return;
            Utility.SendCommandMsg(AppConst.Funtion.REMOTEOPEN);
        }

        // 关闭winform
        private void Controller_Destroy(object sender, FormClosingEventArgs e)
        {
            Dispose(true);
            
            /*
            if (videoSourcePlayer1.VideoSource != null)
            {
                videoSourcePlayer1.SignalToStop();
                videoSourcePlayer1.WaitForStop();
                videoSourcePlayer1.VideoSource = null;
            }
            */
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (m_OpencvSharpHelper.CanCamera)
            {
                m_OpencvSharpHelper.Stop();
            }
            else
            {
                m_OpencvSharpHelper.Start();

            }
           
            
        }
    }
}
