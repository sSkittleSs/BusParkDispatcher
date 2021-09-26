using BusParkDispatcher.Infrastructure;
using BusParkDispatcher.ViewModels;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusParkDispatcher.Models
{
    class RouteInfoReport : Report
    {
        #region Properties
        public BusesOnTheRoute BusesOnTheRoute { set; get; }

        public DateTime Date { get; set; }

        public bool IsWeekend { get; set; }

        public ICollection<BusStopTime> Timetable { set; get; } = new List<BusStopTime>();

        public int RunsAmount { get; set; }
        #endregion

        #region Constructors
        public RouteInfoReport(int номерМаршрута)
        {
            BusesOnTheRoute = new BusesOnTheRoute(номерМаршрута);

            var кодРасписания = MainWindowViewModel.Database.Маршруты.FirstOrDefault((obj) => obj.НомерМаршрута == номерМаршрута)?.КодРасписания ?? -1;

            var расписание = MainWindowViewModel.Database.Расписания.FirstOrDefault((obj) => obj.КодРасписания == кодРасписания);

            if (расписание == default)
                throw new Exception($"Расписания с id {кодРасписания} для маршрута {номерМаршрута} не обнаружено!");

            Date = расписание.Дата;
            IsWeekend = расписание.ЯвляетсяВыходным;

            foreach (var item in расписание.ВремяРасписанияОстановки.Where((obj) => obj.КодРасписания == кодРасписания))
            {
                var busStop = new BusStopTime(item.Остановки.Название, item.Время.Время1, item.Остановки.Описание);
                if (Timetable.Count((obj) => obj.НазваниеОстановки == busStop.НазваниеОстановки) == 0)
                    Timetable.Add(busStop);
            }

            RunsAmount = Timetable.Count != 0 ? расписание.ВремяРасписанияОстановки.Count((obj) => obj.КодРасписания == кодРасписания) / Timetable.Count : 0;
        }
        #endregion

        #region Methods
        public override ExcelPackage AddNewSheet(ExcelPackage package, string sheetName = default)
        {
            // Функция заполнения отчета в Excel

            // Создаем новый лист
            var sheet = package.Workbook.Worksheets.Add((sheetName == default ? "Полная информация о маршруте" : sheetName));

            // Настроиваем шапку листа
            const int firstTableRow = 2;
            sheet.Cells[firstTableRow, 1].Value = "Полная информация о маршруте";
            sheet.Cells[firstTableRow, 1].Style.Font.Bold = true;
            sheet.Cells[firstTableRow, 1].Style.Font.Size = 18;

            var currentRow = firstTableRow + 1;
            var column = 2;

            // Дополняем шапку некоторыми данными
            sheet.Cells[currentRow, column].Value = "Номер маршрута";
            sheet.Cells[currentRow + 1, column].Value = "Количество автобусов";
            sheet.Cells[currentRow + 2, column].Value = "Количество остановок";
            sheet.Cells[currentRow + 3, column].Value = "Количество рейсов";

            sheet.Cells[currentRow, column + 1].Value = BusesOnTheRoute.НомерМаршрута;
            sheet.Cells[currentRow + 1, column + 1].Value = BusesOnTheRoute.КоличествоАвтобусов;
            sheet.Cells[currentRow + 2, column + 1].Value = Timetable.Count;
            sheet.Cells[currentRow + 3, column + 1].Value = RunsAmount;
            sheet.Cells[currentRow, column, currentRow + 3, column].Style.Font.Bold = true;

            currentRow += 5;

            sheet.Cells[currentRow, column].Value = "Маршрут";
            sheet.Cells[currentRow, column].Style.Font.Bold = true;

            currentRow++;

            // Настраиваем стили ячеек
            sheet.Cells[currentRow, column, currentRow + Timetable.Count, column].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells[currentRow, column, currentRow, column + 1].Style.Font.Bold = true;
            sheet.Cells[currentRow, column + 1, currentRow + Timetable.Count, column + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            sheet.Cells[currentRow, column, currentRow + Timetable.Count, column + 1].Style.Border.BorderAround(ExcelBorderStyle.Double);
            sheet.Cells[currentRow, column, currentRow, column + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            sheet.Cells[currentRow, column].Value = "Название остановки";
            sheet.Cells[currentRow, column + 1].Value = "Описание";

            currentRow++;

            foreach (var item in Timetable)
            {
                sheet.Cells[currentRow, column].Value = item.НазваниеОстановки;
                sheet.Cells[currentRow, column + 1].Value = item.ОписаниеОстановки;
                currentRow++;
            }

            sheet.Cells[firstTableRow, column, currentRow, column + 1].AutoFitColumns();

            // Возвращаем объект файла Excel
            return package;
        }
        #endregion
    }
}
