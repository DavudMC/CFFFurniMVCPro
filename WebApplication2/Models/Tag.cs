using WebApplication2.Models.Common;

namespace WebApplication2.Models
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<BlogTag> BlogTags { get; set; }
    }
}
