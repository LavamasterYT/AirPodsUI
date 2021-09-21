#pragma warning disable CA1416 // Validate platform compatibility

using Microsoft.Win32;
using System;

namespace AirPodsUI.Core
{
    public class Settings : IDisposable
    {
        public int RefreshRate
        {
            get
            {
                if (_key.GetValue("RefreshRate") == null)
                {
                    _key.SetValue("RefreshRate", 1000);
                    return 1000;
                }
                else
                {
                    try
                    {
                        return (int)_key.GetValue("RefreshRate");
                    }
                    catch (Exception)
                    {
                        _key.SetValue("RefreshRate", 1000);
                        return 1000;
                    }
                }
            }
            set
            {
                _key.SetValue("RefreshRate", value);
            }
        }
        public int Offset
        {
            get
            {
                if (_key.GetValue("Offset") == null)
                {
                    _key.SetValue("Offset", 60);
                    return 10;
                }
                else
                {
                    try
                    {
                        return (int)_key.GetValue("Offset");
                    }
                    catch (Exception)
                    {
                        _key.SetValue("Offset", 60);
                        return 60;
                    }
                }
            }
            set
            {
                _key.SetValue("Offset", value);
            }
        }
        public bool RunAtStartup
        {
            get
            {
                if (_key.GetValue("RunAtStartup") == null)
                {
                    _key.SetValue("RunAtStartup", false);
                    return false;
                }
                else
                {
                    try
                    {
                        return (int)_key.GetValue("RunAtStartup") == 0 ? false : true;
                    }
                    catch (Exception)
                    {
                        _key.SetValue("RunAtStartup", false);
                        return false;
                    }
                }
            }
            set
            {
                _key.SetValue("RunAtStartup", value == false ? 0 : 1);
            }
        }
        public bool AllowIDEditing
        {
            get
            {
                if (_key.GetValue("AllowIDEditing") == null)
                {
                    _key.SetValue("AllowIDEditing", false);
                    return false;
                }
                else
                {
                    try
                    {
                        return (int)_key.GetValue("AllowIDEditing") == 0 ? false : true;
                    }
                    catch (Exception)
                    {
                        _key.SetValue("AllowIDEditing", false);
                        return false;
                    }
                }
            }
            set
            {
                _key.SetValue("AllowIDEditing", value == false ? 0 : 1);
            }
        }

        private RegistryKey _key;

        public Settings()
        {
            _key = Registry.CurrentUser.CreateSubKey("Software\\AirPodsUI");
        }

        public void Dispose()
        {
            _key.Close();
            _key.Dispose();
        }
    }
}
#pragma warning restore CA1416 // Validate platform compatibility
