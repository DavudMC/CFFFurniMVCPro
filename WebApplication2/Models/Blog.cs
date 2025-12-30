using System.ComponentModel.DataAnnotations;
using WebApplication2.Models.Common;

namespace WebApplication2.Models
{
    public class Blog : BaseEntity
    {
        [Required]
        public string Title { get; set; }
        [Required]
        [MinLength(5)]
        public string Text { get; set; }
        public Employee? Employee { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? PostedDate { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<BlogTag> BlogTags { get; set; }


    }
}
