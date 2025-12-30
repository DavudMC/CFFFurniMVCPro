namespace WebApplication2.ViewModels.EmployeeViewModel
{
    public class CreateEmployeeVm
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public List<int> ServiceIds { get; set; }
    }
}
