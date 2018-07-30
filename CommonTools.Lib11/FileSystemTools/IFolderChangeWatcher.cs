using System;

namespace CommonTools.Lib11.FileSystemTools
{
    public interface IFolderChangeWatcher
    {
        event EventHandler<string> FileChanged;

        void   StartWatching  (string folderPath);
        void   StopWatching   ();

        string TargetFolder   { get; }
    }
}
