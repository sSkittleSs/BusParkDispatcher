using BusParkDispatcher.ViewModels;
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
    class TimetableReport
    {
        public int НомерМаршрута { get; set; }

        public DateTime Дата { get; set; }

        public bool ЯвляетсяВыходным { get; set; }

        public ICollection<BusStopTime> Расписание { set; get; }

        public TimetableReport(int номерМаршрута)
        {
            НомерМаршрута = номерМаршрута;

            var кодРасписания = MainWindowViewModel.Database.Маршруты.FirstOrDefault((obj) => obj.НомерМаршрута == НомерМаршрута)?.КодРасписания ?? -1;

            var расписание = MainWindowViewModel.Database.Расписания.FirstOrDefault((obj) => obj.КодРасписания == кодРасписания);

            if (расписание == default)
                throw new Exception($"Расписания с id {кодРасписания} для маршрута {номерМаршрута} не обнаружено!");

            Дата = расписание.Дата;
            ЯвляетсяВыходным = расписание.ЯвляетсяВыходным;

            foreach (var item in расписание.ВремяРасписанияОстановки.Where((obj) => obj.КодРасписания == кодРасписания))
                Расписание.Add(new BusStopTime(item.Остановки.Название, item.Время.Время1, item.Остановки.Описание));
        }

        public TimetableReport(int номерМаршрута, DateTime дата, bool являетсяВыходным, ICollection<BusStopTime> расписание)
        {
            НомерМаршрута = номерМаршрута;
            Дата = дата;
            ЯвляетсяВыходным = являетсяВыходным;
            Расписание = расписание;
        }

        private byte[] GenerateReportToNewExcelFile()
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
            catch(Exception e)
            {
                NotificationManager.ShowError(e.Message);
                return false;
            }

            NotificationManager.ShowSuccess("Отчет успешно создан по следующему пути: " + path);
            return true;
        }

        public ExcelPackage AddNewSheet(ExcelPackage package, string sheetName = default)
        {
            var sheet = package.Workbook.Worksheets.Add((sheetName == default ? "Расписание маршрута №" + НомерМаршрута : sheetName));

            sheet.Cells["B1:B3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            sheet.Cells["B1"].Value = $"Номер маршрута:";
            sheet.Cells["B2"].Value = $"Дата:";
            sheet.Cells["B3"].Value = $"Является выходным днем:";


            sheet.Cells["C1:C3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            sheet.Cells["C1"].Value = НомерМаршрута;
            sheet.Cells["C2"].Value = Дата;
            sheet.Cells["C3"].Value = ЯвляетсяВыходным;

            const int firstTableRow = 7;

            var currentRow = firstTableRow;
            var column = 2;

            sheet.Cells[currentRow, column, currentRow + Расписание.Count, column].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells[currentRow, column, currentRow + Расписание.Count, column].Style.Font.Bold = true;
            sheet.Cells[currentRow, column + 1, currentRow + Расписание.Count, column + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            sheet.Cells[currentRow, column, currentRow + Расписание.Count, column + 2].Style.Border.BorderAround(ExcelBorderStyle.Double);
            sheet.Cells[currentRow, column, currentRow, column + 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            sheet.Cells[currentRow, column].Value = "Время";
            sheet.Cells[currentRow, column + 1].Value = "Название остановки";
            sheet.Cells[currentRow, column + 2].Value = "Описание";

            currentRow++;

            foreach(var item in Расписание)
            {
                sheet.Cells[currentRow, column].Value = item.Время;
                sheet.Cells[currentRow, column + 1].Value = item.НазваниеОстановки;
                sheet.Cells[currentRow, column + 2].Value = item.ОписаниеОстановки;
                currentRow++;
            }

            sheet.Cells[2, column, currentRow, column + 2].AutoFitColumns();
            sheet.Cells[firstTableRow, column, currentRow, column].Style.Numberformat.Format = "hh:mm";

            return package;
        }
    }
}
