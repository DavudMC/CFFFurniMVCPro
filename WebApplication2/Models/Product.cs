using System.ComponentModel.DataAnnotations;
using WebApplication2.Models.Common;

namespace WebApplication2.Models
{
    public class Product : BaseEntity
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string ImageName { get; set; }
        public string? ImageUrl { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
