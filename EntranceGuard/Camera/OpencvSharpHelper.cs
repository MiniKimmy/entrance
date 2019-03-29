namespace EntranceGuard
{
    using OpenCvSharp;

    /// <summary>
    /// opencvsharp 
    /// </summary>
    class OpencvSharpHelper
    {
        private bool canCamera = false;
        public bool CanCamera { get { return canCamera; } }

        private int sleepTime = 50;  // 摄像头捕捉帧率

        public OpencvSharpHelper(){ }

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
            Run();
        }

        private void Run()
        {
            var capture = new VideoCapture(CaptureDevice.Any);
            using (var window = new Window("capture"))
            {
                Mat image = new Mat();   // Frame image buffer
                while (canCamera)        // When the movie playback reaches end, Mat.data becomes NULL.
                {
                    capture.Read(image);  
                    window.ShowImage(image);
                    Cv2.WaitKey(sleepTime);
                }

                System.IO.File.WriteAllBytes("test.png",image.ToBytes());  // test
            }
        }
    }
}
