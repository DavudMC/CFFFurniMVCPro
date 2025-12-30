using System.ComponentModel.DataAnnotations;

namespace WebApplication2.ViewModels.BlogViewModel
{
    public class UpdateBlogVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? PostedDate { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageName { get; set; }
        public List<int> TagIds { get; set; } = [];
    }
}
