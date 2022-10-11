using System;

namespace API.ViewModel
{
    public class ReimburseVM
    {
        public int Id { get; set; }
        public DateTime? Submitted { get; set; }
        public int Status { get; set; }
        public string Approver { get; set; }
        public string Description { get; set; }
        public string CommentManager { get; set; }
        public string CommentFinace { get; set; }
        public string Purpose { get; set; }
        public float? Total { get; set; }

        public string EmployeeId { get; set; }
    }
}
