using BusParkDispatcher.Commands.Base;
using BusParkDispatcher.ViewModels;

namespace BusParkDispatcher.Commands
{
    class DragWindowCommand : Command
    {
        public override bool CanExecute(object parameter = null) => true;

        public override void Execute(object parameter = null) => MainWindowViewModel.MainWindow.DragMove();
    }
}
