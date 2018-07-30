using CommonTools.Lib11.DataStructures;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.ThreadTools;
using FolderWatcherExe.ChangeHandlers;
using PermissionSetter.Lib11.Configuration;
using NLog;

namespace FolderWatcherExe
{
    public class MainWindowVM : UpdatedExeVMBase<IPermissionSettings>
    {
        protected override string CaptionPrefix => "[PermissionSetter] Folder Watcher";
        private ILogger _nlog;


        public MainWindowVM(IPermissionSettings cfg) : base(cfg)
        {
            _nlog      = LogManager.GetCurrentClassLogger();
            PermSetter = new EveryoneReadWritesHandler(cfg);
            PermSetter.Logged += (s, e) => OnEventLogged(e);
            PermSetter.StartWatching();

            OnEventLogged($"{this.Caption}");
            UpdateNotifier.ExecuteOnFileChanged = true;
        }


        public EveryoneReadWritesHandler PermSetter { get; }
        public UIList<string> Logs { get; } = new UIList<string>();

        
        private void OnEventLogged(string text)
        {
            _nlog.Info(text);
            UIThread.Run(() => Logs.Add(text));
        }
    }
}
