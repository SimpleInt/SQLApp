using Azure.Storage.Blobs;
using Microsoft.Web.WebView2.Wpf;
using SQLApp.resources.post.edit;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SQLApp.resources.post
{
	class create
	{
		public static void Poster(string user, string connectionString, string txt, string guid, int id, Window window, StackPanel layerList, SqlConnection sqlUsers, string userpath)
		{
			#region CreateObjects
			Image myImage3 = new Image();
			StackPanel newStack = new StackPanel
			{
				Orientation = Orientation.Horizontal,
				HorizontalAlignment = HorizontalAlignment.Right
			};
			Rectangle rec = new Rectangle()
			{
				Height = 2,
				Fill = Brushes.Gray,
				Margin = new Thickness(25),
			};
			TextBlock newTxtBlock = new TextBlock
			{
				Text = txt,
				TextWrapping = TextWrapping.WrapWithOverflow,
				HorizontalAlignment = HorizontalAlignment.Center,
				Margin = new Thickness(25),
				FontSize = 24
			};
			#region Buttons+Img
			BitmapImage imgEdit = new BitmapImage();
			imgEdit.BeginInit();
			try
			{
				imgEdit.UriSource = new Uri(Directory.GetCurrentDirectory() + $@"\icons\edit.png", UriKind.RelativeOrAbsolute);
			}
			catch (Exception)
			{
				imgEdit.UriSource = new Uri($"https://regeximg.blob.core.windows.net/icons/edit.png", UriKind.RelativeOrAbsolute);
			}
			imgEdit.EndInit();

			BitmapImage imgDownload = new BitmapImage();
			imgDownload.BeginInit();
			try
			{
				imgDownload.UriSource = new Uri(Directory.GetCurrentDirectory() + $@"\icons\download.png", UriKind.RelativeOrAbsolute);
			}
			catch (Exception)
			{

				imgDownload.UriSource = new Uri($"https://regeximg.blob.core.windows.net/icons/download.png", UriKind.RelativeOrAbsolute);
			}
			
			imgDownload.EndInit();

			BitmapImage imgDel = new BitmapImage();
			imgDel.BeginInit();
			try
			{
				imgDel.UriSource = new Uri(Directory.GetCurrentDirectory() + $@"\icons\delete.png", UriKind.RelativeOrAbsolute);
			}
			catch (Exception)
			{
				imgDel.UriSource = new Uri("https://regeximg.blob.core.windows.net/icons/delete.png", UriKind.RelativeOrAbsolute);
			}
			imgDel.EndInit();


			Button btnEdit = new Button
			{
				Background = new ImageBrush(imgEdit),
				Width = 50,
				Height = 50
			};
			btnEdit.AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new RoutedEventHandler((object sender, RoutedEventArgs e) =>
			{
				GC.Collect();

				EditById id1 = new EditById(id, user, window);
				id1.ShowDialog();
			}));

			Button btnDownload = new Button
			{
				Background = new ImageBrush(imgDownload),
				Width = 50,
				Height = 50
			};
			btnDownload.AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new RoutedEventHandler((object sender, RoutedEventArgs e) =>
			{
				string[] ext = guid.Split(".");
				_ = Download(guid, connectionString, ext[ext.Length-1]);
			}));

			Button btnDel = new Button
			{
				Background = new ImageBrush(imgDel),
				Width = 50,
				Height = 50
			};
			Button newWeb = new Button()
			{
				FontSize = 48,
				Content = "Ver video"
			};
			
			newWeb.AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new RoutedEventHandler((object sender, RoutedEventArgs e) =>
			{
				string cssStyle = @"
				<style>video{
				  width: 100vw;
				  height: 100vh;
				  object-fit: cover;
				  position: fixed;
				  top: 0;
				  left: 0;
				}</style>";
				web.NavigateToString($@"{cssStyle}
						<video controls preload='metadata'>
							<source src='https://regeximg.blob.core.windows.net/users/{guid}'  type='video/mp4'>
						</video>");
			}));
			btnDel.AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new RoutedEventHandler((object sender, RoutedEventArgs e) =>
			{
				try
				{

					sqlUsers.Open();
					SqlCommand command = new SqlCommand($"DELETE FROM {userpath} WHERE ID={id};", sqlUsers);
					command.ExecuteNonQueryAsync();
					layerList.Children.Remove(newStack);
					web.NavigateToString("<h1>Se ha dejado de reproducir por un post eliminado</h1>");
					layerList.Children.Remove(newWeb);
					layerList.Children.Remove(myImage3);
					layerList.Children.Remove(newTxtBlock);
					layerList.Children.Remove(rec);
					GC.Collect();
				}
				finally
				{
					sqlUsers.Close();

					post.fileBlob = "";
				}
			}));
			
			
			#endregion
			#endregion

			if (guid != "")
			{
				string[] file = guid.Split(".");
				newStack.Children.Add(btnDownload);
				newStack.Children.Add(btnDel);
				newStack.Children.Add(btnEdit);
				if (file[file.Length - 1] == "mp4" || file[file.Length - 1] == "avi")
				{

					//newWeb.NavigateToString($@"
					  //         <!DOCTYPE html>
					  //         <html>
					  //         <head>
					  //             <meta http-equiv='Content-Type' content='text/html; charset=unicode' />
					  //             <meta http-equiv='X-UA-Compatible' content='IE=9' /> 
					  //             <title></title>
					  //         </head>
					  //         <body>
					  //             <div>
					  //                  <video preload='metadata' controls='true'>
							//<source src='https://regeximg.blob.core.windows.net/users/{guid}' type='video/mp4' />
					  //                 </video>
					  //             </div>
					  //         </body>
					  //         </html>");
					layerList.Children.Insert(0, rec);
					layerList.Children.Insert(0, newWeb);
				}
				else if (file[file.Length - 1] == "jpg" || file[file.Length - 1] == "png")
				{
					#region ImgSource
					myImage3 = new Image();
					BitmapImage bi3 = new BitmapImage();
					bi3.BeginInit();
					bi3.UriSource = new Uri($"https://regeximg.blob.core.windows.net/users/{guid}", UriKind.RelativeOrAbsolute);
					bi3.EndInit();
					myImage3.Width = 512;
					myImage3.HorizontalAlignment = HorizontalAlignment.Center;
					myImage3.Stretch = Stretch.Uniform;
					myImage3.Source = bi3;
					#endregion

					layerList.Children.Insert(0, rec);
					layerList.Children.Insert(0, myImage3);
				}
				//else
				//{
				//	MessageBox.Show("el archivo no se reconoce");
				//}

				layerList.Children.Insert(0, newTxtBlock);
				layerList.Children.Insert(0, newStack);
			}
			else
			{
				newStack.Children.Add(btnDel);
				newStack.Children.Add(btnEdit);

				layerList.Children.Insert(0, rec);
				layerList.Children.Insert(0, newTxtBlock);
				layerList.Children.Insert(0, newStack);
			}
		}
		public static WebView2 web;
		public static async Task Upload(string filename, string connectionString, Image img, Button button, string extension)
		{
			Guid g = Guid.NewGuid();
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
				idk = extension;
			}
			// intialize BobClient 
			BlobClient blobClient = new BlobClient(
				connectionString: connectionString,
				blobContainerName: "users",
				blobName: $"{g}.{idk}");

			// upload the file

			post.fileBlob = $"{g}.{idk}";
			button.IsEnabled = false;
			await blobClient.UploadAsync(filename);
			button.IsEnabled = true;
		}

		public static async Task Download(string filename, string connectionString, string ext)
		{
			BlobClient blobClient = new BlobClient(
				connectionString: connectionString,
				blobContainerName: "users",
				blobName: $"{filename}");

			// upload the file
			Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog
			{
				FilterIndex = 1,
				RestoreDirectory = true
			};
			if (ext.Equals("mp4"))
			{
				saveFile.Filter = "Video|*.mp4";
			}else if(ext.Equals("jpg"))
			{
				saveFile.Filter = "Pictures|*.jpg;*.png";
			}
			else
			{
				saveFile.Filter = "Files|*.*";
			}
			bool? result = saveFile.ShowDialog();
			if (result == true)
			{
				await blobClient.DownloadToAsync(saveFile.FileName+"."+ext);
				MessageBox.Show("El archivo se ha descargado");
			}
		}
	}
}