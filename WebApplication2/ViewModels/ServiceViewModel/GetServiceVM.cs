namespace WebApplication2.ViewModels.ServiceViewModel
{
    public class GetServiceVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
        public List<string> EmployeeNames { get; set; }
    }
}
