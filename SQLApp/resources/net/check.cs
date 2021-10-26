using System.Text.RegularExpressions;
using System.Windows;

namespace SQLApp.resources.net
{
	class check
	{
		#region CheckTheEmail
		public static void Email(string user, string email, string pass)
		{
			Regex regex = new Regex(@"[0-9a-zA-Z]+@+[0-9a-zA-Z].+[0-9a-zA-Z]");

			string password = Encrypt.encrypt(pass);
			if (regex.IsMatch(email) && Connetion.checkUser(user) == false && Connetion.checkEmail(email) == false)
			{
				if (Connetion.createUser(user.Trim()))
				{
					Connetion.insertNewUser(user.Trim(), email, password);
					MessageBox.Show("Se ha registrado con exito", "Felicidades", MessageBoxButton.OK, MessageBoxImage.Information);
				}
			}
			else
			{
				MessageBox.Show("¡El email ya ha sido utilizado!");
			}
		}
		#endregion
	}
}
