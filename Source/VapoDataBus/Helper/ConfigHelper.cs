using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace VapoDataBus.Helper
{
    public class ConfigHelper : IConfigHelper
    {
        public string AppSetting(string key, string defaultValue = null)
        {
            var value = ConfigurationManager.AppSettings.Get(key);

            return (value != null) ? value : defaultValue;
          
        }
    }
}
