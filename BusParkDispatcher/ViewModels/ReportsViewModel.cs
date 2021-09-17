using BusParkDispatcher.Commands.Base;
using BusParkDispatcher.Infrastructure;
using BusParkDispatcher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BusParkDispatcher.ViewModels
{
    class ReportsViewModel : ObservableObject
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Contructors

        #endregion

        #region Commands / Methods
        public DelegateCommand CreateTimetableReport => new DelegateCommand((obj) =>
        {
            var расписание = new List<BusStopTime>();

#if DEBUG
            расписание.Add(new BusStopTime("Каменногорская 18", new TimeSpan(8, 30, 0)));
            расписание.Add(new BusStopTime("Каменногорская", new TimeSpan(8, 35, 0)));
            расписание.Add(new BusStopTime("Казимировская 21", new TimeSpan(8, 40, 0)));
            расписание.Add(new BusStopTime("Казимировская", new TimeSpan(8, 45, 0)));
#else
            var маршруты = MainWindowViewModel.Database.Маршруты.Local;
            var timetableReports = new List<TimetableReport>();
            foreach (var item in маршруты)
                timetableReports.Add(new TimetableReport(item.НомерМаршрута));
#endif

            SaveReportToExcelFile(new TimetableReport(159, DateTime.Today, true, расписание));
        });

        public DelegateCommand CreateBusesOnTheRoutesReport => new DelegateCommand((obj) =>
        {
            // Создаем коллекцию из объектов автобусов на маршрутах
            var автобусыНаМаршрутах = new List<BusesOnTheRoute>();

#if DEBUG
            автобусыНаМаршрутах.Add(new BusesOnTheRoute(1, 22));
            автобусыНаМаршрутах.Add(new BusesOnTheRoute(159, 8));
            автобусыНаМаршрутах.Add(new BusesOnTheRoute(42, 6));
            автобусыНаМаршрутах.Add(new BusesOnTheRoute(36, 2));
#else
            // получаем все маршруты из базы данных
            var маршруты = MainWindowViewModel.Database.Маршруты.Local;
            foreach (var item in маршруты) // Проходим по каждому маршруту и создаем объект на основе номера маршрута
                автобусыНаМаршрутах.Add(new BusesOnTheRoute(item.НомерМаршрута));
#endif

            SaveReportToExcelFile(new BusesOnTheRouteReport(автобусыНаМаршрутах));

        });

        public void SaveReportToExcelFile(IExcelReportGeneratable report)
        {
            // SaveReportToExcelFile метод

            // Создаем объект для запроса пути к новому файлу
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Настроиваем фильтры
            saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;

            // Если пользователь выбрал путь до файла, тогда записываем по данному пути уже готовые байты файла.
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                report.WriteToNewExcelFile(saveFileDialog.FileName);
        }
#endregion
    }
}
