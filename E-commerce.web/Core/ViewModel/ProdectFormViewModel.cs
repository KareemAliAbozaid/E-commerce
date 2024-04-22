using E_commerce.Web.Core.Consts;
using ExpressiveAnnotations.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace E_commerce.Web.Core.ViewModel
{
    public class ProdectFormViewModel
    {
        public int Id { get; set; }
        [Remote("AllowItem", null!, AdditionalFields = "Id,CategoryId", ErrorMessage = Errors.Duplicated)]      
        public string Name { get; set; } = null!;
        [Display(Name = "Category")]
        [Remote("AllowItem", null!, AdditionalFields = "Id,Name", ErrorMessage = Errors.Duplicated)]
        public int CategoryId { get; set; }
        public IEnumerable<SelectListItem>? Categories { get; set; }
        public string Price { get; set; } = null!;
        public string? LowerPrice { get; set; }
        public IFormFile? Image { get; set; }
        [AssertThat("PublishingDate <= Today()", ErrorMessage = Errors.NotAllowFutureDates)]
        public DateTime PublishingDate {  get; set; }= DateTime.Now;
        public string? ImageUrl { get; set; }
        public string Description { get; set; } = null!;
    }
}
