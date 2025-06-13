namespace AdminDashboardApi.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;
        public decimal AmountT { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
