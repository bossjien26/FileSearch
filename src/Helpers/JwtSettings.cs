namespace Helpers
{
    public class JwtSettings
    {
        public string Secret { get; set; }

        public string Issuer { get; set; }

        public string ValidAudience { get; set; }
    }
}