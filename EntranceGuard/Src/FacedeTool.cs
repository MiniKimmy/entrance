/// <summary>
/// 外观者工具类.
/// </summary>
namespace EntranceGuard
{
    using System;

    class FacedeTool
    {
        private static bool debugMode = true;       // true表示demo测试模式

        public static void Debug(Controller controller, string format, params object[] info)
        {
            if (debugMode)
            {
                controller.ConsoleBox.AppendText(string.Format(format, info));
                controller.ConsoleBox.ScrollToCaret();       //滚动到光标处
                System.Windows.Forms.Application.DoEvents();
            }
        }

        public static void Debug(Controller controller, string info)
        {
            if (debugMode)
            {
                controller.ConsoleBox.AppendText(string.Format("{0} {1}\r\n", DateTime.Now.ToString("HH:mm:ss"), info));
                controller.ConsoleBox.ScrollToCaret();
                System.Windows.Forms.Application.DoEvents();
            }
        }
    }
}
