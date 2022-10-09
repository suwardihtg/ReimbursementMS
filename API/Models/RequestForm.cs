using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class RequestForm
    {
        [Key]
        public int Id { get; set; }
        public DateTime? Start_Date { get; set; }
        public DateTime? End_Date { get; set; }
        public Categori? Category { get; set; }
        public float? Total { get; set; }
        public int Attachments { get; set; }

        [JsonIgnore]
        public virtual Reimburse Reimburses { get; set; }
        public int ReimburseId { get; set; }
    }

    public enum Categori
    {
        Parkir,
        Tranportation
    }
}
