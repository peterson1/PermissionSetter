using CommonTools.Lib11.FileSystemTools;
using CommonTools.Lib11.StringTools;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CommonTools.Lib45.FileSystemTools
{
    public class FolderChangeWatcher1 : IFolderChangeWatcher, IDisposable
    {
        private      EventHandler<string> _fileChanged;
        public event EventHandler<string>  FileChanged
        {
            add    { _fileChanged -= value; _fileChanged += value; }
            remove { _fileChanged -= value; }
        }

        private FileSystemWatcher _fsWatchr;


        public string TargetFolder { get; private set; }


        public void StartWatching(string folderPath)
        {
            if (_fsWatchr != null) return;

            if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException($"Directory not found:{L.f}{folderPath}");


            TargetFolder = Path.IsPathRooted(folderPath) 
                         ? folderPath
                         : Path.Combine(CurrentExe.GetDirectory(), folderPath);

            _fsWatchr                     = new FileSystemWatcher(TargetFolder);
            _fsWatchr.NotifyFilter        = NotifyFilters.LastWrite;
            _fsWatchr.Changed            += new FileSystemEventHandler(OnLdbChanged);
            _fsWatchr.EnableRaisingEvents = true;
        }


        private void OnLdbChanged(object sender, FileSystemEventArgs e)
        {
            RaiseFolderChanged(e.FullPath);
        }


        protected virtual async void RaiseFolderChanged(string filePath)
        {
            try
            {
                _fileChanged?.Invoke(this, filePath);
            }
            catch (IOException)
            {
                await Task.Delay(1000);
                RaiseFolderChanged(filePath);
            }
        }


        public void StopWatching()
        {
            if (_fsWatchr == null) return;
            _fsWatchr.EnableRaisingEvents = false;
            _fsWatchr = null;
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    StopWatching();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
