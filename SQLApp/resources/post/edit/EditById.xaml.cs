using Azure.Storage.Blobs;
using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using SQLApp.resources.lang;
using System.Globalization;
using System.IO;

namespace SQLApp.resources.post.edit
{
	/// <summary>
	/// Interaction logic for EditById.xaml
	/// </summary>
	public partial class EditById : Window
	{
		#region SetVariables
		static SqlConnection sqlUsers = new SqlConnection(sql.Users);
		static string connectionString = sql.ConnectionString;
		static int id;
		static string userPath;
		static string user;
		#endregion
		#region Constructor
		public EditById(int Key, string User, Window win)
		{
			InitializeComponent();
			changeLang(App.ReadSetting("lang"));
			fileBlob = "";
			txtEdit.AcceptsReturn = true;
			win.Close();
			user = User;
			userPath = $"[dbo].[user_{user}]";
			id = Key;
			try
			{
				sqlUsers.Open();
				lblId.Content = "id: " + id;
				SqlCommand command = new SqlCommand($"SELECT * FROM {userPath} WHERE id = '{id}'", sqlUsers);
				_ = loadingString(command.ExecuteReaderAsync().Result);
			}
			finally
			{
				sqlUsers.Close();
			}
		}


		void changeLang(string lang)
		{
			Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(lang);
			lblEditById.Content = idiom.TID_EditById;
			lblEditContent.Content = idiom.TID_EditContent;
			btnSE.Content = idiom.TID_SE;
			btnDel.Content = idiom.TID_DEL;
			checkPriv.Content = idiom.TID_ISPUBLIC;
			btnAP.Content = idiom.TID_ATTPIC;
		}
		#endregion
		#region LoadByID
		async Task loadingString(SqlDataReader reader)
		{
			BitmapImage bi3 = new BitmapImage();
			bi3.BeginInit();
			if (reader.HasRows)
			{
				while (await reader.ReadAsync())
				{
					txtEdit.Text = reader.GetString(1);
					if (reader.GetBoolean(4))
						checkPriv.IsChecked = false;
					else
						checkPriv.IsChecked = true;

					bi3.UriSource = new Uri($"https://regeximg.blob.core.windows.net/users/{reader.GetString(2)}", UriKind.RelativeOrAbsolute);
				}
			}

			bi3.EndInit();
			imgSource.Source = bi3;
		}
		#endregion
		#region UploadFiles
		private void btnAP_Click(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog openFile = new Microsoft.Win32.OpenFileDialog
			{
				Filter = "Pictures|*.jpg;*.png|Videos|*.mp4;",
				FilterIndex = 1,
				RestoreDirectory = true
			};

			Nullable<bool> result = openFile.ShowDialog();
			if (result == true)
			{
				string filename = openFile.FileName; // get it from OpenFileDialog
				var length = new FileInfo(filename).Length / 1000;
				if (length <= @const.maxKb)
				{
					string[] ext = openFile.FileName.Split(".");
					_ = UploadFile(openFile.FileName, imgSource, ext[ext.Length - 1]);
				}
				else
				{
					MessageBox.Show($"El archivo pesa más que {@const.maxKb / 1000} MB");
				}

			}
		}
		async Task UploadFile(string filename, Image img, string extension)
		{
			Guid g = Guid.NewGuid();
			MessageBox.Show(extension);
			string idk;
			if (extension.Equals("jpg") || extension.Equals("png"))
			{
				BitmapImage bi3 = new BitmapImage();
				bi3.BeginInit();
				bi3.UriSource = new Uri($"{filename}", UriKind.RelativeOrAbsolute);
				bi3.EndInit();
				img.Source = bi3;
				idk = "jpg";
			}
			else
			{
				img.Source = null;
				idk = "mp4";
			}
			// intialize BobClient 
			BlobClient blobClient = new BlobClient(
				connectionString: connectionString,
				blobContainerName: "users",
				blobName: $"{g}.{idk}");

			// upload the file


			fileBlob = $"{g}.{idk}";
			btnSE.IsEnabled = false;
			await blobClient.UploadAsync(filename);
			btnSE.IsEnabled = true;
		}

		static string fileBlob { get; set; }
		#endregion
		#region Save
		private void btnSE_Click(object sender, RoutedEventArgs e)
		{
			_ = saveAs();
		}
		async Task saveAs()
		{
			byte isPublic = 1;
			try
			{
				if (checkPriv.IsChecked == true) isPublic = 0;

				sqlUsers.Open();
				SqlCommand command;
				if (fileBlob != "")
					command = new SqlCommand($"UPDATE {userPath} SET POST = '{txtEdit.Text}', POST_IMG = '{fileBlob}', ISPUBLIC = '{isPublic}' WHERE ID = {id}", sqlUsers);
				else
					command = new SqlCommand($"UPDATE {userPath} SET POST = '{txtEdit.Text}', ISPUBLIC = '{isPublic}' WHERE ID = {id}", sqlUsers);

				GC.Collect();
				command.ExecuteNonQueryAsync().Wait();

			}
			finally
			{
				fileBlob = "";
				await sqlUsers.CloseAsync();

				post post = new post(user);
				post.Show();
				Close();
			}
		}
		#endregion
		#region Delete
		private void btnDel_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				sqlUsers.Open();
				SqlCommand command = new SqlCommand($"DELETE FROM {userPath} WHERE ID={id};", sqlUsers);
				command.ExecuteNonQueryAsync();
				post post = new post(user);
				GC.Collect();
				post.Show();
			}
			finally
			{
				sqlUsers.Close();
				fileBlob = "";
				Close();
			}
		}
		#endregion
	}
}
