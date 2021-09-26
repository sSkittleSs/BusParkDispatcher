using BusParkDispatcher.Commands.Base;
using BusParkDispatcher.Infrastructure;
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
    class HoursInDriveReportAdditionalViewModel : ObservableObject
    {
        #region Fields
        private int кодВодителя;
        private Водители selectedDriver;
        #endregion

        #region Properties
        public int КодВодителя
        {
            set => SetProperty(ref кодВодителя, value);
            get => кодВодителя;
        }

        public Водители SelectedDriver
        {
            set
            {
                SetProperty(ref selectedDriver, value);
                КодВодителя = SelectedDriver?.КодВодителя ?? 0;

                if (SelectedDriver.Автобусы.Count == 0)
                    NotificationManager.ShowWarning($"Водителю ({SelectedDriver.ФИО}) не присвоен автобус.");
            }
            get => selectedDriver;
        }

        public ICollection<Водители> Водители { set; get; }
        #endregion

        #region Constructors
        public HoursInDriveReportAdditionalViewModel()
        {
            Водители = MainWindowViewModel.Database.Водители.Local;
        }
        #endregion

        #region Commands / Methods
        public DelegateCommand CreateReport => new DelegateCommand((obj) =>
        {
            try
            {
                ReportsViewModel.SaveReportToExcelFile(new HoursInDriveReport(КодВодителя));

                if (obj is AdditionalWindow window)
                {
                    window.DialogResult = true;
                    window.Close();
                }
            }
            catch (Exception e) { NotificationManager.ShowError(e.Message); }

        });

        public DelegateCommand CreateReportForAllDrivers => new DelegateCommand((obj) =>
        {
            try
            {
                var excelPackage = new ExcelPackage();
                foreach (var item in Водители)
                    excelPackage = new HoursInDriveReport(item.КодВодителя).AddNewSheet(excelPackage, item.ФИО);

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
