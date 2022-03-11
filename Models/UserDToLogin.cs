namespace AppBackEnd.Models
{
    public class UserDToLogin
    {
        //TO Do add validation
        public string Username { get; set; } = string.Empty;
        public string PasswordHashed { get; set; } = string.Empty; 
    }
}