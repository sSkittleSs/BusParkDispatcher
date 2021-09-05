using BusParkDispatcher.Commands.Base;
using BusParkDispatcher.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusParkDispatcher.Commands
{
    class OpenHelpCommand : Command
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
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
