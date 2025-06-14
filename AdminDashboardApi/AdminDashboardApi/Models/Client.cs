namespace AdminDashboardApi.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public decimal BalanceT { get; set; }
        public string Tags { get; set; } = "";
    }

}
