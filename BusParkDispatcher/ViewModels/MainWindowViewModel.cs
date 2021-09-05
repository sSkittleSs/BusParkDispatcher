using BusParkDispatcher.Commands.Base;
using BusParkDispatcher.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BusParkDispatcher.ViewModels
{
    class MainWindowViewModel : ObservableObject
    {
        #region Fields
        private static int windowWidth = 800;
        private static int windowHeight = 460;
        private static int maxControlWidth = 796;
        private static int maxControlHeight = 380;
        private UserControl currentView;
        #endregion

        #region Properties
        public int MinWidth { get; set; } = 800;
        public int MinHeight { get; set; } = 460;

        public int WindowWidth
        {
            set
            {
                SetProperty(ref windowWidth, value);
                MaxControlWidth = value;
            }
            get => windowWidth;
        }

        public int WindowHeight
        {
            set
            {
                SetProperty(ref windowHeight, value);
                MaxControlHeight = value;
            }
            get => windowHeight;
        }

        public int MaxControlWidth
        {
            set => SetProperty(ref maxControlWidth, value - 4);
            get => maxControlWidth;
        }

        public int MaxControlHeight
        {
            set => SetProperty(ref maxControlHeight, value - 80);
            get => maxControlHeight;
        }

        public static MainWindow MainWindow { set; get; } = (MainWindow)Application.Current.MainWindow;

        public UserControl CurrentView
        {
            set => SetProperty(ref currentView, value);
            get => currentView;
        }
        #endregion

        #region Constructors
        #endregion

        #region Commands / Methods
        public void ChangeView(UserControl userControl) => CurrentView = userControl;

        #endregion
    }
}
