using System.Windows;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using System;
using Azure.Storage.Blobs;
using SQLApp.resources.lang;
using SQLApp.resources.post;

namespace SQLApp
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    base.OnStartup(e);
        //    IE11WebBrowserRegisitry.ChangeInternetExplorerVersion();
        //}

        //protected override void OnExit(ExitEventArgs e)
        //{
        //    base.OnExit(e);
        //    IE11WebBrowserRegisitry.RemoveRegistry();
        //}
        private void Application_Startup(object sender, StartupEventArgs e)
		{
			checkIcons();
			MainWindow mw = new MainWindow(ReadSetting("lang"));
			mw.Show();
		}
		#region This only download the images (example: edit.png)
		static void checkIcons()
        {
            List<string> files = new List<string>();
            files.Add("edit.png");
            files.Add("delete.png");
            files.Add("download.png");
            var filters = new String[] { "png" };

            try
            {
                string[] s = GetFilesFrom(Directory.GetCurrentDirectory() + @"\icons", filters, false);
                for (int i = 0; i < files.Count; i++)
                {
                    if (files.Contains(s[i]) == false)
                    {
                        Download($"{files[i]}", Directory.GetCurrentDirectory() + $@"\icons\{files[i]}");
                    }
                }
            }
            catch
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\icons");
                for (int i = 0; i < files.Count; i++)
                {
                    Download($"{files[i]}", Directory.GetCurrentDirectory() + $@"\icons\{files[i]}");
                }
            }
        }
        public static String[] GetFilesFrom(String searchFolder, String[] filters, bool isRecursive)
        {
            List<String> filesFound = new List<String>();
            var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (var filter in filters)
            {
                filesFound.AddRange(Directory.GetFiles(searchFolder, String.Format("*.{0}", filter), searchOption));
            }
            return filesFound.ToArray();
        }
        static void Download(string filename, string path)
        {
            BlobClient blobClient = new BlobClient(
                connectionString: sql.ConnectionString,
                blobContainerName: "icons",
                blobName: $"{filename}");


            blobClient.DownloadTo(path);
        }
        #endregion
        #region ReadSettings: lang, email & password
        public static string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "Not Found";
                return result;
            }
            catch (ConfigurationErrorsException)
            {
                MessageBox.Show("Error");
                return "False";
            }
        }

        public static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                MessageBox.Show("Error writing app settings");
            }
        }
        #endregion
    }
}
