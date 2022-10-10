using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Employee
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public string ManagerId { get; set; }

        [JsonIgnore]
        public virtual Job Jobs { get; set; }
        public int? JobId { get; set; }

        [JsonIgnore]
        public virtual Department Departments { get; set; }
        public int? DepartmentId { get; set; }

        [JsonIgnore]
        public virtual Account Accounts { get; set; }

        [JsonIgnore]
        public virtual ICollection<Reimburse> Reimburses { get; set; }

    }
    public enum Gender
    {
        Male,
        Female
    }
}
