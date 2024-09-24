using BusParkDispatcher.Commands.Base;
using BusParkDispatcher.Models;
using BusParkDispatcher.Models.Database;
using BusParkDispatcher.Views;
using BusParkDispatcher.Views.Windows;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        private object currentView;
        private List<CultureInfo> languages;
        private CultureInfo selectedLanguage;
        public static Пользователи currentUser = null;
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

        public object CurrentView
        {
            set => SetProperty(ref currentView, value);
            get => currentView;
        }

        public List<CultureInfo> Languages
        {
            set => SetProperty(ref languages, value);
            get => languages;
        }

        public Пользователи CurrentUser
        {
            set => SetProperty(ref currentUser, value);
            get => currentUser;
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
            ChangeView(Activator.CreateInstance(System.Reflection.Assembly.GetExecutingAssembly().GetType("BusParkDispatcher.Views.SignInView")));

            LoadDb();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
        #endregion

        #region Commands / Methods
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
            MainWindowViewModel.Database.КоличествоОстановокНаМаршрутеВодителя.Load();
        }

        public void ChangeView(object userControl) => CurrentView = userControl;

        public DelegateCommand OpenView => new DelegateCommand((obj) =>
        {
            try
            {
                if (CurrentUser == null && (obj.ToString() != "SignUpView" && obj.ToString() != "SignInView"))
                {
                    NotificationManager.ShowWarning("Для выполнения данного действия требуется авторизация!");
                    return;
                }
                if (obj is string viewName) ChangeView(Activator.CreateInstance(System.Reflection.Assembly.GetExecutingAssembly().GetType("BusParkDispatcher.Views." + viewName)));
            }
            catch (Exception e) { NotificationManager.ShowError(e.Message); }
        });

        public DelegateCommand ReloadDatabase => new DelegateCommand((obj) => { LoadDb(); });

        public static void ChangeView(string viewName) => GetCurrentViewModel().OpenView.Execute(viewName);

        public static MainWindowViewModel GetCurrentViewModel() => (MainWindowViewModel)MainWindow.DataContext;
        #endregion
    }
}
