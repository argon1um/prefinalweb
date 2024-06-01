using Microsoft.AspNetCore.Mvc;

namespace ah4cClientApp.Services
{
    public class DateChecker
    {
        public static bool DatesCheck(DateOnly admissionDate, DateOnly issueDate)
        {
            if ((admissionDate) > (issueDate))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
