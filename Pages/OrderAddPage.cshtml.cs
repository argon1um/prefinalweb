using ah4cClientApp.DTO;
using AHRestAPI.Models;
using AnimalHouseRestAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ah4cClientApp.Pages
{
    public class OrderAddPageModel : PageModel
    {
        public string[] counters;
        public string roomId;
        public static DateOnly admDatefq;
        public static DateOnly issueDatefq;

        public string animalType;
        public List<Service> servicesList;
        public List<Animaltype> animalTypes;
        private static JsonSerializerSettings mainsettings = new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
        public static string address = "http://localhost:8081/";

        public class AdditionalService
        {
            public string Description { get; set; }
            public decimal Price { get; set; }
            public bool IsPerDay { get; set; }
            public int Quantity { get; set; }
        }

        public class BookingViewModel
        {
            public List<AdditionalService> Services { get; set; }
            public decimal TotalCost { get; set; }

            public BookingViewModel()
            {
                Services = new List<AdditionalService>
            {
                new AdditionalService { Description = "Проживание в номере гостиницы без оказания доп услуг", Price = 200, IsPerDay = true },
                new AdditionalService { Description = "Проживание в номере гостинцы с еждедневными прогулками с питомцем", Price =350, IsPerDay = true },
                new AdditionalService { Description = "Постоянное медицинское наблюдение за питомцем", Price = 150, IsPerDay = true},
                new AdditionalService { Description = "Фотосессия для вашего питомца", Price = 500, IsPerDay = false }
            };
            }
        }

        [BindProperty]
        public BookingViewModel Booking { get; set; }

        private decimal CalculateTotalCost()
        {
            decimal total = 0; // Базовая стоимость проживания
            foreach (var service in Booking.Services)
            {
                total += service.Quantity * service.Price;
            }
            return total;
        }

        public IActionResult OnPostCalculate()
        {
            Booking.TotalCost = CalculateTotalCost();
            return new JsonResult(new { success = true, totalCost = Booking.TotalCost });
        }

        public IActionResult OnPost(int roomid)
        {
            
            Booking.TotalCost = CalculateTotalCost();
            string serv1count = Request.Form["addservice_0"];
            string serv2count = Request.Form["addservice_1"];
            string serv3count = Request.Form["addservice_2"];
            string serv4count = Request.Form["addservice_3"];
            string email = Request.Form["email"];
            string strClientName = Request.Form["clientName"];
            string strClientPhone = Request.Form["clientPhone"];
            string strRoomid = Request.Form["roomId"];
            string strIssueDate = Request.Form["issueDate"];
            string strAdmDate = Request.Form["admissionDate"];
            string animalType = Request.Form["animaltypes"];
            string animalBreed = Request.Form["animalBreed"];
            string animalName = Request.Form["animalName"];
            string totalPrice = Request.Form["totalPrice"];
            string animalAge = Request.Form["animalAge"];
            string animalWeight = Request.Form["animalWeight"];
            string animalHeight = Request.Form["animalHeight"];
            string acceptrules = Request.Form["acceptrules"];
            string gen = Request.Form["gens"];
            if (string.IsNullOrEmpty(strClientName) || string.IsNullOrEmpty(strClientPhone) || string.IsNullOrEmpty(strRoomid) || string.IsNullOrEmpty(strAdmDate) || string.IsNullOrEmpty(strIssueDate) && string.IsNullOrEmpty(animalType) || string.IsNullOrEmpty(animalBreed)
                || string.IsNullOrEmpty(animalName) || string.IsNullOrEmpty(animalAge) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(animalWeight) || string.IsNullOrEmpty(animalHeight) || string.IsNullOrEmpty(gen))
            {
                return new JsonResult(new { success = false, message = "Заполните все поля" });
            }
            else if (!decimal.TryParse(strClientPhone, out decimal clientphone))
            {
                return new JsonResult(new { success = false, message = "Номер телефона введён некорректно" });
            }
            else if (!email.Contains("@"))
            { 
                
                return new JsonResult(new { success = false, message = "Адрес электронной почты введён некорректно" });
            }
            else
            {
                if (acceptrules == null)
                {
                    return new JsonResult(new { success = false, message = "Вы не согласились с обработкой персональных данных" });
                }
                else
                {
                    if (DateOnly.Parse(strAdmDate) > DateOnly.Parse(strIssueDate))
                    {
                        return new JsonResult(new { success = false, message = "Дата принятия не может быть позже даты выселения" });
                    }
                    decimal ttlprice = decimal.Parse(totalPrice.Replace('.', ','));

                    OrderAddDTO order = new OrderAddDTO
                    {
                        orderNoteId = 0,
                        orderId = 0,
                        admDate = DateOnly.Parse(strAdmDate),
                        issueDate = DateOnly.Parse(strIssueDate),
                        clientPhone = decimal.Parse(strClientPhone),
                        clientName = strClientName,
                        roomId = roomid,
                        clientEmail = email,
                        animalType = animalType,
                        animalName = animalName,
                        animalAge = int.Parse(animalAge),
                        animalWeight = int.Parse(animalWeight) / 1000,
                        animalHeight = int.Parse(animalHeight) % 100,
                        animalBreed = animalBreed,
                        Totalprice = ttlprice,
                        animalGen = gen
                    };
                    var response = new HttpClient().PostAsJsonAsync(address + "orders/addneworder", order).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return new JsonResult(new { success = true, message = "Заявка добавлена" });
                    }
                    else
                    {
                        return new JsonResult(new { success = false, message = "Произошла ошибка при добавлении заявки" });
                    }
                }
            }
        }

        public async Task<IActionResult> OnGet()
        {
            if (BookingPageModel.issueDatefb == DateOnly.MinValue|| BookingPageModel.admissionDatefb == DateOnly.MinValue)
            {
                return Redirect("BookingPage");
            }
            admDatefq = BookingPageModel.admissionDatefb;
            issueDatefq = BookingPageModel.issueDatefb;
            Booking = new BookingViewModel();
            var typesResponse = await new HttpClient().GetAsync(address + "getAnimalTypes");
            roomId = Request.Query["roomId"];
            if (typesResponse.IsSuccessStatusCode)
            {
                var jsonString = await typesResponse.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<Animaltype>>(jsonString, mainsettings);
                ViewData["AnimalTypes"] = data;
            }
            return Page();
        }
    }
}
