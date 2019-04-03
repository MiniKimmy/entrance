/// <summary>
/// Controller的C层
/// </summary>
namespace EntranceGuard
{
    using OpenCvSharp;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Newtonsoft.Json.Linq;

    public partial class Controller : Form
    {
        private string controllerSN = null;
        public string ControllerSN { get { return controllerSN; } private set { controllerSN = value;} }

        private string controllerIP = null;
        public string ControllerIP { get { return controllerIP; } private set { controllerIP = value; } }

        private string serverIP = null;
        public string ServerIP { get { return serverIP; } private set { serverIP = value; } }

        private string serverPort = null;
        public string ServerPort { get { return serverPort; } private set { serverPort = value; } }

        private static Controller instance;
        public static Controller Instance { get { return instance; } }
        private OpencvSharpHelper m_OpencvSharpHelper;  //opencvsharp

        private List<Task> callbackList;
        public bool isLockOpenning = false;
        //protected readonly object locker = new object();

        /// <summary>
        /// 初始化View层.加载winform
        /// </summary>
        public Controller()
        {
            InitializeComponent();
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
            RemoveFlashing();
            m_OpencvSharpHelper = new OpencvSharpHelper();
            callbackList = new List<Task>();
            instance = this;
            FacadeTool.Debug("Loading...");
            Reset();
        }

        /// <summary>
        /// 复位
        /// </summary>
        private void Reset()
        {
            if (Utility.CheckEntranceInit()) return;

            this.controllerSN = null;
            this.controllerIP = null;
            this.serverIP = null;
            this.serverPort = null;

            // 间隔时间重复发出初始化本地网络配置请求.
            callbackList.Add(new Task(() => Utility.RepeatAction(InitLocalServer, AppConst.repeatActionDelta, () => { return this.serverIP != null; }, ClearCallbackList)));

            // 间隔时间重复发出初始化门锁控制器请求.
            if (AppConst.isOpenLock) callbackList.Add(new Task(() => Utility.RepeatAction(InitLock, AppConst.repeatActionDelta, () => { return this.controllerSN != null && this.controllerIP != null; }, () => { ClearCallbackList(); InitCamera(); })));
            else if (AppConst.isOpenCamera) callbackList.Add(new Task(() => InitCamera()));

            //if (AppConst.isOpenLock) await InitLock1();
            //if (AppConst.isOpenCamera) callbackList.Add(new Task(() => InitCamera()));

            res = Parallel.ForEach(callbackList, (x) => x.Start());
        }

        

        ParallelLoopResult res;

        /// <summary>
        /// 清除callbackList
        /// </summary>
        private void ClearCallbackList()
        {
            if (Utility.CheckEntranceInit() && res.IsCompleted)
            { 
                FacadeTool.Debug("callbackList clear");
                callbackList.Clear();
            }
            else
            {
                FacadeTool.Debug("not completed");
            }
        }

        public int requestCount = 0;
        //public const int requestNum = 2;  // 一次最多2个QPS
        bool isDetecting = false;
        CascadeClassifier cascade = new CascadeClassifier(AppConst.HaarCascadeAlt);

        private void InitCamera()
        {
            if (!AppConst.isOpenCamera) return;

            var capture = new VideoCapture(CaptureDevice.Any);
            //capture.Set(CaptureProperty.FrameWidth, 640);
            //capture.Set(CaptureProperty.FrameHeight, 320);
            capture.AutoFocus = true;
            CancellationTokenSource cts = new CancellationTokenSource();

            Mat image = new Mat();   // Frame image buffer
            while (true)             // When the movie playback reaches end, Mat.data becomes NULL.
            {
                capture.Read(image);
                Cv2.ImShow("capture", image);
               
                if (!isLockOpenning && !isDetecting && DetectFace(image))
                {
                    var f = Task.Run(async delegate
                    {
                        using (Mat src = capture.RetrieveMat())
                        {
                            //var img = await Utility.GetBASE64_2(src.ToBytes());
                            var img = Utility.GetBASE64(src.ToBytes());
                            //var tmp = CaptureRequest2(img);
                            var tmp = await CaptureRequest3(img);
                            FacadeTool.Debug(tmp);
                        }
                    }, cts.Token);

                    f.ContinueWith((x) =>
                    {
                        System.GC.Collect();
                    },TaskContinuationOptions.LongRunning);
                
                    #region old
                    /*
                        if (requestCount < requestNum)
                        {
                            new Task(() =>
                            {
                                Mat src = capture.RetrieveMat();
                                m_OpencvSharpHelper.test2.Add(Utility.GetBASE64(src.ToBytes()));
                                requestCount++;
                            }).Start();
                        }
                        
                        else {
                            var tmp = CaptureRequest();
                            System.GC.Collect();
                            System.Diagnostics.Debug.WriteLine(tmp);
                            if (tmp != null)
                            {
                                ///System.IO.File.WriteAllText(System.Windows.Forms.Application.StartupPath + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ff") + ".txt", tmp.ToString());
                            }
                        }
                        */
                    #endregion
                }

                System.GC.Collect();
                Cv2.WaitKey(40);
            }
        }

