using ah4cClientApp.DTO;
using AHRestAPI.Models;
using AHRestAPI.ModelsDTO;
using AnimalHouseRestAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json.Converters;
using System.Security.Policy;

namespace ah4cClientApp.Pages
{
    public class UserCabPageModel : PageModel
    {
        public ClientResponseLogin user = AuthPageModel.client;
        public List<OrderGetDTO> Orders;

        public IActionResult OnGet()
        {
            if (user != null)
            {
                Orders = new HttpClient().GetFromJsonAsync<List<OrderGetDTO>>("http://localhost:8081/orders/orderslist").Result.Where(x => x.ClientPhone == user.ClientPhone).ToList();
                return Page();
            }
            else
            {
                return BadRequest();
            }
        }

        public IActionResult OnGetCreateDoc(int id)
        {
            var stream = new HttpClient().GetStreamAsync($"http://localhost:8081/orders/GenerateDoc?id={id}").Result;
            return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { FileDownloadName = "Квитанция.xlsx" };
        }

        public IActionResult OnGetGenerateAgreement(int id)
        {
            var stream = new HttpClient().GetStreamAsync($"http://localhost:8081/orders/GenerateAgreement?id={id}").Result;
            return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document" ) { FileDownloadName = "agreement.docx" };
        }

        public IActionResult OnPost()
        {
            AuthPageModel.client = null;
            WorkerAuthPageModel.worker = null;
            ah4cClientApp.Pages.IndexModel.check = false;
            return Redirect("/index");
        }
    }
}