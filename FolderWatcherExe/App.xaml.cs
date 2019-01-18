using CommonTools.Lib45.ApplicationTools;
using FolderWatcherExe.Configuration;
using NLog;
using NLog.Config;
using NLog.Targets;
using System.Windows;

namespace FolderWatcherExe
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            LogManager.Configuration = CreateNLogConfig();

            this.Initialize<CommandLineArgs>(args
                => new MainWindowVM(args).Show<MainWindow>());
        }


        private LoggingConfiguration CreateNLogConfig()
        {
            var cfg = new LoggingConfiguration();
            var trg = new FileTarget()
            {
                FileName = "${basedir}/logs/${longdate}.log",
                Layout   = "[${time}]   ${message}  ${exception}",
            };
            cfg.AddRule(LogLevel.Trace, LogLevel.Fatal, trg);
            return cfg;
        }
    }
}
