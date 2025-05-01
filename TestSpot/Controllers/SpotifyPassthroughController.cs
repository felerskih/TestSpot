using Microsoft.AspNetCore.Mvc;
using TestSpot.Services;

namespace TestSpot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpotifyPassthroughController : ControllerBase
    {

        private readonly ILogger<SpotifyPassthroughController> _logger;
        private readonly ISpotifyClient _client;
        //you need to organize this variable instead of having it in two places
        private readonly string ClientId = "a257496c12174eb4a05b28fea0459d1d";
        public SpotifyPassthroughController(ILogger<SpotifyPassthroughController> logger, ISpotifyClient client)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet()]
        [Route("GetTopItems")]
        public async Task<string> GetTopItems()
        {
            await _client.GetToken();

            return await _client.GetTopItems();
        }

        [HttpGet()]
        [Route("GetArtist")]
        public async Task<string> GetArtist()
        {
            await _client.GetToken();

            return await _client.GetArtist("");
        }

        //Todo: put in Healthcheck Controller
        [HttpGet()]
        [Route("GetToken")]
        public async Task<string> GetToken()
        {
            return await _client.GetToken();
        }

        [HttpGet()]
        [Route("Auth")]
        //test this from console with cors
        public async Task<IActionResult> Auth()
        {
            //Make this gooder
            var redUrl = $"https://accounts.spotify.com/authorize?respone_type=code&client_id={ClientId}&scope=user-top-read&redirect_uri=http://127.0.0.1:44318/Callback";
            var red = new RedirectResult(redUrl);
            return red;
        }


        [HttpGet()]
        [Route("Callback")]
        public async Task<string> Callback()
        {
            return "";
        }
    }
}
