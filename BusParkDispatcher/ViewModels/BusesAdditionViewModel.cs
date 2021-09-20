using BusParkDispatcher.Commands.Base;
using BusParkDispatcher.Models;
using BusParkDispatcher.Models.Database;
using BusParkDispatcher.Views;
using BusParkDispatcher.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusParkDispatcher.ViewModels
{
    class BusesAdditionViewModel : ObservableObject
    {
        #region Fields
        private string регистрационныйНомер;
        private string количествоМест;
        private int кодТипа;
        private int кодМаршрута;
        private ТипыАвтобусов selectedBusType;
        private Маршруты selectedRoute;
        #endregion

        #region Properties
        public string РегистрационныйНомер
        {
            set
            {
                if (value.Length > 9)
                    return;

                SetProperty(ref регистрационныйНомер, value);
            }
            get => регистрационныйНомер;
        }

        public string КоличествоМест
        {
            set
            {
                if (int.TryParse(value, out int res))
                {
                    if (res < 0)
                    {
                        NotificationManager.ShowError("Количество мест не может быть отрицательным.");
                        return;
                    }

                    SetProperty(ref количествоМест, value);
                }
            }
            get => количествоМест;
        }

        public int КодТипа
        {
            set => SetProperty(ref кодТипа, value);
            get => кодТипа;
        }

        public int КодМаршрута
        {
            set => SetProperty(ref кодМаршрута, value);
            get => кодМаршрута;
        }

        public ТипыАвтобусов SelectedBusType
        {
            set
            {
                SetProperty(ref selectedBusType, value);
                КодТипа = SelectedBusType?.КодТипа ?? 0;
            }
            get => selectedBusType;
        }

        public Маршруты SelectedRoute
        {
            set
            {
                SetProperty(ref selectedRoute, value);
                КодМаршрута = SelectedRoute?.КодМаршрута ?? 0;
            }
            get => selectedRoute;
        }

        public ICollection<ТипыАвтобусов> ТипыАвтобусов { set; get; }
        public ICollection<Маршруты> Маршруты { set; get; }
        #endregion

        #region Constructors
        public BusesAdditionViewModel()
        {
            ТипыАвтобусов = MainWindowViewModel.Database.ТипыАвтобусов.Local;
            Маршруты = MainWindowViewModel.Database.Маршруты.Local;
        }
        #endregion

        #region Commands / Methods
        public DelegateCommand Add => new DelegateCommand((obj) =>
        {
            MainWindowViewModel.Database.Автобусы.Local.Add(new Автобусы() { РегистрационныйНомер = РегистрационныйНомер, КоличествоМест = Convert.ToInt32(КоличествоМест), КодТипа = КодТипа, КодМаршрута = КодМаршрута });
            
            if (obj is AdditionalWindow window)
            {
                window.DialogResult = true;
                window.Close();
            }
        });
        #endregion
    }
}
