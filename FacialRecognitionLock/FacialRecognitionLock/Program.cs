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
            LockController _lock = new LockController();
            _lock.Run();

            OpencvSharpHelper _camera = new OpencvSharpHelper();
            _camera.Run();
        }
    }
}
