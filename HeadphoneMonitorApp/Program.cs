using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadphoneMonitorApp
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            // Your initialization code
            App app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
