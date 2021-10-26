using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using SQLApp.Properties;
using SQLApp.resources.lang;
using newPost = SQLApp.resources.post;

namespace SQLApp.resources.net
{
	class Connetion
	{
		#region SetVariables
		static SqlConnection sqlConnection = new SqlConnection(sql.Regedit);
		static SqlConnection sqlUsers = new SqlConnection(sql.Users);
		#endregion
		#region INSERT&CREATE
		public static void insertNewUser(string username, string email, string pass)
		{
			sqlConnection.Open();
			String sql = $"INSERT INTO [dbo].[users](username, email, pass, avatar) VALUES('{username}', '{email}', '{pass}','default.png')";
			SqlCommand command = new SqlCommand(sql, sqlConnection);
			command.ExecuteNonQuery();
			sqlConnection.Close();
		}
		public static bool createUser(string username)
		{

			sqlUsers.Open();
			String sql = $"CREATE TABLE user_{username} (ID int IDENTITY(0,1) PRIMARY KEY,POST varchar(max),POST_IMG varchar(max),LIKES INT,ISPUBLIC bit, TIME DATETIME DEFAULT GETDATE());";
			SqlCommand command = new SqlCommand(sql, sqlUsers);
			try
			{
				command.ExecuteNonQuery();

				sqlUsers.Close();
				return true;
			}
			catch (Exception)
			{
				MessageBox.Show($"Asegurate que tu {username} no tenga simbolos");

				sqlUsers.Close();
				return false;
			}
		}
		#endregion
		#region CHECKING
		public static bool checkEmail(string email)
		{
			sqlConnection.Open();
			string sql = $"SELECT email FROM [dbo].[users] WHERE email = '{email}'";
			SqlCommand command = new SqlCommand(sql, sqlConnection);
			bool g = command.ExecuteReader().HasRows;
			sqlConnection.Close();
			return g;
		}

		public static bool checkUser(string user)
		{
			if (user.Length >= 3)
			{
				sqlConnection.Open();
				String sql = $"SELECT username FROM [dbo].[users] WHERE username = '{user}'";
				SqlCommand command = new SqlCommand(sql, sqlConnection);
				bool g = command.ExecuteReader().HasRows;
				sqlConnection.Close();
				return g;
			}
			else
			{
				return true;
			}
		}
		#endregion
		#region login
		public static async Task into(string email, string password, Window wm)
		{
			try
			{
				sqlConnection.Open();
				SqlCommand command = new SqlCommand($"SELECT * from [dbo].[users] WHERE email = '{email}' and pass = '{password}'", sqlConnection);

				SqlDataReader reader = await command.ExecuteReaderAsync();
				string a = "";

				if (reader.HasRows)
				{
					while (reader.ReadAsync().Result)
					{
						a = reader.GetString(0);
					}

					newPost.post post = new newPost.post(a);
					post.Show();
					App.AddUpdateAppSettings("email", email);
					wm.Close();
				}
				else
				{
					MessageBox.Show("Datos incorrectos");
				}
			}
			finally
			{
				sqlConnection.Close();
			}
		}
		#endregion
	}
}
