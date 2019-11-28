using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

//using System.Windows.Shapes;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace HeadphoneMonitorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isClosing = false;
        private bool _isLoaded = false;
        private bool _isShown = false;
        private bool _isInitCompleted = false;
        private bool _isInitializedComponent = false;

        private static readonly string DefaultWorkingDirectory = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;

        private readonly string AppSettingsFileName = string.Format("{0}\\{1}.AppSettings.xml",
            DefaultWorkingDirectory, Assembly.GetExecutingAssembly().GetName().Name);

        private HeadphoneMonitor _headphoneMonitor;
        private DispatcherTimer _dispatcherTimer;
        private AppSettings _appSettings = new AppSettings();

        private readonly ImageSource VolumeHighImageSource;
        private readonly ImageSource VolumeMutedImageSource;
        private readonly ImageSource HeadphoneImageSource;
        private readonly ImageSource HeadphoneXImageSource;

        private readonly System.Drawing.Icon VolumeHighIcon;
        private readonly System.Drawing.Icon VolumeMutedIcon;
        private readonly System.Drawing.Icon HeadphoneIcon;
        private readonly System.Drawing.Icon HeadphoneXIcon;

        private System.Windows.Forms.NotifyIcon _notifyIcon;
        private System.Windows.Forms.MenuItem _menuItemShow;
        private System.Windows.Forms.MenuItem _menuItemMinimize;
        private System.Windows.Forms.MenuItem _menuItemMinimizeToTray;
        private System.Windows.Forms.MenuItem _menuItemExit;

        private bool _exitRequested = false;

        private bool? _lastIsHeadphonesConnected = null;
        private bool? _lastIsMaute = null;
        private Exception _unhandledException = null;

        public ProcessPriority[] ProcessPriorities { get; } = new ProcessPriority[]
        {
            ProcessPriority.Low,
            ProcessPriority.BelowNormal,
            ProcessPriority.Normal,
            ProcessPriority.AboveNormal,
            ProcessPriority.High
        };

        public MainWindow()
        {
            InitializeComponent();
            _isInitializedComponent = true;

            Dispatcher.UnhandledException += Dispatcher_UnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // Init readonly fields

            VolumeHighImageSource = ((ImageBrush)Resources["volumeHighImage"]).ImageSource;
            VolumeMutedImageSource = ((ImageBrush)Resources["volumeMutedImage"]).ImageSource;
            HeadphoneImageSource = ((ImageBrush)Resources["headphoneImage"]).ImageSource;
            HeadphoneXImageSource = ((ImageBrush)Resources["headphoneXImage"]).ImageSource;

            VolumeHighIcon = GetIcon("Images/Status-audio-volume-high.ico");
            VolumeMutedIcon = GetIcon("Images/Status-audio-volume-muted.ico");
            HeadphoneIcon = GetIcon("Images/Iconmoon-Viva-Headphones.ico");
            HeadphoneXIcon = GetIcon("Images/Iconmoon-Viva-Headphones-X.ico");

            this.Loaded += MainWindow_Loaded;
            this.ContentRendered += Window_ContentRendered;

            Init();
        }

        private System.Drawing.Icon GetIcon(string path)
        {
            Uri volumeHighImageUri = new Uri(string.Format(path), UriKind.Relative);
            return new System.Drawing.Icon(Application.GetResourceStream(volumeHighImageUri).Stream);
        }

        private bool InitHeadphoneMonitor()
        {
            _headphoneMonitor = new HeadphoneMonitor();

            if (_headphoneMonitor.SpeakersDevice == null)
            {
                MessageBox.Show(
                    this,

                    "Could not find SpeakersDevice.\n" +
                    "The app cannot function correctly.\n" +
                    "App closing...",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                Exit();

                return false;
            }

            if (!_headphoneMonitor.HasHeadphoneProperty)
            {
                MessageBox.Show(
                    this,

                    "Could not find HeadphoneProperty.\n" +
                    "The app cannot function correctly.\n",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                //Exit();

                //return false;
            }

            _headphoneMonitor.RegisterNotifications();
            _headphoneMonitor.SpeakersVolumeNotification += HeadphoneMonitor_SpeakersVolumeNotification;
            _headphoneMonitor.HeadphonesConnectionChanged += HeadphoneMonitor_HeadphonesConnectionChanged;

            return true;
        }

        private bool Init()
        {
            InitWorkingDirectory();

            if (!InitHeadphoneMonitor())
            {
                return false;
            }

            InitNotifyIcon();

            LoadAppSettings();

            UpdateMonitor();
            UpdateVolumeDisplay(true);
            UpdateHeadphonesDisplay(true);

            _dispatcherTimer = new DispatcherTimer(DispatcherPriority.Send);
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(500);
            _dispatcherTimer.Tick += DispatcherTimer_Tick;
            _dispatcherTimer.Start();

            string version = FileVersionInfo.GetVersionInfo(
                Assembly.GetExecutingAssembly().Location).FileVersion;

            labelDesc.Content = "Version: " + version;

            _menuItemMinimize.Visible = !(checkBoxToTrayOnMinimize.IsChecked ?? false);

            ParseCommandLineArgs();

            checkBoxLaunchMinimized.Foreground = _appSettings.LaunchOnStartup ? Brushes.Black : Brushes.Gray;

            _isInitCompleted = true;

            return true;
        }

        private static void InitWorkingDirectory()
        {
            if (DefaultWorkingDirectory != Directory.GetCurrentDirectory())
            {
                Directory.SetCurrentDirectory(DefaultWorkingDirectory);
            }
        }

        private void ParseCommandLineArgs()
        {
            string[] commandLineArgs = (Application.Current as App).CommandLineArgs;
            if (commandLineArgs != null && commandLineArgs.Length > 0)
            {
                if (commandLineArgs[0] == "-m")
                {
                    this.MinimizeToTray();
                }
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            this._isShown = true;
            this.ContentRendered -= Window_ContentRendered;

            SetStartupProgram();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if ((checkBoxToTrayOnClose.IsChecked ?? false) && !_exitRequested)
            {
                MinimizeToTray();
                e.Cancel = true;
            }
            else
            {
                _isClosing = true;
            }

            _exitRequested = false;

            base.OnClosing(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            if (_notifyIcon != null)
            {
                _notifyIcon.Dispose();
                _notifyIcon = null;
            }

            SaveAppSettings(true);

            base.OnClosed(e);
        }

        private void Exit()
        {
            _exitRequested = true;
            this.Close();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized &&
                (checkBoxToTrayOnMinimize.IsChecked ?? false))
            {
                WindowState = WindowState.Normal;
                MinimizeToTray();
            }

            base.OnStateChanged(e);
        }

        private void InitNotifyIcon()
        {
            _notifyIcon = new System.Windows.Forms.NotifyIcon();
            _notifyIcon.Icon = HeadphoneIcon;
            _notifyIcon.Visible = true;
            _notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
            _notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu();
            _notifyIcon.Text = this.Title;
            _menuItemShow = new System.Windows.Forms.MenuItem("Show");
            _menuItemShow.Click += MenuItemShow_Click;
            _notifyIcon.ContextMenu.MenuItems.Add(_menuItemShow);

            _menuItemMinimize = new System.Windows.Forms.MenuItem("Minimize");
            _menuItemMinimize.Click += MenuItemMinimize_Click;
            _notifyIcon.ContextMenu.MenuItems.Add(_menuItemMinimize);

            _menuItemMinimizeToTray = new System.Windows.Forms.MenuItem("Minimize to tray");
            _menuItemMinimizeToTray.Click += MenuItemMinimizeToTray_Click;
            _notifyIcon.ContextMenu.MenuItems.Add(_menuItemMinimizeToTray);

            _menuItemExit = new System.Windows.Forms.MenuItem("Exit");
            _menuItemExit.Click += MenuItemExit_Click;
            _notifyIcon.ContextMenu.MenuItems.Add(_menuItemExit);
        }

        /// <summary>
        /// Load AppSettings from file.
        /// </summary>
        private void LoadAppSettings()
        {
            bool loaded = false;

            if (File.Exists(AppSettingsFileName))
            {
                try
                {
                    _appSettings = AppSettings.Load(AppSettingsFileName);
                    loaded = true;
                }
                catch
                {
                    //
                }
            }

            if (!loaded)
            {
                _appSettings = new AppSettings();
                SaveAppSettings(false);
            }

            SetAppFromAppSettings();
        }

        /// <summary>
        /// Save AppSettings to file.
        /// </summary>
        private void SaveAppSettings(bool setAppSettingsFromApp)
        {
            if (setAppSettingsFromApp)
            {
                SetAppSettingsFromApp();
            }

            _appSettings.Save(AppSettingsFileName);
        }

        private void SetAppFromAppSettings()
        {
            comboxHeadphonesConnectedAction.SelectedItem = _appSettings.HeadphonesConnectedAction;
            comboxHeadphonesNotConnectedAction.SelectedItem = _appSettings.HeadphonesNotConnectedAction;

            checkBoxToTrayOnMinimize.IsChecked = _appSettings.MinimizeToTrayOnMinimize;
            checkBoxToTrayOnClose.IsChecked = _appSettings.MinimizeToTrayOnClose;

            checkBoxLaunchOnStartup.IsChecked = _appSettings.LaunchOnStartup;
            checkBoxLaunchMinimized.IsChecked = _appSettings.LaunchMinimized;

            Process.GetCurrentProcess().PriorityClass = (ProcessPriorityClass)_appSettings.ProcessPriority;
            comboxProcessPriority.SelectedItem = _appSettings.ProcessPriority;
        }

        private void SetAppSettingsFromApp()
        {
            _appSettings.HeadphonesConnectedAction = (VolumeAction)comboxHeadphonesConnectedAction.SelectedItem;
            _appSettings.HeadphonesNotConnectedAction = (VolumeAction)comboxHeadphonesNotConnectedAction.SelectedItem;

            _appSettings.MinimizeToTrayOnMinimize = checkBoxToTrayOnMinimize.IsChecked ?? false;
            _appSettings.MinimizeToTrayOnClose = checkBoxToTrayOnClose.IsChecked ?? false;

            _appSettings.LaunchOnStartup = checkBoxLaunchOnStartup.IsChecked ?? false;
            _appSettings.LaunchMinimized = checkBoxLaunchMinimized.IsChecked ?? false;

            _appSettings.ProcessPriority = (ProcessPriority)comboxProcessPriority.SelectedItem;
        }

        private void UpdateMonitor()
        {
            if (_headphoneMonitor == null) return;

            _headphoneMonitor.HeadphonesConnectedAction = (VolumeAction)comboxHeadphonesConnectedAction.SelectedItem;
            _headphoneMonitor.HeadphonesNotConnectedAction = (VolumeAction)comboxHeadphonesNotConnectedAction.SelectedItem;
            _headphoneMonitor.Monitor();
        }

        private void UpdateVolumeDisplay(bool forceUpdate = false)
        {
            //_headphoneMonitor.SpeakersVolumeNotification -= HeadphoneMonitor_SpeakersVolumeNotification;
            bool isMute = _headphoneMonitor.Mute;

            if (_lastIsMaute == null || _lastIsMaute != isMute || forceUpdate)
            {
                _lastIsMaute = isMute;

                if (!isMute)
                {
                    volumeImage.Source = VolumeHighImageSource;
                    //this.Icon = VolumeHighImageSource;
                    //_notifyIcon.Icon = VolumeHighIcon;
                }
                else
                {
                    volumeImage.Source = VolumeMutedImageSource;
                    //this.Icon = VolumeMutedImageSource;
                    //_notifyIcon.Icon = VolumeMutedIcon;
                }
            }

            //_headphoneMonitor.SpeakersVolumeNotification += HeadphoneMonitor_SpeakersVolumeNotification;
        }

        private void UpdateHeadphonesDisplay(bool forceUpdate = false)
        {
            bool connected = _headphoneMonitor.CheckIfHeadphonesConnected(false);

            if (_lastIsHeadphonesConnected == null ||
                _lastIsHeadphonesConnected != connected ||
                forceUpdate)
            {
                _lastIsHeadphonesConnected = connected;

                if (connected)
                {
                    headphonesImage.Source = HeadphoneImageSource;
                    this.Icon = HeadphoneImageSource;
                    _notifyIcon.Icon = HeadphoneIcon;
                }
                else
                {
                    headphonesImage.Source = HeadphoneXImageSource;
                    this.Icon = HeadphoneXImageSource;
                    _notifyIcon.Icon = HeadphoneXIcon;
                }
            }
        }

        private void MinimizeToTray()
        {
            this.Hide();
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            if (this.Visibility != Visibility.Visible)
            {
                ShowAndBringToFront();
            }
            else
            {
                MinimizeToTray();
            }
        }

        private void MenuItemMinimizeToTray_Click(Object sender, EventArgs e)
        {
            MinimizeToTray();
        }

        private void MenuItemMinimize_Click(Object sender, EventArgs e)
        {
            if (this.Visibility != Visibility.Visible)
            {
                this.Show();
            }
            this.WindowState = WindowState.Minimized;
        }

        private void MenuItemShow_Click(Object sender, EventArgs e)
        {
            ShowAndBringToFront();
        }

        private void ShowAndBringToFront()
        {
            // Keep this order!
            this.Show();
            this.WindowState = WindowState.Normal;
            this.Topmost = true;
            this.Topmost = false;
            this.Activate();
            this.Focus();
        }

        private static void SetStartupProgram(bool launchOnStartup, params string[] args)
        {
            SetStartupProgram(launchOnStartup, args as IEnumerable<string>);
        }

        private static void SetStartupProgram(bool launchOnStartup, IEnumerable<string> args)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            string appLocation = Assembly.GetExecutingAssembly().Location;
            string appName = Assembly.GetExecutingAssembly().GetName().Name;

            if (launchOnStartup)
            {
                if (args != null && args.Count() > 0)
                {
                    args = args.Where(s => !string.IsNullOrEmpty(s));
                }

                string commandLine;
                if (args != null && args.Count() > 0)
                {
                    commandLine = string.Format("\"{0}\" {1}", appLocation, string.Join(" ", args));
                }
                else
                {
                    commandLine = string.Format("\"{0}\"", appLocation);
                }

                rk.SetValue(appName, commandLine);
            }
            else
            {
                rk.DeleteValue(appName, false);
            }
        }

        private void SetStartupProgram()
        {
            List<string> args = new List<string>();
            if (_appSettings.LaunchMinimized)
            {
                args.Add("-m");
            }

            SetStartupProgram(_appSettings.LaunchOnStartup, args);
        }

        private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if (_unhandledException != e.Exception)
            {
                _unhandledException = e.Exception;

                string msg = string.Format(
                    "An unhandled exception occurred.\n\nError: {0}\n\nFor more information, see {1}",
                    e.Exception.Message,
                    ErrorLogger.FileNeme);

                MessageBox.Show(msg, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);

                string errorLogMsg =
                    "Error Message:\r\n" + e.Exception.Message + "\r\n\r\n" +
                    "Error Source:\r\n" + e.Exception.Source + "\r\n\r\n" +
                    "Stack Trace:\r\n" + e.Exception.StackTrace;

                ErrorLogger.Log(ErrorLogger.ErrorType.UnhandledException, errorLogMsg, DateTime.Now);
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string msg;
            string errorLogMsg;
            Exception ex = e.ExceptionObject as Exception;

            if (ex != null)
            {
                if (_unhandledException != ex)
                {
                    _unhandledException = ex;

                    msg = string.Format(
                        "An unhandled exception occurred.\n\nError: {0}\n\nFor more information, see {1}",
                        ex.Message,
                        ErrorLogger.FileNeme);

                    errorLogMsg =
                        "Error Message:\r\n" + ex.Message + "\r\n\r\n" +
                        "Error Source:\r\n" + ex.Source + "\r\n\r\n" +
                        "Stack Trace:\r\n" + ex.StackTrace;
                }
                else
                {
                    msg = string.Format(
                        "An unhandled exception occurred.\n\nError: ???, see {0}",
                        ErrorLogger.FileNeme);

                    errorLogMsg =
                        "Error Message:\r\n" + e.ExceptionObject + "\r\n\r\n";
                }

                //MessageBox.Show(msg, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);

                ErrorLogger.Log(ErrorLogger.ErrorType.UnhandledException, errorLogMsg, DateTime.Now);
            }

            if (e.IsTerminating)
            {
                try
                {
                    Exit();
                }
                catch { }
            }
        }

        private void MenuItemExit_Click(Object sender, EventArgs e)
        {
            Exit();
        }

        private void DispatcherTimer_Tick(Object sender, EventArgs e)
        {
            _headphoneMonitor.Monitor();
        }

        private void HeadphoneMonitor_SpeakersVolumeNotification(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                UpdateVolumeDisplay();
            }),
            DispatcherPriority.Send);
        }

        private void HeadphoneMonitor_HeadphonesConnectionChanged(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                UpdateHeadphonesDisplay();
            }),
            DispatcherPriority.Send);
        }

        private void comboxHeadphonesConnectedAction_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            if (!_isInitCompleted) return;

            UpdateMonitor();
            SaveAppSettings(true);
        }

        private void comboxHeadphonesNotConnectedAction_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            if (!_isInitCompleted) return;

            UpdateMonitor();
            SaveAppSettings(true);
        }

        private void btnMinimizeToTray_Click(Object sender, RoutedEventArgs e)
        {
            MinimizeToTray();
        }

        private void btnExit_Click(Object sender, RoutedEventArgs e)
        {
            Exit();
        }

        private void volumeImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _headphoneMonitor.Mute = !_headphoneMonitor.Mute;
        }

        private void checkBoxToTrayOnMinimize_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (!_isInitCompleted) return;

            SaveAppSettings(true);

            _menuItemMinimize.Visible = !(checkBoxToTrayOnMinimize.IsChecked ?? false);
        }

        private void checkBoxToTrayOnClose_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (!_isInitCompleted) return;

            SaveAppSettings(true);
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            //_menuItemShow.Enabled = !(this.WindowState != WindowState.Minimized && this.Visibility == Visibility.Visible && this.IsFocused) ;
            //_menuItemMinimize.Enabled = !(checkBoxToTrayOnMinimize.IsChecked ?? false) ;
            _menuItemMinimize.Enabled = this.WindowState != WindowState.Minimized || this.Visibility != Visibility.Visible;
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _menuItemMinimizeToTray.Enabled = (this.Visibility == Visibility.Visible);
            _menuItemMinimize.Enabled = this.WindowState != WindowState.Minimized || this.Visibility != Visibility.Visible;
            //_menuItemShow.Enabled = !(this.WindowState != WindowState.Minimized && this.Visibility == Visibility.Visible && this.IsFocused);
        }

        private void checkBoxLaunchOnStartup_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (!_isInitCompleted) return;

            SaveAppSettings(true);

            SetStartupProgram();

            checkBoxLaunchMinimized.Foreground = _appSettings.LaunchOnStartup ? Brushes.Black : Brushes.Gray;
        }

        private void checkBoxLaunchMinimized_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (!_isInitCompleted) return;

            SaveAppSettings(true);

            SetStartupProgram();
        }

        private void comboxProcessPriority_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isInitCompleted) return;

            SaveAppSettings(true);

            Process.GetCurrentProcess().PriorityClass = (ProcessPriorityClass)_appSettings.ProcessPriority;
        }
    }
}