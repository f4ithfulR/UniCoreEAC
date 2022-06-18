using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;

namespace UniCoreEAC
{
    internal class Worker
    {
        private readonly ContextMenuStrip systrayMenu;
        private readonly NotifyIcon notifyIcon;

        private Boolean processRunning = false;
        private System.Threading.Timer timer = null;

        private Boolean firstRun = true;

        public Worker(ContextMenuStrip systrayMenu, NotifyIcon notifyIcon)
        {
            this.systrayMenu = systrayMenu;
            this.notifyIcon = notifyIcon;
        }

        public void StartTimer(int delaySeconds)
        {
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(delaySeconds);

            if (timer != null)
            {
                timer.Dispose();
            }

            timer = new System.Threading.Timer((e) =>
            {
                DoWork();
            }, null, startTimeSpan, periodTimeSpan);
        }

        private void DoWork()
        {
            if (!firstRun)
            {
                #nullable enable
                Process? eacProcess = null;
                foreach (Process process in Process.GetProcesses())
                {
                    if (process.ProcessName.ToString() == "EasyAntiCheat")
                    {
                        eacProcess = process;
                        break;
                    }
                }
                if (eacProcess != null && !processRunning)
                {
                    // now running...
                    eacProcess.ProcessorAffinity = (System.IntPtr)1;
                    VisualizeStatus(true, "EasyAntiCheck affinity set");
                    processRunning = true;
                    ShowNotification();
                }
                else if (eacProcess == null && processRunning)
                {
                    // not running anymore
                    VisualizeStatus(false, "EasyAntiCheat not running");
                    processRunning = false;

                }
            }
            else
            {
                firstRun = false;
            }
        }

        private void VisualizeStatus(Boolean running, String text)
        {
            if (systrayMenu.InvokeRequired)
            {
                systrayMenu.Invoke(new Action(() =>
                {
                    ToolStripMenuItem menuItem = (ToolStripMenuItem)systrayMenu.Items[2];
                    menuItem.Checked = running;
                    menuItem.Text = text;
                    this.notifyIcon.Text = "UniCoreEAC - " + text;
                }));
            }
            else
            {
                ToolStripMenuItem menuItem = (ToolStripMenuItem)systrayMenu.Items[2];
                menuItem.Checked = running;
                menuItem.Text = text;
                this.notifyIcon.Text = "UniCoreEAC - " + text;
            }
        }

        private void ShowNotification()
        {
            if (Settings.GetValueBool(Settings.SETTING_SHOWNOTIFICATIONS, true)) { 
                new Thread(() => this.notifyIcon.ShowBalloonTip(
                    2000,
                    "",
                    "Affinity of EasyAntiCheat set to one core.",
                    ToolTipIcon.Info)).Start();
            }
        }
    }
}
