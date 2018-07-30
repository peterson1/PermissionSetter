using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.FileSystemTools;
using PermissionSetter.Lib11.Configuration;
using System.IO;
using static FolderWatcherExe.Properties.Settings;


namespace FolderWatcherExe.Configuration
{
    public class CommandLineArgs : IPermissionSettings
    {
        public CommandLineArgs()
        {
            TargetFolder    = ExeDirIfBlank (Default.TargetFolder);
            UpdatedCopyPath = BlankIfMissing(Default.UpdatedCopyPath);
            WatcherDelayMS  = Default.WatcherDelayMS;
        }


        public string  TargetFolder     { get; }
        public string  UpdatedCopyPath  { get; }
        public uint    WatcherDelayMS   { get; }


        private string BlankIfMissing(string filePath)
            => File.Exists(filePath) ? filePath : "";


        private string ExeDirIfBlank(string foldrPath)
            => foldrPath.IsBlank() ? CurrentExe.GetDirectory() 
                                   : foldrPath;
    }
}
