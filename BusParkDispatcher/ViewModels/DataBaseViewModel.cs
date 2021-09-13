using BusParkDispatcher.Commands.Base;
using BusParkDispatcher.Infrastructure;
using BusParkDispatcher.Models;
using BusParkDispatcher.Models.Database;
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
                if (MessageBox.Show("Вы желаете сохранить изменения?", "Внимание", button: MessageBoxButton.YesNo, icon: MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    SaveDb();
                }
            }
            catch (Exception e) { NotificationManager.ShowError(e.Message); }
        });

        public DelegateCommand ChangeTable => new DelegateCommand((tableName) =>
        {
            SetDataSource(tableName);
        });

        public DelegateCommand Update => new DelegateCommand((obj) =>
        {
            MainWindowViewModel.Database.UndoChanges();
            ChangeTable?.Execute(lastTable);
            NotificationManager.ShowSuccess($"Таблица '{lastTable}' успешно обновлена к значениям из БД!");
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
            try
            {
                MainWindowViewModel.Database.SaveChanges();
                NotificationManager.ShowSuccess("Изменения успешно сохранены!");
            }
            catch (Exception e) { NotificationManager.ShowError(e.Message); }
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
            if (src is ObservableCollection<Маршруты> routes)
                return from r in routes select new { r.КодМаршрута, r.НомерМаршрута, r.Описание, r.КодРасписания };

            if (src is ObservableCollection<Расписания> timetables)
                return from t in timetables select new { t.КодРасписания, t.Дата, t.ЯвляетсяВыходным };

            return (src as IEnumerable<DbTable>);
        }
        #endregion
        #endregion
    }
}
