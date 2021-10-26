using System.Globalization;
using System.Threading;
using System.Windows;
using SQLApp.resources.net;
using SQLApp.resources.lang;

namespace SQLApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public static string lang { get; set; }
		#region Constructor
		public MainWindow(string lang)
		{
			InitializeComponent();
			txtEmail.Text = App.ReadSetting("email");
			changeLang(lang);
		}

		#endregion
		#region RegisterAndLogin
		private void btnCheck_Click(object sender, RoutedEventArgs e)
		{
			check.Email(txtUserName.Text, txtEmail.Text, txtPassword.Password);
		}
		private void btnCheckLogin_Click(object sender, RoutedEventArgs e)
		{
			_ = Connetion.into(txtEmail.Text,Encrypt.encrypt(txtPassword.Password), this);

		}
		#endregion
		#region 'Span'
		static bool isRegister = true;
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			txtEmail.Clear();
			txtPassword.Clear();
			if (isRegister == true)
			{
				username.Visibility = Visibility.Visible;
				txtUserName.Visibility = Visibility.Visible;
				lblSpam.Visibility = Visibility.Visible;
				lblSpamLogin.Visibility = Visibility.Hidden;

				btnChecklogin.IsEnabled = false;
				btnChecklogin.Visibility = Visibility.Hidden;
				btnCheck.IsEnabled = true;
				btnCheck.Visibility = Visibility.Visible;
				lblTitleLogin.Visibility = Visibility.Hidden;
				lblTitleSing.Visibility = Visibility.Visible;
				isRegister = false;
			}
			else
			{
				username.Visibility = Visibility.Hidden;
				txtUserName.Visibility = Visibility.Hidden;
				lblSpam.Visibility = Visibility.Hidden;
				lblSpamLogin.Visibility = Visibility.Visible;

				btnChecklogin.IsEnabled = true;
				btnChecklogin.Visibility = Visibility.Visible;
				btnCheck.IsEnabled = false;
				btnCheck.Visibility = Visibility.Hidden;


				lblTitleLogin.Visibility = Visibility.Visible;
				lblTitleSing.Visibility = Visibility.Hidden;
				isRegister = true;
			}
		}
		private void btnEnter(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Enter)
			{
				if (btnChecklogin.IsEnabled == true)
				{
					
					_ = Connetion.into(txtEmail.Text, Encrypt.encrypt(txtPassword.Password), this);

				}
				else
				{
					check.Email(txtUserName.Text, txtEmail.Text, txtPassword.Password);
				}
			}
		}
		#endregion
		#region Change idiom
		private void rdSp_Checked(object sender, RoutedEventArgs e)
		{
			changeLang("es-ES");
			rdChi.IsChecked = false;
			rdEn.IsChecked = false;
			rdRus.IsChecked = false;
		}
		private void rdEn_Checked(object sender, RoutedEventArgs e)
		{
			changeLang("");
			rdChi.IsChecked = false;
			rdSp.IsChecked = false;
			rdRus.IsChecked = false;
		}

		private void rdRus_Checked(object sender, RoutedEventArgs e)
		{
			changeLang("ru-Ru");
			rdChi.IsChecked = false;
			rdSp.IsChecked = false;
			rdEn.IsChecked = false;
		}

		private void rdChi_Checked(object sender, RoutedEventArgs e)
		{
			changeLang("zh-CN");
			rdEn.IsChecked = false;
			rdSp.IsChecked = false;
			rdRus.IsChecked = false;
		}
		void changeLang(string language)
		{
			App.AddUpdateAppSettings("lang", language);

			Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(App.ReadSetting("lang"));

			lblTitleLogin.Content = idiom.TID_LOGIN;
			lblTitleSing.Content = idiom.TID_REGISTER;

			lblSpam.Text = idiom.TID_SpanRegister;
			lblSpamLogin.Text = idiom.TID_SpanLogin;
			btnClickme.Content = idiom.TID_SpanClic;

			Title = idiom.TID_TITLE;
			btnChecklogin.Content = idiom.TID_LOGIN;
			btnCheck.Content = idiom.TID_REGISTER;

			checkIdiom();
		}
		void checkIdiom()
		{
			switch (App.ReadSetting("lang"))
			{
				case "es-ES":
					rdChi.IsChecked = false;
					rdEn.IsChecked = false;
					rdRus.IsChecked = false;
					rdSp.IsChecked = true;
					break;
				case "ru-Ru":
					rdRus.IsChecked = true;
					rdChi.IsChecked = false;
					rdEn.IsChecked = false;
					rdSp.IsChecked = false;
					break;
				case "zh-CN":
					rdChi.IsChecked = true;
					rdEn.IsChecked = false;
					rdRus.IsChecked = false;
					rdSp.IsChecked = false;
					break;
				case "":
					rdChi.IsChecked = false;
					rdEn.IsChecked = true;
					rdRus.IsChecked = false;
					rdSp.IsChecked = false;
					break;
				default:
					break;
			}
		}
		#endregion
	}
}
