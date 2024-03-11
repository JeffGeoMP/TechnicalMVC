using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechnicalMVC.Models.Entities
{
    public class QuotationDetail : AttributesLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        public Quotation Quotation { get; set; }
        public int QuotationId { get; set; }

        [Required]
        public Product Product { get; set; }
        public int ProductId { get; set; }

        [Required]
        public int Qty { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal Subtotal { get; set; }
    }
}