        /// <summary>
        /// 写入test2019年4月3日21:20:49
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private Task WriteFile(string path,string content)
        {
            var t = new Task(()=>
            {
                System.IO.File.WriteAllText(path, content);
            }
            );
            t.Start();
            return t;
        }


        private Task<bool> DetectFace1(Mat src)
        {
            var t = new Task<bool>(() =>
            {
                if (isDetecting) return false;
                isDetecting = true;

                using (var gray = new Mat())
                {
                    Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
                    Rect[] faces = cascade.DetectMultiScale(gray, 1.08, 2, HaarDetectionType.ScaleImage);
                    FacadeTool.Debug("face count: " + faces.Length);

                    Utility.Invoke(() => { isDetecting = false; }, AppConst.repeatActionDelta);
                    return faces.Length == 0 ? false : true; ;
                }
            });

            t.Start();
            return t;
        }

        private bool DetectFace(Mat src)
        {
            if (isDetecting) return false;
            isDetecting = true;

            using (var gray = new Mat())
            {
                Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
                Rect[] faces = cascade.DetectMultiScale(gray, 1.08, 2, HaarDetectionType.ScaleImage);
                FacadeTool.Debug("face count: " + faces.Length);

                Utility.Invoke(() => { isDetecting = false; }, AppConst.repeatActionDelta/2);
                return faces.Length == 0 ? false : true; ;
            }
        }

        /// <summary>
        /// 初始化配置本地服务器IP和PORT
        /// </summary>
        private void InitLocalServer()
        {
            if (AppConst.isOpenLocalServer)
            {
                //this.serverIP = "169.254.236.150"; //test local
                this.serverIP = "192.168.1.108";     //test RPI3B
                this.serverPort = AppConst.controllerPort.ToString();
                return;
            }

            //this.serverIP = Utility.AutoSearchServerIP();
            //this.serverPort = AppConst.controllerPort.ToString();  // 默认缺省

            //if (serverIP != null) FacadeTool.Debug("serverIP:" + this.serverIP + ",serverPORT:" + this.serverPort);
        }

        /// <summary>
        /// 初始化检测门锁控制器SN和IP.
        /// </summary>
        private void InitLock()
        {
            if (!AppConst.isOpenLock) return;

            //test local lock
            if (AppConst.isOpenLocalLock)
            {
                this.controllerSN = "123329697";
                this.controllerIP = "192.168.1.120"; // local lock IP
                return;
            }

            FacadeTool.Debug("searching SN...");
    
            var config = Utility.AutoSerachControllerSNandIP();
            //await Task.Delay(TimeSpan.FromSeconds(AppConst.repeatActionDelta));

            if (config == null) return;

            this.controllerSN = config[0];
            this.controllerIP = config[1];
            FacadeTool.Debug("controllerIP:" + this.controllerSN + ",controllerIP:" + this.controllerIP);
        }


        // test buton
        private void button2_Click(object sender, EventArgs e)
        {
            FacadeTool.Debug("test btn");
            FacadeTool.Debug("controllerIP:" + this.controllerSN + ",controllerIP:" + this.controllerIP);
            FacadeTool.Debug("serverIP:" + this.serverIP + ",serverPORT:" + this.serverPort);

            if (!Utility.CheckEntranceInit()) return;
            Utility.SendCommandMsg(AppConst.Funtion.REMOTEOPEN);     //远程开门 test
            //Utility.SendCommandMsg(AppConst.Funtion.STATUS);       // 查询控制器状态 test
        }


        // btn 1
        private void button1_Click(object sender, EventArgs e)
        {
            FacadeTool.Debug("btn1 click");

            var capture = new VideoCapture(CaptureDevice.Any);
            capture.Set(CaptureProperty.FrameWidth, 640);
            capture.Set(CaptureProperty.FrameHeight, 320);
            capture.AutoFocus = true;

            Mat image = new Mat();    
            while (true)              
            {
                capture.Read(image);
                Cv2.ImShow("capture2", image);
                if (DetectFace(image))
                {
                    FacadeTool.Debug("cap22222222222222");
                }

                Cv2.WaitKey(40);
            }
        }

