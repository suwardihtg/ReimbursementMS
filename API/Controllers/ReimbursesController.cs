using API.Base;
using API.Context;
using API.Models;
using API.Repository.Data;
using API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReimbursesController : BaseController<Reimburse, ReimburseRepository, int>
    {
        private ReimburseRepository reimburseRepository;
        public IConfiguration _configuration;
        private readonly MyContext context;
        public ReimbursesController(ReimburseRepository repository, IConfiguration configuration, MyContext context) :base(repository)
        {
            this.reimburseRepository = repository;
            this._configuration = configuration;
            this.context = context;
        }

        [HttpPost("ReimburseInsert")]
        public ActionResult ExpenseForm(ReimburseVM reimburseVM)
        {
            var result = reimburseRepository.ReimburseForm(reimburseVM);
            switch (result)
            {
                case 1:
                    return Ok();
                default:
                    return BadRequest();
            }
        }

        [HttpPut("ReimburseUpdate/{code}")]
        public ActionResult ExpenseFormUpdate(ReimburseVM reimburseVM, int code)
        {
            var result = reimburseRepository.ReimburseFormUpdate(reimburseVM, code);
            if (result == 1)
            {
                if (code == 1)
                {
                    reimburseRepository.NotifRequest(reimburseVM.Id);
                }
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("ReimburseData/{employeeid}")]
        public ActionResult GetReimburse(string employeeid)
        {
            var result = reimburseRepository.GetReimburse(employeeid);

            if (result.Count() != 0)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpGet("GetID/{email}")]
        public ActionResult ExpesnseID(string email)
        {
            var result = reimburseRepository.ReimburseID(email);

            if (result != null)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        //<!----------------- Finances ------------------->

        [HttpGet("ReimburseDataFinances")]
        public ActionResult GetReimburseFinances()
        {
            var result = reimburseRepository.GetReimburseFinance();

            if (result.Count() != 0)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpGet("ReimburseDataFinancesReject")]
        public ActionResult GetExpenseFinancesReject()
        {
            var result = reimburseRepository.GetReimburseFinanceReject();

            if (result.Count() != 0)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpPut("Approval/{code}")]
        public ActionResult Approval(ReimburseVM reimburseVM, int code)
        {

            var result = reimburseRepository.ReimburseFormUpdate(reimburseVM, code);
            if (result == 1)
            {
                switch (code)
                {
                    case 3:
                        reimburseRepository.NotifRejectM(reimburseVM.Id);
                        break;
                    case 4:
                        reimburseRepository.NotifApproveM(reimburseVM.Id);
                        break;
                    case 5:
                        reimburseRepository.NotifRejectF(reimburseVM.Id);
                        break;
                    case 6:
                        reimburseRepository.NotifApproveF(reimburseVM.Id);
                        break;
                    default:
                        break;
                }
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }

        }


        //<!----------------- Manager ------------------->

        [HttpGet("ExpenseDataManager")]
        public ActionResult GetExpenseManager()
        {
            var result = reimburseRepository.GetReimburseManager();

            if (result.Count() != 0)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

      

        [HttpGet("GetReimburseManagerReject")]
        public ActionResult GetExpenseManagerReject()
        {
            var result = reimburseRepository.GetReimburseManagerReject();

            if (result.Count() != 0)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        //<!----------------- Manager & Finances -------------------> 

        [HttpGet("GetReimbursePosted")]
        public ActionResult GetExpensePosted()
        {
            var result = reimburseRepository.GetReimbursePosted();

            if (result.Count() != 0)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

    }
}
