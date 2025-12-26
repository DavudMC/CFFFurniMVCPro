using System.ComponentModel.DataAnnotations;

namespace WebApplication2.ViewModels.ProductViewModel
{
    public class CreateProductVm 
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public IFormFile Image { get; set; }        

    }
}
