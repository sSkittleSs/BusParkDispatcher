using BusParkDispatcher.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusParkDispatcher.Infrastructure
{
    abstract class Report : IExcelReportGeneratable
    {
        #region Methods
        //protected virtual byte[] GenerateReportToNewExcelFile()
        //{
        //    var package = AddNewSheet(new ExcelPackage());

        //    return package.GetAsByteArray();
        //}

        public virtual bool WriteToNewExcelFile(string path) => WriteToFile(path, AddNewSheet(new ExcelPackage()));

        public static bool WriteToFile(string path, ExcelPackage package)
        {
            try
            {
                File.WriteAllBytes(path, package?.GetAsByteArray());
            }
            catch (Exception e)
            {
                NotificationManager.ShowError(e.Message);
                return false;
            }

            NotificationManager.ShowSuccess("Отчет успешно создан по следующему пути: " + path);
            return true;
        }

        public abstract ExcelPackage AddNewSheet(ExcelPackage package, string sheetName = default);

        #endregion
    }
}
