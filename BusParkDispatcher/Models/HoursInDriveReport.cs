using BusParkDispatcher.Infrastructure;
using BusParkDispatcher.ViewModels;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusParkDispatcher.Models
{
    class HoursInDriveReport : Report
    {
        #region Properties
        public DateTime Date { set; get; } = DateTime.Today;

        public string DriverName { set; get; }

        public string BusNumber { get; set; }

        public int RouteNumber { get; set; }

        public int RunsAmount { get; set; }

        public int WorkedDays { set; get; }

        public int WorkedHours { set; get; }

        public int WorkedMinutes { set; get; }

        public double TotalWorkedHours { set; get; }
        #endregion

        #region Constructors
        public HoursInDriveReport(int кодВодителя)
        {
            var driver = MainWindowViewModel.Database.Водители.Local.FirstOrDefault((obj) => obj.КодВодителя == кодВодителя);
            
            if (driver == default)
                throw new Exception("Не удалось получить информацию о водителе.");

            DriverName = driver.ФИО;

            if (driver.Автобусы.Count == 0)
            {
                BusNumber = "Отсутствует";
                RouteNumber = -1;
                RunsAmount = 0;
                WorkedDays = 0;
                WorkedHours = 0;
                WorkedMinutes = 0;
                return;
            }

            var bus = driver.Автобусы.Last();

            BusNumber = bus.РегистрационныйНомер;
            RouteNumber = bus.Маршруты.НомерМаршрута;

            var timetable = bus.Маршруты.Расписания;
            var distincTimetable = new List<BusStopTime>();

            foreach (var item in timetable.ВремяРасписанияОстановки)
            {
                var busStop = new BusStopTime(item.Остановки.Название, item.Время.Время1, item.Остановки.Описание);
                if (distincTimetable.Count((obj) => obj.НазваниеОстановки == busStop.НазваниеОстановки) == 0)
                    distincTimetable.Add(busStop);
            }

            RunsAmount = distincTimetable.Count != 0 ? timetable.ВремяРасписанияОстановки.Count / distincTimetable.Count : 0;

            var workedTime = new TimeSpan();
            var lastTime = new TimeSpan();
            var lawedTime = TimeSpan.FromMinutes(480);

            foreach (var item in timetable.ВремяРасписанияОстановки)
            {
                var nextWorkedTime = item.Время.Время1 - (lastTime > item.Время.Время1 ?
                        -(new TimeSpan(24, 0, 0).Subtract(item.Время.Время1))
                        : lastTime);

                if (nextWorkedTime > new TimeSpan(2, 0, 0))
                    break;

                if (workedTime + nextWorkedTime < lawedTime)
                    workedTime += nextWorkedTime;
                else
                    break;

                lastTime = item.Время.Время1;
            }

            
            workedTime = TimeSpan.FromDays(workedTime.TotalDays * 22);

            WorkedDays = workedTime.Days;
            WorkedHours = workedTime.Hours;
            WorkedMinutes = workedTime.Minutes;
            TotalWorkedHours = workedTime.TotalHours;
        }
        #endregion

        #region Methods
        public override ExcelPackage AddNewSheet(ExcelPackage package, string sheetName = default)
        {
            // Функция заполнения отчета в Excel

            // Создаем новый лист
            var sheet = package.Workbook.Worksheets.Add((sheetName == default ? "Информация об отработанном времени" : sheetName));

            // Настроиваем шапку листа
            const int firstTableRow = 2;
            sheet.Cells[firstTableRow, 1].Value = "Информация об отработанном времени за месяц";
            sheet.Cells[firstTableRow, 1].Style.Font.Bold = true;
            sheet.Cells[firstTableRow, 1].Style.Font.Size = 18;

            var currentRow = firstTableRow + 2;
            var column = 2;

            // Дополняем шапку некоторыми данными
            sheet.Cells[currentRow, column].Value = "Дата";
            sheet.Cells[currentRow + 1, column].Value = "ФИО водителя";
            sheet.Cells[currentRow + 2, column].Value = "Регистрационный номер автобуса";
            sheet.Cells[currentRow + 3, column].Value = "Номер маршрута";
            sheet.Cells[currentRow + 4, column].Value = "Количество рейсов (для маршрута)";
            sheet.Cells[currentRow + 5, column].Value = "Отработанное время";
            sheet.Cells[currentRow + 6, column].Value = "Отработанное часы";
            sheet.Cells[currentRow, column, currentRow + 6, column].Style.Font.Bold = true;
            sheet.Cells[currentRow, column, currentRow + 6, column].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

            sheet.Cells[currentRow, column + 1].Value = Date;
            sheet.Cells[currentRow, column + 1].Style.Numberformat.Format = "dd.mm.yyyy";

            sheet.Cells[currentRow + 1, column + 1].Value = DriverName;
            sheet.Cells[currentRow + 2, column + 1].Value = BusNumber;
            sheet.Cells[currentRow + 3, column + 1].Value = RouteNumber == -1 ? "Отсутствует" : RouteNumber.ToString();
            sheet.Cells[currentRow + 4, column + 1].Value = RunsAmount;
            sheet.Cells[currentRow + 5, column + 1].Value = $"Дней: {WorkedDays}; Часов: {WorkedHours}; Минут: {WorkedMinutes}";
            sheet.Cells[currentRow + 6, column + 1].Value = TotalWorkedHours;
            sheet.Cells[currentRow, column + 1, currentRow + 6, column + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

            sheet.Cells[firstTableRow, column, currentRow + 6, column + 1].AutoFitColumns();

            // Возвращаем объект файла Excel
            return package;
        }
        #endregion
    }
}
