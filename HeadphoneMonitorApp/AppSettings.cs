using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;

namespace HeadphoneMonitorApp
{
    /// <summary>
    /// Loads and saves application settings. 
    /// </summary>
    [Serializable]
    public class AppSettings : ICloneable
    {
        #region Setting Properties

        public VolumeAction HeadphonesConnectedAction { get; set; }

        public VolumeAction HeadphonesNotConnectedAction { get; set; }

        public bool MinimizeToTrayOnMinimize { get; set; }

        public bool MinimizeToTrayOnClose { get; set; }

        public bool LaunchOnStartup { get; set; }

        public bool LaunchMinimized { get; set; }

        public ProcessPriority ProcessPriority { get; set; }

       #endregion Setting Properties


       #region Setting Defaults

       public static readonly VolumeAction DefaultHeadphonesConnectedAction = VolumeAction.Unmute;

        public static readonly VolumeAction DefaultHeadphonesNotConnectedAction = VolumeAction.Mute;

        public static readonly bool DefaultMinimizeToTrayOnMinimize = false;

        public static readonly bool DefaultMinimizeToTrayOnClose = false;

        public static readonly bool DefaultLaunchOnStartup = false;

        public static readonly bool DefaultLaunchMinimized = false;

        public static readonly ProcessPriority DefaultProcessPriority = ProcessPriority.Normal;

        #endregion Setting Defaults


        #region Constructors

        static AppSettings()
        {
        }

        /// <summary>
        /// AppSettings constructor.
        /// </summary>
        public AppSettings()
        {
            Init();
        }

        #endregion Constructors


        #region Methods

        /// <summary>
        /// Initialize the settings - the settings are set to their default.
        /// </summary>
        public void Init()
        {
            HeadphonesConnectedAction = DefaultHeadphonesConnectedAction;
            HeadphonesNotConnectedAction = DefaultHeadphonesNotConnectedAction;
            MinimizeToTrayOnMinimize = DefaultMinimizeToTrayOnMinimize;
            MinimizeToTrayOnClose = DefaultMinimizeToTrayOnClose;
            LaunchOnStartup = DefaultLaunchOnStartup;
            LaunchMinimized = DefaultLaunchMinimized;
            ProcessPriority = DefaultProcessPriority;
        }

        /// <summary>
        /// Save setting to a file (XML).
        /// </summary>
        /// <param name="fileName"></param>
        public void Save(string fileName)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(AppSettings));

            Stream stream = new FileStream(fileName,
                                           FileMode.Create,
                                           FileAccess.Write,
                                           FileShare.None);

            xmlSerializer.Serialize(stream, this);
            stream.Close();
        }

        /// Load setting from a file (XML).
        public static AppSettings Load(string fileName)
        {
            AppSettings appSettings = null;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(AppSettings));

            using (Stream stream = new FileStream(fileName,
                                           FileMode.Open,
                                           FileAccess.Read,
                                           FileShare.None))
            {

                appSettings = (AppSettings)xmlSerializer.Deserialize(stream);
            }

            appSettings.Verify();

            return appSettings;
        }

        /// <summary>
        /// Verify that the settings are OK.
        /// Currently this method is empty.
        /// </summary>
        public void Verify()
        {
        }

        public AppSettings Clone()
        {
            AppSettings appSettings = new AppSettings();

            //...

            return appSettings;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion Methods
    }
}




