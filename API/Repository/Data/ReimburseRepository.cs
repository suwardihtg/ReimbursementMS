using API.Context;
using API.Models;
using API.ViewModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace API.Repository.Data
{
    public class ReimburseRepository : GeneralRepository<MyContext, Reimburse, int>
    {
        private readonly MyContext context;
        public IConfiguration _configuration;
        public ReimburseRepository(MyContext myContext, IConfiguration configuration):base(myContext)
        {
            this.context = myContext;
            this._configuration = configuration;
        }
        public int ReimburseForm(ReimburseVM reimburseVM)
        {
            Reimburse reimburse = new Reimburse();
            {
                reimburse.Approver = reimburseVM.Approver;
                //reimburse.Description = reimburseVM.Description;
                //reimburse.Total = reimburseVM.Total;
                reimburse.Submitted = (reimburseVM.Submitted == null) ? DateTime.Now : reimburseVM.Submitted;
                switch (reimburseVM.Status)
                {
                    case 0:
                        reimburse.Status = Status.Draft;
                        break;
                    case 1:
                        reimburse.Status = Status.Posted;
                        break;
                    case 2:
                        reimburse.Status = Status.Approved;
                        break;
                    case 3:
                        reimburse.Status = Status.Rejected;
                        break;
                    case 4:
                        reimburse.Status = Status.Canceled;
                        break;
                    case 5:
                        reimburse.Status = Status.ApprovedByManager;
                        break;
                    case 6:
                        reimburse.Status = Status.ApprovedByFinance;
                        break;
                    case 7:
                        reimburse.Status = Status.RejectedByManager;
                        break;
                    case 8:
                        reimburse.Status = Status.RejectedByFinance;
                        break;
                    default:
                        break;
                }
                reimburse.Id = reimburseVM.Id;
            }
            context.Reimburses.Add(reimburse);
            context.SaveChanges();
            DateTime aDate = DateTime.Now;
            ReimburseHistory reimburseHistory = new ReimburseHistory();
            {
                reimburseHistory.Date = DateTime.Now;
                reimburseHistory.Message = "Created " + aDate.ToString("dddd, dd MMMM yyyy HH:mm:ss");
                reimburseHistory.Id = reimburse.Id;
            }

            context.ReimburseHistories.Add(reimburseHistory);
            context.SaveChanges();
            return 1;
        }

        public int ReimburseFormUpdate(ReimburseVM reimburseVM, int code)
        {
            var history = "";
            switch (code)
            {
                case 1:
                    history = "Expense Submitted ";
                    break;
                case 2:
                    history = "Draft Saved ";
                    break;
                case 3:
                    history = "Rejected by your Manager ";
                    break;
                case 4:
                    history = "Accepted by your Manager ";
                    break;
                case 5:
                    history = "Rejected by Finance ";
                    break;
                case 6:
                    history = "Accepted by Finance ";
                    break;
                case 7:
                    history = "Rejected by Senior Manager ";
                    break;
                case 8:
                    history = "Accepted by Senior Manager ";
                    break;
                case 9:
                    history = "Rejected by Director ";
                    break;
                case 10:
                    history = "Accepted by Director ";
                    break;
                case 12:
                    history = "Deleted ";
                    break;
                default:
                    break;
            }

            if (code != 11)
            {
                DateTime aDate = DateTime.Now;

                ReimburseHistory reimburseHistory = new ReimburseHistory();
                {
                    reimburseHistory.Date = DateTime.Now;
                    reimburseHistory.Message = history + aDate.ToString("dddd, dd MMMM yyyy HH:mm:ss");
                    reimburseHistory.Id = reimburseVM.Id;
                }
                context.ReimburseHistories.Add(reimburseHistory);
                context.SaveChanges();
            }


            /*var data = (from a in context.Employees
                        join b in context.Reimburses on a.Id equals b.Id
                        where b.Id == reimburseVM.Id
                        select new { reimburses = b }).Single();*/

            var data = (from a in context.Employees
                        join b in context.Reimburses on a.Id equals b.EmployeeId
                        where b.Id == reimburseVM.Id
                        select new { reimburses = b }).Single();

            var reimburse = data.reimburses;
            reimburse.Approver = reimburseVM.Approver;
            reimburse.CommentManager = reimburseVM.CommentManager;
            reimburse.CommentFinance = reimburseVM.CommentFinace;
            reimburse.Submitted = (reimburseVM.Submitted == null) ? DateTime.Now : reimburseVM.Submitted;
            reimburse.Purpose = reimburseVM.Purpose;
           // reimburse.Description = reimburseVM.Description;
            //reimburse.Total = reimburseVM.Total;
            switch (reimburseVM.Status)
            {
                case 0:
                    reimburse.Status = Status.Draft;
                    break;
                case 1:
                    reimburse.Status = Status.Posted;
                    break;
                case 2:
                    reimburse.Status = Status.Approved;
                    break;
                case 3:
                    reimburse.Status = Status.Rejected;
                    break;
                case 4:
                    reimburse.Status = Status.Canceled;
                    break;
                case 5:
                    reimburse.Status = Status.ApprovedByManager;
                    break;
                case 6:
                    reimburse.Status = Status.ApprovedByFinance;
                    break;
                case 7:
                    reimburse.Status = Status.RejectedByManager;
                    break;
                case 8:
                    reimburse.Status = Status.RejectedByFinance;
                    break;
                default:
                    break;
            }
            reimburse.Id = reimburseVM.Id;
            var reimburses = reimburse;
            context.SaveChanges();
            return 1;
        }
        public int LastReimburse()
        {
            var data = (from a in context.Employees
                        join b in context.Reimburses on a.Id equals b.EmployeeId
                        select new { id = b.Id }).LastOrDefault();

            return 1;
        }

        public ReimburseIDVM ReimburseID(string email)
        {
            var data = (from a in context.Employees
                        where a.Email == email
                        join b in context.Reimburses on a.Id equals b.EmployeeId
                        select new ReimburseIDVM()
                        { Id = b.Id }).ToList().LastOrDefault();

            return data;
        }

        public IEnumerable<ReimburseVM> GetReimburse(string employeeid)
        {
            var register = from a in context.Employees
                           where a.Id == employeeid
                           join b in context.Reimburses on a.Id equals b.EmployeeId
                           where b.Status != Status.Canceled
                           select new ReimburseVM()
                           {
                               Approver = (from c in context.Employees where c.Id == a.ManagerId select c.FirstName + " " + c.LastName).Single().ToString(),
                               Submitted = b.Submitted,
                               Id = b.Id,
                               Purpose = b.Purpose,
                               CommentFinace = b.CommentFinance,
                               CommentManager = b.CommentManager,
                               Status = (int)b.Status,
                           };
            var data = register.ToList().OrderBy(issue => (issue.Status, true)); ;
            return data;
        }

        //financess
        public IEnumerable<ReimburseManagerVM> GetReimburseFinance()
        {
            var expense = from a in context.Employees
                          join b in context.Reimburses on a.Id equals b.EmployeeId
                          where b.Status == Status.ApprovedByManager
                          select new ReimburseManagerVM()
                          {
                              Status = (int)b.Status,
                              EmployeeId = b.EmployeeId,
                              Id = b.Id,
                              Name = a.FirstName + " " + a.LastName,
                              DateTime = b.Submitted,
                              Purpose = b.Purpose
                          };
            return expense.ToList();
        }
        public IEnumerable<ReimburseManagerVM> GetReimburseFinanceReject()
        {
            var expense = from a in context.Employees
                          join b in context.Reimburses on a.Id equals b.EmployeeId
                          where b.Status == Status.RejectedByFinance
                          select new ReimburseManagerVM()
                          {
                              Status = (int)b.Status,
                              EmployeeId = b.EmployeeId,
                              Id = b.Id,
                              Name = a.FirstName + " " + a.LastName,
                              DateTime = b.Submitted,
                              Purpose = b.Purpose
                          };
            return expense.ToList();
        }

        //Managerrr
        public IEnumerable<ReimburseManagerVM> GetReimburseManager()
        {
            var expense = from a in context.Employees
                          join b in context.Reimburses on a.Id equals b.EmployeeId
                          where b.Status == Status.Posted
                          select new ReimburseManagerVM()
                          {
                              Status = (int)b.Status,
                              EmployeeId = b.EmployeeId,
                              Id = b.Id,
                              Name = a.FirstName + " " + a.LastName,
                              DateTime = b.Submitted,
                              Purpose = b.Purpose
                          };
            return expense.ToList();
        }
        public IEnumerable<ReimburseManagerVM> GetReimburseManagerReject()
        {
            var expense = from a in context.Employees
                          join b in context.Reimburses on a.Id equals b.EmployeeId
                          where b.Status == Status.RejectedByManager
                          select new ReimburseManagerVM()
                          {
                              Status = (int)b.Status,
                              EmployeeId = b.EmployeeId,
                              Id = b.Id,
                              Name = a.FirstName + " " + a.LastName,
                              DateTime = b.Submitted,
                              Purpose = b.Purpose
                          };
            return expense.ToList();
        }

        //<!----------------- Manager & Finances -------------------> 

        public IEnumerable<ReimburseManagerVM> GetReimbursePosted()
        {
            var expense = from a in context.Employees
                          join b in context.Reimburses on a.Id equals b.EmployeeId
                          where b.Status != Status.Draft
                          select new ReimburseManagerVM()
                          {
                              Status = (int)b.Status,
                              EmployeeId = b.EmployeeId,
                              Id = b.Id,
                              Name = a.FirstName + " " + a.LastName,
                              DateTime = b.Submitted,
                              Purpose = b.Purpose
                          };
            return expense.ToList();
        }

        //<!-------------------- Notif ------------------------> 
        public int NotifRequest(int reimburseid)
        {
            var data = (from a in context.Employees
                        join b in context.Reimburses on a.Id equals b.EmployeeId
                        where b.Id == reimburseid
                        select new { Employee = a, Expense = b }).Single();

            StringBuilder sb = new StringBuilder();

            sb.Append("<div>");
            sb.Append($"<p> Dear {data.Employee.FirstName} {data.Employee.LastName},<br> Your Reimbursement Request is being Processed <p>");
            sb.Append("<div>");
            sb.Append(" You have made a Reimbursment Request with ID ");
            sb.Append($"<h1> # {reimburseid} <h1>");
            sb.Append("<h4> Best Regards, <h4>");
            sb.Append("<h4> Reimbursement Team <h4>");

            if (data != null)
            {
                try
                {
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("suwardihutagalung28@gmail.com.com");
                    mail.To.Add(data.Employee.Email);
                    mail.Subject = $"【Reimbursment】Request Successful";
                    mail.Body = sb.ToString();
                    mail.Body = sb.ToString();
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new
                            NetworkCredential("suwardihutagalung28@gmail.com", "hutagalung97");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
                catch (Exception)
                {
                    return 2;
                }
                return 1;
            }
            return 3;
        }

        //<!----------------- Notif Finances -------------------> 
        public int NotifApproveF(int reimburseid)
        {
            var data = (from a in context.Employees
                        join b in context.Reimburses on a.Id equals b.EmployeeId
                        where b.Id == reimburseid
                        select new { Employee = a, Expense = b }).Single();

            StringBuilder sb = new StringBuilder();

            sb.Append($"<p> Dear {data.Employee.FirstName} {data.Employee.LastName},<br/> Your Reimbursement Have been <b>accepted</b> by Finances ");
            sb.Append($"<br/> Your Reimbursment Request ID # {reimburseid} <p/>");
            sb.Append($"<p> Please check your Account for more detail <p/>");
            sb.Append("<p> Best Regards,");
            sb.Append("<br/> Finance Department <p/>");


            if (data != null)
            {
                try
                {
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("suwardihutagalung28@gmail.com");
                    mail.To.Add(data.Employee.Email);
                    mail.Subject = $"【Reimbursment】Finances Approve";
                    mail.Body = sb.ToString();
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new
                            NetworkCredential("suwardihutagalung@gmail.com", "hutagalung97");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
                catch (Exception)
                {
                    return 2;
                }
                return 1;
            }
            return 3;
        }

        public int NotifRejectF(int reimburseid)
        {
            var data = (from a in context.Employees
                        join b in context.Reimburses on a.Id equals b.EmployeeId
                        where b.Id == reimburseid
                        select new { Employee = a, Expense = b }).Single();

            StringBuilder sb = new StringBuilder();



            sb.Append($"<p> Dear {data.Employee.FirstName} {data.Employee.LastName},<br/> Your Reimbursement Have been <b>rejected</b> by Finance");
            sb.Append($"<br/> Your Reimbursment Request ID # {reimburseid} <p/>");
            sb.Append($"<p> Additional message : {data.Expense.CommentManager}");
            sb.Append($"<br/> Please check your Account for more detail <p/>");
            sb.Append("<p> Best Regards,");
            sb.Append("<br/> Finance Department <p/>");

            if (data != null)
            {
                try
                {
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("suwardihutagalung@gmail.com");
                    mail.To.Add(data.Employee.Email);
                    mail.Subject = $"【Reimbursment】Rejected";
                    mail.Body = sb.ToString();
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new
                            NetworkCredential("suwardihutagalung@gmail.com", "hutagalung97");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
                catch (Exception)
                {
                    return 2;
                }
                return 1;
            }
            return 3;
        }

        //<!----------------- Notif Manager -------------------> 

        public int NotifApproveM(int reimburseid)
        {
            var data = (from a in context.Employees
                        join b in context.Reimburses on a.Id equals b.EmployeeId
                        where b.Id == reimburseid
                        select new { Employee = a, Expense = b }).Single();

            StringBuilder sb = new StringBuilder();

            sb.Append($"<p> Dear {data.Employee.FirstName} {data.Employee.LastName},<br/> Your Reimbursement Have been <b>accepted</b> by Manager ");
            sb.Append($"<br/> Your Reimbursment Request ID # {reimburseid} <p/>");
            sb.Append($"<p> Please check your Account for more detail <p/>");
            sb.Append("<p> Best Regards,");
            sb.Append("<br/> Manager <p/>");

            if (data != null)
            {
                try
                {
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("suwardihutagalung@gmail.com");
                    mail.To.Add(data.Employee.Email);
                    mail.Subject = $"【Reimbursment】Manager Approve";
                    mail.Body = sb.ToString();
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new
                            NetworkCredential("suwardihutagalung@gmail.com", "hutagalung97");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
                catch (Exception)
                {
                    return 2;
                }
                return 1;
            }
            return 3;
        }

        public int NotifRejectM(int reimburseid)
        {
            var data = (from a in context.Employees
                        join b in context.Reimburses on a.Id equals b.EmployeeId
                        where b.Id == reimburseid
                        select new { Employee = a, Expense = b }).Single();

            StringBuilder sb = new StringBuilder();

            sb.Append($"<p> Dear {data.Employee.FirstName} {data.Employee.LastName},<br/> Your Reimbursement Have been <b>rejected</b> by Manager ");
            sb.Append($"<br/> Your Reimbursment Request ID # {reimburseid} <p/>");
            sb.Append($"<p> Additional message : {data.Expense.CommentManager}");
            sb.Append($"<br/> Please check your Account for more detail <p/>");
            sb.Append("<p> Best Regards,");
            sb.Append("<br/> Manager<p/>");

            if (data != null)
            {
                try
                {
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("suwardihutagalung@gmail.com");
                    mail.To.Add(data.Employee.Email);
                    mail.Subject = $"【Reimbursment】 Rejected";
                    mail.Body = sb.ToString();
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new
                            NetworkCredential("suwardihutagalung@gmail.com", "hutagalung97");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
                catch (Exception)
                {
                    return 2;
                }
                return 1;
            }
            return 3;
        }


    }
}