        /// <summary>
        /// 捕捉图形发送http请求对比人脸3333
        /// </summary>
        /// <returns></returns>
        public Task<JObject> CaptureRequest3(string image)
        {
            var t = new Task<Newtonsoft.Json.Linq.JObject>(()=>
            {
                //lock (locker)
                //{
                    var res = m_OpencvSharpHelper.m_faceHelper.SearchFace(image);
                    if (res != null && CheckFaceMatch(res))
                    {
                        // 开门
                        if (AppConst.isOpenLock)
                        {
                            Utility.SendCommandMsg(AppConst.Funtion.REMOTEOPEN); // 远程开门按钮事件处理.
                            Utility.SendCommandMsg(AppConst.Funtion.STATUS);     // 查询状态并切换状态
                            Utility.Invoke(() => { Utility.SendCommandMsg(AppConst.Funtion.STATUS); }, AppConst.lockOpenningDelay);
                        }
                    }

                    return res;
                //}
            });

            t.Start();
            return t;
        }


        #region  CaptureRequest2
        /*
        /// <summary>
        /// 捕捉图形发送http请求对比人脸2222
        /// </summary>
        /// <returns></returns>
        public Newtonsoft.Json.Linq.JObject CaptureRequest2(string image)
        {
            lock (locker)
            {
                var res = m_OpencvSharpHelper.m_faceHelper.SearchFace(image);
                if (res != null && (string)res["error_msg"] == "SUCCESS")
                {
                    // 开门
                    if (AppConst.isOpenLock)
                    {
                        Utility.SendCommandMsg(AppConst.Funtion.REMOTEOPEN); // 远程开门按钮事件处理.
                        Utility.SendCommandMsg(AppConst.Funtion.STATUS);     // 查询状态并切换状态
                        Utility.Invoke(() => { Utility.SendCommandMsg(AppConst.Funtion.STATUS); }, AppConst.lockOpenningDelay);
                    }

                    return res;
                }
            }

            return null;
        }

        /// <summary>
        /// 捕捉图形发送http请求对比人脸1111
        /// </summary>
        /// <returns></returns>
        public Newtonsoft.Json.Linq.JObject CaptureRequest()
        {
            lock (locker)
            {
                for (int i = 0; i < m_OpencvSharpHelper.test2.Count; i++)
                {
                    var res = m_OpencvSharpHelper.m_faceHelper.SearchFace(m_OpencvSharpHelper.test2[i]);

                    if (res != null && (string)res["error_msg"] == "SUCCESS")
                    {
                        // 开门
                        if (AppConst.isOpenLock)
                        {
                            Utility.SendCommandMsg(AppConst.Funtion.REMOTEOPEN); // 远程开门按钮事件处理.
                            Utility.SendCommandMsg(AppConst.Funtion.STATUS);     // 查询状态并切换状态
                            Utility.Invoke(() => { Utility.SendCommandMsg(AppConst.Funtion.STATUS); }, AppConst.lockOpenningDelay);
                        }

                        this.requestCount = 0;
                        //m_OpencvSharpHelper.test.Clear();
                        m_OpencvSharpHelper.test2.Clear();
                       
                        return res;
                    }
                }
            }

            this.requestCount = 0;
            //m_OpencvSharpHelper.test.Clear();
            m_OpencvSharpHelper.test2.Clear();
            return null;
        }

        */
        #endregion

        /// <summary>
        /// 检测成功比对
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool CheckFaceMatch(Newtonsoft.Json.Linq.JObject result)
        {
            if ((string)result["error_msg"] != "SUCCESS") return false;

            int score = 0;
            if (result["result"].HasValues)
            {
                var test = result["result"]["user_list"].AsEnumerable();
                foreach (var item in test)
                {
                    //Console.WriteLine(item["user_id"]);
                    score = ((int)item["score"]);
                    break;
                }
            }

            return score < 90 ?false:true;
        }
     
        private void button3_Click(object sender, EventArgs e)
        {
            //isLockOpenning = false;
            Utility.SendCommandMsg(AppConst.Funtion.STATUS);           // 查询控制器状态 test
            Utility.SendCommandMsg(AppConst.Funtion.REMOTEOPEN);       // 查询控制器状态 test
        }


        // 关闭winform
        private void Controller_Destroy(object sender, FormClosingEventArgs e)
        {
            Dispose(true);
        }
    }
}
