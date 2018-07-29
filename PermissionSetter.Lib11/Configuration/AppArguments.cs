using CommonTools.Lib11.FileSystemTools;

namespace PermissionSetter.Lib11.Configuration
{
    public class AppArguments : IHasUpdatedCopy
    {
        public string  UpdatedCopyPath  { get; set; }
        public string  TargetFolder     { get; set; }
    }
}
