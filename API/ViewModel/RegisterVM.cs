using System;

namespace API.ViewModel
{
    public class RegisterVM
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ManagerId { get; set; }
        public int DepartmentId { get; set; }
        public int JobId { get; set; }
    }
}
