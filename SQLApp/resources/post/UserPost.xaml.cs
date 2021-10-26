using SQLApp.resources.lang;
using System;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace SQLApp.resources.post
{
	/// <summary>
	/// Interaction logic for UserPost.xaml
	/// </summary>
	public partial class UserPost : UserControl
	{
		static StackPanel layerList;
		public UserPost()
		{
			InitializeComponent();
			counter.Content = setPost.Text.Length + "/" + setPost.MaxLength;
			setPost.AcceptsReturn = true;
			changeLang();
		}
		public UserPost(string User, StackPanel layer, Window post)
		{
			InitializeComponent();
			user = User;
			userpath = $"[dbo].[user_{user}]";

			layerList = layer;
			setPost.AcceptsReturn = true;

			try
			{
				sqlUsers.Open();
				SqlCommand command = new SqlCommand($"SELECT * FROM {userpath}", sqlUsers);
				_ = load.AllStrings(command.ExecuteReaderAsync().Result, user, connectionString, post, layerList, sqlUsers, userpath);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Voce a um tomto" + ex);
			}
			finally
			{
				changeLang();
				sqlUsers.Close();
			}
		}
		void changeLang()
		{
			Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(App.ReadSetting("lang"));
			btnimg.Content = idiom.TID_ATTPIC;
			checkPriv.Content = idiom.TID_ISPUBLIC;
			button.Content = idiom.TID_POST;
		}
		static SqlConnection sqlConnection = new SqlConnection(sql.Regedit);
		static SqlConnection sqlUsers = new SqlConnection(sql.Users);
		static string connectionString = sql.ConnectionString;

		static string userpath { get; set; }
		static string user;

		private void btnImage(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog openFile = new Microsoft.Win32.OpenFileDialog
			{
				Filter = "PicturesOrVideos|*.jpg;*.png;*.mp4|all|*.*",
				FilterIndex = 1,
				RestoreDirectory = true
			};

			Nullable<bool> result = openFile.ShowDialog();
			if (result == true)
			{
				string filename = openFile.FileName; // get it from OpenFileDialog
				long length = new FileInfo(filename).Length / 1000;
				if (length <= @const.maxKb)
				{
					string[] ext = openFile.FileName.Split(".");
					_ = create.Upload(openFile.FileName, connectionString, imgAtt, button, ext[ext.Length - 1]);
				}
				else
				{
					MessageBox.Show($"El archivo pesa más que {@const.maxKb / 1000} MB");
				}

			}
		}


		#region searchUsers
		private void btnSUN(object sender, RoutedEventArgs e)
		{
			//MessageBox.Show("Al agregar los videos y archivos, se ha tenido que desactivar esta funcion, ya que esta en desarrollo");
			search.user(shUN, sqlConnection);
		}


		private void cmd(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Enter)
			{
				//MessageBox.Show("Al agregar los videos y archivos, se ha tenido que desactivar esta funcion, ya que esta en desarrollo");
				search.user(shUN, sqlConnection);
			}
		}

		#endregion
		#region Post

		private void button_Click(object sender, RoutedEventArgs e)
		{
			post.posters(checkPriv, setPost, imgAtt);
		}


		#endregion

		private void setPost_TextChanged(object sender, TextChangedEventArgs e)
		{
			counter.Content = setPost.Text.Length + "/" + setPost.MaxLength;
		}
	}
}
