namespace FacialRecognitionLock
{
    using Baidu.Aip.Face;
    using System.Threading.Tasks;
    using OpenCvSharp;
    using System.Linq;

    /// <summary>
    /// 人脸识别Wrap
    /// </summary>
    class OpencvSharpHelper : IExecute
    {
        private int sleepTime;                      // 视频流静息间断时间(毫秒)
        private const int requestTimeout = 60000;   // 1min请求超时
        private const double scaleFactor = 1.08;    // 检测规模大小                     
        private const int minNeighbors = 2;         // 检测至少人脸个数
        public const int standardCaptureScore = 80; // 人脸捕捉符合标准分数
        private bool isDetecting;                   // 是否正在人脸检测
        private double delayCapture = 2;            // 捕捉视频流帧的延迟
        private int width = 640;                    // 分辨率width
        private int height = 320;                   // 分辨率height

        private string appid;                       // APPID
        private string apiKey;                      // AK
        private string secretKey;                   // SK
        private Baidu.Aip.Face.Face client;         // 人脸识别api
        private CascadeClassifier HaarCascade;      // 人脸检测Haar算法xml配置

        public OpencvSharpHelper()
        {
            this.sleepTime = 40;
            this.isDetecting = false;
            this.HaarCascade = new OpenCvSharp.CascadeClassifier(AppConst.HaarCascadeAlt);
            this.appid = AppConst.APP_ID;        
            this.apiKey = AppConst.API_KEY;         
            this.secretKey = AppConst.SECRET_KEY;      
            this.client = new Baidu.Aip.Face.Face(apiKey, secretKey);
            this.client.Timeout = requestTimeout; 

            FacadeTool.Debug("appid:{0},ak:{1},sk:{2}", appid, apiKey, scaleFactor);
        }

        /// <summary>
        /// 启动摄像头
        /// </summary>
        public void Run()
        {
            if (!AppConst.isOpenCamera) return;

            var capture = new VideoCapture(CaptureDevice.Any);
            capture.Set(CaptureProperty.FrameWidth, this.width);
            capture.Set(CaptureProperty.FrameHeight, this.height);
            capture.AutoFocus = true;

            Mat image = new Mat();   // Frame image buffer
            while (true)             
            {
                capture.Read(image);
                Cv2.ImShow("capture", image);

                if (!LockController.Instance.IsLockOpenning &&
                    !this.isDetecting &&
                    this.DetectFace(image, this.delayCapture))
                {
                    var t = Task.Run(async delegate
                    {
                        using (Mat src = capture.RetrieveMat())
                        {
                            // 图片转化为BASE64格式.
                            var img = await Utility.GetBASE64Async(src.ToBytes());  // Async
                            //var img = Utility.GetBASE64(src.ToBytes());           // Synchronous

                            // 请求百度API并处理返回信息.
                            //await CaptureRequestHandleAsync(img);
                            CaptureRequestHandle(img);
                        }
                    });

                    t.ContinueWith((x) => { System.GC.Collect(); });
                }

                System.GC.Collect();
                Cv2.WaitKey(this.sleepTime);
            }
        }

        /// <summary>
        /// 人脸对比返回信息处理
        /// </summary>
        private void CaptureRequestHandle(string image)
        {
            var check = this.SearchFace(image);
            if (check && AppConst.isOpenLock)
            {
                Utility.SendCommandMsg(LockController.Funtion.REMOTEOPEN); // 远程开门按钮事件处理.
                Utility.SendCommandMsg(LockController.Funtion.STATUS);     // 查询状态并切换状态
                Utility.Invoke(() => { Utility.SendCommandMsg(LockController.Funtion.STATUS); }, AppConst.lockOpenningDelay);
            }
            
        }

        /// <summary>
        /// 人脸对比返回信息处理(Async)
        /// </summary>
        private Task CaptureRequestHandleAsync(string image)
        {
            var t = new Task(() =>
            {
                var check = this.SearchFace(image);
                if (check && AppConst.isOpenLock)
                {
                    Utility.SendCommandMsg(LockController.Funtion.REMOTEOPEN); // 远程开门按钮事件处理.
                    Utility.SendCommandMsg(LockController.Funtion.STATUS);     // 查询状态并切换状态
                    Utility.Invoke(() => { Utility.SendCommandMsg(LockController.Funtion.STATUS); }, AppConst.lockOpenningDelay);
                }
            });

            t.Start();
            return t;
        }


        /// <summary>
        /// 人脸检测
        /// </summary>
        /// <param name="src">视频流捕捉的某一帧的画面</param>
        public bool DetectFace(Mat src, double captureDelay)
        {
            if (isDetecting) return false;
            isDetecting = true;

            Rect[] faces = null;
            using (var gray = new Mat())
            {
                Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
                faces = this.HaarCascade.DetectMultiScale(gray, scaleFactor, minNeighbors, HaarDetectionType.ScaleImage);
                FacadeTool.Debug("face count: " + faces.Length);
            }

            Utility.Invoke(() => { isDetecting = false; }, captureDelay);
            return faces.Length == 0 ? false : true;
        }


        /// <summary>
        /// 人脸对比
        /// </summary>
        /// <param name="img">BASE64格式图片</param>
        /// <param name="groupIdList">人脸库组</param>
        /// <param name="imageType">图片格式(默认:BASE64)</param>
        /// <param name="options">可选属性(默认:活体检测、图片高质量)</param>
        private bool SearchFace(string img, string groupIdList = "test1,test", string imageType = "BASE64", System.Collections.Generic.Dictionary<string, object> options = null)
        {
            
            if(options == null)
            {
                options = new System.Collections.Generic.Dictionary<string, object>{
                    {"quality_control", "HIGH"}, //图片质量控制 NONE: 不进行控制 LOW:较低的质量要求 NORMAL: 一般的质量要求 HIGH: 较高的质量要求 默认 NONE
                    {"liveness_control", "LOW"}, //活体检测控制 NONE: 不进行控制 LOW:较低的活体要求(高通过率 低攻击拒绝率) NORMAL: 一般的活体要求(平衡的攻击拒绝率, 通过率) HIGH: 较高的活体要求(高攻击拒绝率 低通过率) 默认NONE
                };
              
            }

            var result = client.Search(img, imageType, groupIdList, options);
            if ((string)result["error_msg"] != "SUCCESS") return false;

            int score = 0;
            if (result["result"].HasValues)
            {
                var test = result["result"]["user_list"].AsEnumerable();
                foreach (var item in test)
                {
                    score = ((int)item["score"]);
                    break;
                }
            }

            FacadeTool.Debug(result);

            // 在RPI3B里查看返回的json信息.
            //System.IO.File.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ff") + ".txt", result.ToString());
            return score < standardCaptureScore ? false : true;
        }

       
    }
}
