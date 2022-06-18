using Microsoft.Win32.TaskScheduler;
using System;
using System.IO;
using System.Windows.Forms;

namespace UniCoreEAC
{
    internal class AutostartHelper
    {
        private readonly TaskService ts = new TaskService();

        public Boolean CheckAutoStartEnabled()
        {
            Task t = ts.GetTask("UniCoreEAC");
            return t != null && t.Enabled;
        }

        public void AddToAutostart()
        {
            TaskDefinition td = this.ts.NewTask();
            td.RegistrationInfo.Description = "Limits EasyAntiCheat's affinity to one core";
            td.Principal.RunLevel = TaskRunLevel.Highest;
            td.Settings.MultipleInstances = TaskInstancesPolicy.IgnoreNew;
            td.Triggers.Add(new LogonTrigger { UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name });
            td.Actions.Add(new ExecAction(Path.Combine(Environment.CurrentDirectory, AppDomain.CurrentDomain.FriendlyName + ".exe")));

            ts.RootFolder.RegisterTaskDefinition(@"UniCoreEAC", td);
        }

        public void RemoveFromAutostart()
        {
            try
            {
                ts.RootFolder.DeleteTask("UniCoreEAC");
            }
            catch (Exception e)
            {
                MessageBox.Show("Error removing SingleCoreEAC from autostart: " + e.Message, "UniCoreEAC");
            }

        }
    }
}
