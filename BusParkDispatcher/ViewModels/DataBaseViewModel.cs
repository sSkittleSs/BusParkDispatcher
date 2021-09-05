using BusParkDispatcher.Commands.Base;
using BusParkDispatcher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BusParkDispatcher.ViewModels
{
    class DataBaseViewModel : ObservableObject
    {
        #region Fields
        private object items;
        private int selectedItem = 1;
        #endregion

        #region Properties
        public object Items
        {
            set => SetProperty(ref items, value);
            get => items;
        }

        public int SelectedItem
        {
            set => SetProperty(ref selectedItem, value);
            get => selectedItem;
        }
        #endregion

        #region Constructors
        public DataBaseViewModel()
        {
            LoadData.Execute();
        }
        #endregion

        #region Commands / Methods

        public DelegateCommand LoadData => new DelegateCommand((obj) =>
        {
            SelectedItem = 1;
        });

        public DelegateCommand SaveChanges => new DelegateCommand((obj) =>
        {
            try
            {
                if (MessageBox.Show("Вы желаете сохранить изменения?", "Внимание", button: MessageBoxButton.YesNo, icon: MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    //MainWindowVM.DataBase.SaveChangesAsync().Await(() => NotificationManager.ShowSuccess("Изменения успешно сохранены!"));
                }
            }
            catch (Exception e) { NotificationManager.ShowError(e.Message); }
        });

        public DelegateCommand Update => new DelegateCommand((obj) =>
        {
            var switchedGrid = "";


            switch (SelectedItem)
            {
                case 1: break;
                default: NotificationManager.ShowError("Ошибка индекса!"); return;
            }

            NotificationManager.ShowSuccess($"Таблица '{switchedGrid}' успешно обновлена к значениям из БД!");
        });
        #endregion
    }
}
