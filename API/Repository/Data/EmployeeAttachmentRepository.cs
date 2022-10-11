using API.Context;
using API.Models;

namespace API.Repository.Data
{
    public class EmployeeAttachmentRepository : GeneralRepository<MyContext, EmployeeAttachment, string>
    {
        private readonly MyContext context;
        public EmployeeAttachmentRepository(MyContext myContext) : base(myContext)
        {
            this.context = myContext;
        }
        public int Filepath(string filename)
        {
            EmployeeAttachment atc = new EmployeeAttachment();
            atc.FilePath = filename;
            context.EmployeeAttachments.Add(atc);
            var a = context.SaveChanges();
            return a;
        }
    }
}
