using BusParkDispatcher.Commands.Base;
using BusParkDispatcher.Infrastructure;
using BusParkDispatcher.Models;
using BusParkDispatcher.Views;
using BusParkDispatcher.Views.Windows;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
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
            CheckDialogResult(() => new AdditionalWindow() { DataContext = new AdditionalWindowViewModel() { CurrentView = new TimetableReportAdditionalView() } }.ShowDialog() ?? false));


        public DelegateCommand CreateRouteInfoReport => new DelegateCommand((obj) =>
            CheckDialogResult(() => new AdditionalWindow() { DataContext = new AdditionalWindowViewModel() { CurrentView = new RouteInfoReportAdditionalView() } }.ShowDialog() ?? false));

        public DelegateCommand CreateHoursInDriveReport => new DelegateCommand((obj) =>
            CheckDialogResult(() => new AdditionalWindow() { DataContext = new AdditionalWindowViewModel() { CurrentView = new HoursInDriveReportAdditionalView() } }.ShowDialog() ?? false));

        public DelegateCommand CreateBusesOnTheRoutesReport => new DelegateCommand((obj) =>
        {
            // Создаем коллекцию из объектов автобусов на маршрутах
            var busesOnTheRoutes = new List<BusesOnTheRoute>();

            // получаем все маршруты из базы данных
            var routes = MainWindowViewModel.Database.Маршруты.Local;
            foreach (var item in routes) // Проходим по каждому маршруту и создаем объект на основе номера маршрута
                busesOnTheRoutes.Add(new BusesOnTheRoute(item.НомерМаршрута));

            SaveReportToExcelFile(new BusesOnTheRouteReport(busesOnTheRoutes));

        });

        public static bool SaveReportToExcelFile(IExcelReportGeneratable report)
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
            {
                
                return report.WriteToNewExcelFile(saveFileDialog.FileName);
            }
            else
            {
                NotificationManager.ShowInformation("Процесс сохранения отчета был прерван пользователем.");
                return false;
            }
        }

        public static bool SaveExcelPackage(ExcelPackage package)
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
            {
                try
                {
                    File.WriteAllBytes(saveFileDialog.FileName, package?.GetAsByteArray());
                }
                catch (Exception e)
                {
                    NotificationManager.ShowError(e.Message);
                    return false;
                }

                NotificationManager.ShowSuccess("Отчет успешно создан по следующему пути: " + saveFileDialog.FileName);
                return true;
            }
            else
            {
                NotificationManager.ShowInformation("Процесс сохранения отчета был прерван пользователем.");
                return false;
            }
        }

        public void CheckDialogResult(Func<bool> dialog)
        {
            if (dialog?.Invoke() ?? false)
            {
                //NotificationManager.ShowSuccess("Новая запись успешно добавлена.\nНе забудьте сохранить изменения.");
            }
            else
            {
                NotificationManager.ShowInformation("Процесс создания отчета был прерван.");
            }
        }
        #endregion
    }
}
