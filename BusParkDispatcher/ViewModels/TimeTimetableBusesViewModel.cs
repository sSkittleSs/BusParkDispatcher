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
    class TimeTimetableBusesViewModel : ObservableObject
    {
        #region Fields
        private int кодВремени;
        private int кодРасписания;
        private int кодОстановки;
        private Время selectedTime;
        private Расписания selectedTimetable;
        private Остановки selectedBusStop;
        #endregion

        #region Properties
        public int КодВремени
        {
            set => SetProperty(ref кодВремени, value);
            get => кодВремени;
        }

        public int КодРасписания
        {
            set => SetProperty(ref кодРасписания, value);
            get => кодРасписания;
        }

        public int КодОстановки
        {
            set => SetProperty(ref кодОстановки, value);
            get => кодОстановки;
        }

        public Время SelectedTime
        {
            set
            {
                SetProperty(ref selectedTime, value);
                КодВремени = SelectedTime?.КодВремени ?? 0;
            }
            get => selectedTime;
        }

        public Расписания SelectedTimetable
        {
            set
            {
                SetProperty(ref selectedTimetable, value);
                КодРасписания = SelectedTimetable?.КодРасписания ?? 0;
            }
            get => selectedTimetable;
        }

        public Остановки SelectedBusStop
        {
            set
            {
                SetProperty(ref selectedBusStop, value);
                КодОстановки = SelectedBusStop?.КодОстановки ?? 0;
            }
            get => selectedBusStop;
        }

        public ICollection<Время> Время { set; get; }
        public ICollection<Расписания> Расписания { set; get; }
        public ICollection<Остановки> Остановки { set; get; }
        #endregion

        #region Constructors
        public TimeTimetableBusesViewModel()
        {
            Время = MainWindowViewModel.Database.Время.Local;
            Расписания = MainWindowViewModel.Database.Расписания.Local;
            Остановки = MainWindowViewModel.Database.Остановки.Local;
        }
        #endregion

        #region Commands / Methods
        public DelegateCommand Add => new DelegateCommand((obj) =>
        {
            MainWindowViewModel.Database.ВремяРасписанияОстановки.Local.Add(new ВремяРасписанияОстановки() { КодВремени = КодВремени, КодОстановки = КодОстановки, КодРасписания = КодРасписания });

            if (obj is AdditionalWindow window)
            {
                window.DialogResult = true;
                window.Close();
            }
        });
        #endregion
    }
}
