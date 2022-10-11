using System;

namespace API.ViewModel
{
    public class ReimburseManagerVM
    {
        public int Id { get; set; }
        public DateTime? DateTime { get; set; }
        public float? Total { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Purpose { get; set; }
    }
}
