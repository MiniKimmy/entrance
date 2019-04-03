namespace EntranceGuard
{
    using OpenCvSharp;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
  

    /// <summary>
    /// opencvsharp工具类 
    /// </summary>
    class OpencvSharpHelper
    {
        private bool canCamera = false;
        public bool CanCamera { get { return canCamera; } }

        private const int sleepTime = 40;  // 摄像头捕捉帧率
        private const int requestNum = 2;  // 一次发送2个请求检测人脸

        private int requestCount = 0;
        public FacialRecognitionHelper m_faceHelper;

        //public List<Mat> test;
        public List<string> test2;

        public OpencvSharpHelper()
        {
            //test = new List<Mat>();
            test2 = new List<string>();
            m_faceHelper = new FacialRecognitionHelper();
        }

        /// <summary>
        /// 关闭摄像头
        /// </summary>
        public void Stop(){
            this.canCamera = false;
            
        }

        /// <summary>
        /// 打开摄像头
        /// </summary>
        public void Start(){
            this.canCamera = true;
            new Task(()=>
            { 
                  new Task(Run, TaskCreationOptions.AttachedToParent).Start();
            }).Start();
        }
      
        private void Run()
        {
            var capture = new VideoCapture(CaptureDevice.Any);
            capture.Set(CaptureProperty.FrameWidth, 640);
            capture.Set(CaptureProperty.FrameHeight, 360);
            capture.AutoFocus = true;
            capture.TriggerDelay = 2000;

            using (var window = new Window("capture"))
            {
                Mat image = new Mat();   // Frame image buffer
                while (canCamera)        // When the movie playback reaches end, Mat.data becomes NULL.
                {
                    capture.Read(image);  
                    window.ShowImage(image);

                    if (requestCount < requestNum)
                    {
                        FacadeTool.Debug("requestCount = " + requestCount);

                        new Task(async delegate
                        {
                            Mat src = capture.RetrieveMat();
                            test2.Add(Utility.GetBASE64(src.ToBytes()));
                            //test.Add(src);
                            await Task.Delay(1000);
                            this.requestCount++;
                        }).Start();
                    }
                    else
                    {
                        FacadeTool.Debug("init okokokok");
                        ///FacadeTool.Debug(Utility.AddListener<Newtonsoft.Json.Linq.JObject>(CaptureRequest, 2, () => { return this.requestCount >= requestNum; }));
                        System.GC.Collect();
                    }
                   
                    Cv2.WaitKey(sleepTime);
                }
            }
        }

        protected readonly object locker = new object();
        public Newtonsoft.Json.Linq.JObject CaptureRequest()
        {
            lock (locker)
            {
                for (int i = 0; i < this.test2.Count; i++)
                {
                    var res = m_faceHelper.SearchFace(this.test2[i]);
                    if (res["result"].HasValues)
                    {
                        this.requestCount = 0;
                        //this.test.Clear();
                        this.test2.Clear();
                        //FacadeTool.Debug("test.count" + this.test.Count);
                        return res;
                    }
                }
                this.requestCount--;
            }
            
            //this.test.Clear();
            this.test2.Clear();
            return null;
        }
    }
}
