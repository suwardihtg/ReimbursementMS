using System;

namespace API.ViewModel
{
    public class ReqFormVM
    {
        public int Id { get; set; }
        public DateTime? Receipt_Date { get; set; }
        public DateTime? Start_Date { get; set; }
        public DateTime? End_Date { get; set; }
        public int Category { get; set; }
        public string Type { get; set; }
        public string Payee { get; set; }
        public string Description { get; set; }
        public float? Total { get; set; }
        public int ReimburseId { get; set; }
        public string Attachments { get; set; }
    }
}
