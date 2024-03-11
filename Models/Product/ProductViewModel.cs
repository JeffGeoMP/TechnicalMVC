namespace TechnicalMVC.Models.Product
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        public CategoryViewModel Category { get; set; }
        public decimal Price { get; set; }
    }
}
