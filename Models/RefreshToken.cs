using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AppBackEnd.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("BiblioUser")]
        public string BiblioUserId { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public DateTime Expires { get; set; }
        public DateTime? Revoke { get; set; }
        [NotMappedAttribute]
        public bool IsActive { 
            get {
                var now = DateTime.Now;
                return (now>Expires&&now>Revoke);
        } }
        
    }
}