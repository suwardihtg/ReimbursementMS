using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
//using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API.Models
{
    public class Account
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [JsonIgnore]
        public virtual Employee Employees { get; set; }


    }
}
