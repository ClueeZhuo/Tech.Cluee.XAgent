using System.Configuration;

namespace Hisense.OpenMES.HttpServer
{
    /// <summary>
    /// 配置服务
    /// </summary>
    public static class ConfigService
    {
        /// <summary>
        /// 通用获取配置类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="defVal">默认值</param>
        /// <returns></returns>
        private static T Get<T>(string key, T defVal)
        {
            var text = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(text))
                return defVal;
            try
            {
                var value = Convert.ChangeType(text, typeof(T));
                return (T)value;
            }
            catch
            {
                return defVal;
            }
        }

        /// <summary>
        /// 登录类型
        /// </summary>
        public static int ServerPort
        {
            get
            {
                var config = Get("ServerPort", 80);
                return config;
            }
        }
    }
}
