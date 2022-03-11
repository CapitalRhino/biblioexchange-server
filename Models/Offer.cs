using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AppBackEnd.Models
{
public class Offer
    {
        [KeyAttribute]
        public int Id { get; set; }
        
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }
        public int BookId { get; set; }
        [ForeignKey("BiblioUserId")]
        public virtual BiblioUser BiblioUser { get; set; }
        public string BiblioUserId { get; set; } 
        public string Description { get; set; }
        [Required]
        public DateTime UploadTime { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
    }
}