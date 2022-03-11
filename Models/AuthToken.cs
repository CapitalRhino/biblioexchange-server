namespace AppBackEnd.Models
{
    public class AuthToken
    {
        public string Username { get; set; }
        public string jti { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public int exp { get; set; }
    }
}