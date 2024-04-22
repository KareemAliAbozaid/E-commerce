namespace E_commerce.Web.Core.Models
{
    public class Product
    {
        public int Id { get; set; }
        [MaxLength(100,ErrorMessage ="Name Length can't be more than 100 chr..!")]
        public string Name { get; set; } = null!;
        public string Description { get; set; }=null!;
        public double Price { get; set; }
        public double LowerPrice { get; set; }
        public string? ImageUrl {  get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public DateTime CreatedOn { get; set; }=DateTime.Now;
        public DateTime? LastUpdatedOn { get; set; }

    }
}
