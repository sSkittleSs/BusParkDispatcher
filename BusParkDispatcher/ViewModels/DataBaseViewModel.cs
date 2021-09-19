using BusParkDispatcher.Commands.Base;
using BusParkDispatcher.Infrastructure;
using BusParkDispatcher.Models;
using BusParkDispatcher.Models.Database;
using BusParkDispatcher.Views;
using BusParkDispatcher.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BusParkDispatcher.ViewModels
{
    class DataBaseViewModel : ObservableObject
    {
        #region Fields
        public IEnumerable<object> _items;
        private int selectedItem = 1;
        private string searchText;
        private string lastTable;
        #endregion

        #region Properties
        public IEnumerable<object> Items
        {
            set => SetProperty(ref _items, value);
            get => _items;
        }

        public int SelectedItem
        {
            set => SetProperty(ref selectedItem, value);
            get => selectedItem;
        }

        public string SearchText
        {
            set
            {

                if (string.IsNullOrEmpty(value))
                {
                    ChangeTable.Execute(lastTable);
                }
                else if (SearchText?.Length > value.Length)
                {
                    ChangeTable.Execute(lastTable);
                    Items = Search(value);
                }
                else
                {
                    Items = Search(value);
                }

                SetProperty(ref searchText, value);
            }
            get => searchText;
        }
        #endregion

        #region Constructors
        public DataBaseViewModel()
        {
            LoadDb();
            ChangeTable.Execute("Маршруты");
        }
        #endregion

        #region Commands / Methods

        public DelegateCommand SaveChanges => new DelegateCommand((obj) =>
        {
            try
            {
                // Спрашиваем согласие пользователя на сохранение изменений.
                if (MessageBox.Show("Вы желаете сохранить изменения?", "Внимание", button: MessageBoxButton.YesNo, icon: MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    // Вызываем метод для сохранения изменений
                    SaveDb();
                }
            }
            catch (Exception e) { NotificationManager.ShowError(e.Message); } // В случае возникновения исключений выводим сообщение об ошибке.
        });

        public DelegateCommand ChangeTable => new DelegateCommand((tableName) =>
        {
            SetDataSource(tableName);
        });

        public DelegateCommand Add => new DelegateCommand((obj) =>
        {
            switch(lastTable)
            {
                case "Автобусы":
                    CheckDialogResult(() => new AdditionalWindow() { DataContext = new AdditionalWindowViewModel() { CurrentView = new BusesAdditionView() } }.ShowDialog() ?? false);
                    break;
                case "Водители":
                    CheckDialogResult(() => new AdditionalWindow() { DataContext = new AdditionalWindowViewModel() { CurrentView = new DriversAdditionView() } }.ShowDialog() ?? false);
                    break;
                case "Время":
                    NotificationManager.ShowWarning("Таблица ''Время'' не может быть изменена.");
                    break;
                case "ВремяРасписанияОстановки":
                    CheckDialogResult(() => new AdditionalWindow() { DataContext = new AdditionalWindowViewModel() { CurrentView = new TimeTimetablesBusesAdditionView() } }.ShowDialog() ?? false);
                    break;
                case "Маршруты":
                    CheckDialogResult(() => new AdditionalWindow() { DataContext = new AdditionalWindowViewModel() { CurrentView = new RoutesAdditionView() } }.ShowDialog() ?? false);
                    break;
                case "Остановки":
                    CheckDialogResult(() => new AdditionalWindow() { DataContext = new AdditionalWindowViewModel() { CurrentView = new BusStopsAdditionView() } }.ShowDialog() ?? false);
                    break;
                case "Расписания":
                    CheckDialogResult(() => new AdditionalWindow() { DataContext = new AdditionalWindowViewModel() { CurrentView = new TimetablesAdditionView() } }.ShowDialog() ?? false);
                    break;
                case "ТипыАвтобусов":
                    CheckDialogResult(() => new AdditionalWindow() { DataContext = new AdditionalWindowViewModel() { CurrentView = new BusesTypesAdditionView() } }.ShowDialog() ?? false);
                    break;
                default:
                    break;
            }
        });

        public DelegateCommand AssignDriverToBus => new DelegateCommand((obj) =>
        {
            try
            {
                CheckDialogResult(() => new AdditionalWindow() { DataContext = new AdditionalWindowViewModel() { CurrentView = new DriversAssignView() } }.ShowDialog() ?? false);
            } catch (Exception e) { NotificationManager.ShowError(e.Message); }
        });

        public DelegateCommand UndoChanges => new DelegateCommand((obj) =>
        {
            try
            {
                // Спрашиваем согласие пользователя на отмену внесенных изменений.
                if (MessageBox.Show("Вы желаете отменить все внесенные изменения?\n\nЭто действие невозможно отменить.", "Внимание", button: MessageBoxButton.YesNo, icon: MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    MainWindowViewModel.Database.UndoChanges(); // Вызываем метод-расширение для отмены изменений
                    ChangeTable?.Execute(lastTable); // Заново загружаем последнюю таблицу данных
                    NotificationManager.ShowSuccess($"Таблица '{lastTable}' успешно обновлена к значениям из БД!"); // Уведомляем пользователя об успехе операции, если исключений не возникло
                }
            }
            catch (Exception e) { NotificationManager.ShowError(e.Message); } // В случае возникновения исключений выводим сообщение об ошибке.

            
        });

        #region Database methods
        public void LoadDb()
        {
            MainWindowViewModel.Database.Автобусы.Load();
            MainWindowViewModel.Database.Водители.Load();
            MainWindowViewModel.Database.Время.Load();
            MainWindowViewModel.Database.Маршруты.Load();
            MainWindowViewModel.Database.Остановки.Load();
            MainWindowViewModel.Database.Расписания.Load();
            MainWindowViewModel.Database.ВремяРасписанияОстановки.Load();
            MainWindowViewModel.Database.ТипыАвтобусов.Load();
        }

        public void SetDataSource(object tableName)
        {
            string name = Convert.ToString(tableName);
            var dbSet = GetPropValue(MainWindowViewModel.Database, name);
            var local = GetPropValue(dbSet, "Local");

            Items = SelectData(local);

            lastTable = (string)tableName;
        }

        public void SaveDb()
        {
            // Метод SaveDB()
            try
            {
                // Отправляем запрос на сохранение изменений
                MainWindowViewModel.Database.SaveChanges();

                // Выводим сообщение об успехе в случае, если исключений не возникло.
                NotificationManager.ShowSuccess("Изменения успешно сохранены!"); 
            }
            catch (Exception e) { NotificationManager.ShowError(e.Message); } // Если исключение возникло, выводим сообщение об ошибке
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        public static object InvokeMethod(object src, string MethodName)
        {
            return src.GetType().GetMethod(MethodName).Invoke(src, null);
        }

        public IEnumerable<object> Search(string value)
        {
            ObservableCollection<object> items = new ObservableCollection<object>();
            foreach (DbTable row in Items)
            {
                if (row.IsSearchable(value))
                {
                    items.Add(row);
                }
            }

            return items;
        }

        public IEnumerable<object> SelectData(object src)
        {
            if (src is ObservableCollection<Автобусы> buses)
                return from b in buses select new { b.РегистрационныйНомер, ТипАвтобуса = b.ТипыАвтобусов.Наименование, b.КоличествоМест, b.Маршруты.НомерМаршрута };

            if (src is ObservableCollection<Водители> drivers)
                return from d in drivers select new { d.ФИО, d.НомерТелефона };

            if (src is ObservableCollection<Время> time)
                return from t in time select new { Время = t.Время1 };

            if (src is ObservableCollection<ВремяРасписанияОстановки> timeTimetablesBusStops)
                return from ttbs in timeTimetablesBusStops select new { Время = ttbs.Время.Время1, Остановка = ttbs.Остановки.Название, КодРасписания = ttbs.Расписания.КодРасписания };

            if (src is ObservableCollection<Маршруты> routes)
                return from r in routes select new { r.КодМаршрута, r.НомерМаршрута, r.Описание, r.КодРасписания };

            if (src is ObservableCollection<Остановки> busStops)
                return from bs in busStops select new { bs.Название, bs.Описание };

            if (src is ObservableCollection<Расписания> timetables)
                return from t in timetables select new { t.КодРасписания, t.Дата, t.ЯвляетсяВыходным };

            if (src is ObservableCollection<ТипыАвтобусов> busesTypes)
                return from bt in busesTypes select new { bt.Наименование, bt.Описание };

            return (src as IEnumerable<DbTable>);
        }

        public void CheckDialogResult(Func<bool> dialog)
        {
            if (dialog?.Invoke() ?? false)
                NotificationManager.ShowSuccess("Новая запись успешно добавлена.\nНе забудьте сохранить изменения.");
            else
                NotificationManager.ShowError("Запись не была добавлена.");
        }
        #endregion
        #endregion
    }
}
