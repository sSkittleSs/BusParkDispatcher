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
    class WorkPlanReport : Report
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

        public List<List<BusStopTime>> Timetable { set; get; } = new List<List<BusStopTime>>();
        #endregion

        #region Constructors
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3358:Ternary operators should not be nested", Justification = "<Ожидание>")]
        public WorkPlanReport(int кодВодителя)
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

            for (int i = 0; i < 5; i++)
                Timetable.Add(new List<BusStopTime>());

            var workedTime = new TimeSpan();
            var workedTimeForWeek = new TimeSpan();
            var lastTime = new TimeSpan();
            var lawedTime = TimeSpan.FromMinutes(480);

            var random = new Random();
            for (int i = 0; i < Timetable.Count; i++)
            {
                var randomTurn = random.Next(RunsAmount);

                for (int j = randomTurn * distincTimetable.Count; j < timetable.ВремяРасписанияОстановки.Count; j = (j + 1) % timetable.ВремяРасписанияОстановки.Count)
                {
                    var nextWorkedTime = timetable.ВремяРасписанияОстановки.ElementAt(j).Время.Время1 - (Timetable[i].Count != 0 ?
                            (lastTime > timetable.ВремяРасписанияОстановки.ElementAt(j).Время.Время1 ?
                                -(new TimeSpan(24, 0, 0).Subtract(timetable.ВремяРасписанияОстановки.ElementAt(j).Время.Время1))
                                : lastTime)
                            : timetable.ВремяРасписанияОстановки.ElementAt(j).Время.Время1);

                    if (nextWorkedTime > new TimeSpan(2, 0, 0))
                        break;

                    if (workedTime + nextWorkedTime < lawedTime)
                    {
                        workedTime += nextWorkedTime;

                        Timetable[i].Add(new BusStopTime(timetable.ВремяРасписанияОстановки.ElementAt(j).Остановки.Название, timetable.ВремяРасписанияОстановки.ElementAt(j).Время.Время1));
                    }
                    else
                    {
                        break;
                    }

                    lastTime = timetable.ВремяРасписанияОстановки.ElementAt(j).Время.Время1;
                }

                workedTimeForWeek = workedTimeForWeek.Add(workedTime);
                workedTime = new TimeSpan();
            }

            WorkedDays = workedTimeForWeek.Days;
            WorkedHours = workedTimeForWeek.Hours;
            WorkedMinutes = workedTimeForWeek.Minutes;
            TotalWorkedHours = workedTimeForWeek.TotalHours;
        }
        #endregion

        #region Methods
        public override ExcelPackage AddNewSheet(ExcelPackage package, string sheetName = default)
        {
            // Функция заполнения отчета в Excel

            // Создаем новый лист
            var sheet = package.Workbook.Worksheets.Add((sheetName == default ? "Рабочий график на неделю" : sheetName));

            // Настроиваем шапку листа
            const int firstTableRow = 2;
            sheet.Cells[firstTableRow, 1].Value = "Рабочий график на неделю";
            sheet.Cells[firstTableRow, 1].Style.Font.Bold = true;
            sheet.Cells[firstTableRow, 1].Style.Font.Size = 18;

            var currentRow = firstTableRow + 2;
            var column = 2;

            // Дополняем шапку некоторыми данными
            sheet.Cells[currentRow, column].Value = "Дата создания";
            sheet.Cells[currentRow + 1, column].Value = "ФИО водителя";
            sheet.Cells[currentRow + 2, column].Value = "Регистрационный номер автобуса";
            sheet.Cells[currentRow + 3, column].Value = "Номер маршрута";
            sheet.Cells[currentRow + 4, column].Value = "Количество рейсов (для маршрута)";
            sheet.Cells[currentRow + 5, column].Value = "Рассчитываемое время на неделю";
            sheet.Cells[currentRow + 6, column].Value = "Рассчитываемые часы на неделю";
            sheet.Cells[currentRow, column, currentRow + 6, column].Style.Font.Bold = true;
            sheet.Cells[currentRow, column, currentRow + 6, column].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            sheet.Cells[currentRow, column + 1].Value = Date;
            sheet.Cells[currentRow, column + 1].Style.Numberformat.Format = "dd.mm.yyyy";

            sheet.Cells[currentRow + 1, column + 1].Value = DriverName;
            sheet.Cells[currentRow + 2, column + 1].Value = BusNumber;
            sheet.Cells[currentRow + 3, column + 1].Value = RouteNumber == -1 ? "Отсутствует" : RouteNumber.ToString();
            sheet.Cells[currentRow + 4, column + 1].Value = RunsAmount;
            sheet.Cells[currentRow + 5, column + 1].Value = $"Дней: {WorkedDays}; Часов: {WorkedHours}; Минут: {WorkedMinutes}";
            sheet.Cells[currentRow + 6, column + 1].Value = TotalWorkedHours;
            sheet.Cells[currentRow, column + 1, currentRow + 6, column + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            currentRow += 8;

            // Маршрутный лист

            sheet.Cells[currentRow, column].Value = "Маршрутный лист";
            sheet.Cells[currentRow, column].Style.Font.Bold = true;
            sheet.Cells[currentRow + 1, column].Value = "-------------------------------------------------";
            sheet.Cells[currentRow + 1, column + 1].Value = "------------------------------------";

            currentRow += 2;

            var currentDate = DateTime.Today;
            var timetableCounter = 0;
            foreach (var item in Timetable)
            {
                currentDate = currentDate.AddDays(1);

                sheet.Cells[currentRow, column].Value = DayOfWeekToRussian(currentDate.DayOfWeek);
                sheet.Cells[currentRow, column].Style.Font.Bold = true;
                sheet.Cells[currentRow, column + 1].Value = currentDate;
                sheet.Cells[currentRow, column + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                sheet.Cells[currentRow, column + 1].Style.Numberformat.Format = "dd.mm.yyyy";
                sheet.Cells[currentRow, column + 1].Style.Font.Bold = true;
                currentRow++;

                // Настраиваем стили ячеек
                sheet.Cells[currentRow, column, currentRow + Timetable[timetableCounter].Count, column].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[currentRow, column, currentRow, column + 1].Style.Font.Bold = true;
                sheet.Cells[currentRow, column + 1, currentRow + Timetable[timetableCounter].Count, column + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                sheet.Cells[currentRow, column, currentRow + Timetable[timetableCounter].Count, column + 1].Style.Border.BorderAround(ExcelBorderStyle.Double);
                sheet.Cells[currentRow, column, currentRow, column + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                sheet.Cells[currentRow, column, currentRow + Timetable[timetableCounter].Count, column].Style.Numberformat.Format = "hh:mm";

                sheet.Cells[currentRow, column].Value = "Время";
                sheet.Cells[currentRow, column + 1].Value = "Название остановки";

                currentRow++;

                foreach(var timetable in Timetable[timetableCounter])
                {
                    sheet.Cells[currentRow, column].Value = timetable.Время;
                    sheet.Cells[currentRow, column + 1].Value = timetable.НазваниеОстановки;

                    currentRow++;
                }
                sheet.Cells[currentRow, column].Value = "Отбыл в";
                sheet.Cells[currentRow++, column + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                sheet.Cells[currentRow, column + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[currentRow++, column + 1].Value = "дата/подпись";
                sheet.Cells[currentRow, column].Value = "Прибыл в";
                sheet.Cells[currentRow++, column + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                sheet.Cells[currentRow, column + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[currentRow++, column + 1].Value = "дата/подпись";

                currentRow += 2;
                timetableCounter++;
            }

            //

            sheet.Cells[firstTableRow, column, currentRow, column + 1].AutoFitColumns();

            // Возвращаем объект файла Excel
            return package;
        }

        private string DayOfWeekToRussian(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday: return "Понедельник";
                case DayOfWeek.Tuesday: return "Вторник";
                case DayOfWeek.Wednesday: return "Среда";
                case DayOfWeek.Thursday: return "Четверг";
                case DayOfWeek.Friday: return "Пятница";
                case DayOfWeek.Saturday: return "Суббота";
                case DayOfWeek.Sunday: return "Воскресенье";
                default: return dayOfWeek.ToString();
            }
        }
        #endregion
    }
}
