namespace FacialRecognitionLock
{
    /// <summary>
    /// 主程序入口
    /// </summary>
    class Program 
    {
        [System.STAThread]
        static void Main(string[] args)
        {
            // lock开启
            LockController _lock = new LockController();
            _lock.Run();

            // camera开启
            OpencvSharpHelper _camera = new OpencvSharpHelper();
            _camera.Run();
        }
    }

}
