using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace UniCoreEAC
{
    internal class Systray : ApplicationContext
    {
        private readonly AutostartHelper AutostartHelper = new AutostartHelper();
        private readonly NotifyIcon Icon = new NotifyIcon();

        public Systray()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(NotifyIcon));

            Icon.Icon = Resources.ucEAC;
            Icon.Text = "SingleCoreEAC - EasyAntiCheat not running";
            Icon.Visible = true;

            ContextMenuStrip contextMenu = new ContextMenuStrip();

            ToolStripMenuItem menuItemInfo = new ToolStripMenuItem("UniCoreEAC - v" + Assembly.GetExecutingAssembly().GetName().Version.ToString())
            {
                Enabled = false
            };

            ToolStripMenuItem menuItemStatus = new ToolStripMenuItem("EasyAntiCheat not running")
            {
                Checked = false,
                Enabled = false
            };

            ToolStripMenuItem menuItemShowNotifications = new ToolStripMenuItem("Show notifications")
            {
                Checked = CheckShowNotifications()
            };
            menuItemShowNotifications.Click += new EventHandler(MenuItemShowNotifications_Click);

            ToolStripMenuItem menuItemAutostart = new ToolStripMenuItem("Start UniCoreEAC with Windows")
            {
                Checked = CheckAutoStartEnabled()
            };
            menuItemAutostart.Click += new EventHandler(MenuItemAutostart_Click);

            ToolStripMenuItem menuItemSettings = new ToolStripMenuItem("Settings");

            ToolStripMenuItem menuItemAbout = new ToolStripMenuItem("About");
            menuItemAbout.Click += new EventHandler(MenuItemAbout_Click);

            ToolStripMenuItem menuItemExit = new ToolStripMenuItem("Exit");
            menuItemExit.Click += new EventHandler(MenuItemExit_Click);

            contextMenu.Items.Add(menuItemInfo);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(menuItemStatus);
            menuItemSettings.DropDownItems.Add(menuItemShowNotifications);
            menuItemSettings.DropDownItems.Add(menuItemAutostart);
            contextMenu.Items.Add(menuItemSettings);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(menuItemAbout);
            contextMenu.Items.Add(menuItemExit);

            Icon.ContextMenuStrip = contextMenu;

            Worker worker = new Worker(contextMenu, Icon);
            worker.StartTimer(5);
        }

        private bool CheckShowNotifications()
        {
            return Settings.GetValueBool(Settings.SETTING_SHOWNOTIFICATIONS, true);
        }

        #nullable enable
        private void MenuItemShowNotifications_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
                menuItem.Checked = !menuItem.Checked;
                Settings.SetValueBool(Settings.SETTING_SHOWNOTIFICATIONS, menuItem.Checked);
            }
        }

        #nullable enable
        private void MenuItemAutostart_Click(object? sender, EventArgs e)
        {
            if (sender != null)
            {
                ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
                if (menuItem.Checked)
                {
                    AutostartHelper.RemoveFromAutostart();
                }
                else
                {
                    AutostartHelper.AddToAutostart();
                }
                menuItem.Checked = !menuItem.Checked;
            }
        }

        #nullable enable
        private void MenuItemAbout_Click(object? sender, EventArgs e)
        {
            new About().ShowDialog();
        }

        #nullable enable
        private void MenuItemExit_Click(object? sender, EventArgs e)
        {
            Icon.Visible = false;
            Icon.Dispose();

            Application.Exit();
        }

        private Boolean CheckAutoStartEnabled()
        {
            return AutostartHelper.CheckAutoStartEnabled();
        }
    }
}
