using ah4cClientApp.DTO;
using AnimalHouseRestAPI.Models;
using AnimalHouseRestAPI.ModelsDTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace ah4cClientApp.Pages
{
    public class RegistrationPageModel : PageModel
    {
        private static JsonSerializerSettings mainsettings = new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore};
        public static string address = "http://localhost:8081/";
        public IActionResult OnPost()
        {
            var showerror = false;
            string clientname = Request.Form["clientname"];
            string clientpassword = Request.Form["clientpassword"];
            string clientemail = Request.Form["clientemail"];
            string clientphone = Request.Form["clientphone"];
            if (string.IsNullOrEmpty(clientname) || string.IsNullOrEmpty(clientpassword) || string.IsNullOrEmpty(clientemail) || string.IsNullOrEmpty(clientphone))
            {
                showerror = true;
                if (showerror)
                {
                    ViewData["showerror"] = "true";
                    ViewData["customerror"] = "Заполните все поля";
                    return Page();
                }
                return BadRequest();
            }
            else
            {
                if(decimal.TryParse(clientphone, out decimal result))
                {
                    ClientResponseLogin newclient = new ClientResponseLogin();
                    newclient.ClientName = clientname;
                    newclient.ClientPassword = clientpassword;
                    newclient.ClientEmail = clientemail;
                    newclient.ClientCountoforders = 0;
                    newclient.ClientPhone = result;

                    var response = new HttpClient().PostAsJsonAsync(address + "clients/signUp", newclient).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        ViewData["showerror"] = "true";
                        ViewData["customerror"] = "Успешная регистрация";
                        return Page();
                    }
                    else
                    {
                        showerror = true;
                        if (showerror)
                        {
                            ViewData["showerror"] = "true";
                            ViewData["customerror"] = "Логин занят";
                            return Page();
                        }
                        return NotFound();
                    }
                }
                else
                {
                    showerror = true;
                    if (showerror)
                    {
                        ViewData["showerror"] = "true";
                        ViewData["customerror"] = "Номер телефона введен не корректно";
                        return Page();
                    }
                    return NotFound();
                }
            }
        }
    }
}
