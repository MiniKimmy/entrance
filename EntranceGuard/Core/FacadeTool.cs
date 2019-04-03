namespace EntranceGuard
{
    /// <summary>
    /// 外观者工具类.
    /// </summary>
    class FacadeTool
    {
        private static bool debugMode = true;       // true表示demo测试模式

        /// <summary>
        /// Debug到控制台查看信息
        /// </summary>
        public static void Debug(string format, params object[] info)
        {
            if (debugMode)
            {
                System.Diagnostics.Debug.WriteLine(format,info);
            }
        }

        /// <summary>
        /// Debug到控制台查看信息
        /// </summary>
        public static void Debug(object obj)
        {
            if (debugMode)
            {
                System.Diagnostics.Debug.WriteLine(obj);
            }
        }
    }
}
