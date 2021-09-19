using BusParkDispatcher.Commands.Base;
using BusParkDispatcher.Views.Windows;

namespace BusParkDispatcher.ViewModels
{
    class BusStopsAdditionViewModel : ObservableObject
    {
        #region Fields
        private string название;
        private string описание;
        #endregion

        #region Properties
        public string Название
        {
            set
            {
                if (value.Length > 30)
                    return;

                SetProperty(ref название, value);
            }
            get => название;
        }

        public string Описание
        {
            set
            {
                if (value.Length > 150)
                    return;

                SetProperty(ref описание, value);
            }
            get => описание;
        }

        #endregion

        #region Constructors
        #endregion

        #region Commands / Methods
        public DelegateCommand Add => new DelegateCommand((obj) =>
        {
            MainWindowViewModel.Database.Остановки.Add(new Models.Database.Остановки() { Название = Название, Описание = Описание });

            if (obj is AdditionalWindow window)
            {
                window.DialogResult = true;
                window.Close();
            }
        });
        #endregion
    }
}
