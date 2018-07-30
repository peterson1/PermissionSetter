using CommonTools.Lib11.FileSystemTools;
using CommonTools.Lib45.FileSystemTools;
using PermissionSetter.Lib11.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FolderWatcherExe.ChangeHandlers
{
    public class EveryoneReadWritesHandler
    {
        private IPermissionSettings  _cfg;
        private IFolderChangeWatcher _watchr;

        public event EventHandler<string> Logged = delegate { };


        public EveryoneReadWritesHandler(IPermissionSettings permissionSettings)
        {
            _cfg    = permissionSettings;
            _watchr = new ThrottledFolderWatcher1(1000);
            _watchr.FileChanged += (s, e) => OnFileChanged(e);
        }


        private async void OnFileChanged(string filePath)
        {
            var nme = Path.GetFileNameWithoutExtension(filePath);
            await WaitForDelay((int)_cfg.WatcherDelayMS, nme);
            //if (HasEveryoneAccess(filePath, nme)) return;

            Log($"Attempting to grant Everyone full control of “{nme}” ...");

            var ok = filePath.TryGrantEveryoneFullControl();

            Log(ok ? $"Successful. Everyone now has full control of “{nme}”."
                   : $"Failed to grant Everyone full control of “{nme}”.");
        }


        private bool HasEveryoneAccess(string filePath, string nme)
        {
            var has = filePath.HasEveryoneFullControl();
            Log(has ? $"Everyone already has full control of “{nme}”."
                    : $"Not everyone has full control of “{nme}”.");
            return has;
        }


        private async Task WaitForDelay(int delayMS, string filename)
        {
            Log($"“{filename}” changed.  Delaying for {delayMS} ms ...");
            await Task.Delay(delayMS);
            Log($"Executing handler for “{filename}” ...");
        }


        public void StartWatching()
        {
            _watchr.StartWatching(_cfg.TargetFolder);
            Log($"Started watching target folder (delay: {_cfg.WatcherDelayMS}ms):");
            Log(_cfg.TargetFolder);
        }


        private void Log(string text) => Logged?.Invoke(this, text);
    }
}
