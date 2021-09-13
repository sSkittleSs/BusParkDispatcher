using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusParkDispatcher.Infrastructure
{
    interface IExcelReportGeneratable
    {
        bool WriteToNewExcelFile(string path);

        ExcelPackage AddNewSheet(ExcelPackage package, string sheetName = default);
    }
}
