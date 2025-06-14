namespace AdminDashboardApi.Auth
{
    public class JwtSettings
    {
        public string SecretKey { get; set; } = null!;
        public string Issuer { get; set; } = "AdminDashboard";
        public string Audience { get; set; } = "AdminDashboardClient";
        public int ExpirationMinutes { get; set; } = 60;
    }
}
