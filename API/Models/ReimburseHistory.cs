using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class ReimburseHistory
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }

        [JsonIgnore]
        public virtual Reimburse Reimburses { get; set; }
        public int ReimburseId { get; set; }
    }
}
