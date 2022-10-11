using API.Context;
using API.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace API.Repository.Data
{
    public class ReimburseHistoryRepository : GeneralRepository<MyContext, ReimburseHistory, string>
    {
        private readonly MyContext context;

        public IConfiguration _configuration;
        public ReimburseHistoryRepository(MyContext myContext, IConfiguration configuration) : base(myContext)
        {
            this.context = myContext;
            _configuration = configuration;
        }

        public IEnumerable<ReimburseHistory> GetHistory(int reimburseid)
        {
            var reimburse = from a in context.ReimburseHistories
                          where a.Id == reimburseid
                          select new ReimburseHistory()
                          {
                              Message = a.Message
                          };

            return reimburse.ToList();
        }
    }
}
