using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using SQLApp.resources.post.view;

namespace SQLApp.resources.post
{
	class search
	{
		public static void user(TextBox shUN, SqlConnection sqlConnection)
		{
			try
			{
				sqlConnection.Open();
				SqlCommand command = new SqlCommand($"SELECT * From [dbo].[users] where username = '{shUN.Text}'", sqlConnection);

				SqlDataReader reader = command.ExecuteReader();

				if (reader.HasRows)
				{
					onlyViewer only = new onlyViewer(shUN.Text);
					only.Show();
				}
				else
				{
					MessageBox.Show("No se encuentra el nombre de usuario, la consulta es sencible a las mayusculas & minusculas", "Ha ocurrido un error");
				}
			}
			finally
			{
				sqlConnection.Close();
			}
		}
	}
}
