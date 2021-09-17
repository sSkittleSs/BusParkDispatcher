using BusParkDispatcher.Infrastructure;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusParkDispatcher.Models
{
    class BusesOnTheRouteReport : IExcelReportGeneratable
    {
        #region Properties
        public ICollection<BusesOnTheRoute> АвтобусыНаМаршрутах { set; get; }
        #endregion

        #region Constructors
        public BusesOnTheRouteReport(int номерМаршрута)
        {
            АвтобусыНаМаршрутах.Add(new BusesOnTheRoute(номерМаршрута));
        }

        public BusesOnTheRouteReport(int[] номераМаршрутов)
        {
            foreach (var номерМаршрута in номераМаршрутов)
                АвтобусыНаМаршрутах.Add(new BusesOnTheRoute(номерМаршрута));
        }

        public BusesOnTheRouteReport(ICollection<BusesOnTheRoute> автобусыНаМаршрутах)
        {
            АвтобусыНаМаршрутах = автобусыНаМаршрутах;
        }
        #endregion

        #region Methods
        protected byte[] GenerateReportToNewExcelFile()
        {
            var package = AddNewSheet(new ExcelPackage());

            return package.GetAsByteArray();
        }

        public bool WriteToNewExcelFile(string path)
        {
            try
            {
                File.WriteAllBytes(path, GenerateReportToNewExcelFile());
            }
            catch (Exception e)
            {
                NotificationManager.ShowError(e.Message);
                return false;
            }

            NotificationManager.ShowSuccess("Отчет успешно создан по следующему пути: " + path);
            return true;
        }

        public ExcelPackage AddNewSheet(ExcelPackage package, string sheetName = default)
        {
            // Функция заполнения отчета в Excel

            // Создаем новый лист
            var sheet = package.Workbook.Worksheets.Add((sheetName == default ? "Количество автобусов на маршрутах" : sheetName));

            // Настроиваем шапку листа
            const int firstTableRow = 2;
            sheet.Cells[firstTableRow, 1].Value = "Количество автобусов на маршрутах";
            sheet.Cells[firstTableRow, 1].Style.Font.Bold = true;
            sheet.Cells[firstTableRow, 1].Style.Font.Size = 18;

            var currentRow = firstTableRow + 2;

            var column = 2;

            // Настраиваем стили ячеек
            sheet.Cells[currentRow, column, currentRow, column + 1].Style.Font.Bold = true;
            sheet.Cells[currentRow, column, currentRow + АвтобусыНаМаршрутах.Count, column].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            sheet.Cells[currentRow, column + 1, currentRow + АвтобусыНаМаршрутах.Count, column + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells[currentRow, column, currentRow + АвтобусыНаМаршрутах.Count, column + 1].Style.Border.BorderAround(ExcelBorderStyle.Double);
            sheet.Cells[currentRow, column, currentRow, column + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            // Дополняем шапку некоторыми данными
            sheet.Cells[currentRow, column].Value = "Маршрут";
            sheet.Cells[currentRow, column + 1].Value = "Количество автобусов";

            currentRow++;

            // Проходимся по коллекции автобусов на маршрутах
            foreach (var item in АвтобусыНаМаршрутах)
            {
                // Добавляем в таблицу данные о номере маршрута и количестве автобусов на данном маршруте
                sheet.Cells[currentRow, column].Value = item.НомерМаршрута;
                sheet.Cells[currentRow, column + 1].Value = item.КоличествоАвтобусов;
                currentRow++;
            }

            sheet.Cells[firstTableRow, column, currentRow, column + 2].AutoFitColumns();

            // Возвращаем объект файла Excel
            return package;
        }
        #endregion
    }
}
