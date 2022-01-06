namespace Hisense.OpenMES.HttpServer
{
    /// <summary>
    /// PluginVersionInfo
    /// </summary>
    [Serializable]
    internal class PluginVersionInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        internal PluginVersionInfo()
        {

        }

        /// <summary>
        /// Name
        /// </summary>
        internal string Name { get; set; }

        /// <summary>
        /// Version
        /// </summary>
        internal string Version { get; set; }

        /// <summary>
        /// PackageURL
        /// </summary>
        internal string PackageURL { get; set; }

        /// <summary>
        /// ChangeLog
        /// </summary>
        internal string ChangeLog { get; set; }

        /// <summary>
        /// MandatoryInfo
        /// </summary>
        internal Mandatory MandatoryInfo { get; set; }

        /// <summary>
        /// Mandatory
        /// </summary>
        internal class Mandatory
        {
            /// <summary>
            /// Value
            /// </summary>
            internal bool Value { get; set; }

            /// <summary>
            /// Mode
            /// </summary>
            internal int Mode { get; set; }
        }
    }
}
