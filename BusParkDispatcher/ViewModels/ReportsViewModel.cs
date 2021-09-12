using BusParkDispatcher.Commands.Base;
using BusParkDispatcher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            расписание.Add(new BusStopTime("Каменногорская 18", new TimeSpan(8, 30, 0)));
            расписание.Add(new BusStopTime("Каменногорская", new TimeSpan(8, 35, 0)));
            расписание.Add(new BusStopTime("Казимировская 21", new TimeSpan(8, 40, 0)));
            расписание.Add(new BusStopTime("Казимировская", new TimeSpan(8, 45, 0)));

            var timetableReport = new TimetableReport(159, DateTime.Today, true, расписание);

            timetableReport.WriteToNewExcelFile("D:\\\\test.xlsx");
        });
        #endregion
    }
}
