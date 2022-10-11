using API.Context;
using API.Models;
using API.ViewModel;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace API.Repository.Data
{
    public class RequestFormRepository : GeneralRepository<MyContext, RequestForm, int>
    {
        private readonly MyContext context;
        public IConfiguration _configuration;
        public RequestFormRepository(MyContext myContext, IConfiguration configuration) : base(myContext)
        {
            this.context = myContext;
            this._configuration = configuration;
        }

        public int NewForm(ReqFormVM fromVM)
        {
            EmployeeAttachment atc = new EmployeeAttachment();
            {
                atc.FilePath = fromVM.Attachments;
            }
            context.EmployeeAttachments.Add(atc);
            context.SaveChanges();
            RequestForm form = new RequestForm();
            {
                form.Start_Date = fromVM.Start_Date;
                form.End_Date = fromVM.End_Date;
                form.Attachments = atc.Id;

                switch (fromVM.Category)
                {
                    case 0:
                        form.Category = Categori.Transportation;
                        break;
                    case 1:
                        form.Category = Categori.Parking;
                        break;
                    default:
                        break;
                }
                //form.Payee = fromVM.Payee;
                //form.Description = fromVM.Description;
                form.Total = fromVM.Total;
                form.Id = fromVM.ReimburseId;
            }

            context.RequestForms.Add(form);

            context.SaveChanges();

            return 1;
        }

        public IEnumerable<ReqFormVM> GetForm(int reimburseid)
        {
            var register = from a in context.Reimburses
                           where a.Id == reimburseid
                           join b in context.RequestForms on a.Id equals b.Id
                           join c in context.EmployeeAttachments on b.Attachments equals c.Id
                           select new ReqFormVM()
                           {
                               Id = b.Id,                             
                               Total = b.Total,                      
                               Category = (int)b.Category,
                               Attachments = c.FilePath
                           };

            return register.ToList();
        }

        public TotalVM TotalExpenseForm(int reimburseid)
        {
            var sum = (from a in context.Reimburses
                       where a.Id == reimburseid
                       join b in context.RequestForms on a.Id equals b.Id
                       select b.Total.Value).Sum();

            TotalVM total = new TotalVM();
            total.Total = sum;
            return total;
        }

        public int FormUpdate(ReqFormVM fromVM)
        {
            var data = (from a in context.RequestForms
                        where a.Id == fromVM.Id
                        select new { form = a }).Single();
            var form = data.form;
            form.Start_Date = fromVM.Start_Date;
            form.End_Date = fromVM.End_Date;
            switch (fromVM.Category)
            {
                case 0:
                    form.Category = Categori.Transportation;
                    break;
                case 1:
                    form.Category = Categori.Parking;
                    break;
                default:
                    break;
            }

            form.Total = fromVM.Total;


            context.SaveChanges();
            return 1;
        }


        public AttachmentsVM Getatc(int imgid)
        {
            var imgPath = (from a in context.EmployeeAttachments where a.Id == imgid select a.FilePath).ToList();


            AttachmentsVM path = new AttachmentsVM();
            path.Name = imgPath[0].ToString();
            return path;
        }
    }
}
