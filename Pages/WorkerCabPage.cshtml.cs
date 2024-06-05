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
                Orders = new HttpClient().GetFromJsonAsync<List<OrderGetDTO>>("http://localhost:8081/orders/payedorders").Result;
                
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
            AuthPageModel.client = null;
            WorkerAuthPageModel.worker = null;
            return Redirect("/index");
        }
        public IActionResult OnGetApply(int id)
        {
            var response = new HttpClient().PostAsJsonAsync(string.Format("http://localhost:8081/orders/statuschange?orderid={0}", id), id).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/WorkerCabPage");
            }
            else
            {
                return RedirectToPage("/WorkerCabPage");
            }
        }
    }
}
