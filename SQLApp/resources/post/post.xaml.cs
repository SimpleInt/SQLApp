using Azure.Storage.Blobs;
using SQLApp.resources.lang;
using System;
using System.Data.SqlClient;
using System.Globalization;
using Microsoft.Web.WebView2.Core;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SQLApp.resources.post
{
	/// <summary>
	/// Interaction logic for post.xaml
	/// </summary>
	public partial class post : Window
	{
		#region SetVariables
		static SqlConnection sqlConnection = new SqlConnection(sql.Regedit);
		static SqlConnection sqlUsers = new SqlConnection(sql.Users);
		static string connectionString = sql.ConnectionString;

		static string userpath { get; set; }
		static string user;

		#endregion
		#region Constructor
		public post(string User)
		{
			InitializeComponent();
			
			create.web = myMediaweb;
			Closed += Post_Closed;
			lyrList = layerList;
			win = this;
			UserPost post = new UserPost(User, layerList, this);
			post.InitializeComponent();
			GC.Collect();

			lblUser.Content = User;
			user = User;

			Title = idiom.TID_POSTOF + user;
			userpath = $"[dbo].[user_{User}]";
			try
			{
				sqlUsers.Open();
				SqlCommand command = new SqlCommand($"SELECT * FROM {userpath}", sqlUsers);
				load.Avatar(sqlConnection, user, btnImg);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Voce a um tomto" + ex);
			}
			finally
			{
				sqlUsers.Close();
			}
		}

		private void Post_Closed(object sender, EventArgs e)
		{ 
			myMediaweb.NavigateToString("<br/>");
		}
		#endregion
		#region CreatePosts
		static StackPanel lyrList;
		static Window win;
		public static void posters(CheckBox checkPriv, TextBox setPost, Image imgAtt)
		{
			byte i = 1;
			try
			{
				if (checkPriv.IsChecked == true) i = 0;

				sqlUsers.Open();
				SqlCommand sql = new SqlCommand($"INSERT INTO {userpath}(POST, POST_IMG, ISPUBLIC)VALUES('{setPost.Text}','{post.fileBlob}', {i})", sqlUsers);
				sql.ExecuteNonQuery();


				sql = new SqlCommand($"SELECT TOP(1) * FROM {userpath} ORDER BY 1 DESC", sqlUsers);
				
				_ = load.LastString(sql.ExecuteReaderAsync().Result, user, connectionString, win, lyrList, sqlUsers, userpath);

				setPost.Clear();
				imgAtt.Source = null;
			}
			catch
			{
				MessageBox.Show("Ha ocurrido un error, por favor intentalo de nuevo");
			}
			finally
			{
				post.fileBlob = "";
				sqlUsers.Close();
			}
		}
		public static string fileBlob { get; set; }

		#endregion
		#region Change Avatar

		private void btnChangeAvatar(object sender, RoutedEventArgs e)
		{
			_ = ChangeAvatar();
		}
		private async Task ChangeAvatar()
		{
			sqlConnection.Open();
			Microsoft.Win32.OpenFileDialog openFile = new Microsoft.Win32.OpenFileDialog
			{
				Filter = "Pictures|*.jpg;*.png",
				FilterIndex = 1,
				RestoreDirectory = true
			};

			bool? result = openFile.ShowDialog();
			if (result == true)
			{
				Guid g = Guid.NewGuid();
				BlobClient blobClient = new BlobClient(
					connectionString: connectionString,
					blobContainerName: "users",
					blobName: $"Avatar_{g}.jpg");

				// upload the file

				SqlCommand sql = new SqlCommand($"UPDATE [dbo].[users]set avatar = 'Avatar_{g}.jpg' where username = '{user}'", sqlConnection);
				sql.ExecuteNonQuery();
				sqlConnection.Close();
				BitmapImage bi3 = new BitmapImage();
				bi3.BeginInit();
				bi3.UriSource = new Uri($"https://regeximg.blob.core.windows.net/users/Avatar_{g}.jpg", UriKind.RelativeOrAbsolute);
				bi3.EndInit();
				btnImg.ImageSource = bi3;

				await blobClient.UploadAsync(openFile.FileName);
			}
		}
		#endregion

	}
}
