using BusParkDispatcher.Commands.Base;
using BusParkDispatcher.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusParkDispatcher.ViewModels
{
    class TimetablesAdditionViewModel : ObservableObject
    {
        #region Fields
        private DateTime дата = DateTime.Today;
        private bool являетсяВыходным;
        #endregion

        #region Properties
        public DateTime Дата
        {
            set => SetProperty(ref дата, value);
            get => дата;
        }

        public bool ЯвляетсяВыходным
        {
            set => SetProperty(ref являетсяВыходным, value);
            get => являетсяВыходным;
        }

        #endregion

        #region Constructors
        #endregion

        #region Commands / Methods
        public DelegateCommand Add => new DelegateCommand((obj) =>
        {
            MainWindowViewModel.Database.Расписания.Local.Add(new Models.Database.Расписания() { Дата = Дата, ЯвляетсяВыходным = ЯвляетсяВыходным });

            if (obj is AdditionalWindow window)
            {
                window.DialogResult = true;
                window.Close();
            }
        });
        #endregion
    }
}
