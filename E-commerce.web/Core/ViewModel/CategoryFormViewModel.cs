using System.ComponentModel.DataAnnotations;

namespace E_commerce.Web.Core.ViewModel
{
    public class CategoryFormViewModel
    {
        public int Id { get; set; }
        [MaxLength(100, ErrorMessage = "Name Length can not be more than 100 chr...!")]
        public string Name { get; set; } = null!;
        public int DisplayOrder { get; set; }
    }
}
