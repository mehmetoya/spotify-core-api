using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static SpotifyApi.Models.PlaylistModel;

namespace SpotifyApi.Services
{
    public class RequestService
    {
        public async Task<Playlists> GetPlayLists(string token, string user)
        {
            string url = string.Format("https://api.spotify.com/v1/users/{0}/playlists?offset=0&limit=50", user);
            Playlists playLists = await this.GetSpotifyType<Playlists>(token, url);
            return playLists;
        }

        private async Task<T> GetSpotifyType<T>(string token, string url)
        {
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = "GET";
                request.Headers.Add("Authorization", "Bearer " + token);
                request.ContentType = "application/json; charset=utf-8";
                T type = default(T);

                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(dataStream))
                        {
                            string responseFromServer = reader.ReadToEnd();
                            type = JsonConvert.DeserializeObject<T>(responseFromServer);
                        }
                    }
                }

                return type;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
