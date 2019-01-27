using CommonTools.Lib11.FileSystemTools;
using CommonTools.Lib45.FileSystemTools;
using PermissionSetter.Lib11.Configuration;
using System;
using System.IO;
using System.Linq;
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
            _watchr.FileChanged += async (s, e) => await OnFileChanged(e, true);
        }


        private async Task OnFileChanged(string filePath, bool withDelay)
        {
            var nme   = Path.GetFileNameWithoutExtension(filePath);
            var delay = withDelay ? _cfg.WatcherDelayMS : 0;

            await WaitForDelay((int)delay, nme);
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


        public async void StartWatching()
        {
            await ProcessAllFiles();
            _watchr.StartWatching(_cfg.TargetFolder);
            Log($"Started watching target folder (delay: {_cfg.WatcherDelayMS}ms):");
            Log(_cfg.TargetFolder);
        }


        private async Task ProcessAllFiles()
        {
            var files = Directory.GetFiles((_cfg.TargetFolder));
            var sorted = files.Select           (_ => new FileInfo(_))
                              .OrderByDescending(_ => _.LastWriteTime)
                              .Select           (_ => _.FullName)
                              .ToList           ();

            foreach (var file in sorted)
                await OnFileChanged(file, false);
        }


        private void Log(string text) => Logged?.Invoke(this, text);
    }
}
