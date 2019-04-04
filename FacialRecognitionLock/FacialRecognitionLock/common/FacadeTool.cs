namespace FacialRecognitionLock
{
    /// <summary>
    /// 外观者工具类.
    /// </summary>
    class FacadeTool
    {
        /// <summary>
        /// Debug到控制台"输出"查看信息
        /// </summary>
        public static void Debug(string format, params object[] info)
        {
            if (AppConst.isDebugMode == true)
            {
                System.Diagnostics.Debug.Write(Utility.GetCurrentTime() + " ");
                System.Diagnostics.Debug.WriteLine(format, info);
            }
        }

        /// <summary>
        /// Debug到控制台"输出"查看信息
        /// </summary>
        public static void Debug(object obj)
        {
            if (AppConst.isDebugMode == true)
            {
                System.Diagnostics.Debug.Write(Utility.GetCurrentTime() + " ");
                System.Diagnostics.Debug.WriteLine(obj);
            }
        }

    }
}
