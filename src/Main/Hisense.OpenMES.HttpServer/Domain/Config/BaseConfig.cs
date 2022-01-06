using NewLife.Log;
using NewLife.Serialization;

using System.Text;

namespace Hisense.OpenMES.HttpServer
{
    /// <summary>
    /// 配置文件父类
    /// </summary>
    internal class BaseConfig
    {
        /// <summary>
        /// 获取配置文件的服务器物理文件路径
        /// </summary>
        /// <typeparam name="T">配置信息类</typeparam>
        /// <returns>配置文件路径</returns>
        private static string GetConfigPath<T>()
        {
            string path = string.Empty;
            path = AppDomain.CurrentDomain.BaseDirectory + "/OpenMES/AutoUpdate/";
            return path + typeof(T).Name + ".json";
        }

        /// <summary>
        /// 更新配置信息，将配置信息对象序列化至相应的配置文件中，文件格式为带签名的UTF-8
        /// </summary>
        /// <typeparam name="T">配置信息类</typeparam>
        /// <param name="config">配置信息</param>
        internal static void UpdateConfig<T>(T config)
        {
            if (config == null)
            {
                return;
            }

            Type configClassType = typeof(T);
            string configFilePath = GetConfigPath<T>();
            try
            {
                #region XML
                //XmlSerializer xmlSerializer = new XmlSerializer(configClassType);
                //using (XmlTextWriter xmlTextWriter = new XmlTextWriter(configFilePath, Encoding.UTF8))
                //{
                //    xmlTextWriter.Formatting = Formatting.Indented;
                //    XmlSerializerNamespaces xmlNamespace = new XmlSerializerNamespaces();
                //    xmlNamespace.Add(string.Empty, string.Empty);
                //    xmlSerializer.Serialize(xmlTextWriter, config, xmlNamespace);
                //} 
                #endregion

                configFilePath.EnsureDirectory(true);

                using (var stream = new FileStream(configFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    var writer = new StreamWriter(stream, Encoding.UTF8);
                    writer.Write(config.ToJson());

                    writer?.Flush();
                }
            }
            catch (Exception exp)
            {
                XTrace.WriteException(exp);
            }
        }

        /// <summary>
        /// 获取配置信息，首先从缓存中读取，如果读取失败则从配置文件中反序列化配置对象并写入缓存
        /// </summary>
        /// <typeparam name="T">配置信息类</typeparam>
        /// <returns>配置信息</returns>
        protected static T GetConfig<T>() where T : class, new()
        {
            Type configClassType = typeof(T);
            object configObject = null;
            if (configObject == null)
            {
                string configFilePath = GetConfigPath<T>();
                if (File.Exists(configFilePath))
                {
                    #region XML
                    //using (XmlTextReader xmlTextReader = new XmlTextReader(configFilePath))
                    //{
                    //    XmlSerializer xmlSerializer = new XmlSerializer(configClassType);
                    //    configObject = xmlSerializer.Deserialize(xmlTextReader);
                    //} 
                    #endregion

                    configFilePath.EnsureDirectory(true);

                    using (var stream = new FileStream(configFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        var reader = new StreamReader(stream, Encoding.UTF8);
                        configObject = reader?.ReadToEnd()?.ToJsonEntity<T>();
                    }
                }
            }

            T config = configObject as T;
            if (config == null)
            {
                return new T();
            }
            else
            {
                return config;
            }
        }
    }
}
