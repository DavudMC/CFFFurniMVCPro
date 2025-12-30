using WebApplication2.Models.Common;

namespace WebApplication2.Models
{
    public class Service : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public string? ImagePath { get; set; }
        public ICollection<EmployeeService> EmployeeServices { get; set; }
    }
}
