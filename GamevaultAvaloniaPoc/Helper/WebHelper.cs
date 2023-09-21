using GamevaultAvaloniaPoc.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GamevaultAvaloniaPoc.Helper
{
	internal class WebHelper
	{
		internal static string GetRequest(string uri)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
			request.UserAgent = $"GameVaultAvaloniaPoc";
			request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			var authenticationString = $"{SettingsViewModel.Instance.Username}:{SettingsViewModel.Instance.Password}";
			var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.UTF8.GetBytes(authenticationString));
			request.Headers.Add("Authorization", "Basic " + base64EncodedAuthenticationString);
			using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
			using (Stream stream = response.GetResponseStream())
			using (StreamReader reader = new StreamReader(stream))
			{
				return reader.ReadToEnd();
			}
		}
		internal static string Put(string uri, string payload, bool returnBody = false)
		{
			var request = (HttpWebRequest)WebRequest.Create(uri);
			request.UserAgent = $"GameVaultAvaloniaPoc";
			request.Method = "PUT";
			request.ContentType = "application/json";
			if (payload != null)
			{
				var authenticationString = $"{SettingsViewModel.Instance.Username}:{SettingsViewModel.Instance.Password}";
				var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.UTF8.GetBytes(authenticationString));
				request.Headers.Add("Authorization", "Basic " + base64EncodedAuthenticationString);
				request.ContentLength = System.Text.UTF8Encoding.UTF8.GetByteCount(payload);
				Stream dataStream = request.GetRequestStream();
				using (StreamWriter sr = new StreamWriter(dataStream))
				{
					sr.Write(payload);
				}
				dataStream.Close();
			}
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			if (returnBody)
			{
				using (StreamReader responseStreamReader = new StreamReader(response.GetResponseStream()))
				{
					return responseStreamReader.ReadToEnd();
				}
			}
			else
			{
				return response.StatusCode.ToString();
			}
		}
	}
}
