using BusParkDispatcher.Commands.Base;
using BusParkDispatcher.Models;
using BusParkDispatcher.Models.Database;
using BusParkDispatcher.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusParkDispatcher.ViewModels
{
    class RoutesAdditionViewModel : ObservableObject
    {
        #region Fields
        private string номерМаршрута;
        private string описание;
        private int кодРасписания;
        private Расписания selectedTimetable;
        #endregion

        #region Properties
        public string НомерМаршрута
        {
            set
            {
                if (int.TryParse(value, out int res))
                {
                    if (res < 0)
                    {
                        NotificationManager.ShowError("Номер маршрута не может быть отрицательным.");
                        return;
                    }

                    SetProperty(ref номерМаршрута, value);
                }
            }
            get => номерМаршрута;
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

        public int КодРасписания
        {
            set => SetProperty(ref кодРасписания, value);
            get => кодРасписания;
        }

        public Расписания SelectedTimetable
        {
            set
            {
                SetProperty(ref selectedTimetable, value);
                КодРасписания = selectedTimetable?.КодРасписания ?? 0;
            }
            get => selectedTimetable;
        }

        public ICollection<Расписания> Расписания { set; get; }
        #endregion

        #region Constructors
        public RoutesAdditionViewModel()
        {
            Расписания = MainWindowViewModel.Database.Расписания.Local;
        }
        #endregion

        #region Commands / Methods
        public DelegateCommand Add => new DelegateCommand((obj) =>
        {
            MainWindowViewModel.Database.Маршруты.Add(new Маршруты() { НомерМаршрута = Convert.ToInt32(НомерМаршрута), Описание = Описание, КодРасписания = КодРасписания });

            if (obj is AdditionalWindow window)
            {
                window.DialogResult = true;
                window.Close();
            }
        });
        #endregion
    }
}
