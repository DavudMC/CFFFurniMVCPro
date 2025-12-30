using System.ComponentModel.DataAnnotations;
using WebApplication2.Models;

namespace WebApplication2.ViewModels.BlogViewModel
{
    public class CreateBlogVm
    {
        [Required]
        public string Title { get; set; }
        [Required]
        [MinLength(5)]
        public string Text { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? PostedDate { get; set; }

        [Required]
        public IFormFile Image { get; set; }
        public List<int> TagIds { get; set; }
        
    }
}
