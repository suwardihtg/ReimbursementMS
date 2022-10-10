using API.Context;
using API.Hash;
using API.Models;
using API.ViewModel;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace API.Repository.Data
{
    public class AccountRepository :  GeneralRepository<MyContext, Account, string>
    {
        private readonly MyContext context;
        public IConfiguration _configuration;

        public AccountRepository(MyContext myContext, IConfiguration configuration) : base(myContext)
        {
            this.context = myContext;
            this._configuration = configuration;
        }

        public int Register(RegisterVM registerVM)
        {
            Employee employee = new Employee();
            {
                employee.Id = registerVM.Id;
                employee.FirstName = registerVM.FirstName;
                employee.LastName = registerVM.LastName;
                employee.Gender = (registerVM.Gender == "Male") ? Gender.Male : Gender.Female;
                employee.Email = registerVM.Email;
                employee.ManagerId = registerVM.ManagerId;
                employee.DepartmentId = employee.DepartmentId;
                employee.JobId = registerVM.JobId;
            }

            Account account = new Account();
            {
                account.Id = registerVM.Id;
                account.Password = Hashing.HashPassword(registerVM.Password);
            }

            context.Employees.Add(employee);
            context.Accounts.Add(account);
            context.SaveChanges();
            return 1;
        }

        public int Login(LoginVM loginVM)
        {
            var dataPass = (from a in context.Employees
                            where a.Email == loginVM.Email
                            join b in context.Accounts on a.Id equals b.Id
                            select new { Account = b, Employee = a }).FirstOrDefault();

            if (dataPass == null)
            {
                return 4;
            }
            else if (dataPass.Employee.Email != null)
            {
                var cekPassword = Hashing.ValidatePassword(loginVM.Password, dataPass.Account.Password);
                if (cekPassword == true)
                {
                    return 1;
                }
                return 2;
            }
            return 3;

        }


    }
}
