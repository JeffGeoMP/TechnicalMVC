using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechnicalMVC.Models.Entities
{
    public class Quotation : AttributesLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        public string Serie { get; set; }

        [Required]
        public Client Client { get; set; }

        public int ClientId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal Total { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Discount { get; set; }
    }
}
