using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechnicalMVC.Models.Entities
{
    public class Product : AttributesLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        public string SKU { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public FamilyProducts FamilyProduct { get; set; }

        [Required]
        public int FamilyProductId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal Price { get; set; }
    }
}
