using ah4cClientApp.DTO;
using AHRestAPI.ModelsDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ah4cClientApp.Pages
{
    public class WorkerCabPageModel : PageModel
    {
        public WorkerResponseDTO user = WorkerAuthPageModel.worker;
        public List<OrderGetDTO> Orders;

        public IActionResult OnGet()
        {

            if (user != null)
            {
                Orders = new HttpClient().GetFromJsonAsync<List<OrderGetDTO>>("http://localhost:8081/orders/orderslist").Result;
                return Page();
            }
            else
            {
                return BadRequest();
            }


        }

        public IActionResult OnPost()
        {
            ah4cClientApp.Pages.IndexModel.check = false;
            return Redirect("/index");
        }
    }
}
