using System;
using System.IO;
using Newtonsoft.Json;
using ScreenSwitcher.JDOs;
using WPFUtilsLib.Helpers;

namespace ScreenSwitcher.Classes
{
    public class JsonHandler
    {
        public event EventHandler<Exception>? ErrorOcurred;

        public SettingsJDO? ReadJSONData(string JsonPath, string EncryptionKey)
        {
            if (File.Exists(JsonPath))
            {
                SettingsJDO? jsonData = null;

                try
                {
                    FileEncriptionManager.DecryptFile(JsonPath, EncryptionKey);
                    jsonData = JsonConvert.DeserializeObject<SettingsJDO>(File.ReadAllText(JsonPath))!;
                    FileEncriptionManager.EncryptFile(JsonPath, EncryptionKey);
                }
                catch(Exception ex)
                {
                    ErrorOcurred?.Invoke(this, ex);
                }
                
                return jsonData;
            }
            else
            {
                try
                {
                    File.WriteAllText(JsonPath, "");
                    FileEncriptionManager.EncryptFile(JsonPath, EncryptionKey);
                    return null;
                }
                catch(Exception ex)
                {
                    ErrorOcurred?.Invoke(this, ex);
                    return null;
                }
            }
        }

        public bool WriteJSONData(string JsonPath, string EncryptionKey, SettingsJDO Data)
        {
            try
            {
                if (File.Exists(JsonPath))
                {
                    FileEncriptionManager.DecryptFile(JsonPath, EncryptionKey);
                }

                File.WriteAllText(JsonPath, JsonConvert.SerializeObject(Data, Formatting.Indented));
                FileEncriptionManager.EncryptFile(JsonPath, EncryptionKey);
                return true;
            }
            catch(Exception ex)
            {
                ErrorOcurred?.Invoke(this, ex);
                return false;
            }
            
        }
    }
}
