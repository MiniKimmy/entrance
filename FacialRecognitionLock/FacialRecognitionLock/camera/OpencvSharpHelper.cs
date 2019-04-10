namespace FacialRecognitionLock
{
    using Baidu.Aip.Face;
    using System.Threading.Tasks;
    using OpenCvSharp;
    using System.Linq;
    using System.Collections.Generic;

    /// <summary>
    /// 人脸识别Wrap
    /// </summary>
    class OpencvSharpHelper : IExecute
    {
        private const double std_scaleFactor = 1.08;    // 检测规模大小标准                     
        private const int std_minNeighbors = 2;         // 检测至少人脸个数标准
        private const double std_delayCapture = 2;      // 捕捉视频流帧的延迟标准
        private const int std_requestTimeout = 60000;   // 1min请求超时标准
        private const int std_CaptureScore = 80;        // 人脸捕捉符合分数标准
        private const int std_groupListLen = 50;        // 一次获取人脸库组别数量标准
        private const float std_face_liveness = 0.9f;   // 活体分数标准
        private const int std_DPI_width = 640;          // 分辨率width标准
        private const int std_DPI_height = 320;         // 分辨率height标准
        private const int std_sleepTime = 40;           // 视频流静息间断时间(毫秒)标准

        private string appid;                           // appid
        private string apiKey;                          // ak
        private string secretKey;                       // sk
        private Baidu.Aip.Face.Face client;             // 百度人脸识别api
        private bool isDetecting;                       // 是否正在人脸检测
        private string groupIdList;                     // 检测的用户组列表
        private CascadeClassifier HaarCascade;          // 人脸检测haar算法xml配置

        public OpencvSharpHelper()
        {
            this.isDetecting = false;
            this.HaarCascade = new OpenCvSharp.CascadeClassifier(AppConst.HaarCascadeAlt);

            this.appid = AppConst.APP_ID;        
            this.apiKey = AppConst.API_KEY;         
            this.secretKey = AppConst.SECRET_KEY;      
            this.client = new Baidu.Aip.Face.Face(apiKey, secretKey);
            this.client.Timeout = std_requestTimeout;
            
            FacadeTool.Debug("appid:{0},ak:{1},sk:{2},group:{3}", appid, apiKey, std_scaleFactor, groupIdList);
        }

        /// <summary>
        /// 启动摄像头
        /// </summary>
        public void Run()
        {
            if (!AppConst.isOpenCamera || !Utility.CheckNetwork()) return;
            groupIdList = this.GetGroupList();  // 获取用户组列表

            var capture = new VideoCapture(CaptureDevice.Any);
            capture.Set(CaptureProperty.FrameWidth, std_DPI_width);
            capture.Set(CaptureProperty.FrameHeight, std_DPI_height);
            capture.AutoFocus = true;

            Mat image = new Mat();   // Frame image buffer
            while (true)             
            {
                capture.Read(image);
                Cv2.ImShow("capture", image);

                if (!LockController.Instance.IsLockOpenning &&
                    !this.isDetecting &&
                    this.DetectFace(image, std_delayCapture))
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

                            // 活体检测
                            if (Faceverify(img))
                            {
                                FacadeTool.Debug("true!!!");
                                CaptureRequestHandle(img);
                            }
                        }
                    });

                    t.ContinueWith((x) => { System.GC.Collect(); });
                }

                System.GC.Collect();
                Cv2.WaitKey(std_sleepTime);
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
        private bool DetectFace(Mat src, double captureDelay)
        {
            if (isDetecting) return false;
            isDetecting = true;

            Rect[] faces = null;
            using (var gray = new Mat())
            {
                Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
                faces = this.HaarCascade.DetectMultiScale(gray, std_scaleFactor, std_minNeighbors, HaarDetectionType.ScaleImage);
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
        private bool SearchFace(string img, string groupIdList = null, string imageType = "BASE64", System.Collections.Generic.Dictionary<string, object> options = null)
        {
            // 默认可选参数
            if(options == null)
            {
                options = new System.Collections.Generic.Dictionary<string, object>{
                    {"quality_control", "HIGH"}, //图片质量控制 NONE: 不进行控制 LOW:较低的质量要求 NORMAL: 一般的质量要求 HIGH: 较高的质量要求 默认 NONE
                    {"liveness_control", "HIGH"}, //活体检测控制 NONE: 不进行控制 LOW:较低的活体要求(高通过率 低攻击拒绝率) NORMAL: 一般的活体要求(平衡的攻击拒绝率, 通过率) HIGH: 较高的活体要求(高攻击拒绝率 低通过率) 默认NONE
                };
            }

            if(groupIdList == null)
            {
                groupIdList = this.groupIdList;
            }

            var result = client.Search(img, imageType, groupIdList, options);
            if ((string)result["error_msg"] != "SUCCESS") return false;

            FacadeTool.Debug(result);

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
            // System.IO.File.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ff") + ".txt", result.ToString());
            return score < std_CaptureScore ? false : true;
        }

        /// <summary>
        /// 获取用户组列表
        /// </summary>
        private string GetGroupList(System.Collections.Generic.Dictionary<string, object> options = null)
        {
            // 默认可选参数
            if (options == null)
            {
                options = new Dictionary<string, object>{
                    {"start", 0},                 // 起始index
                    {"length", std_groupListLen}  // 获取组数量
                };
            }

            var result = client.GroupGetlist(options);
            FacadeTool.Debug(result);

            if ((string)result["error_msg"] != "SUCCESS") return null;
            if (result["result"].HasValues)
            {
                var test = result["result"]["group_id_list"].AsEnumerable();
                string ret = "";
                foreach (var item in test) {
                    ret += item + ",";
                }

                return ret.Remove(ret.Length - 1, 1);
            }
  
            return null;
        }

        /// <summary>
        /// 活体检测
        /// </summary>
        public bool Faceverify(string img, string imageType = "BASE64")
        {
            var faces = new Newtonsoft.Json.Linq.JArray {
                new Newtonsoft.Json.Linq.JObject {
                    {"image", img},
                    {"image_type", imageType},
            
                }
            };

            var result = this.client.Faceverify(faces);
            if ((string)result["error_msg"] != "SUCCESS") return false;
            if (result["result"].HasValues)
            {
                var res = (float)result["result"]["std_face_liveness"];
                return res >= 0.9 ? true : false;
            }

            return false;
        }
    }
}
