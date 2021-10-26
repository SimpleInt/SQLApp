using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SQLApp.resources.post.view
{
	class create
	{
		public static void Post(string txt, string guid, StackPanel layerList)
		{
			TextBlock newTxtBlock = new TextBlock
			{
				Text = txt,
				TextWrapping = TextWrapping.WrapWithOverflow,
				HorizontalAlignment = HorizontalAlignment.Center,
				Margin = new Thickness(25),
				FontSize = 24
			};
			Image myImage3 = new Image();
			Rectangle rec = new Rectangle()
			{
				Height = 2,
				Fill = Brushes.Gray,
				Margin = new Thickness(25),
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
				web.NavigateToString($@"
					{cssStyle}
						<video controls>
							<source src='https://regeximg.blob.core.windows.net/users/{guid}'  type='video/mp4'>
						</video>");
				//viewVideo view = new viewVideo(@$"https://regeximg.blob.core.windows.net/users/{guid}", txt);
				//view.Show();
			}));
			if (guid != "")
			{
				string[] file = guid.Split(".");
				if (file[file.Length - 1] == "mp4" || file[file.Length - 1] == "avi")
				{

					//newWeb.NavigateToString($@"
					//           <!DOCTYPE html>
					//           <html>
					//           <head>
					//               <meta http-equiv='Content-Type' content='text/html; charset=unicode' />
					//               <meta http-equiv='X-UA-Compatible' content='IE=9' /> 
					//               <title></title>
					//           </head>
					//           <body>
					//               <div>
					//                    <video preload='metadata' controls='true'>
					//		<source src='https://regeximg.blob.core.windows.net/users/{guid}' type='video/mp4' />
					//                   </video>
					//               </div>
					//           </body>
					//           </html>");
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
			}
		}
		public static WebView2 web;
	}
}
