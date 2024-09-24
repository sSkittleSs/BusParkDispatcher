using BusParkDispatcher.Commands.Base;
using BusParkDispatcher.Infrastructure;
using BusParkDispatcher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BusParkDispatcher.ViewModels
{
    class PasswordRecoveryViewModel : ObservableObject
    {
        #region Fields
        private Brush borderBrush = Brushes.Blue;
        private string email;
        #endregion

        #region Properties
        public Brush BorderBrush
        {
            set => SetProperty(ref borderBrush, value);
            get => borderBrush;
        }

        public string Email
        {
            set
            {
                SetProperty(ref email, value);

                if (value?.Length == 0) BorderBrush = Brushes.Blue;
                else if (!MailValidator.IsValidEmail(Email)) BorderBrush = Brushes.Red;
                else BorderBrush = Brushes.Green;
            }
            get => email;
        }
        #endregion

        #region Commands / Methods
        public DelegateCommand Recovery => new DelegateCommand((obj) =>
        {
            if (obj is Window window && MainWindowViewModel.Database.Пользователи.FirstOrDefault((user) => user.Почта == Email) != default)
            {
                MailManager.SendPasswordRecoveryLetter(Email);
                window.DialogResult = true;
                window.Close();
            }
            else
            {
                NotificationManager.ShowWarning("Аккаунта с такой почтой не существует!");
            }
        });
        #endregion
    }
}
