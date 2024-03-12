using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TechnicalMVC.Models.Quotation
{
    public class QuotationDetailViewModel
    {
        public int Id { get; set; }

        public string SKU { get; set; }

        public string Description { get; set; }

        public int Qty { get; set; }

        public decimal Price { get; set; }

        public decimal Subtotal { get; set; }
    }
}
