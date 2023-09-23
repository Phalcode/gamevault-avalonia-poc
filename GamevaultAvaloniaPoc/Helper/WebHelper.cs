using GamevaultAvaloniaPoc.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GamevaultAvaloniaPoc.Helper
{
    internal class WebHelper
    {
        internal async static Task<string> GetRequest(string uri)
        {
            using (var client = new HttpClient())
            {
                var authenticationString = $"{SettingsViewModel.Instance.Username}:{SettingsViewModel.Instance.Password}";
                var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.UTF8.GetBytes(authenticationString));
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);
                var response = await client.GetAsync(SettingsViewModel.Instance.ServerUrl + "api/v1/users/me");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }
        public static async Task DownloadImageFromUrlAsync(string imageUrl, string cacheFile)
        {
            using (HttpClient client = new HttpClient())
            {
                var authenticationString = $"{SettingsViewModel.Instance.Username}:{SettingsViewModel.Instance.Password}";
                var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.UTF8.GetBytes(authenticationString));
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);
                long offset = 0;
                using (HttpResponseMessage response = await client.GetAsync(imageUrl))
                {
                    using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
                    {
                        using (FileStream fs = new FileStream(cacheFile, FileMode.Create, FileAccess.Write, FileShare.Read))
                        {
                            while (streamToReadFrom.Position < streamToReadFrom.Length)
                            {
                                var nextChunkSize = Math.Min(1024, (int)(streamToReadFrom.Length - offset));
                                var data = new byte[nextChunkSize];

                                streamToReadFrom.Read(data, 0, nextChunkSize);
                                fs.Write(data, 0, nextChunkSize);
                                offset += nextChunkSize;
                            }
                        }
                    }
                }
            }
        }
    }
}
