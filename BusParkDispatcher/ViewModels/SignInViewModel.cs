using BusParkDispatcher.Commands.Base;
using BusParkDispatcher.Infrastructure;
using BusParkDispatcher.Models;
using BusParkDispatcher.Models.Database;
using BusParkDispatcher.Views;
using BusParkDispatcher.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusParkDispatcher.ViewModels
{
    class SignInViewModel : ObservableObject
    {
        #region Fields
        private string login;

        private string password = string.Empty;
        #endregion

        #region Properties
        public string Login
        {
            set => SetProperty(ref login, value);
            get => login;
        }

        public string Password
        {
            set => SetProperty(ref password, value);
            get => password;
        }
        #endregion

        #region Methods / Commands
        public DelegateCommand Authorize => new DelegateCommand((obj) =>
        {
            // Проверяем почту на пустое значение
            if (Login == null || Login == string.Empty)
            {
                NotificationManager.ShowWarning("Почта не введена!"); // Выводим уведомление
                return;
            }

            if (Password == null || Password == string.Empty)
            {
                NotificationManager.ShowWarning("Пароль не введен!");
                return;
            }

            Пользователи user = null;
            if (MailValidator.IsValidEmail(Login)) // Если в поле логина введена почта, то выполняем поиск по почте и паролю
            {
                try
                {
                    foreach (var item in MainWindowViewModel.Database.Пользователи.Where((u) => u.Почта == Login))
                    {
                        if (!item.CheckPassword(Password)) continue;
                        user = item;
                    }
                }
                catch (Exception e) { NotificationManager.ShowError(e.Message); }
            }
            else // иначе ищем аккаунт по логину
            {
                try
                {
                    // Если количество пользователей с таким логином меньше двух, то ищем аккаунт
                    switch (MainWindowViewModel.Database.Пользователи.Count((us) => us.Логин == Login))
                    {
                        case 0: break;
                        case 1:
                            var userFromDB = MainWindowViewModel.Database.Пользователи.FirstOrDefault((u) => u.Логин == Login);
                            if (userFromDB?.CheckPassword(Password) ?? false) user = userFromDB;
                            break;
                        default:
                            NotificationManager.ShowWarning("Вход по логину невозможен по соображениям безопасности.");
                            break;
                    }
                }
                catch (Exception e) { NotificationManager.ShowError(e.Message); }
            }

            // Если пользователь найден, то авторизируем его
            if (user != null)
            {
                try
                {
                        MainWindowViewModel.GetCurrentViewModel().CurrentUser = user;
                        MainWindowViewModel.ChangeView("MainView");

                        NotificationManager.ShowSuccess("Вы успешно авторизованы в системе!");
                }
                catch (Exception e) { NotificationManager.ShowError(e.Message); }
            }
            else // Иначе выдаем уведомление
            {
                NotificationManager.ShowError("Неверный логин или пароль!");
            }
        });

        private void CheckDialogResult(Func<bool> dialog)
        {
            if (!dialog?.Invoke() ?? false) NotificationManager.ShowError("Восстановление пароля было прервано пользователем.");
        }

        public DelegateCommand PasswordRecovery => new DelegateCommand((obj) =>
        {
            CheckDialogResult(()
                => new AdditionalWindow()
                {
                    DataContext = new AdditionalWindowViewModel()
                    {
                        CurrentView = new PasswordRecoveryView()
                    }
                }.ShowDialog() ?? false);
        });
        public DelegateCommand OpenRegistration => new DelegateCommand((obj) => { MainWindowViewModel.ChangeView("SignUpView"); });
        #endregion
    }
}
