namespace E_commerce.Web.Core.Models
{
	public class Category
	{
        public int Id { get; set; }
		[MaxLength(100,ErrorMessage ="Name Length can not be more than 100 chr...!")]
		public string Name { get; set; } = null!;
		public int DisplayOrder {  get; set; }
		public DateTime CreatedOn { get; set; }= DateTime.Now;
		public DateTime? LastUpdatedOn { get; set;}
    }
}
