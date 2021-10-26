using System.Security.Cryptography;
using System.Text;

namespace SQLApp.resources.net
{
	class Encrypt
	{
		#region Ecrypt the password
		public static string encrypt(string filename)
		{
			using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
			{
				byte[] bytes = Encoding.ASCII.GetBytes(filename);

				byte[] buffer = md5.ComputeHash(bytes);
				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < buffer.Length; i++)
				{
					sb.Append(buffer[i].ToString("x2"));
				}
				return sb.ToString();
			}
		}
		#endregion
	}
}
