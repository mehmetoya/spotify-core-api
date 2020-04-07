using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpotifyApi.Services;
using static SpotifyApi.Models.PlaylistModel;

namespace SpotifyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpotifyController : ControllerBase
    {
        private TokenService _tokenService;
        private RequestService _requestService;

        public SpotifyController()
        {
            _tokenService = new TokenService();
            _requestService = new RequestService();

        }

        [HttpGet("GetPlaylist/{userId}")]
        public Task<Playlists> GetPlaylist(string userId)
        {   
            var token = _tokenService.GetAccessToken();
            var request = _requestService.GetPlayLists(token.Result.ToString(), userId);

            return request;


        }
    }
}
