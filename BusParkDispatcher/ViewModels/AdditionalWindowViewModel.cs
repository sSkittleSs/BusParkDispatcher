using BusParkDispatcher.Commands.Base;
using BusParkDispatcher.Models;
using BusParkDispatcher.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace BusParkDispatcher.ViewModels
{
    class AdditionalWindowViewModel : ObservableObject
    {
        #region Fields
        private UserControl currentView;
        private string windowTitle;
        private int maxControlWidth = 796;
        private int maxControlHeight = 300;
        private Brush borderBrush = Brushes.Blue;
        #endregion

        #region Properties
        public UserControl CurrentView
        {
            set => SetProperty(ref currentView, value);
            get => currentView;
        }

        public string WindowTitle
        {
            set => SetProperty(ref windowTitle, value);
            get => windowTitle;
        }

        public int MaxControlWidth
        {
            set => SetProperty(ref maxControlWidth, value - 4);
            get => maxControlWidth;
        }

        public int MaxControlHeight
        {
            set => SetProperty(ref maxControlHeight, value - 60);
            get => maxControlHeight;
        }

        public Brush BorderBrush
        {
            set => SetProperty(ref borderBrush, value);
            get => borderBrush;
        }
        #endregion

        #region Constructors
        #endregion

        #region Commands / Methods
        public DelegateCommand Exit => new DelegateCommand((obj) =>
        {
            if (obj is AdditionalWindow window)
            {
                window.DialogResult = false;
                window.Close();
            }
            else
            {
                NotificationManager.ShowError("Произошла ошибка при закрытии окна.\nПопробуйте закрыть его иным способом.");
            }
        });
        #endregion
    }
}
