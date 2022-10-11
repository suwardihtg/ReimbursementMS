using API.Context;
using API.Models;

namespace API.Repository.Data
{
    public class JobRepository : GeneralRepository<MyContext,Job, int>
    {
        public JobRepository(MyContext myContext) :base(myContext)
        {

        }
    }
}
