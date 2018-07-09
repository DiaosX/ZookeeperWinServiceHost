using System;
using System.Configuration;

namespace ZookeeperWinServiceHost
{
    public static class ConfigUtil
    {

        public static T GetConfigVal<T>(string key, Func<string, T> parser, T defaultVal)
        {
            try
            {
                var value = ConfigurationManager.AppSettings[key];
                if (null == value)
                {
                    return defaultVal;
                }
                return parser(value);
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
