using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using BuildingEditor.View;

namespace BuildingEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            var args = e.Args;

            MainWindow window;
            if (args.Length > 0)
                window = new MainWindow(args[0]);
            else
                window = new MainWindow();

            window.Show();
        }
    }
}
