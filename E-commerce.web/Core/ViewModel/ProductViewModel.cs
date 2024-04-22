namespace E_commerce.Web.Core.ViewModel
{
    public class ProductViewModel
    {
        public int Id { get; set; }      
        public string Name { get; set; } = null!;
        public string Category {  get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price { get; set; }
        public double LowerPrice { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime PublishingDate { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
