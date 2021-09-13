using BusParkDispatcher.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusParkDispatcher.Models
{
    class BusesOnTheRoute
    {
        #region Properties
        public int НомерМаршрута { get; set; }

        public int КоличествоАвтобусов { get; set; }
        #endregion

        #region Constructors
        public BusesOnTheRoute(int номерМаршрута)
        {
            НомерМаршрута = номерМаршрута;
            var кодМаршрута = MainWindowViewModel.Database.Маршруты.Local.FirstOrDefault((obj) => obj.НомерМаршрута == НомерМаршрута)?.КодМаршрута ?? -1;

            КоличествоАвтобусов = MainWindowViewModel.Database.Автобусы.Local.Where((obj) => obj.КодМаршрута == кодМаршрута).Count();
        }

        public BusesOnTheRoute(int номерМаршрута, int количествоАвтобусов)
        {
            НомерМаршрута = номерМаршрута;
            КоличествоАвтобусов = количествоАвтобусов;
        }
        #endregion
    }
}
