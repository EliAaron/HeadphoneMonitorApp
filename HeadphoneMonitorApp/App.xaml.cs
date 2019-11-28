using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace HeadphoneMonitorApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public string[] CommandLineArgs { get; private set; }

        public App()
        {
            //this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            CommandLineArgs = e.Args;

            base.OnStartup(e);
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string msg = string.Format(
                "An unhandled exception occurred.\n\nError: {0}\n\nFor more iformatin, see {1}",
                e.Exception.Message,
                ErrorLogger.FileNeme);

            MessageBox.Show(msg, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);

            string errorLogMsg =
                "Error Mesege:\r\n" + e.Exception.Message + "\r\n\r\n" +
                "Error Source:\r\n" + e.Exception.Source + "\r\n\r\n" +
                "Stack Trace:\r\n" + e.Exception.StackTrace;

            ErrorLogger.Log(ErrorLogger.ErrorType.UnhandledException, errorLogMsg, DateTime.Now);
        }
    }
}
