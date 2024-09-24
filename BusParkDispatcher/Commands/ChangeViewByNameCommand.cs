using BusParkDispatcher.Commands.Base;
using BusParkDispatcher.Models;
using BusParkDispatcher.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusParkDispatcher.Commands
{
    class ChangeViewByNameCommand : Command
    {
        public override bool CanExecute(object parameter = null) => true;

        public override void Execute(object parameter = null)
        {
            try { MainWindowViewModel.GetCurrentViewModel().OpenView?.Execute(parameter as string); }
            catch (Exception e) { NotificationManager.ShowError(e.Message); }
        }
    }
}
