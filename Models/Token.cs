namespace AppBackEnd.Models
{
    public class Token
    {
        public string AccessToken { get; set; }
        public virtual ICollection<string>  Roles { get; set; }
    }
}