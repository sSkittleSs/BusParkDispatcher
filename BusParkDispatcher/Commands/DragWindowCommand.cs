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
    class DragWindowCommand : Command
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter) => MainWindowViewModel.MainWindow.DragMove();
    }
}
