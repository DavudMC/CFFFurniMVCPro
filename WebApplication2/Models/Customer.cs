using WebApplication2.Models.Common;

namespace WebApplication2.Models
{
    public class Customer : BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }


    }
}
