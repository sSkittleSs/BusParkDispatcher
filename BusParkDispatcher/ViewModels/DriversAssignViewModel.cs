using BusParkDispatcher.Commands.Base;
using BusParkDispatcher.Models.Database;
using BusParkDispatcher.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusParkDispatcher.ViewModels
{
    class DriversAssignViewModel : ObservableObject
    {
        #region Fields
        private int кодВодителя;
        private int кодАвтобуса;
        private Водители selectedDriver;
        private Автобусы selectedBus;
        #endregion

        #region Properties
        public int КодВодителя
        {
            set => SetProperty(ref кодВодителя, value);
            get => кодВодителя;
        }

        public int КодАвтобуса
        {
            set => SetProperty(ref кодАвтобуса, value);
            get => кодАвтобуса;
        }

        public Водители SelectedDriver
        {
            set
            {
                SetProperty(ref selectedDriver, value);
                КодВодителя = SelectedDriver?.КодВодителя ?? 0;
            }
            get => selectedDriver;
        }

        public Автобусы SelectedBus
        {
            set
            {
                SetProperty(ref selectedBus, value);
                КодАвтобуса = SelectedBus?.КодАвтобуса ?? 0;
            }
            get => selectedBus;
        }

        public ICollection<Водители> Водители { set; get; }
        public ICollection<Автобусы> Автобусы { set; get; }
        #endregion

        #region Constructors
        public DriversAssignViewModel()
        {
            Водители = MainWindowViewModel.Database.Водители.Local;
            Автобусы = MainWindowViewModel.Database.Автобусы.Local;
        }
        #endregion

        #region Commands / Methods
        public DelegateCommand Add => new DelegateCommand((obj) =>
        {
            var driver = MainWindowViewModel.Database.Водители.Local.FirstOrDefault((item) => item.КодВодителя == КодВодителя);
            MainWindowViewModel.Database.Автобусы.Local.FirstOrDefault((item) => item.КодАвтобуса == КодАвтобуса).Водители.Add(driver);

            if (obj is AdditionalWindow window)
            {
                window.DialogResult = true;
                window.Close();
            }
        });
        #endregion
    }
}
