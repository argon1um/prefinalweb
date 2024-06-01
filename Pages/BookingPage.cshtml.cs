using ah4cClientApp.Services;
using AHRestAPI.Models;
using AnimalHouseRestAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace ah4cClientApp.Pages
{
    public class BookingPageModel : PageModel
    {
        private static JsonSerializerSettings mainsettings = new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
        public static string address = "http://localhost:8081";
        public static List<Room> rooms = new List<Room>();
        public static int selectedRoomid;
        public DateOnly admissionDate;
        public DateOnly issueDate;
        public static DateOnly admissionDatefb;
        public static DateOnly issueDatefb;


        public void OnGet()
        {
            
            var response = new HttpClient().GetStringAsync(address + $"/rooms/checkAvailible?admDate={admissionDate}&issueDate={issueDate}").Result;
            rooms = JsonConvert.DeserializeObject<List<Room>>(response);
        }


        public void OnPost(DateOnly admissionDate, DateOnly issueDate)
        {

            if (!DateChecker.DatesCheck(admissionDate, issueDate))
            {
                var showerror = true;
                if (showerror)
                {
                    ViewData["showerror"] = "true";
                    ViewData["customerror"] = "Дата принятия не может быть позже даты выселения!";

                }
            }
            else
            {
                admissionDatefb = admissionDate;
                issueDatefb = issueDate;
                var response = new HttpClient().GetStringAsync(address + $"/rooms/checkAvailible?admDate={admissionDate.ToString("yyyy-MM-dd")}&issueDate={issueDate.ToString("yyyy-MM-dd")}").Result;
                rooms = JsonConvert.DeserializeObject<List<Room>>(response);
                
            }
        }


    }
}