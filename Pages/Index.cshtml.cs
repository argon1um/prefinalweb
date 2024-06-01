using ah4cClientApp.Services;
using AnimalHouseRestAPI.Models;
using AnimalHouseRestAPI.ModelsDTO;
using Azure.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Web;

namespace ah4cClientApp.Pages
{
    public class IndexModel : PageModel
    {
        
        public static bool check = false;
        public static List<Service> services = new List<Service>();
        AuthService auth = new AuthService();
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public void OnGet()
        {
            var response = new HttpClient().GetStringAsync("http://localhost:8081/services/alllist").Result;
            services = JsonConvert.DeserializeObject<List<Service>>(response);
        }

    }
}