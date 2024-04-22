namespace E_commerce.Web.Core.ViewModel
{
    public class CategoryViewModel
    {
        public int Id { get; set; }      
        public string Name { get; set; } = null!;
        public int DisplayOrder { get; set; }
        public DateTime CreatedOn { get; set; } 
        public DateTime? LastUpdatedOn { get; set; }
    }
}
