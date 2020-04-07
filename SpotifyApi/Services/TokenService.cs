using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SpotifyApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyApi.Services
{
    public class TokenService
    {
        public async Task<string> GetAccessToken()
        {
            string spotifyClient = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("SpotifyApiConfig")["SpotifyClientId"];
            string spotifySecret = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("SpotifyApiConfig")["SpotifyClientSecret"];
            SpotifyTokenModel token = new SpotifyTokenModel();
            string postString = string.Format("grant_type=client_credentials");

            byte[] byteArray = Encoding.UTF8.GetBytes(postString);
            string url = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("SpotifyApiConfig")["SpotifyApiTokenUrl"];

            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            var authHeader = Convert.ToBase64String(Encoding.Default.GetBytes($"{spotifyClient}:{spotifySecret}"));
            request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + authHeader);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            string responseFromServer = reader.ReadToEnd();

                            token = JsonConvert.DeserializeObject<SpotifyTokenModel>(responseFromServer);
                        }
                    }
                }
            }

            return token.access_token;
        }
    }
}
