using BusParkDispatcher.Commands.Base;
using System.Windows;

namespace BusParkDispatcher.Commands
{
    class CloseApplicationCommand : Command
    {
        public override bool CanExecute(object parameter = null) => true;

        public override void Execute(object parameter = null) => Application.Current.Shutdown();
    }
}
