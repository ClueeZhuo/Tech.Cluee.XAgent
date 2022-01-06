namespace Hisense.OpenMES.HttpServer
{
    /// <summary>
    /// Version
    /// </summary>
    [Serializable]
    internal class Version
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        internal Version()
        {

        }

        /// <summary>
        /// OpenMESVersionInfo
        /// </summary>
        internal PluginVersionInfo OpenMESVersionInfo { get; set; }

        /// <summary>
        /// PlugInVersionInfos
        /// </summary>
        internal PluginVersionInfo[] PlugInVersionInfos { get; set; }
    }
}
