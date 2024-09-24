using BusParkDispatcher.Infrastructure;
using BusParkDispatcher.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusParkDispatcher.Models.Database
{
    public class Пользователи
    {
        #region Properties
        [Key]
        public int КодПользователя { set; get; }

        [Required]
        [Index(IsUnique = true)]
        [MaxLength(255)]
        public string Почта { set; get; }

        [Required]
        public string Логин { set; get; }

        [Required]
        public string ЗашифрованныйПароль { set; get; }

        [Required]
        public string КодШифрования { set; get; }

        public bool ЯвляетсяАдминистратором { set; get; }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public override string ToString() => Логин;

        public string SetPassword(string password)
        {
            var saltBytes = new byte[4];
            new Random().NextBytes(saltBytes);
            КодШифрования = SecurityCryptography.CalculateHashToString(saltBytes);
            ЗашифрованныйПароль = CalculatePassword(password);
            return ЗашифрованныйПароль;
        }

        public byte[] CalculateSummaryHash(string password) => SecurityCryptography.CalculateHash(SecurityCryptography.CalculateHashToString(password) + КодШифрования);

        public string CalculatePassword(string password) => SecurityCryptography.HashToString(CalculateSummaryHash(password));

        public bool CheckPassword(string password) => ЗашифрованныйПароль == CalculatePassword(password);

        public string SetNewPassword()
        {
            var password = GenerateNewPassword();
            SetPassword(password);
            return password;
        }

        private string GenerateNewPassword()
        {
            var password = new StringBuilder();
            var random = new Random();
            for (int i = 0; i < 6; i++) password.Append(random.Next(2) != 0 ? (char)(random.Next(65, 90)) : (char)(random.Next(48, 57)));
            return password.ToString();
        }

        public static bool HasAdminPermissions() => MainWindowViewModel.currentUser?.ЯвляетсяАдминистратором ?? false;

        public static void HasNotPermissionNotification() => NotificationManager.ShowWarning("У вас недостаточно прав для данного действия!");
        #endregion
    }
}
