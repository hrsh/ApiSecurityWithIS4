using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebClient.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly HttpClient _client;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("identity_api_client");
            _client.BaseAddress = new Uri("https://localhost:5002");
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("secret")]
        public async Task<IActionResult> Secret()
        {
            var r = await _client.GetAsync("api/v1/product/secret");
            //if (r.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            //    return View("Login");

            var t = await r.Content.ReadAsStringAsync();
            
            return View(t);
        }
    }
}
