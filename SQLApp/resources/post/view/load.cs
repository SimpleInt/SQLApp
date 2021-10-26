using SQLApp.resources.lang;
using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SQLApp.resources.post.view
{
	class load
	{
		static string user { get; set; }
		static SqlConnection sqlConnection = new SqlConnection(sql.Regedit);
		public load(string User)
		{
			user = User;
		}
		public void Avatar(Image avatarImg)
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
				avatarImg.Source = bi3;
			}
			sqlConnection.Close();
		}
		public void comments(StackPanel stack, string userpt)
		{
			sqlConnection.Open();
			
			SqlCommand command = new SqlCommand($"SELECT comments FROM[dbo].[users] where username = '{userpt}'", sqlConnection);
			SqlDataReader reader = command.ExecuteReader();
			string text = "";
			if (reader.HasRows)
			{
				while (reader.Read())
				{
					text = reader.GetString(0);
				}
			}
			Regex regex = new Regex(@"[A-Za-z0-9]+|[A-Za-z0-9]+|[A-Za-z0-9]");
			var s = regex.Matches(text);
			for (int i = 0; i < s.Count; i++)
			{
				string user = s[i].Value;
				string content = s[i += 1].Value;
				
				TextBlock newTxtBlock = new TextBlock
				{
					TextWrapping = TextWrapping.Wrap,
					Text = user+" ha publicado: "+content
				};
				stack.Children.Add(newTxtBlock);
			}
			sqlConnection.Close();
		}
		public void posts(SqlDataReader reader, StackPanel layerList)
		{
			if (reader.HasRows)
			{
				while (reader.Read())
				{
					if (reader.GetBoolean(4))
					{
						create.Post(reader.GetString(1), reader.GetString(2), layerList);
					}
				}
			}
		}
	}
}
