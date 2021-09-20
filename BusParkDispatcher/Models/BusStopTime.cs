using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusParkDispatcher.Models
{
    class BusStopTime
    {
        public TimeSpan Время { get; set; }

        public string НазваниеОстановки { get; set; }

        public string ОписаниеОстановки { get; set; }

        public BusStopTime(string названиеОстановки, TimeSpan время, string описаниеОстановки = "Описание отсутствует")
        {
            НазваниеОстановки = названиеОстановки;
            Время = время;
            ОписаниеОстановки = описаниеОстановки;
        }
    }
}
