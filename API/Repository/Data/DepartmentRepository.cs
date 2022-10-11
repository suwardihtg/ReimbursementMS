using API.Context;
using API.Models;

namespace API.Repository.Data
{
    public class DepartmentRepository : GeneralRepository<MyContext, Department, int>
    {
        public DepartmentRepository(MyContext myContext) : base(myContext)  
        {

        }
    }
}
