using System.ComponentModel.DataAnnotations;

namespace WebApplication2.ViewModels.ProductViewModel
{
    public class UpdateProductVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImagePath { get; set; }
    }
}
