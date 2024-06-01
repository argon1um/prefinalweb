namespace ah4cClientApp.DTO
{
    public class ClientResponseLogin
    {
        public int? ID { get; set; }
        public string? ClientName { get; set; }
        public decimal ClientPhone { get; set; }
        public string ClientPassword { get; set; }
        public string? ClientEmail { get; set; }
        public int? ClientCountoforders { get; set; }
    }
}
