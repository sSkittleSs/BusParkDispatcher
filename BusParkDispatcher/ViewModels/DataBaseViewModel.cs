﻿using BusParkDispatcher.Commands.Base;
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
                    if (new AdditionalWindow() { DataContext = new AdditionalWindowViewModel() { CurrentView = new BusesAdditionView(), WindowTitle = "Добавление автобуса" } }.ShowDialog() ?? false)
                        NotificationManager.ShowSuccess("Новая запись успешно добавлена.\nНе забудьте сохранить изменения.");
                    else
                        NotificationManager.ShowError("Запись не была добавлена.");
                    break;
                default:
                    break;
            }
        });

        public DelegateCommand Update => new DelegateCommand((obj) =>
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
