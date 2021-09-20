using BusParkDispatcher.Commands.Base;
using BusParkDispatcher.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusParkDispatcher.ViewModels
{
    class DriversAdditionViewModel : ObservableObject
    {
        #region Fields
        private string фио;
        private string номерТелефона;
        #endregion

        #region Properties
        public string ФИО
        {
            set
            {
                if (value.Length > 150)
                    return;

                SetProperty(ref фио, value);
            }
            get => фио;
        }

        public string НомерТелефона
        {
            set
            {
                if (value.Length > 12 || !long.TryParse(value, out long result))
                    return;

                SetProperty(ref номерТелефона, value);
            }
            get => номерТелефона;
        }

        #endregion

        #region Constructors
        #endregion

        #region Commands / Methods
        public DelegateCommand Add => new DelegateCommand((obj) =>
        {
            MainWindowViewModel.Database.Водители.Local.Add(new Models.Database.Водители() { ФИО = ФИО, НомерТелефона = Convert.ToInt64(НомерТелефона) });

            if (obj is AdditionalWindow window)
            {
                window.DialogResult = true;
                window.Close();
            }
        });
        #endregion
    }
}
