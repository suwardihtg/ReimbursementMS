using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Job
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
