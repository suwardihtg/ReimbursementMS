using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Reimburse
    {
        [Key]
        public int Id { get; set; }
        public Status Status { get; set; }
        public DateTime? Submitted { get; set; }
        public string Approver { get; set; }
        public string Purpose { get; set; }
        public string CommentManager { get; set; }
        public string CommentFinance { get; set; }
        public float? TotalPayment { get; set; }

        [JsonIgnore]
        public virtual Employee Employees { get; set; }
        public string EmployeeId { get; set; }

        [JsonIgnore]
        public virtual ICollection<ReimburseHistory> ReimburseHistories { get; set; }

        [JsonIgnore]
        public virtual ICollection<RequestForm> RequestForms { get; set; }
    }
    
    public enum Status
    {
        Draft,
        Posted,
        Approved,
        Rejected,
        Canceled,
        ApprovedByManager,
        ApprovedByFinance,
        RejectedByManager,
        RejectedByFinance
    }
}
