namespace Pokemon_Review_API.Helper
{
    public class JwtOption
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Lifetime { get; set; }
        public string SigningKey { get; set; }

    }
}
