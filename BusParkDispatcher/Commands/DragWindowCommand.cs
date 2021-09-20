using BusParkDispatcher.Commands.Base;
using BusParkDispatcher.ViewModels;
using System.Windows;

namespace BusParkDispatcher.Commands
{
    class DragWindowCommand : Command
    {
        public override bool CanExecute(object parameter = null) => true;

        public override void Execute(object parameter = null)
        {
            if (parameter is Window window)
                window.DragMove();
        }
    }
}
