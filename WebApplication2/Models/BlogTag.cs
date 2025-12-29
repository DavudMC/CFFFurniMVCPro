using WebApplication2.Models.Common;

namespace WebApplication2.Models
{
    public class BlogTag : BaseEntity
    {
        public Blog Blog { get; set; }
        public int BlogId { get; set; }
        public Tag Tag { get; set; }
        public int TagId { get; set; }

    }
}
