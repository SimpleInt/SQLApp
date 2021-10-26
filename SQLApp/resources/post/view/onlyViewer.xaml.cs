using SQLApp.resources.lang;
using System.Data.SqlClient;
using System.Windows;

namespace SQLApp.resources.post.view
{
	/// <summary>
	/// Interaction logic for onlyViewer.xaml
	/// </summary>
	public partial class onlyViewer : Window
	{
		static string userpath { get; set; }
		static SqlConnection sqlCon = new SqlConnection(sql.Users);
		static string user = "";


		public onlyViewer(string username)
		{
			InitializeComponent();
			create.web = myMediaweb;
			user = username;
			load nL = new load(user);
			Title = "viendo a: " + username;
			Username.Content = user;
			userpath = $"[dbo].[user_{user}]";
			try
			{
				sqlCon.Open();
				SqlCommand command = new SqlCommand($"SELECT * FROM {userpath}", sqlCon);

				nL.posts(command.ExecuteReader(), layerList);
				nL.Avatar(avatarImg);
			}
			finally
			{
				sqlCon.Close();
			}
		}
	}
}
