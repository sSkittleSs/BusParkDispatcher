using BusParkDispatcher.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BusParkDispatcher
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
		#region Fields
		public static event EventHandler LanguageChanged;

		private static List<CultureInfo> languages = new List<CultureInfo>();
		#endregion

		#region Properties
		public static List<CultureInfo> Languages
		{
			get
			{
				return languages;
			}
		}

		public static CultureInfo Language
		{
			get
			{
				return System.Threading.Thread.CurrentThread.CurrentUICulture;
			}
			set
			{
				if (value == null) throw new ArgumentNullException("value");
				if (value == System.Threading.Thread.CurrentThread.CurrentUICulture) return;

				//1. Меняем язык приложения:
				System.Threading.Thread.CurrentThread.CurrentUICulture = value;

				//2. Создаём ResourceDictionary для новой культуры
				ResourceDictionary dict = new ResourceDictionary();
				switch (value.Name)
				{
					case "en-US":
						dict.Source = new Uri(String.Format("/Languages/lang.{0}.xaml", value.Name), UriKind.Relative);
						break;
					default:
						dict.Source = new Uri("/Languages/lang.xaml", UriKind.Relative);
						break;
				}

				//3. Находим старую ResourceDictionary и удаляем его и добавляем новую ResourceDictionary
				ResourceDictionary oldDict = (from d in Current.Resources.MergedDictionaries
											  where d.Source != null && d.Source.OriginalString.StartsWith("/Languages/lang.")
											  select d).First();
				if (oldDict != null)
				{
					int ind = Current.Resources.MergedDictionaries.IndexOf(oldDict);
                    Current.Resources.MergedDictionaries.Remove(oldDict);
					Current.Resources.MergedDictionaries.Insert(ind, dict);
				}
				else
				{
					Current.Resources.MergedDictionaries.Add(dict);
				}

				//4. Вызываем евент для оповещения всех окон.
				LanguageChanged?.Invoke(Current, new EventArgs());
			}
		}
		#endregion

		#region Contructors
		public App()
		{
			Languages.Clear();
			Languages.Add(new CultureInfo("ru-RU")); //Нейтральная культура для этого проекта
			Languages.Add(new CultureInfo("en-US"));

			LanguageChanged += App_LanguageChanged;
		}
		#endregion

		#region Methods
		private void App_LanguageChanged(Object sender, EventArgs e)
		{
			BusParkDispatcher.Properties.Settings.Default.DefaultLanguage = Language;
			BusParkDispatcher.Properties.Settings.Default.Save();
		}
		#endregion
	}
}
