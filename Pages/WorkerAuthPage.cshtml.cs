using ah4cClientApp.DTO;
using ah4cClientApp.Services;
using AHRestAPI.Models;
using AHRestAPI.ModelsDTO;
using AnimalHouseRestAPI.ModelsDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace ah4cClientApp.Pages
{

    public class WorkerAuthPageModel : PageModel
    {
        public string address = "http://localhost:8081/";
        private readonly ILogger<IndexModel> _logger;
        public static WorkerResponseDTO worker;
        AuthService auth = new AuthService();
        public IActionResult OnPost(string login, string password)
        {

            login = Request.Form["username"];
            password = Request.Form["password"];
            string result = auth.AuthCheck(login, password);
            if (result == "LoginCheckFault")
            {
                var showerror = true;
                if (showerror)
                {
                    ViewData["showerror"] = "true";
                    ViewData["customerror"] = "Логин не может быть пустым!";
                    return Page();
                }
                return RedirectToPage("./WorkerAuthPage");
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
                return RedirectToPage("./WorkerAuthPage");
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
                return RedirectToPage("./WorkerAuthPage");
            }
            else
            {


                var User = new UserAuthDTO(login, password);
                var response = new HttpClient().PostAsJsonAsync("http://localhost:8081/clients/auth", User).Result;


                if (response.IsSuccessStatusCode)
                {
                    IndexModel.check = true;
                    worker = JsonConvert.DeserializeObject<WorkerResponseDTO>(response.Content.ReadAsStringAsync().Result);
                    return RedirectToPage("WorkerCabPage");
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
                    return RedirectToPage("./WorkerAuthPage");
                }
            }
        }
    }
}

