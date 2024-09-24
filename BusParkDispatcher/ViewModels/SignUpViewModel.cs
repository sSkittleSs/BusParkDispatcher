using BusParkDispatcher.Commands.Base;
using BusParkDispatcher.Infrastructure;
using BusParkDispatcher.Models;
using BusParkDispatcher.Views;
using BusParkDispatcher.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusParkDispatcher.ViewModels
{
    class SignUpViewModel : ObservableObject
    {
        #region Fields
        private string login;

        private string email;

        private string password = string.Empty;

        private string confirmedPassword = string.Empty;
        #endregion

        #region Properties

        public string Login
        {
            set => SetProperty(ref login, value);
            get => login;
        }

        public string Email
        {
            set => SetProperty(ref email, value);
            get => email;
        }

        public string Password
        {
            set => SetProperty(ref password, value);
            get => password;
        }

        public string ConfirmedPassword
        {
            set => SetProperty(ref confirmedPassword, value);
            get => confirmedPassword;
        }
        #endregion

        #region Methods / Commands
        public DelegateCommand Registration => new DelegateCommand((obj) =>
        {

            // Проверяем поле логина на пустоту
            if (Login == null || Login == string.Empty)
            {
                NotificationManager.ShowWarning("Некорректный логин!");
                return;
            }

            // Проверяем поле почты на корректность
            if (!MailValidator.IsValidEmail(Email))
            {
                NotificationManager.ShowWarning($"Почта {Email} имеет некорректный формат!");
                return;
            }

            if (Password.Length < 6) // Проверяем длину пароля на количество символов
            {
                NotificationManager.ShowWarning("Длина пароля не может быть меньше 6");
                return;
            }

            // Проверяем совпадение паролей
            if (Password != ConfirmedPassword)
            {
                NotificationManager.ShowWarning("Пароли не совпадают!");
                return;
            }

            try
            {
                // Ищем пользователей с такой же почтой
                if (MainWindowViewModel.Database.Пользователи.FirstOrDefault((us) => us.Почта == Email) == null)
                {
                    // Если не нашли, тогда открываем диалоговое окно для подтверждения почты
                    if (new AdditionalWindow()
                    {
                        DataContext = new AdditionalWindowViewModel()
                        {
                            CurrentView = new EmailConfirmationView()
                            {
                                DataContext = new EmailConfirmationViewModel(Email)
                            }
                        }
                    }.ShowDialog() ?? false)
                    {

                        // Добавляем пользователя в БД
                        var newUser = new Models.Database.Пользователи()
                        {
                            Логин = Login,
                            Почта = Email,
                        };
                        newUser.SetPassword(Password);
                        MainWindowViewModel.Database.Пользователи.Add(newUser);
                        MainWindowViewModel.Database.SaveChangesAsync();
                        NotificationManager.ShowSuccess("Вы успешно зарегистрированы в системе!");
                        MainWindowViewModel.ChangeView("SignInView");
                    }
                    else // Если код неверный - выдаем ошибку
                    {
                        NotificationManager.ShowError("Код подтверждения не был введен. Пользователь не зарегистрирован!");
                    }
                }
                else // Если пользователь уже существует
                {
                    NotificationManager.ShowWarning("Пользователь с такой почтой уже зарегистрирован!");
                }
            }
            catch (Exception e) { NotificationManager.ShowError(e.Message); }
        });
        #endregion
    }
}
