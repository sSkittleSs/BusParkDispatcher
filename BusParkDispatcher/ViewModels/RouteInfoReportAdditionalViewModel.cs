using BusParkDispatcher.Commands.Base;
using BusParkDispatcher.Models;
using BusParkDispatcher.Models.Database;
using BusParkDispatcher.Views.Windows;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusParkDispatcher.ViewModels
{
    class RouteInfoReportAdditionalViewModel : ObservableObject
    {
        #region Fields
        private int кодМаршрута;
        private Маршруты selectedRoute;
        #endregion

        #region Properties
        public int КодМаршрута
        {
            set => SetProperty(ref кодМаршрута, value);
            get => кодМаршрута;
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

        public ICollection<Маршруты> Маршруты { set; get; }
        #endregion

        #region Constructors
        public RouteInfoReportAdditionalViewModel()
        {
            Маршруты = MainWindowViewModel.Database.Маршруты.Local;
        }
        #endregion

        #region Commands / Methods
        public DelegateCommand CreateReport => new DelegateCommand((obj) =>
        {
            try
            {
                ReportsViewModel.SaveReportToExcelFile(new RouteInfoReport(SelectedRoute?.НомерМаршрута ?? -1));

                if (obj is AdditionalWindow window)
                {
                    window.DialogResult = true;
                    window.Close();
                }
            }
            catch (Exception e) { NotificationManager.ShowError(e.Message); }

        });

        public DelegateCommand CreateReportForAllRoutes => new DelegateCommand((obj) =>
        {
            try
            {
                var excelPackage = new ExcelPackage();
                foreach (var item in Маршруты)
                    excelPackage = new RouteInfoReport(item.НомерМаршрута).AddNewSheet(excelPackage, $"Маршрут №{item.НомерМаршрута}");

                ReportsViewModel.SaveExcelPackage(excelPackage);

                if (obj is AdditionalWindow window)
                {
                    window.DialogResult = true;
                    window.Close();
                }
            }
            catch (Exception e) { NotificationManager.ShowError(e.Message); }

        });
        #endregion
    }
}
