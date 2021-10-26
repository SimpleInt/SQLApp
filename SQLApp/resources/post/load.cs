using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SQLApp.resources.post
{
	class load
	{
		public static void Avatar(SqlConnection sqlConnection, string user, ImageBrush btnImg)
		{
			sqlConnection.Open();
			SqlCommand command = new SqlCommand($"SELECT avatar FROM [dbo].[users] where username= '{user}'", sqlConnection);
			SqlDataReader reader = command.ExecuteReader();
			BitmapImage bi3 = new BitmapImage();
			string uri = "";
			if (reader.HasRows)
			{
				while (reader.Read())
				{
					uri = reader.GetString(0);
				}

				bi3.BeginInit();
				bi3.UriSource = new Uri($"https://regeximg.blob.core.windows.net/users/{uri}", UriKind.RelativeOrAbsolute);
				bi3.EndInit();
				btnImg.ImageSource = bi3;
			}
			sqlConnection.Close();
		}
		
		public static async Task AllStrings(SqlDataReader reader, string user, string connectionString, Window win, StackPanel layerList, SqlConnection sqlUsers, string userpath)
		{
			if (reader.HasRows)
			{
				while (await reader.ReadAsync())
				{
					create.Poster(user, connectionString, reader.GetString(1), reader.GetString(2), reader.GetInt32(0), win, layerList, sqlUsers, userpath);
				}
			}
		}
		
		public static async Task LastString(SqlDataReader reader, string user, string connectionString, Window win, StackPanel layerList, SqlConnection sqlUsers, string userpath)
		{
			if (reader.HasRows)
			{
				while (await reader.ReadAsync())
				{
					create.Poster(user, connectionString, reader.GetString(1), reader.GetString(2), reader.GetInt32(0), win, layerList, sqlUsers, userpath);
				}
			}
		}
	}
}