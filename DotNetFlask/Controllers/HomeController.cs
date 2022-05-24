using DotNetFlask.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace DotNetFlask.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _config=config;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(HomeModel model)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_config["AppSettings:WebAPIPath"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("mutipart/form-data"));
            client.Timeout = Timeout.InfiniteTimeSpan;
            var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>(_config["AppSettings:Param"], Convert.ToString(model.Name)), });
            var resultapi = client.PostAsync(_config["AppSettings:MethodName"], content).Result;

            HomeModel home = new HomeModel();
            home.OutPut = "Not Found";
            if (resultapi.IsSuccessStatusCode)
            {
                var outputData = resultapi.Content.ReadAsStringAsync().Result;
                var jsonPath = outputData.ToString().Trim().Replace("JSON path:", "");
                home.OutPut = jsonPath;
            }

            return View("out", home);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}