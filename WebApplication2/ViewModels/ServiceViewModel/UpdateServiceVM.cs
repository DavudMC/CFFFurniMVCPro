namespace WebApplication2.ViewModels.ServiceViewModel
{
    public class UpdateServiceVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
        public List<int> EmployeeIds { get; set; }
        public string? ImageName { get; set; }
    }
}
