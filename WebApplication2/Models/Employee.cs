using WebApplication2.Models.Common;

namespace WebApplication2.Models
{
    public class Employee : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<EmployeeService> EmployeeServices { get; set; } = [];

    }
}
