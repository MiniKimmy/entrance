namespace EntranceGuard
{
    using System.Windows.Forms;

    /// <summary>
    /// 应用程序的主入口点。
    /// </summary>
    static class Program
    {
        [System.STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Controller());
        }
    }
}
