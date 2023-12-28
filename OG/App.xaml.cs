using og.Utils;
using System.Configuration;
using System.Data;
using System.Windows;

namespace og
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            FileUtils.CheckDirectory(Constants.BasePath);
            FileUtils.CheckDirectory(Constants.LogPath);

            Logger.Start();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Logger.Log("Application ended");
            base.OnExit(e);
        }
    }

}
