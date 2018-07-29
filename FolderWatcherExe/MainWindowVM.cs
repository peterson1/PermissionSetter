using CommonTools.Lib45.BaseViewModels;
using PermissionSetter.Lib11.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderWatcherExe
{
    public class MainWindowVM : UpdatedExeVMBase<AppArguments>
    {
        protected override string CaptionPrefix => "[PermissionSetter] Folder Watcher";


        public MainWindowVM(AppArguments appArguments) : base(appArguments)
        {
        }

    }
}
