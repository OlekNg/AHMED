using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Main
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Debug.WriteLine("Configuring Log4net");
            var fileInfo = new System.IO.FileInfo("Log4net.conf");
            if (fileInfo.Exists)
            {
                Debug.WriteLine("Found log4net conf.");
                log4net.Config.XmlConfigurator.ConfigureAndWatch(fileInfo);
            }
            
            base.OnStartup(e);
        }
    }
}
