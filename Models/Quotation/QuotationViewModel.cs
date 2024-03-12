namespace TechnicalMVC.Models.Quotation
{
    public class QuotationViewModel
    {
        public int Id { get; set; }
        public string Serie { get; set; }
        public ClientViewModel Client { get; set; }
        public decimal Total { get; set; }
        public string CreatedAt { get; set; }

        public List<QuotationDetailViewModel> Detail { get; set; }
    }
}
