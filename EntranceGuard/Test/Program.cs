/// <summary>
/// 应用程序的主入口点。
/// </summary>
namespace EntranceGuard
{
    using System.Windows.Forms;

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
