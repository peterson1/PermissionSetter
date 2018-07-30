using CommonTools.Lib11.FileSystemTools;

namespace PermissionSetter.Lib11.Configuration
{
    public interface IPermissionSettings : IHasUpdatedCopy
    {
        string   TargetFolder    { get; }
        uint     WatcherDelayMS  { get; }
    }
}
