namespace AppBackEnd.Models
{
    public class OfferDtoAdd
    {
        public int BookId { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
    }
}