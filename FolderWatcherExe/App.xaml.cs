using CommonTools.Lib45.ApplicationTools;
using FolderWatcherExe.Configuration;
using PermissionSetter.Lib11.Configuration;
using System.Windows;

namespace FolderWatcherExe
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.Initialize<CommandLineArgs>(args
                => new MainWindowVM(args).Show<MainWindow>());
        }
    }
}
