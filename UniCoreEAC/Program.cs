using System;
using System.Threading;
using System.Windows.Forms;

namespace UniCoreEAC
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (Mutex mutex = new Mutex(false, "UniCoreEAC"))
            {
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show("SingleCoreEAC already running", "SingleCoreEAC", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Application.Run(new Systray());
                }
            }
        }
    }
}
