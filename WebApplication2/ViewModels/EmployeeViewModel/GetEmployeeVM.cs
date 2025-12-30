namespace WebApplication2.ViewModels.EmployeeViewModel
{
    public class GetEmployeeVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
        public List<string> ServiceNames { get; set; }
    }
}
