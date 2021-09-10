using BusParkDispatcher.Commands.Base;
using BusParkDispatcher.Models;
using System.Diagnostics;
using System.IO;

namespace BusParkDispatcher.Commands
{
    class OpenHelpCommand : Command
    {
        public override bool CanExecute(object parameter = null) => true;

        public override void Execute(object parameter = null)
        {
            if (File.Exists(Directory.GetCurrentDirectory() + "\\Справка\\index.htm"))
            {
                Process.Start(Directory.GetCurrentDirectory() + "\\Справка\\index.htm");
                NotificationManager.ShowInformation("Справочная система открывается, ожидайте...");
            }
            else
            {
                NotificationManager.ShowError("Невозможно запустить справочную систему, поскольку она отсутствует или повреждена.");
            }
        }
    }
}
