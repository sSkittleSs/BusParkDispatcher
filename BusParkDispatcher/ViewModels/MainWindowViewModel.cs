﻿using BusParkDispatcher.Commands.Base;
using BusParkDispatcher.Models;
using BusParkDispatcher.Models.Database;
using BusParkDispatcher.Views;
using BusParkDispatcher.Views.Windows;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private List<CultureInfo> languages;
        private CultureInfo selectedLanguage;
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

        public List<CultureInfo> Languages
        {
            set => SetProperty(ref languages, value);
            get => languages;
        }

        public CultureInfo SelectedLanguage
        {
            set
            {
                SetProperty(ref selectedLanguage, value);

                App.Language = SelectedLanguage;
            }
            get => selectedLanguage;
        }

        public static ApplicationContext Database { set; get; } = new ApplicationContext();
        #endregion

        #region Constructors
        public MainWindowViewModel()
        {
            Languages = App.Languages;
            App.Language = BusParkDispatcher.Properties.Settings.Default.DefaultLanguage;
            SelectedLanguage = App.Language;
            OpenMain?.Execute();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
        #endregion

        #region Commands / Methods
        public void ChangeView(UserControl userControl) => CurrentView = userControl;

        public DelegateCommand OpenMain => new DelegateCommand((obj) =>
        {
            ChangeView(new MainView());
        });

        public DelegateCommand OpenDataBase => new DelegateCommand((obj) =>
        {
            ChangeView(new DataBaseView());
        });

        public DelegateCommand OpenReports => new DelegateCommand((obj) =>
        {
            ChangeView(new ReportsView());
        });
        #endregion
    }
}
