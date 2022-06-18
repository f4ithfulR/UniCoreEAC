using Microsoft.Win32;
using System;

namespace UniCoreEAC
{
    public static class Settings
    {

        public static readonly string SETTING_SHOWNOTIFICATIONS = "ShowNotifications";

        private static RegistryKey GetRegistryKey()
        {
            RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\UniCoreEAC", RegistryKeyPermissionCheck.ReadWriteSubTree);
            return registryKey;
        }

        public static void SetValueString(string name, string value)
        {
            try
            {
                GetRegistryKey().SetValue(name, value, RegistryValueKind.String);
            }
            catch (Exception)
            {
            }
        }

        public static string GetValueString(string name, string defaultValue)
        {
            string value = null;
            try
            {
                value = (string)GetRegistryKey().GetValue(name, null);
            }
            catch (Exception)
            {
            }

            if (value == null)
            {
                SetValueString(name, defaultValue);
                return defaultValue;
            }
            return value;
        }

        public static void SetValueInt32(string name, int value)
        {
            try
            {
                GetRegistryKey().SetValue(name, value, RegistryValueKind.DWord);
            }
            catch (Exception)
            {
            }
        }

        public static int GetValueInt32(string name, int defaultValue)
        {
            int value = defaultValue;
            try
            {
                var k = GetRegistryKey();
                value = (int)(k.GetValue(name, defaultValue));
            }
            catch (Exception)
            {
            }

            if (value == defaultValue)
            {
                SetValueInt32(name, defaultValue);
                return defaultValue;
            }
            return value;
        }

        public static void SetValueBool(string name, bool value)
        {
            SetValueInt32(name, value ? 1 : 0);
        }

        public static bool GetValueBool(string name, bool defaultValue)
        {
            return GetValueInt32(name, defaultValue ? 1 : 0) == 1;
        }
    }
}
