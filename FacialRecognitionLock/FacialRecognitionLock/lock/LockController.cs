namespace FacialRecognitionLock
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// lock控制器
    /// </summary>
    class LockController : IExecute
    {
        public enum Funtion : byte // 门锁协议功能号
        {
            NONE       = 0x00,     /// 无
            STATUS     = 0x20,     /// 查询状态
            REMOTEOPEN = 0x40,     /// 远程开门
        }

        // Test Local (本地测试)
        private static string sn = "123329697";      // lock sn(本地测试)
        private static string ip = "192.168.43.150";  // lock ip(本地测试)

        private static LockController instance = null;
        public static LockController Instance { get { if (instance == null) instance = new LockController(); return instance; } }

        private string controllerSN = null;
        private string controllerIP = null;
        private string serverIP = null;
        private string serverPort = null;
        private bool isLockOpenning;

        public string ControllerSN { get { return controllerSN; } private set { controllerSN = value; } }
        public string ControllerIP { get { return controllerIP; } private set { controllerIP = value; } }
        public string ServerIP { get { return serverIP; } private set { serverIP = value; } }
        public string ServerPort { get { return serverPort; } private set { serverPort = value; } }
        public bool IsLockOpenning { get { return isLockOpenning; } set { isLockOpenning = value; } }

        private List<Task> callbackList;
        private ParallelLoopResult m_parallelLoopResult;

        public LockController(string serverPort = null)
        {
            instance = this;
            isLockOpenning = false;
            this.serverPort = serverPort;
            callbackList = new List<Task>();
        }

        /// <summary>
        /// 初始化配置本地服务器IP和PORT
        /// </summary>
        private void InitLocalServer()
        {
            if (!Utility.CheckNetwork()) {
                FacadeTool.Debug("network is unavailable");
                if (AppConst.isDebugMode)
                    System.IO.File.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ff") + ".txt", "network is unaviable!");
                return;
            }

            //Test Local(本地测试)
            if (AppConst.isOpenLocalServer) {
                this.serverIP = AppConst.localServerIP;
                if(this.serverPort ==null) this.serverPort = AppConst.serverPort;
                return;
            }

            this.serverIP = Utility.AutoSearchServerIP();
            if (this.serverPort == null) this.serverPort = AppConst.serverPort;
            if (serverIP != null ) FacadeTool.Debug("serverIP:" + this.serverIP + ",serverPORT:" + this.serverPort);
            if (AppConst.isDebugMode)  System.IO.File.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ff") + ".txt", "serverIP:" + this.serverIP + ",serverPORT:" + this.serverPort);
        }

        /// <summary>
        /// 初始化检测门锁控制器SN和IP.
        /// </summary>
        private void InitLock()
        {
            if (!AppConst.isOpenLock) return;

            //Test Local(本地测试)
            if (AppConst.isOpenLocalLock)
            {
                this.controllerSN = sn;   // lock sn
                this.controllerIP = ip;   // lock ip
                return;
            }

            FacadeTool.Debug("searching SN...");
            var config = Utility.AutoSerachControllerSNandIP();
            if (config == null) return;

            this.controllerSN = config[0];
            this.controllerIP = config[1];
            FacadeTool.Debug("controllerIP:" + this.controllerSN + ",controllerIP:" + this.controllerIP);
        }

        /// <summary>
        /// 检查lock控制器配置是否正常.
        /// </summary>
        private bool CheckLockInit()
        {
            if (string.IsNullOrEmpty(this.ControllerSN) || string.IsNullOrEmpty(this.ServerIP)) return false;
            else return true;
        }

        /// <summary>
        /// 清除callbackList
        /// </summary>
        private void ClearCallbackList()
        {
            if (this.CheckLockInit() && m_parallelLoopResult.IsCompleted)
            {
                FacadeTool.Debug("callbackList clear");
                callbackList.Clear();
            }
            else
            {
                FacadeTool.Debug("not completed");
            }
        }

        /// <summary>
        /// 连接门锁主控制器
        /// </summary>
        public void Run()
        {
            if (this.CheckLockInit()) return;

            this.controllerSN = null;
            this.controllerIP = null;
            this.serverIP = null;
            this.serverPort = null;

            // 新版的n3kAdrtC.dll V3.6.3 使用同步才会正常.
            InitLocalServer();
            InitLock();

            /* V3.6 使用并发子线程的方式.旧版已过期
            // 间隔时间重复发出初始化本地网络配置请求.
            callbackList.Add(new Task(() =>
            Utility.RepeatAction(InitLocalServer, AppConst.repeatActionDelta,
                () => { return this.serverIP != null && this.serverPort != null; }, ClearCallbackList)));

            // 间隔时间重复发出初始化门锁控制器请求.
            if (AppConst.isOpenLock) callbackList.Add(new Task(() =>
                Utility.RepeatAction(InitLock, AppConst.repeatActionDelta,
                () => { return this.controllerSN != null && this.controllerIP != null; },
                ClearCallbackList)));

            // 并发执行所有任务
            m_parallelLoopResult = Parallel.ForEach(callbackList, (x) => x.Start());
            */
        }
    }
}
