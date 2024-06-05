using ah4cClientApp.DTO;
using ah4cClientApp.Pages.Shared;
using ah4cClientApp.Services;
using AHRestAPI.ModelsDTO;
using AnimalHouseRestAPI.Models;
using AnimalHouseRestAPI.ModelsDTO;
using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using NuGet.Protocol;

namespace ah4cClientApp.Pages
{
    public class AuthPageModel : PageModel
    {
        public string address = "http://localhost:8081/";
        private readonly ILogger<IndexModel> _logger;
        public static ClientResponseLogin client;
        public static WorkerResponseDTO worker;
        AuthService auth = new AuthService();


        public IActionResult OnPost(string phone, string password)
        {
            
            phone = Request.Form["username"];
            password = Request.Form["password"];
            string result = auth.AuthCheck(phone, password);
            if (result == "LoginCheckFault")
            {
                var showerror = true;
                if (showerror)
                {
                    ViewData["showerror"] = "true";
                    ViewData["customerror"] = "Логин не может быть пустым!";
                    return Page();
                }
                return RedirectToPage("./AuthPage");
            }
            else if (result == "PasswordCheckFault")
            {
                var showerror2 = true;
                if (showerror2)
                {
                    ViewData["showerror"] = "true";
                    ViewData["customerror"] = "Пароль не может быть пустым!";
                    return Page();
                }
                return RedirectToPage("./AuthPage");
            }
            else if (result == "CheckFault")
            {
                var showerror3 = true;
                if (showerror3)
                {
                    ViewData["showerror"] = "true";
                    ViewData["customerror"] = "Заполните все поля!";
                    return Page();
                }
                return RedirectToPage("./AuthPage");
            }
            else
            {


                var User = new UserAuthDTO(phone, password);
                var response = new HttpClient().PostAsJsonAsync("http://localhost:8081/clients/auth", User).Result;
                

                if (response.IsSuccessStatusCode)
                {
                    IndexModel.check = true;
                    client = JsonConvert.DeserializeObject<ClientResponseLogin>(response.Content.ReadAsStringAsync().Result);
                    return RedirectToPage("UserCabPage");
                }
                else
                {
                    var showerror4 = true;
                    if (showerror4)
                    {
                        ViewData["showerror"] = "true";
                        ViewData["customerror"] = "Неверный логин или пароль!";
                        return Page();
                    }
                    return RedirectToPage("./AuthPage");
                }
            }
        }
    }
}
