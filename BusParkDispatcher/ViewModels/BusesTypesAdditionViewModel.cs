using BusParkDispatcher.Commands.Base;
using BusParkDispatcher.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusParkDispatcher.ViewModels
{
    class BusesTypesAdditionViewModel : ObservableObject
    {
        #region Fields
        private string наименование;
        private string описание;
        #endregion

        #region Properties
        public string Наименование
        {
            set
            {
                if (value.Length > 50)
                    return;

                SetProperty(ref наименование, value);
            }
            get => наименование;
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
            MainWindowViewModel.Database.ТипыАвтобусов.Add(new Models.Database.ТипыАвтобусов() { Наименование = Наименование, Описание = Описание });

            if (obj is AdditionalWindow window)
            {
                window.DialogResult = true;
                window.Close();
            }
        });
        #endregion
    }
}
